using Learning.Auth;
using Learning.Entities;
using Learning.Tutor.Abstract;
using Learning.Utils;
using Learning.Utils.Config;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth.Account
{
   public class AuthService:IAuthService
    {
        #region variables
        readonly IAuthRepo _authRepo;
        readonly IEmailService emailService;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISecurePassword _securePassword;
        private readonly AppConfig _appConfig;
        private readonly ITutorService _tutorService;
        #endregion

        #region constructor
        public AuthService(IAuthRepo authRepo,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager,
            IEmailService _email,SignInManager<AppUser> signInManager,ISecurePassword securePassword,AppConfig appConfig, ITutorService tutorService)
        {
            _signInManager = signInManager;
            httpContext = httpContextAccessor;
            this.emailService = _email;
            this._userManager = userManager;
            _tutorService = tutorService;
            _appConfig = appConfig;
            _securePassword = securePassword;
            this._authRepo = authRepo;
        }
        #endregion

        #region methods
        public async Task<AppUser> GetUser(LoginViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.UserName).ConfigureAwait(true) ?? await _userManager.FindByEmailAsync(viewModel.UserName).ConfigureAwait(true);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, false);
                if (result.Succeeded)
                {
                   
                    return user;
                }
            }
            return null;
        }

        public async Task<Student> GetStudent(LoginViewModel viewModel)
        {
            //Do the logic for Password with password salt
            var passwordHash = _securePassword.Secure(_appConfig.SecretKey.StudentSaltKey, viewModel.Password);
            var user = await _authRepo.GetStudentAsync(viewModel.UserName, passwordHash);
            return user;
        } 

        public async Task<List<Student>> GetAssociatedStudents(int parentUserId)
        {
           return await _authRepo.GetAssociatedStudents(parentUserId);
        }

        public async Task<AppUser> GetUserByUserId(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }
        public async Task<IdentityResult> AddUser(AppUser appUser, string password,AppRole role)
        {
            var errors = await _authRepo.AddUser(appUser, password, role);
            if (errors.Succeeded)
            {
                string body = await GetEmailConfirmationBody(appUser);
                await emailService.SendEmailConfirmation(appUser.Email, body);
            }
            return errors;
        }
        public async Task<IdentityResult> UpdateUser(AppUser app)
        {
            return await _userManager.UpdateAsync(app);
        }
        public Task<Student> AddStudent(Student student)
        {
            return _authRepo.AddStudent(student);
        }
        public int AddStudentAccountRecoveryQuestions(List<StudentAccountRecoveryAnswer> recoveryAnswer)
        {
            return _authRepo.UpsertStudentSecretAnswer(recoveryAnswer);
        }
       public List<StudentAccountRecoveryAnswer> GetStudentAccountRecoveryAnswers(int userid)
        {
            return _authRepo.GetStudentAccountRecoveryAnswers(userid);
        }
        public async Task<string> EmailConfirmation(string token, string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            var decodebytes = WebEncoders.Base64UrlDecode(token);
            var decodedtoken = Encoding.UTF8.GetString(decodebytes);
            var auth = await _userManager.ConfirmEmailAsync(user,decodedtoken);
            if (auth.Succeeded)
                return "Success";
            else
            {
                return auth.Errors.FirstOrDefault().Description;
            }
        }
        public async Task<bool> ForgotPassword(string email)
        {
            string body = await GetForgotPasswordBody(email);
            if (!string.IsNullOrEmpty(body))
            {
                await emailService.SendForgotPassword(email, body);
                return true;
            }
            else
                return false;
        }
        public async Task<string> GetForgotPasswordBody(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return string.Empty;
            var tokenByte =Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(user));
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenByte);
            string link;
            if (httpContext.HttpContext.Request.Host.Value == "api.domockexam.com")
            {
                link = $"domockexam.com/#/ResetPassword?Token={tokenEncoded}&&Email={email}";
            }
            else
                link = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host.Value}/Account/ResetPassword?Token={tokenEncoded}&&Email={email}";
            
            var emailbody = await emailService.GetEmailTemplateContent(EmailTemplate.ResetPassword);
           emailbody= emailbody.Replace("{link}", link);
            return emailbody;
        }
        public async Task<IdentityResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var decodedByte = WebEncoders.Base64UrlDecode(model.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedByte);
            //var student = await _authRepo.GetAssociatedStudents(user.Id);
            //if (student.Any())
            //    _authRepo.UpdateStudentPassword(student.FirstOrDefault().Id, model.Password);
            return await _userManager.ResetPasswordAsync(user,decodedToken, model.Password);
        }
        public async Task<string> GetEmailConfirmationBody(AppUser user)
        {
            var emailBody =await emailService.GetEmailTemplateContent(EmailTemplate.ConfirmEmail);
            var tokenBytes = Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
            string link = "";
            if (httpContext.HttpContext.Request.Host.Value == "api.domockexam.com")
            {
                link = $"domockexam.com/#/ConfirmEmail?token={tokenEncoded}&&email={user.Email}";
            }else
            link = $"{ httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/Account/ConfirmEmail?token={tokenEncoded}&&email={user.Email}";
            emailBody = emailBody.Replace("{link}", link);
            return emailBody;
        }
        public async Task<bool> IsEmailExists(string email, int? id)
        {
           return await _authRepo.IsEmailExists(email,id);
        }
        public async Task<bool> IsUserNameExists(string username, int? id)
        {
            return await _authRepo.IsUserNameExists(username, id);

        }
       
        public async Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username)
        {
            return await _authRepo.GetScreenAccessByUserName(username);
        }

       
        public async Task LogOut() => await _signInManager.SignOutAsync();

       public Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId=null)
        {
            return _authRepo.GetScreenAccessPrivilage(userID, roleId);
        }
        public bool IsStudentUserNameExists(string username, int? id=0)
        {
            return _authRepo.IsStudentUserNameExists(username, id);
        }
        public Task<int> AddTutor(Tutor entity)
        {
           return _authRepo.AddTutor(entity);
        }
       public int UpsertStudentSecretAnswer(List<StudentAccountRecoveryAnswer> recoveryAnswer)
        {
            return _authRepo.UpsertStudentSecretAnswer(recoveryAnswer);
        }
        public int UpdateStudentPassword(int studentId, string password)
        {
            return _authRepo.UpdateStudentPassword(studentId, password);
        }

        public Task<int> AddTeacher(Teacher teacher)
        {
            return _authRepo.AddTeacher(teacher);
        }

        public int UpserStudentSecretAnswer(List<StudentAccountRecoveryAnswer> recoveryAnswer)
        {
           return _authRepo.UpsertStudentSecretAnswer(recoveryAnswer);
        }

        #endregion

    }
}

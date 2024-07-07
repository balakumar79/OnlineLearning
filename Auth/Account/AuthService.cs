using Learning.Auth;
using Learning.Entities;
using Learning.Tutor.Abstract;
using Learning.Entities;
using Learning.Entities.Config;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;
using Microsoft.Extensions.Logging;

namespace Auth.Account
{
    public class AuthService : IAuthService
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
        private readonly ILogger<AuthService> _logger;
        #endregion

        #region constructor
        public AuthService(IAuthRepo authRepo, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, ILogger<AuthService> logger,
            IEmailService _email, SignInManager<AppUser> signInManager, ISecurePassword securePassword, AppConfig appConfig, ITutorService tutorService)
        {
            _signInManager = signInManager;
            httpContext = httpContextAccessor;
            this.emailService = _email;
            this._userManager = userManager;
            _tutorService = tutorService;
            _appConfig = appConfig;
            _securePassword = securePassword;
            this._authRepo = authRepo;
            _logger = logger;
        }
        #endregion

        #region methods
        public async Task<AppUser> GetUser(LoginViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName).ConfigureAwait(true) ?? await _userManager.FindByEmailAsync(viewModel.UserName).ConfigureAwait(true);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, viewModel.Password, viewModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        user.LastAccessedOn = DateTime.UtcNow;
                        await _userManager.UpdateAsync(user);
                        return user;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUser Service");
                throw;
            }
        }

        public async Task<Student> GetStudent(LoginViewModel viewModel)
        {
            try
            {
                //Do the logic for Password with password salt
                var passwordHash = _securePassword.Secure(_appConfig.SecretKey.StudentSaltKey, viewModel.Password);
                var user = await _authRepo.GetStudentAsync(viewModel.UserName, passwordHash);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStudent Service");
                throw;
            }
        }


        public async Task<List<Student>> GetAssociatedStudentsForParent(int parentUserId)
        {
            return await _authRepo.GetAssociatedStudentsForParent(parentUserId);
        }

        public List<Student> GetAssociatedStudentsForTeacher(int teacherId)
        {
            return _authRepo.GetAssociatedStudentsForTeacher(teacherId);
        }

        public async Task<AppUser> GetUserByUserId(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }

        public async Task<IdentityResult> AddUser(AppUser appUser, string password, AppRole role)
        {
            try
            {
                var errors = await _authRepo.AddUser(appUser, password, role);
                if (errors.Succeeded)
                {
                    string body = await GetEmailConfirmationBody(appUser);
                    await emailService.SendEmailConfirmation(appUser.Email, body);
                }
                return errors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddUser Service");
                throw;
            }
        }

        public async Task<IdentityResult> UpdateUser(AppUser app)
        {
            return await _userManager.UpdateAsync(app);
        }

        public Task<StudentModel> AddStudent(Student student)
        {
            return _authRepo.AddStudent(student);
        }

        public int AddStudentAccountRecoveryQuestions(List<AccountRecoveryAnswerModel> recoveryAnswer)
        {
            return _authRepo.UpsertStudentSecretAnswer(recoveryAnswer);
        }

        public List<StudentAccountRecoveryAnswer> GetStudentAccountRecoveryAnswers(int userid)
        {
            return _authRepo.GetStudentAccountRecoveryAnswers(userid);
        }

        public async Task<string> EmailConfirmation(string token, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var decodebytes = WebEncoders.Base64UrlDecode(token);
                var decodedtoken = Encoding.UTF8.GetString(decodebytes);
                var auth = await _userManager.ConfirmEmailAsync(user, decodedtoken);
                if (auth.Succeeded)
                    return "Success";
                else
                {
                    return auth.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EmailConfirmation Service");
                throw;
            }
        }

        public async Task<bool> ForgotPassword(string email)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "ForgotPassword Service");
                throw;
            }
        }

        public async Task<string> GetForgotPasswordBody(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return string.Empty;
                var tokenByte = Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(user));
                var tokenEncoded = WebEncoders.Base64UrlEncode(tokenByte);
                string link;
                if (httpContext.HttpContext.Request.Host.Value == "api.domockexam.com")
                {
                    link = $"https://domockexam.com/#/ResetPassword?Token={tokenEncoded}&&Email={email}";
                }
                else
                    link = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host.Value}/Account/ResetPassword?Token={tokenEncoded}&&Email={email}";

                var emailbody = await emailService.GetEmailTemplateContent(EmailTemplate.ResetPassword);
                emailbody = emailbody.Replace("{link}", link);
                return emailbody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetForgotPasswordBody Service");
                throw;
            }
        }

        public async Task<IdentityResult> ResetPassword(ForgotPasswordViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var decodedByte = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedByte);

                return await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "ResetPassword Service");

                throw;
            }
        }

        public async Task<string> GetEmailConfirmationBody(AppUser user)
        {
            try
            {

                var emailBody = await emailService.GetEmailTemplateContent(EmailTemplate.ConfirmEmail);
                var tokenBytes = Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
                string link = "";
                if (httpContext.HttpContext.Request.Host.Value == "api.domockexam.com")
                {
                    link = $"https://domockexam.com/#/ConfirmEmail?token={tokenEncoded}&&email={user.Email}";
                }
                else
                    link = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/Account/ConfirmEmail?token={tokenEncoded}&&email={user.Email}";
                emailBody = emailBody.Replace("{link}", link);
                return emailBody;
            }
            catch (Exception ex) { _logger.LogError(ex, "GetEmailConfirmationBody Service"); throw; }
        }

        public async Task<bool> IsEmailExists(string email, int? id)
        {
            return await _authRepo.IsEmailExists(email, id);
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

        public Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId = null)
        {
            return _authRepo.GetScreenAccessPrivilage(userID, roleId);
        }

        public bool IsStudentUserNameExists(string username, int? id = 0)
        {
            return _authRepo.IsStudentUserNameExists(username, id);
        }

        public Task<int> AddTutor(Tutor entity)
        {
            return _authRepo.AddTutor(entity);
        }

        public int UpsertStudentSecretAnswer(List<AccountRecoveryAnswerModel> recoveryAnswer)
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

        public int UpserStudentSecretAnswer(List<AccountRecoveryAnswerModel> recoveryAnswer)
        {
            return _authRepo.UpsertStudentSecretAnswer(recoveryAnswer);
        }

        #endregion

    }
}

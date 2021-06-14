using Learning.Entities;
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
        private readonly RoleManager<AppRole> _roleManager;
        #endregion

        #region constructor
        public AuthService(IAuthRepo authRepo,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,
            IEmailService _email,SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            httpContext = httpContextAccessor;
            this.emailService = _email;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._authRepo = authRepo;
        }
        #endregion

        #region methods
        public async Task<AppUser> GetUser(LoginViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.UserName);
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
        public Task<int> AddStudent(Student student)
        {
            return _authRepo.AddStudent(student);
        }
        public async Task<string> EmailConfirmation(string token, int userid)
        {
            var user =await _userManager.FindByIdAsync(userid.ToString());
            var decodebytes = WebEncoders.Base64UrlDecode(token);
            var decodedtoken = Encoding.UTF8.GetString(decodebytes);
            var auth = await _userManager.ConfirmEmailAsync(user,decodedtoken);
            if (auth.Succeeded)
                return string.Empty;
            else
            {
                return auth.Errors.FirstOrDefault().Description;
            }
        }
        public async Task<bool> ForgotPassword(string email)
        {
            string body = await GetForgotPasswordBody(email);
            await emailService.SendForgotPassword(email, body);
            return true;
        }
        public async Task<string> GetForgotPasswordBody(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var tokenByte =Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(user));
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenByte);
            var link = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host.Value}/Account/ResetPassword?Token={tokenEncoded}&&Email={email}";
            var emailbody = await emailService.GetEmailTemplateContent(EmailTemplate.ResetPassword);
           emailbody= emailbody.Replace("{link}", link);
            return emailbody;
        }
        public async Task<IdentityResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var decodedByte = WebEncoders.Base64UrlDecode(model.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedByte);
            return await _userManager.ResetPasswordAsync(user,decodedToken, model.Password);
        }
        public async Task<string> GetEmailConfirmationBody(AppUser user)
        {
            var emailBody =await emailService.GetEmailTemplateContent(EmailTemplate.ConfirmEmail);
            var tokenBytes = Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
            var link = $"{ httpContext.HttpContext.Request.Scheme}://{"localhost:44390"}/Account/ConfirmEmail?token={tokenEncoded}&&userid={user.Id}";
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

        Task<List<ScreenFormeter>> IAuthService.GetScreenAccessPrivilage(int? userID, IList<string> roleId)
        {
            return _authRepo.GetScreenAccessPrivilage(userID, roleId);
        }

        #endregion

    }
}

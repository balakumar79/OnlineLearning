using Auth.Account;
using Learning.Auth;
using Learning.Entities;
using Learning.Tutor.Abstract;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Learning.API.Controllers
{
    //[EnableCors("LearningCors")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        #region variables
        readonly IAuthService authService;
        readonly UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        readonly ITutorService _tutorService;
        readonly Utils.Config.SecretKey _secretkey;
        #endregion

        #region ctor
        public AccountController(IAuthService auth, UserManager<AppUser> userManager, ITutorService tutorService, SignInManager<AppUser> signInManager,
            Utils.Config.SecretKey appSet)
        {
            this._tutorService = tutorService;
            this._userManager = userManager;
            this.authService = auth;
            this._signInManager = signInManager;
            _secretkey = appSet;
        }
        #endregion

        //post: login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model,string UserName, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await authService.GetUser(model);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var screens = await authService.GetScreenAccessPrivilage(roleId: roles, userID: user.Id);
                        var sessionObj = new SessionObject { User = user, RoleID = roles.ToList(), Student = null, Tutor = _tutorService.GetTutorProfile(user.Id) };
                        //await HttpContext.RefreshLoginAsync();
                     var result=    AuthenticationConfig.DoLogin(screens, sessionObj,_secretkey.SecretKeyValue);
                     
                        return Ok(new
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                            useraccess = user.HasUserAccess,
                            result.Value
                        });
                    }
                    else
                    {
                        return new JsonResult(new { result = false }); ;
                    }
                }
                catch (Exception ex)
                {

                    return new JsonResult(new { result = false, error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
                }
            }
            else
            {
                return new JsonResult(ModelState.Select(p=>p.Value).Where(o=>o.Errors.Count>0));
            }
        }
       
        [HttpPost]
        public async Task<object> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    //Gender = registerViewModel.Gender,
                    UserName = registerViewModel.UserName,
                };
                var useresult = await authService.AddUser(user, registerViewModel.ConfirmPassword, new AppRole { Name = "Parent" });
                if (!useresult.Succeeded)
                {
                    return new JsonResult(new { status =useresult});
                }
                else
                {
                    var student = new Entities.Student
                    {
                        FirstName = registerViewModel.StudentFirstName,
                        LastName = registerViewModel.StudentLastName,
                        Grade = registerViewModel.GradeLevels,
                        ParentID = user.Id,
                        Gender = registerViewModel.StudentGender,
                        MotherTongue = registerViewModel.MotherTongue,
                        UserID = registerViewModel.StudentUserName
                    };
                    var res = await authService.AddStudent(student);
                    return new JsonResult(new { result = res });
                }
            }
            else
            {
                return new JsonResult(ModelState.Select(p=>p.Value).Where(l=>l.Errors.Count>0));
            }
        }

      
        [HttpGet]
        public async Task<object> ConfirmEmail(string token, int userid)
        {
            return new JsonResult(new { result = authService.EmailConfirmation(token, userid).Result });
        }

       
        [HttpPost]
        public async Task<object> ForgotPassword(string Email)
        {
           return new JsonResult(new { result = await authService.ForgotPassword(Email) });
            
        }
        [HttpGet]
        public object ResetPassword(string Token, string Email)
        {
            if (!string.IsNullOrWhiteSpace(Token.Trim()) && !string.IsNullOrWhiteSpace(Email.Trim()))
            {
                var model = new ResetPasswordViewModel { Email = Email, Token = Token };
                return  ResultFormatter.JsonResponse(true);
            }
            else
            {
                return ResultFormatter.JsonResponse(false, null, "Invalid request");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(AuthorizationModel.IdentityApplicationDefault);
            return Ok();
        }
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    var resut = await authService.ResetPassword(model);
        //    if (resut.Succeeded)
        //        return View("ResetPasswordConfirmation");
        //    else
        //    {
        //        foreach (var item in resut.Errors)
        //        {
        //            ModelState.AddModelError("", item.Description);
        //        }
        //        return View();
        //    }
        //}

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> LogOut()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Login");
        //}
        //public IActionResult ParentProfile(RegisterViewModel model)
        //{
        //    return View();
        //}
        //public IActionResult IsEmailExists(string ParentEmail, int id)
        //{
        //    var isexists = authService.IsEmailExists(ParentEmail, id).Result;
        //    if (!isexists)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"Email {ParentEmail} is already in use");
        //    }
        //}
        //public async Task<IActionResult> IsUserNameExists(string username, int id)
        //{
        //    var result = await authService.IsUserNameExists(username, id);
        //    if (!result)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"Email {username} is already in use");
        //    }
        //}

        [HttpGet]
        public List<Language> GetLanguageList() => (_tutorService.GetLanguages());
        //public Task<JsonResult> GetGradeLevels() => Task.FromResult(Json(authService.GetGradeLevels()));
        //public async Task<JsonResult> GetSubject() => Json(await authService.GetTestSubject());
        //public async Task<JsonResult> GetTestSections(int testid) => Json(await authService.GetTestSections(testid));
        //public async Task<JsonResult> GetTestType() => Json(await _tutorService.GetQuestionTypes());


    }
    
}


using Auth.Account;
using Learning.Auth;
using Learning.Entities;
using Learning.Tutor.Abstract;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutorWebUI.Controllers
{
    public class AccountController : Controller
    {
        readonly IAuthService authService;
        readonly UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        readonly ITutorService _tutorService;
        public AccountController(IAuthService auth,UserManager<AppUser> userManager,ITutorService tutorService,SignInManager<AppUser> signInManager)
        {
            this._tutorService = tutorService;
            this._userManager = userManager;
            this.authService = auth;
            this._signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(string ReturnUrl)
        {
            TempData["rurl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                var user =await authService.GetUser(model);
                if (user!=null)
                {
                 
                   var sessionObj= HttpContext.Session.GetObjectFromJson<SessionObject>("sessionObj");
                    //await AuthenticationConfig.DoLogin(HttpContext, sessionObj.ScreenAccess, sessionObj, model.RememberMe);
                    //await HttpContext.RefreshLoginAsync();
                    if (returnUrl==null)
                    {
                        if (sessionObj.RoleID.Contains(Learning.Utils.Enums.Roles.Minor.ToString()))
                            return RedirectToAction(controllerName: "Student", actionName: "Dashboard");
                        else if (sessionObj.RoleID.Contains(Learning.Utils.Enums.Roles.Parent.ToString()))
                            return RedirectToAction(controllerName: "Parent", actionName: "Dashboard");
                        else if (sessionObj.RoleID.Contains(Learning.Utils.Enums.Roles.Tutor.ToString()))
                            return RedirectToAction(controllerName: "Tutor", actionName: "Dashboard");
                        else if (sessionObj.RoleID.Contains(Learning.Utils.Enums.Roles.Admin.ToString()))
                            return RedirectToAction(controllerName: "Tutor", actionName: "Dashboard");
                        else
                            return Redirect("~/Home");
                    }
                    else
                        return Redirect(returnUrl.ToString());
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }
            else
            {
                foreach (var error in ModelState.Select(p => p.Value).Where(p => p.Errors.Count > 0))
                    ModelState.AddModelError("", error.Errors.FirstOrDefault().ErrorMessage);
                return View(model);
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountUserModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Gender = registerViewModel.Gender.ToString(),
                    UserName = registerViewModel.UserName,
                    
                };
                var useresult = await authService.AddUser(user, registerViewModel.ConfirmPassword, new AppRole { Name = Learning.Utils.Enums.Roles.Tutor.ToString() });
                if (!useresult.Succeeded)
                {
                    foreach (var err in useresult.Errors)
                        ModelState.AddModelError(err.Code, err.Description);
                    return View();
                }
                else
                {
                    var tutor = new Tutor
                    {
                        CreatedAt = DateTime.Now,
                        UserId = user.Id,
                        UserName=user.UserName,
                        ModifiedAt = DateTime.Now,
                        HearAbout = registerViewModel.HearAbout
                    };
                    var result = await authService.AddTutor(tutor);
                    if (result > 0)
                    {
                        TempData["msg"] = "Your registration has been submitted successfully.  You will get notified when your account is activated.";
                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            else
            {
                foreach (var item in ModelState.Select(p => p.Value).Where(l => l.Errors.Count > 0))
                {
                    ModelState.AddModelError("", item.Errors.FirstOrDefault().ErrorMessage);
                }
                return View(registerViewModel);
            }
            return View(registerViewModel);
        }

        public IActionResult RegisterConfirmation()
        {
            return View("RegisterationConfirmation");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {

            ViewData["message"] = await authService.EmailConfirmation(token, email);

            return View("EmailConfirmedConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
           var user= await _userManager.FindByEmailAsync(Email);
            if(user!=null)
           await authService.ForgotPassword(Email);
            return View("ForgotPasswordConfirmation");
        }
        [HttpGet]
        public IActionResult ResetPassword(string Token,string Email)
        {
            if (!string.IsNullOrWhiteSpace(Token.Trim()) && !string.IsNullOrWhiteSpace(Email.Trim()))
            {
                var model = new ResetPasswordViewModel { Email = Email, Token = Token };
                return View(model);
            }
            else
            {
                return View("Error", new ErrorViewModel { Message = "Invalid request", RequestId = "500" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var resut = await authService.ResetPassword(model);
            if(resut.Succeeded)
            return View("ResetPasswordConfirmation");
            else
            {
                foreach (var item in resut.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
        }

       [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ParentProfile(RegisterViewModel model)
        {
            return View();
        }
        public async Task<IActionResult> IsEmailExists(string Email, int id)
        {
            var isexists = await authService.IsEmailExists(Email, id);
            if (!isexists)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use");
            }
        }
        
        public async Task<IActionResult> IsUserNameExists(string username, int id)
        {
            var result = await authService.IsUserNameExists(username, id);
            if (!result)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {username} is already in use");
            }
        }

        
        

    }
}

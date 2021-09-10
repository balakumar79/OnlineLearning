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

namespace Learning.WebUI.Controllers
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
                    var roles =await _userManager.GetRolesAsync(user);
                    var screens = await authService.GetScreenAccessPrivilage(roleId:roles,userID:user.Id);
                    var sessionObj = new SessionObject {User= user, RoleID = roles.ToList(), Student = null, Tutor = _tutorService.GetTutorProfile(user.Id) };
                    await HttpContext.RefreshLoginAsync();
                    await AuthenticationConfig.DoLogin(HttpContext, screens,sessionObj,model.RememberMe);
                    if (returnUrl==null)
                    {
                        if (roles.Contains(Utils.Enums.Roles.Student.ToString()))
                            return RedirectToAction(controllerName: "Student", actionName: "Dashboard");
                        else if (roles.Contains(Utils.Enums.Roles.Parent.ToString()))
                            return RedirectToAction(controllerName: "Parent", actionName: "Dashboard");
                        else if (roles.Contains(Utils.Enums.Roles.Tutor.ToString()))
                            return RedirectToAction(controllerName: "Tutor", actionName: "Dashboard");
                        else if (roles.Contains(Utils.Enums.Roles.Admin.ToString()))
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
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
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
                var useresult = await authService.AddUser(user, registerViewModel.ConfirmPassword, new AppRole { Name = "Parent" });
                if (!useresult.Succeeded)
                {
                    foreach (var err in useresult.Errors)
                        ModelState.AddModelError(err.Code, err.Description);
                    return View(registerViewModel);
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
                    if (res > 0)
                    {
                        return RedirectToAction(nameof(RegisterConfirmation));
                    }
                    else
                    {
                        ModelState.AddModelError("", "error");
                        return View();
                    }
                }
            }
            else
            {
                foreach (var item in ModelState.Select(p=>p.Value).Where(l=>l.Errors.Count>0))
                {
                    ModelState.AddModelError("", item.Errors.FirstOrDefault().ErrorMessage);
                }
                    return View(registerViewModel);
            }
        }

        public IActionResult RegisterConfirmation()
        {
            return View("RegisterationConfirmation");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, int userid)
        {
            ViewData["message"] =  authService.EmailConfirmation(token, userid).Result;

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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ParentProfile(RegisterViewModel model)
        {
            return View();
        }
        public IActionResult IsEmailExists(string ParentEmail, int id)
        {
            var isexists = authService.IsEmailExists(ParentEmail, id).Result;
            if (!isexists)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {ParentEmail} is already in use");
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

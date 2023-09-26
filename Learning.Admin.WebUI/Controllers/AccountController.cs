using Auth.Account;
using Learning.Auth;
using Learning.Entities;
using Learning.Tutor.Abstract;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI.Controllers
{
    public class AccountController : Controller
    {
        readonly IAuthService authService;
        readonly UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        
        public AccountController(IAuthService authService,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this.authService = authService;
            this._signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await authService.GetUser(model);
                if (user!=null)
                {

                    var roles = await _userManager.GetRolesAsync(user);
                    var sessionObj = new SessionObject { User = user, RoleID = roles.ToList(), Student = null, Tutor = null };
                    await AuthenticationConfig.DoLogin(HttpContext, null, sessionObj,model.RememberMe);
                    if (roles.Contains(Entities.Enums.Roles.Admin.ToString()))
                        return Redirect("~/Dashboard");
                    else
                        return Forbid();
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
            }
            return View(Json("returned no result"));
        }

        [AcceptVerbs("Get","Post")]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> LogOut()
        {
           await _signInManager.SignOutAsync();
           await HttpContext.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public IActionResult IsEmailExists(string Email, int id)
        {
            var isexists = authService.IsEmailExists(Email, id).Result;
            if (!isexists)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use");
            }
        }
        [AllowAnonymous]
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


        //public Task<JsonResult> GetLanguageList() => Task.FromResult(Json(authService.GetLanguages()));
        //public Task<JsonResult> GetGradeLevels() => Task.FromResult(Json(authService.GetGradeLevels()));
        //public Task<JsonResult> GetSubject() => Task.FromResult(Json(authService.GetTestSubject()));
        //public Task<JsonResult> GetTestSections(int testid) => Task.FromResult(Json(authService.GetTestSections(testid)));
    }
}

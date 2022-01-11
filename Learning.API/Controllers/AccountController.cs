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
        readonly ISecurePassword _securePassword;
        #endregion

        #region ctor
        public AccountController(IAuthService auth, UserManager<AppUser> userManager, ITutorService tutorService, SignInManager<AppUser> signInManager,
           ISecurePassword securePassword, Utils.Config.SecretKey appSet)
        {
            this._tutorService = tutorService;
            this._userManager = userManager;
            this.authService = auth;
            this._signInManager = signInManager;
            this._securePassword = securePassword;
            _secretkey = appSet;
        }
        #endregion

        //post: login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await authService.GetUser(model);
                    var student = await authService.GetStudent(model);
                    if (user != null || student != null)
                    {
                        if (user != null && student == null)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            var screens = await authService.GetScreenAccessPrivilage(roleId: roles, userID: user.Id);

                            var sessionObj = new SessionObject { User = user, RoleID = roles.ToList(), Student = student, Tutor = _tutorService.GetTutorProfile(user.Id),Childs=await authService.GetAssociatedStudents(user.Id) };
                            //await HttpContext.RefreshLoginAsync();
                            var result = AuthenticationConfig.DoLogin(sessionObj, _secretkey.SecretKeyValue, screens);

                            return Ok(new
                            {
                                user = new
                                {
                                    Id = user.Id,
                                    Username = user.UserName,
                                    Email = user.Email,
                                    useraccess = user.HasUserAccess,
                                    result.Value,
                                    roles = sessionObj.RoleID
                                },
                                childs =  sessionObj.Childs

                            });
                        }
                        else
                        {
                            return await StudentLogin(model);
                        }
                    }
                    else
                    {
                        return new JsonResult(new { result = false });
                    }
                }
                catch (Exception ex)
                {

                    return new JsonResult(new { result = false, error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
                }
            }
            else
            {
                return new JsonResult(ModelState.Select(p => p.Value).Where(o => o.Errors.Count > 0));
            }
        }

        public async Task<IActionResult> StudentLogin(LoginViewModel model)
        {
            var student = await authService.GetStudent(model);
            if (student == null)
                return new JsonResult(new { user = new AppUser() });
            var roles = new List<string> { Utils.Enums.Roles.Minor.ToString() };
            var userscreens = await authService.GetScreenAccessPrivilage(student.Id,roles);
            
            var sessionObj = new SessionObject
            {
                RoleID = roles.ToList(),
                ScreenAccess = userscreens,
                User = new AppUser { Id = student.Id, FirstName = student.FirstName, LastName = student.LastName, UserName = student.UserName, Student = student },
                Student = student
            };
            var result = AuthenticationConfig.DoLogin(sessionObj, _secretkey.SecretKeyValue, userscreens);
            return Ok(new
            {
                student = new
                {
                    studentId = student.Id,
                    username = student.UserName,
                    UserId = student.UserID,
                    firstName = student.FirstName,
                    lastName = student.LastName,
                    languageKnown = student.LanguagesKnown,
                    motherTongue = student.MotherTongue,
                    district = student.StudentDistrict,
                    institution = student.Institution,
                    roles=sessionObj.RoleID
                },
                token = result.Value
            });
        }

        [HttpPost]
        public async Task<object> Register(RegisterViewModel registerViewModel)
        {
            if (authService.IsStudentUserNameExists(registerViewModel.StudentModel.StudentUserName))
                return new JsonResult(new { error = "Student username already exists." });
            if (ModelState.IsValid)
            {
                var roleRequested = (Utils.Enums.Roles)registerViewModel.Role;
                var user = new AppUser
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Gender =((Utils.Enums.Genders)registerViewModel.Gender).ToString(),
                    UserName = registerViewModel.UserName,
                    District = registerViewModel.District
                };
                var useresult =await authService.AddUser(user, registerViewModel.ConfirmPassword, new AppRole { Name = roleRequested.ToString() });
                if (!useresult.Succeeded)
                {
                    return new JsonResult(new { useresult });
                }
                else
                {


                    if (registerViewModel.StudentModel != null && (registerViewModel.Role == (int)Utils.Enums.Roles.Minor||registerViewModel.Role==(int)Utils.Enums.Roles.Major) )
                    {
                        var student = new Entities.Student
                        {
                            FirstName = registerViewModel.StudentModel.StudentFirstName,
                            LastName = registerViewModel.StudentModel.StudentLastName,
                            Grade = registerViewModel.StudentModel.GradeLevels,
                            UserID = user.Id,
                            Password= _securePassword.Secure(_secretkey.StudentSaltKey,registerViewModel.StudentModel.StudentPassword),
                            Gender = registerViewModel.StudentModel.StudentGender,
                            MotherTongue = registerViewModel.StudentModel.MotherTongue,
                            UserName = registerViewModel.StudentModel.StudentUserName ,
                            Institution=registerViewModel.    StudentModel.Institution,
                            LanguagesKnown=registerViewModel.StudentModel.LanguageKnown,
                            StudentDistrict=registerViewModel.StudentModel.StudentDistrict
                        };
                        var res =await authService.AddStudent(student);
                        return new JsonResult(new {result=true, studentId= res.Id, res.UserID, res.UserName });
                    }
                    else if (registerViewModel.Role == (int)Utils.Enums.Roles.Teacher){
                        var teacher = new Teacher
                        {
                            UserId = user.Id,
                        };
                      new JsonResult(new {result=true, teacher = await authService.AddTeacher(teacher) });
                    }
                    else
                    {
                        new JsonResult(new { result = useresult.Succeeded });
                    }

                }
            }
            else
            {
                return new JsonResult(ModelState.Select(p => p.Value).Where(l => l.Errors.Count > 0));
            }
            return new JsonResult(new { result = false, message = "Something went wrong" });
        }

      
        [HttpGet]
        [AllowAnonymous]
        public async Task<object> ConfirmEmail(string token, int userid)
        {
            return new JsonResult(new { result = authService.EmailConfirmation(token, userid).Result });
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ForgotPassword(string Email)
        {
            var result =  authService.ForgotPassword(Email);
           return new JsonResult(new { result = result.Result });
            
        }
        [HttpGet]
        [AllowAnonymous]
        public object ResetPassword(string Token, string Email)
        {
            if (!string.IsNullOrWhiteSpace(Token.Trim()) && !string.IsNullOrWhiteSpace(Email.Trim()))
            {
                var model = new ResetPasswordViewModel { Email = Email, Token = Token };
                return  true;
            }
            else
            {
                return false;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(AuthorizationModel.IdentityApplicationDefault);
            return Ok();
        }

        public IActionResult IsStudentUserNameExists(string username,int id)
        {
           var isexists= authService.IsStudentUserNameExists(username, id);
            if (isexists)
                return new JsonResult($"This student username has been already taken.  Please try another one.");
            else
                return new JsonResult(true);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ResetPassword(ResetPasswordViewModel model)
        {
            var resut = await authService.ResetPassword(model);
            if (resut.Succeeded)
                return new { status=true};
            else
            {
                return new { result = false, errors = resut.Errors };
            }
        }

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


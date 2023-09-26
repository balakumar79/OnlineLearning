using Auth.Account;
using Learning.Auth;
using Learning.Entities;
using Learning.Entities.Enums;
using Learning.LogMe;
using Learning.Teacher.Services;
using Learning.Tutor.Abstract;
using Learning.Entities.Enums;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly Entities.Config.SecretKey _secretkey;
        readonly ISecurePassword _securePassword;
        readonly ITeacherService _teacherService;
        readonly ILoggerRepo _logger;
        #endregion

        #region ctor
        public AccountController(IAuthService auth, UserManager<AppUser> userManager, ITutorService tutorService, SignInManager<AppUser> signInManager,
           ISecurePassword securePassword, Entities.Config.SecretKey appSet, ITeacherService teacherService, ILoggerRepo logger)
        {
            this._tutorService = tutorService;
            this._userManager = userManager;
            this.authService = auth;
            this._signInManager = signInManager;
            this._securePassword = securePassword;
            _logger = logger;
            _secretkey = appSet;
            _teacherService = teacherService;
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
                    var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(true) ?? await _userManager.FindByEmailAsync(model.UserName).ConfigureAwait(true);
                    var student = await authService.GetStudent(model);

                    IList<string> roles = new List<string>();
                    if (user != null)
                    {
                        roles = await _userManager.GetRolesAsync(user);
                    }
                    if (student != null && !roles.Any())
                        roles.Add(((Roles)student.RoleId).ToString());
                    if (user != null && student == null)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                        //email confirmation
                        if (signInResult.IsNotAllowed)
                            return new JsonResult(new { result = false, message = "Email confirmation required !!! You need to confirm your email to activate your account" });

                        //login unsuccessfull
                        if (!signInResult.Succeeded)
                            return new JsonResult(new { result = false, message = "Incorrect password." });
                        user.LastAccessedOn = DateTime.Now;
                        await _userManager.UpdateAsync(user);
                    }
                    //check if login user is valid appuser
                    if (roles.Any(p => p == Roles.Parent.ToString() || p == Roles.Admin.ToString() || p == Roles.Tutor.ToString()))
                    {
                        var screens = await authService.GetScreenAccessPrivilage(roleId: roles, userID: user.Id);

                        var sessionObj = new SessionObject { User = user, RoleID = roles.ToList(), Tutor = _tutorService.GetTutorProfile(user.Id), Childs = await authService.GetAssociatedStudentsForParent(user.Id) };
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
                            childs = sessionObj.Childs

                        });

                    }
                    else if (roles.Any(p => p == Roles.Teacher.ToString()))
                    {
                        var screens = await authService.GetScreenAccessPrivilage(roleId: roles, userID: user.Id);
                        var teacher = _teacherService.GetTeacher(user.Id).FirstOrDefault();
                        var sessionObj = new SessionObject { User = user, RoleID = roles.ToList(), Tutor = _tutorService.GetTutorProfile(user.Id), Childs = authService.GetAssociatedStudentsForTeacher(user.Id), TeacherId = teacher?.TeacherId };
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
                            childs = sessionObj.Childs

                        });
                    }
                    else if (roles.Contains(Roles.Major.ToString()) || roles.Contains(Roles.Minor.ToString()) ||
                        roles.Contains(Roles.Student.ToString()) || student?.RoleId == (int)Roles.Minor)
                    {

                        if (student == null)
                            return new JsonResult(new { status = false, message = "Sorry. No student account found !!!" });
                        user = await _userManager.FindByIdAsync(student.UserID.ToString());
                        if (!user.EmailConfirmed)
                            return new JsonResult(new { status = false, message = "Sorry.  Your account is not enabled to login.  Please confirm your email to activate your student account." });
                        if (roles.Contains(Roles.Major.ToString()))
                            if (user == null)
                            {
                                _logger.InsertLogger(ErrorEnum.Warning, "No first user account found for the user" + user?.UserName + ".  Student: " + student?.UserName, "User can't login.", "/Account/Login");
                                return new JsonResult(new { result = false, message = "This student account needs a first user account.  Please recreate your account in Account/Register link." });

                            }
                            else
                            {
                                if (!await _userManager.IsEmailConfirmedAsync(user))
                                {
                                    return new JsonResult(new { status = false, message = "Your account is yet to be activatede.  We need your email to be confirmed in order to activate your account.  Please click the link which we sent to your email at the time of registration." });
                                }
                            }


                        var userscreens = await authService.GetScreenAccessPrivilage(student.Id, roles);
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
                                roles = sessionObj.RoleID
                            },
                            token = result.Value
                        });
                    }
                    else
                        return new JsonResult(new { result = false, message = "Invalid username or password entered." });
                }
                catch (Exception ex)
                {
                    _logger.InsertLogger(message: ex.Message, description: ex.ToString(), type: ErrorEnum.Error, link: "/Account/Login");
                    return new JsonResult(new { result = false, error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
                }
            }
            else
            {
                return new JsonResult(ModelState.Select(p => p.Value).Where(o => o.Errors.Count > 0));
            }
        }

        [HttpPost]
        public async Task<object> Register(RegisterViewModel registerViewModel)
        {
            try
            {

                if (authService.IsStudentUserNameExists(registerViewModel.StudentModel?.StudentUserName))
                    return ResponseFormat.JsonResult("Student username already exists.", false);
                AppUser user = null;
                var availableRolesId = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();
                if (!availableRolesId.Contains((Roles)registerViewModel.Role))
                    return ResponseFormat.JsonResult("Invalid user role id.", false);
                if (registerViewModel.StudentModel != null)
                    if (!availableRolesId.Contains((Roles)(registerViewModel.StudentModel?.RoleRequested ?? 0)))
                        return ResponseFormat.JsonResult("Invalid student role id.", false);
                if (ModelState.IsValid)
                {
                    var roleRequested = (Roles)registerViewModel.Role;
                    user = new AppUser
                    {
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName,
                        Email = registerViewModel.Email,
                        PhoneNumber = registerViewModel.PhoneNumber,
                        Gender = registerViewModel.GenderId,
                        UserName = registerViewModel.UserName,
                        District = registerViewModel.District
                    };
                    IdentityResult userresult = null;
                    if (roleRequested != Roles.Minor)
                    {
                        userresult = await authService.AddUser(user, registerViewModel.ConfirmPassword, new AppRole { Name = roleRequested.ToString() });
                        if (!userresult.Succeeded)
                            return ResponseFormat.JsonResult(result: userresult.Succeeded, description: userresult, message: string.Join(" | ", userresult.Errors.Select(s => s.Description)));
                    }
                    else
                        return ResponseFormat.JsonResult("Invalid role Id !!!.  First user cannot be other than Parent.", false);

                    if (registerViewModel.StudentModel != null && (registerViewModel.StudentModel?.RoleRequested == (int)Roles.Major || registerViewModel.StudentModel?.RoleRequested == (int)Roles.Minor) || registerViewModel.StudentModel?.RoleRequested == (int)Roles.Student)
                    {
                        registerViewModel.StudentModel.UserId = user == null ? 0 : user.Id;
                        return new JsonResult(await RegisterStudent(registerViewModel.StudentModel));

                    }
                    else if (registerViewModel.Role == (int)Roles.Teacher)
                    {
                        var teacher = new Entities.Teacher
                        {
                            UserId = user.Id,
                        };
                        return ResponseFormat.JsonResult(await authService.AddTeacher(teacher), "Your teacher account has been created successfully.  Please confirm your email to activate your account.");
                        //return JsonResult(new ResponseFormat { Result = true, Description = await authService.AddTeacher(teacher), Message = "Your teacher account has been created successfully.  Please confirm your email to activate your account." });
                    }
                    else
                    {
                        if (user != null)
                            await _userManager.DeleteAsync(user);
                        return ResponseFormat.JsonResult("No user is registered.  Please check if the role id is correct", false);
                    }
                }
                else
                {
                    return ResponseFormat.JsonResult("Some required field/s are empty.", false, ModelState.Select(p => p.Value).Where(l => l.Errors.Count > 0));
                }
            }
            catch (Exception ex)
            {
                _logger.InsertLogger(ex);
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> RegisterStudent(StudentModel registerViewModel)
        {
            if (!authService.IsStudentUserNameExists(registerViewModel.StudentUserName))
            {
                var student = new Entities.Student
                {
                    FirstName = registerViewModel.StudentFirstName,
                    LastName = registerViewModel.StudentLastName,
                    Grade = registerViewModel.GradeLevels,
                    UserID = registerViewModel.UserId,
                    Password = _securePassword.Secure(_secretkey.StudentSaltKey, registerViewModel.StudentPassword),
                    GenderId = registerViewModel.StudentGenderId,
                    MotherTongue = registerViewModel.MotherTongue,
                    UserName = registerViewModel.StudentUserName,
                    Institution = registerViewModel.Institution,
                    LanguagesKnown = registerViewModel.LanguageKnown,
                    StudentDistrict = registerViewModel.StudentDistrict,
                    RoleId = registerViewModel.RoleRequested,
                };

                var res = await authService.AddStudent(student);
                if (registerViewModel.StudentAccountRecoveryAnswerModel.Any())
                {
                    registerViewModel.StudentAccountRecoveryAnswerModel.ForEach(ans =>
                    {
                        ans.StudentId = res.Id;
                        if (ans.Id == 0)
                            ans.Created = DateTime.Now;
                        ans.Updated = DateTime.Now;
                    });
                    authService.UpserStudentSecretAnswer(registerViewModel.StudentAccountRecoveryAnswerModel);
                }
                if (res == null)
                    return ResponseFormat.JsonResult("Sorry !!! Student registration failed.", false);
                else
                    return ResponseFormat.JsonResult(res, "Student account has been created successfully.  ", description: res);
            }
            else
                return ResponseFormat.JsonResult("Student username already taken.", false);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<object> ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return new JsonResult(new { result = false, message = "Invalid token or email." });
            return new JsonResult(new { result = await authService.EmailConfirmation(token, email) });
        }

        [AllowAnonymous, HttpGet]
        public async Task<object> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return ResponseFormat.JsonResult("Email cannot be blank !!!", false);
            var result = await authService.ForgotPassword(Email);
            return new JsonResult(new { result = result });

        }
        //[HttpGet]
        //[AllowAnonymous]
        //public object ResetPassword(string Token, string Email)
        //{
        //    if (!string.IsNullOrWhiteSpace(Token.Trim()) && !string.IsNullOrWhiteSpace(Email.Trim()))
        //    {
        //        var model = new ResetPasswordViewModel { Email = Email, Token = Token };
        //        return  true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        [HttpGet]
        public object ResetStudentPasswordByStudent(ResetStudentPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var answers = authService.GetStudentAccountRecoveryAnswers(model.UserId).FirstOrDefault(s => s.QuestionId == model.QuestionId);
                if (answers?.Answer == model.Answer)
                {
                    var rows = authService.UpdateStudentPassword(model.UserId, model.Password);
                    if (rows == 1)
                        return new JsonResult(new { status = true, message = "Password updated successfully" });
                    else
                        return new JsonResult(new { status = false, message = "Password update failed !!!.  No use account found." });
                }
                else
                {
                    return new JsonResult(new { status = false, message = "Secret question/answer is not correct !!!  Please try again." });
                }
            }
            return new JsonResult(new { status = false, message = string.Join(" | ", ModelState.Select(s => s.Value).Where(e => e.Errors.Count > 0).Select(g => g.Errors.FirstOrDefault())) });
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(AuthorizationModel.IdentityApplicationDefault);
            return Ok();
        }

        [HttpPost]
        public int SaveStudentSecretAnswers(List<AccountRecoveryAnswerModel> studentAccounts)
        {
            studentAccounts.ForEach(student =>
            {
                if (student.Id == 0)
                    student.Created = DateTime.Now;
                student.Updated = DateTime.Now;
            });
            return authService.UpserStudentSecretAnswer(studentAccounts);
        }

        [HttpGet]
        public IActionResult IsStudentUserNameExists(string username, int id)
        {
            var isexists = authService.IsStudentUserNameExists(username, id);
            if (isexists)
                return new JsonResult($"This student username has been already taken.  Please try another one.");
            else
                return new JsonResult(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ResetPassword(ForgotPasswordViewModel model)
        {
            var resut = await authService.ResetPassword(model);
            if (resut.Succeeded)
                return new { status = true };
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
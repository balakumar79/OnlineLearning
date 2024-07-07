using Learning.Auth;
using Learning.Entities;
using Learning.Entities.Domain;
using Learning.Entities.Enums;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Account;
using Learning.ViewModel.Tutor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace TutorWebUI.Controllers
{
    [Authorize(Roles = "Tutor,Admin")]
    public class TutorController : Controller
    {
        #region Variables

        private readonly ITutorService _tutorService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SessionObject sessionObject;

        #endregion

        #region ctor

        public TutorController(ITutorService tutorService, IHttpContextAccessor contextAccessor, UserManager<AppUser> usermanager)
        {
            _tutorService = tutorService;
            _userManager = usermanager;
            sessionObject = contextAccessor.HttpContext.Session.GetObjectFromJson<SessionObject>("UserObj");
            if (sessionObject == null)
                contextAccessor.HttpContext.RefreshLoginAsync().ConfigureAwait(true);
        }

        #endregion



        #region Tutor

        [Authenticate(Permissions.Tutor.DashBoardView)]
        public async Task<IActionResult> DashboardAsync()
        {
            var model = new TutorDashboardViewModel();
            if (int.TryParse(User.Identity.GetUserID(), out int uid))
                model = await _tutorService.GetTutorDashboardModel(uid);
            return View(model);
        }

        public IActionResult TutorProfile()
        {
            Debug.WriteLine(CultureInfo.CurrentCulture);
            var userid = Convert.ToInt32(User.Identity.GetUserID());
            var model = _tutorService.GetTutorProfile(userid);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model, [FromForm] string currentPassword)
        {
            if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword) && !string.IsNullOrEmpty(currentPassword))
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(User.Identity.GetUserID());
                    var result = await _userManager.ChangePasswordAsync(user, currentPassword, model.Password);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", string.Join(" | ", result.Errors.Select(s => s.Description).ToList()));
                        return View(nameof(TutorProfile));
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            TempData["msg"] = "Your password has been reset successfully !!!";
            return RedirectToAction(nameof(TutorProfile));
        }

        #endregion

        #region Section

        public async Task<IActionResult> PartialTestSectionByTestId(int testid)
        {
            var section = await _tutorService.GetTestSectionByTestId(testid);
            var model = (from sec in section
                         join test in _tutorService.GetTestByUserID(User.Identity.GetTutorId()) on sec.TestId equals test.Id

                         select new TestSectionViewModel
                         {
                             SectionName = sec.SectionName,
                             //SubTopic = sec.SubTopic,
                             AddedQuestions = sec.AddedQuestions,
                             Created = sec.Created,
                             Id = sec.Id,
                             IsOnline = (bool)sec.IsOnline,
                             TestId = sec.TestId,
                             TestName = test.Title,
                             TotalMarks = sec.TotalMarks,
                             TotalQuestions = sec.TotalQuestions
                         }).ToList();
            return Json(model);
        }

        public IActionResult PartialTestSectionBySectionId(int sectionid)
        {
            var model = new TestSectionViewModel();
            if (sectionid > 0)
                model = _tutorService.GetTestSections(sectionid).Select(m => new TestSectionViewModel
                {
                    Id = m.Id,
                    SectionName = m.SectionName,
                    //SubTopic = m.SubTopic,
                    AddedQuestions = m.AddedQuestions,
                    Created = m.Created,
                    IsActive = m.IsActive,
                    IsOnline = (bool)m.IsOnline,
                    Modified = m.Modified,
                    TestId = m.TestId,
                    //Topic = m.Topic,
                    TotalMarks = m.TotalMarks,
                    TotalQuestions = m.TotalQuestions
                }).FirstOrDefault();
            return PartialView(model);
        }

        public IActionResult ManageSections()
        {

            return View();
        }

        public IActionResult CreateSection(string returnUrl = null)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection(TestSectionViewModel model)
        {
            var entity = await _tutorService.CreateTestSection(model);
            if (entity)
                return RedirectToAction(nameof(ManageSections));
            else
            {
                ModelState.AddModelError("", "An unknown error has occurred.");
                return View(model);
            }
        }

        public int SetSectionOnlineStatus(int sectionid, bool status)
        {
            return _tutorService.SetOnlineStatus(sectionid, status);
        }

        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public IActionResult DeleteSection(List<int> id, string rurl)
        {
            var count = _tutorService.DeleteSection(id);
            TempData["msg"] = $"A {count} has been deleted successfully.";
            if (!string.IsNullOrWhiteSpace(rurl))
                return Redirect(rurl);
            else return Json(count);
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetTestSectionByTestId(int testid) => Json(await _tutorService.GetTestSectionByTestId(testid));

        public async Task<IActionResult> IsSectionExists(string SectionName, int? Id)
        {

            var isexists = await _tutorService.IsSectionExists(SectionName, Id);
            if (isexists)
                return Json($"Test title {SectionName} already exists.");
            else
                return Json(true);
        }

        //[AllowAnonymous]
        //public async Task<JsonResult> GetTestSections() => Json( _tutorService.GetTestSections(null));

        #endregion

        #region Subject

        [AllowAnonymous]
        public JsonResult GetSubject()
        {
            return Json(_tutorService.GetTestSubject());
        }

        #endregion

        #region Grade


        [AllowAnonymous]
        public JsonResult GetGradeLevels() => Json(_tutorService.GetGradeLevels());


        #endregion

        #region Topics

        public IActionResult GetTopicsByTestId(int testid)
        {
            return Json(_tutorService.GetTopicsByTestId(testid));
        }

        public IActionResult GetSubTopic(int Id)
        {
            return Json(_tutorService.GetSubTopics(Id));
        }

        #endregion

        #region Language

        public IActionResult GetLanguagesForSubject(int subjectid)
        {
            var result = _tutorService.GetLanguagesForSubject(subjectid);
            return Json(result.Select(d => new { d.Id, d.Name }));
        }

        [AllowAnonymous]
        public List<Language> GetLanguage(int? languageid = null)
        {
            var query = _tutorService.GetLanguages();
            if (languageid != null && languageid != 0)
                query.Where(p => p.Id == languageid);
            return query.ToList();
        }

        #endregion

    }
}

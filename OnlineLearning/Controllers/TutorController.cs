using Learning.Tutor.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Http;
using Learning.Auth;
using Microsoft.AspNetCore.Authorization;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Learning.WebUI.Controllers
{

    public class TutorController : Controller
    {
        private readonly ITutorService _tutorService;
        private SessionObject sessionObject;
        public TutorController(ITutorService tutorService, IHttpContextAccessor contextAccessor)
        {
            this._tutorService = tutorService;
            sessionObject = contextAccessor.HttpContext.Session.GetObjectFromJson<SessionObject>("UserObj");
            //if (sessionObject == null)
            //    contextAccessor.HttpContext.RefreshLoginAsync().ConfigureAwait(true);
        }

        [Authenticate(Permissions.Tutor.DashBoardView)]

        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        public IActionResult Partial_Exams()
        {
            var test = User.Identity.GetTutorId();
            var model = _tutorService.GetTestByUserID(test);
            return PartialView(model);
        }
        [Authenticate(Permissions.Tutor.ViewExams)]
        public IActionResult Exams()
        {
            return View();
        }
        public IActionResult TutorProfile()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _tutorService.GetTutorProfile(Convert.ToInt32(id));

            return View(model);
        }
        public IActionResult CreateTest(int? Id)
        {
            TestViewModel model = null;
            if (Id != null)
                model = _tutorService.GetTestById(Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTest(TestViewModel model)
        {
            model.TutorId = User.Identity.GetTutorId();
            var restul = await _tutorService.TestUpsert(model);
            if (restul > 0)
                return RedirectToAction(nameof(Exams));
            else
            {
                ModelState.AddModelError("", "An unknown error occured while creating the test.");
                return View();
            }
        }

        public async Task<IActionResult> PartialTestSection()
        {
            return PartialView();
        }
        public IActionResult CreateSection(string returnUrl)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSection(TestSectionViewModel model)
        {
            var entity = await _tutorService.CreateTestSection(model);
            if (entity)
                return RedirectToAction(nameof(Dashboard));
            else
            {
                ModelState.AddModelError("", "An unknown error has occurred.");
                return View(model);
            }
        }
        public async Task<IActionResult> CreateQuestion()
        {
            return PartialView();
        }

        public async Task<JsonResult> SaveQuestion(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _tutorService.CreateQuestion(model);
                return Json("ok");
            }
            else
            {
                return Json(ModelState.Keys.SelectMany(p => ModelState[p].Errors)
                    .Select(p=>p.ErrorMessage).ToArray());
            }
        }

        public async Task<IActionResult> DeleteTest(int id)
        {
            await _tutorService.DeleteTest(id);
            return RedirectToAction(nameof(Exams));
        }
        public async Task<JsonResult> GetTest()
        {
            return Json(_tutorService.GetTestByUserID(User.Identity.GetTutorId()));
        }
        public async Task<JsonResult> GetTestSections() => Json(await _tutorService.GetTestSections());
        public async Task<JsonResult> GetSubject()
        {
            return Json(await _tutorService.GetTestSubject());
        }
        public JsonResult GetGradeLevels() => Json(_tutorService.GetGradeLevels());

        public async Task<IActionResult> IsTestExists(string Title, int? Id)
        {

            var isexists = await _tutorService.IsTestExists(Title, Id);
            if (isexists)
                return Json($"Test title {Title} already exists.");
            else
                return Json(true);
        }
        public async Task<IActionResult> IsSectionExists(string SectionName, int? Id)
        {

            var isexists = await _tutorService.IsSectionExists(SectionName, Id);
            if (isexists)
                return Json($"Test title {SectionName} already exists.");
            else
                return Json(true);
        }
        public async Task<JsonResult> GetQuestionType()
        {
            return Json(await _tutorService.GetQuestionTypes());
        }

    }
}

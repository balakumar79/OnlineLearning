using Learning.Admin.Abstract;
using Learning.Auth;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI.Controllers
{
    [Authorize(Roles = Utils.Config.RolesConstant.Admin)]
    public class ManageExamsController : Controller
    {
        #region variables
        readonly ITutorService _tutorService;
        readonly IManageExamService _manageExamService;

        #endregion

        #region ctor
        public ManageExamsController(ITutorService tutorService, IManageExamService manageExamService)
        {
            _tutorService = tutorService;
            _manageExamService = manageExamService;
        }
        #endregion
        // GET: ManageExamsController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Partial_Exams()
        {
            var model = _tutorService.GetAllTest();
            return PartialView(model);
        }


        public IActionResult CreateTest(int? Id)
        {
            TestViewModel model = null;
            model = _tutorService.GetTestById(Id) ?? new TestViewModel();
            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> CreateTest(TestViewModel model)
        {
            model.TutorId = User.Identity.GetTutorId();
            var restul = await _tutorService.TestUpsert(model);
            if (restul > 0)
                return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError("", "An unknown error occured while creating the test.");
                return View();
            }
        }

        public async Task<IActionResult> UpdateTestStatus(int testid, int statusid)
        {
            return Json(await _manageExamService.UpdateTestStatus(testid, statusid));
        }
        public IActionResult UpdateQuestionStatus(int questionId, int statusId)
        {
            return Json(_manageExamService.UpdateQuestionStatus(questionId, statusId));
        }
        public IActionResult DeleteTest(int testid)
        {
            return Json(_tutorService.DeleteTest(testid));
        }
        public IActionResult DeleteQuestion(List<int> questionid)
        {
            return Json(_tutorService.DeleteQuestion(questionid));
        }

        public JsonResult GetQuestionsByTestID(int testid)
        {
            return Json(_tutorService.GetQuestionsByTestId(testid));
        }

        public async Task<JsonResult> GetSubject()
        {
            return Json(await _tutorService.GetTestSubject());
        }
        [AllowAnonymous]
        public JsonResult GetGradeLevels() => Json(_tutorService.GetGradeLevels());

        public async Task<IActionResult> IsTestExists(string Title, int? Id)
        {
            var tutorId = User.Identity.GetTutorId();
            var isexists = await _tutorService.IsTestExists(Title, Id, tutorId);
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
        [AllowAnonymous]
        public async Task<JsonResult> GetQuestionType()
        {
            return Json(await _tutorService.GetQuestionTypes());
        }

        public IActionResult GetTopicsByTestId(int testid)
        {
            return Json(_tutorService.GetTopicsByTestId(testid));
        }

        public IActionResult GetSubTopic(int Id)
        {
            return Json(_tutorService.GetSubTopics(Id));
        }

        public IActionResult GetLanguagesForSubject(int subjectid)
        {
            var result = _tutorService.GetLanguagesForSubject(subjectid);
            return Json(result.Select(d => new { d.Id, d.Name }));
        }

        // GET: ManageExamsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageExamsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageExamsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageExamsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageExamsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageExamsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageExamsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

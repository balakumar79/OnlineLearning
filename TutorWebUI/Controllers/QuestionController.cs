using Learning.Auth;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Learning.TutorWebUI.Controllers
{
    public class QuestionController : Controller
    {


        private readonly ITutorService _tutorService;
        public QuestionController(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        #region Question

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewQuestionsById(int TestId)
        {
            return View();
        }

        public IActionResult Partial_QuestionsByTestId(int testId)
        {
            return PartialView(_tutorService.GetQuestionsByTestId(testId));
        }

        [AllowAnonymous]
        public JsonResult GetQuestionDetails(int QuestionId)
        {

            return Json(_tutorService.GetQuestionDetails(QuestionId));
        }

        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public IActionResult CreateQuestion(int questionId = 0)
        {
            var model = new QuestionViewModel();
            var t = User;
            if (questionId > 0)
            {
                model = _tutorService.GetQuestionDetails(questionId);
            }
            return View(model);
        }

        [Authenticate(Permissions.Tutor.CreateQuestion)]
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
                    .Select(p => p.ErrorMessage).ToArray());
            }
        }

        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public int SetQuestionStatus(int questionid, int status)
        {
            return _tutorService.SetQuestionStatus(questionid, status);
        }

        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public IActionResult DeleteQuestions(List<int> QuestionIds, int TestId)
        {
            _tutorService.DeleteQuestion(QuestionIds);
            TempData["msg"] = "Select question has been deleted successfully.";
            return RedirectToAction(actionName: "ViewQuestionsById", new { TestId = TestId });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetQuestionType()
        {
            return Json(await _tutorService.GetQuestionTypes());
        }

        public JsonResult GetQuestionsByTestID(int testid)
        {
            return Json(_tutorService.GetQuestionsByTestId(testid));
        }

        public JsonResult GetComprehensionQuestion(int testId)
        {
            return Json(_tutorService.GetComprehensionQuestionModels(testId));
        }

        #endregion

    }
}

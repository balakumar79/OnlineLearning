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
using Learning.Entities;
using Learning.ViewModel.Account;
using Learning.ViewModel.Tutor;

namespace TutorWebUI.Controllers
{
    [Authorize(Roles =("Tutor,Admin"))]
    public class TutorController : Controller
    {
        private readonly ITutorService _tutorService;
        private readonly UserManager<AppUser> _userManager;

        //private SessionObject sessionObject;
        public TutorController(ITutorService tutorService, IHttpContextAccessor contextAccessor,UserManager<AppUser> usermanager)
        {
            this._tutorService = tutorService;
            this._userManager = usermanager;
            //sessionObject = contextAccessor.HttpContext.Session.GetObjectFromJson<SessionObject>("UserObj");
            //if (sessionObject == null)
            //    contextAccessor.HttpContext.RefreshLoginAsync().ConfigureAwait(true);
        }

        [Authenticate(Permissions.Tutor.DashBoardView)]

        public async Task<IActionResult> DashboardAsync()
        {
            var model = new TutorDashboardViewModel();
            if(int.TryParse(User.Identity.GetUserID(),out int uid))
            model =await _tutorService.GetTutorDashboardModel(uid);
            return View(model);
        }
        public IActionResult Partial_Exams()
        {
            var userid = User.Identity.GetTutorId();
            var model = _tutorService.GetTestByUserID(userid);
            return PartialView(model);
        }
        [Authenticate(Permissions.Tutor.ViewExams)]
        public IActionResult Exams()
        {
            return View();
        }
        public IActionResult TutorProfile()
        {
            var userid = Convert.ToInt32(User.Identity.GetUserID());
            var model = _tutorService.GetTutorProfile(userid);

            return View(model);
        }
        [Authenticate(Permissions.Tutor.CreateTest)]
        public IActionResult CreateTest(int? Id)
        {
            TestViewModel model = null;
                model = _tutorService.GetTestById(Id)??new TestViewModel();
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

        public async Task<IActionResult> PartialTestSectionByTestId(int testid)
        {
            var section =await _tutorService.GetTestSectionByTestId(testid);
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
            if (sectionid >0)
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
        public IActionResult CreateSection(string returnUrl=null)
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
                    .Select(p=>p.ErrorMessage).ToArray());
            }
        }

        [AllowAnonymous]
        public List<Language> GetLanguage(int ? languageid = null)
        {
            var query = _tutorService.GetLanguages();
            if (languageid != null && languageid != 0)
                query.Where(p => p.Id == languageid);
            return query.ToList();
        }

        public async Task<IActionResult> DeleteTest(int id)
        {
            await _tutorService.DeleteTest(id);
            return RedirectToAction(nameof(Exams));
        }
        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public int SetQuestionStatus(int questionid, int status)
        {
           return _tutorService.SetQuestionStatus(questionid, status);
        }
        public int SetSectionOnlineStatus(int sectionid,bool status)
        {
            return _tutorService.SetOnlineStatus(sectionid, status);
        }
        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public async Task<IActionResult> SetExamIsPublishedAsync(int examid,bool isChecked)
        {
            return Json(await _tutorService.SetTestIsPublished(examid, isChecked));
        }
        [Authenticate(Permissions.Tutor.CreateQuestion)]

        public IActionResult DeleteQuestions(List<int> QuestionIds,int TestId)
        {
            _tutorService.DeleteQuestion(QuestionIds);
            TempData["msg"] = "Select question has been deleted successfully.";
            return RedirectToAction(actionName: "ViewQuestionsById", new { TestId = TestId });
        }
        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public IActionResult DeleteSection(List<int> id,string rurl)
        {
           var count= _tutorService.DeleteSection(id);
            TempData["msg"] = $"A {count} has been deleted successfully.";
            if (!string.IsNullOrWhiteSpace(rurl))
                return Redirect(rurl);
            else return Json(count);
        }

        public JsonResult GetTest()
        {
            return Json( _tutorService.GetTestByUserID(User.Identity.GetTutorId()));
        }
        //[AllowAnonymous]
        //public async Task<JsonResult> GetTestSections() => Json( _tutorService.GetTestSections(null));
        [AllowAnonymous]
        public async Task<JsonResult> GetTestSectionByTestId(int testid) => Json(await _tutorService.GetTestSectionByTestId(testid));
        [AllowAnonymous]
        public async Task<JsonResult> GetSubject()
        {
            return Json(await _tutorService.GetTestSubject());
        }
        [AllowAnonymous]
        public JsonResult GetGradeLevels() => Json(_tutorService.GetGradeLevels());

        public async Task<IActionResult> IsTestExists(string Title, int? Id)
        {
            var tutorId = User.Identity.GetTutorId();
            var isexists = await _tutorService.IsTestExists(Title, Id,tutorId);
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

        public JsonResult GetQuestionsByTestID(int testid)
        {
           return Json( _tutorService.GetQuestionsByTestId(testid));
        }

         public JsonResult GetComprehensionQuestion(int testId)
        {
            return Json(_tutorService.GetComprehensionQuestionModels(testId));
        }
         [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model,[FromForm]string currentPassword)
        {
            if (!string.IsNullOrEmpty(model.Password)&&!string.IsNullOrEmpty(model.ConfirmPassword)&&!string.IsNullOrEmpty(currentPassword))
            {
                try
                {
                    var user =await _userManager.FindByIdAsync(User.Identity.GetUserID());
                  var result=await  _userManager.ChangePasswordAsync(user, currentPassword, model.Password);
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
            return Json(result.Select(d=>new {d.Id,d.Name }));
        }

    }
}

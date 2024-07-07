using Learning.Auth;
using Learning.Entities;
using Learning.Entities.Domain;
using Learning.Entities.Enums;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Learning.TutorWebUI.Controllers
{
    public class ExamController : Controller
    {

        #region Variables

        private readonly ITutorService _tutorService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SessionObject sessionObject;

        #endregion

        #region ctor

        public ExamController(ITutorService tutorService, IHttpContextAccessor contextAccessor, UserManager<AppUser> usermanager)
        {
            _tutorService = tutorService;
            _userManager = usermanager;
            sessionObject = contextAccessor.HttpContext.Session.GetObjectFromJson<SessionObject>("UserObj");
            if (sessionObject == null)
                contextAccessor.HttpContext.RefreshLoginAsync().ConfigureAwait(true);
        }

        #endregion

        [Route("Exam")]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Exams));
        }

        #region Exam

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

        public JsonResult GetTestData(PaginationQuery query)
        {
            var userid = User.Identity.GetTutorId();
            var result = _tutorService.GetTestByUserID(userid, query);
            return Json(result);
        }

        [Authenticate(Permissions.Tutor.CreateTest)]
        public IActionResult CreateTest(int? Id)
        {
            TestViewModel model = _tutorService.GetTestById(Id) ?? new TestViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest(TestViewModel model)
        {
            model.CreatedBy = User.Identity.GetTutorId();
            if (model.CreatedBy == 0)
            {
                ModelState.AddModelError("AccessDenied", "Invalid User.  Access Denied");
                return View(model);
            }

            model.RoleId = ((int)Roles.Tutor);
            var restul = await _tutorService.TestUpsert(model);
            if (restul > 0)
                return RedirectToAction(nameof(Exams));
            else
            {
                ModelState.AddModelError("", "An unknown error occured while creating the test.");
                return View();
            }
        }

        public async Task<IActionResult> DeleteTest(int id)
        {
            await _tutorService.DeleteTest(id);
            return RedirectToAction(nameof(Exams));
        }

        [Authenticate(Permissions.Tutor.CreateQuestion)]
        public async Task<IActionResult> SetExamIsPublishedAsync(int examid, bool isChecked)
        {
            return Json(await _tutorService.SetTestIsPublished(examid, isChecked));
        }

        public JsonResult GetTest()
        {
            return Json(_tutorService.GetTestByUserID(User.Identity.GetTutorId()));
        }

        public async Task<IActionResult> IsTestExists(string Title, int? Id)
        {
            var tutorId = User.Identity.GetTutorId();
            var isexists = await _tutorService.IsTestExists(Title, Id, tutorId);
            if (isexists)
                return Json($"Test title {Title} already exists.");
            else
                return Json(true);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learning.Admin.Abstract;
using Learning.Auth;
using Learning.Tutor.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Learning.Admin.WebUI.Controllers
{
    [Authorize(Roles =Utils.Config.RolesConstant.Admin)]
    public class ManageExamsController : Controller
    {
        #region variables
        readonly ITutorService _tutorService;
        readonly IManageExamService _manageExamService;

        #endregion

        #region ctor
        public ManageExamsController(ITutorService tutorService,IManageExamService manageExamService )
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
            var userid = User.Identity.GetTutorId();
            var model = _tutorService.GetAllTest();
            return PartialView(model);
        }

        public async Task<IActionResult> UpdateTestStatus(int testid, int statusid)
        {
           return Json(await _manageExamService.UpdateTestStatus(testid, statusid));
        }
        public IActionResult UpdateQuestionStatus(int questionId,int statusId)
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

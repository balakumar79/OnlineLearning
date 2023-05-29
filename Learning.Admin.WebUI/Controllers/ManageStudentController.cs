using Learning.Admin.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Admin.WebUI.Controllers
{
    public class ManageStudentController : Controller
    {
        readonly IManageStudentService _manageStudentService;
        public ManageStudentController(IManageStudentService manageStudentService)
        {
            _manageStudentService = manageStudentService;
        }
        // GET: ManageStudentController
        public ActionResult Index()
        {
            var model = _manageStudentService.GetStudents();
            return View(model);
        }

        // GET: ManageStudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageStudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageStudentController/Create
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

        // GET: ManageStudentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageStudentController/Edit/5
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

        // GET: ManageStudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageStudentController/Delete/5
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

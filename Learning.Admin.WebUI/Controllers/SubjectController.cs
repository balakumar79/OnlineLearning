using Learning.Admin.Abstract;
using Learning.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI.Controllers
{
    public class SubjectController : Controller
    {
        private readonly IManageSubjectService _manageSubjectService ;
        public SubjectController(IManageSubjectService manageSubjectService)
        {
            _manageSubjectService = manageSubjectService;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(SubjectList));
        }
        public IActionResult SubjectList()
        {
            var model = _manageSubjectService.GetSubjectViewModels();
            return View(model); ;
        }
        public IActionResult Subject (int id){ return View(_manageSubjectService.GetSubjectViewModels(id).FirstOrDefault()); }
        [HttpPost]
        public IActionResult Subject(SubjectViewModel subjectViewModel)
        {
            _manageSubjectService.InsertSubjectLanguageVariant(subjectViewModel);
            TempData["msg"] = "Subject updated successfully !!!";
            return RedirectToAction(nameof(SubjectList));
        }
    }
}

using Learning.Admin.Abstract;
using Learning.Admin.WebUI.Models;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI.Controllers
{
    //[Authorize(ViewModel.Account.AuthorizationModel.Permissions.Administrator.Admin)]
    [Authorize(Roles = Entities.Config.RolesConstant.Admin)]
    public class ManageTutorController : Controller
    {
        private readonly ILogger<ManageTutorController> _logger;
        private readonly IManageTutorService _manageTutor;
        public ManageTutorController(ILogger<ManageTutorController> logger, IManageTutorService manageTutor)
        {
            this._manageTutor = manageTutor;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _manageTutor.GetAllTutors());
        }
        public IActionResult RegisterTutor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterTutor(TutorViewModel model)
        {

            var result = await _manageTutor.CreateTutor(model);
            if (result.Succeeded)
                return RedirectToAction("Tutors");
            else
            {
                foreach (var item in result.Errors)
                {

                    ModelState.AddModelError(item.Code, item.Description);
                }
                    return View(model);
            }
        }
        public IActionResult Tutors()
        {
            return View();
        }
        public async Task<IActionResult> Partial_Tutor()
        {
          return PartialView(await _manageTutor.GetAllTutors());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}

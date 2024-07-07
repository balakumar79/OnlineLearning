using Learning.Admin.Abstract;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI.Controllers
{
    public class ManageTeacherController : Controller
    {
        private readonly IManageTeacherService _manageTeacherService;
        public ManageTeacherController(IManageTeacherService teacherService)
        {
          _manageTeacherService= teacherService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _manageTeacherService.GetTeacherModels());
        }

    }
}

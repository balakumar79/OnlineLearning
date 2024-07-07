using Learning.Admin.Abstract;
using Learning.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Admin.WebUI.Controllers
{
    public class ManageParentController : Controller
    {
        private readonly IManageParentService _manageParentService;
        public ManageParentController(IManageParentService manageParentService)
        {
            _manageParentService = manageParentService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetParentsData(PaginationQuery paginationQuery)
        {
            return Json(_manageParentService.GetParents(paginationQuery));
        }
    }
}

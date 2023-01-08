using Learning.Admin.Abstract;
using Learning.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI.Controllers
{
    [Authorize(Roles = ViewModel.Account.AuthorizationModel.Permissions.Roles.Admin)]
    public class DashboardController : Controller
    {
        public readonly IManageExamService _manageExamService;
        public DashboardController(IManageExamService manageExamService)
        {
            _manageExamService = manageExamService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var model =await _manageExamService.GetDashboardModel(Convert.ToInt32(User.Identity.GetUserID()));
            return View(model);
        }

    }
}

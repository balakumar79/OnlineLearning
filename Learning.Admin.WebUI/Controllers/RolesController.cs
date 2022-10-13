using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.Student.Abstract;
using Learning.Teacher.Services;
using Learning.Tutor.Abstract;
using Learning.Utils;
using Learning.Utils.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Admin.WebUI.Controllers
{
    public class RolesController : Controller
    {
        readonly IManageTutorService _manageTutorService;
        readonly ITutorService _tutorService;
        readonly IStudentService _studentService;
        readonly ITeacherService _teacherService;
        readonly UserManager<AppUser> userManager;
        public RolesController(IManageTutorService manageTutorService)
        {
            _manageTutorService = manageTutorService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DeleteUser(int id)
        {
            return Json(_manageTutorService.DeleteUser(id));
        }
    }
}

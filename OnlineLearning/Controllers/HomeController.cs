using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineLearning.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLearning.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            
            var us = User;
            return View();
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

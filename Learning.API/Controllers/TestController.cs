using Learning.Tutor.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Learning.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private ITutorService _tutorService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, ITutorService tutorService)
        {
            _logger = logger;
            _tutorService = tutorService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var user = User;
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route("/Home/Index")]
        public IActionResult Index()
        {
            return Redirect("/swagger/index.html");
        }
        [HttpGet]
        [Route("/test/GetGradeLevelsByLanguages")]
        public IActionResult GetGradesByLanguages([FromQuery] int[] Languages)
        {
            return ResponseFormat.JsonResult(_tutorService.GetGradeLevelsByLanguages(Languages));
        }

        [HttpGet]
        [Route("/test/GetSubjectsByGrades")]
        public IActionResult GetSubjectsByGrades([FromQuery] int[] Grades)
        {
            return ResponseFormat.JsonResult(_tutorService.GetSubjectsByGrades(Grades));
        }
    }
}

using Learning.Auth;
using Learning.Entities;
using Learning.Student;
using Learning.Student.Abstract;
using Learning.Tutor.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Learning.API.Controllers
{
    
    
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]/{id?}")]

    public class StudentTestController : ControllerBase
    {
        #region variables
        readonly IStudentTestService _studentService;
        readonly ITutorService _tutorService;
        #endregion

        #region ctor
        public StudentTestController(IStudentTestService studentService, ITutorService tutorService)
        {
            _tutorService = tutorService;
            this._studentService = studentService;
        }
        #endregion
        #region methods
        [HttpGet]
        public IEnumerable<StudentTestViewModel> GetStudentTest()
        {
            var userid = Convert.ToInt32(HttpContext.User.Identity.GetUserID());
            return _studentService.GetTestByUserID(userid).ToList();
        }

        [HttpPost]
        public JsonResult SaveStudentAnswerLog(StudentAnswerLog log) => new JsonResult(new { rows = _studentService.InsertStudentAnswerLog(log) });

        [HttpPost]
        public JsonResult SaveStudentTest(StudentTestHistory model)
        {
            var studentTestHistory = _studentService.InsertStudentTestResult(model);
            return new JsonResult(new { rows = studentTestHistory });
        }


        [HttpGet]
        public JsonResult GetTest(int? testid)
        {
            if (testid == null)
                return new JsonResult(Ok(new { test = _tutorService.GetAllTest() }));
            else
                return new JsonResult(Ok(new { test = _tutorService.GetTestById(testid) }));

        }

        [HttpGet]
        public JsonResult GetQuestionsByQuestionId(int questionid)
        {
            return new JsonResult(new { Question = _tutorService.GetQuestionDetails(questionid) });
        }
        [HttpGet]
        public JsonResult GetQuestionsByTestId(int testid)
        {
            return new JsonResult(new { Questions = _tutorService.GetQuestionsByTestId(testid) });
        }
        public async Task<JsonResult> GetSubjects(int? subjectId)
        {
            var sub = await _tutorService.GetTestSubject();
            return new JsonResult(new { subject = subjectId == null ? sub : sub.Where(o => o.Id == subjectId) });

        }
        public async Task<JsonResult> GetTestSectionByTestId(int testid) => new JsonResult(Ok(new {sections=(await _tutorService.GetTestSectionByTestId(testid))}));
        public JsonResult GetGradeLevels(int? gradeid)
        {
            return new JsonResult(Ok(new { grades = gradeid == null ? _tutorService.GetGradeLevels() : _tutorService.GetGradeLevels().Where(s => s.Id == gradeid) }));
        }


        [AllowAnonymous]
        public JsonResult GetLoggedInUser()
        {
            return new JsonResult(new {user= User.Identity,TokenBasedUser= HttpContext.Items["User"] });
        }
        //// POST api/<Test>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Test>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Test>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        #endregion
    }
}

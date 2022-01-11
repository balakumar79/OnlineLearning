using Learning.Auth;
using Learning.Entities;
using Learning.Student;
using Learning.Student.Abstract;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

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
           List<int> userids =new List<int>();
            if (User.IsInRole(Utils.Enums.Roles.Parent.ToString()))
                userids = User.Identity.GetChildIds();
            if(User.IsInRole(Utils.Enums.Roles.Major.ToString())||User.IsInRole(Utils.Enums.Roles.Minor.ToString()))
            userids =new List<int> { Convert.ToInt32(HttpContext.User.Identity.GetUserID()) };
            return _studentService.GetStudentTestByStudentID(userids).ToList();
        }


        [HttpPost]
        [Authenticate(Permissions.Student.Examination)]
        public JsonResult SaveStudentAnswerLog(StudentAnswerLog log)
        { 
            log.StudentId=Convert.ToInt32(User.Identity.GetStudentId());
            return new JsonResult(new { studentAnswerLogId = _studentService.InsertStudentAnswerLog(log) });
        }

        [HttpPost]
        public JsonResult SaveStudentTest(StudentTest studentTest)
        {
            try
            {
                if (User.IsInRole(Utils.Enums.Roles.Minor.ToString()) || User.IsInRole(Utils.Enums.Roles.Major.ToString()))
                    studentTest.StudentId = Convert.ToInt32(User.Identity.GetStudentId());
                else
                {
                    if (studentTest.StudentId == 0)
                        return new JsonResult(new { studentTestId = 0, error = "Please enter the valid student id/s to proceed" });
                    var studentids = User.Identity.GetChildIds().Where(p=>p.ToString().Contains(studentTest.StudentId.ToString()));
                    if (!studentids.Any())
                        return new JsonResult(new { studentTestId = 0, error = "Invalid student id/s." });
                }
                var studentstats = new StudentTestStats
                {
                    Testid = studentTest.TestId,
                    //total registration 1 is passed because to consider this entity (StudentTestStats) as new registration 
                    TotalRegistration = 1,
                    MaximumMarkScored = 0,
                    MinimumMarkScored = 0,
                    UpdatedAt=DateTime.Now
                };
                _studentService.UpsertStudentTestStats(studentstats);
                return new JsonResult(new { studentTestId = _studentService.InsertStudentTest(studentTest) });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveStudentTestHistoryAsync(StudentTestHistory model)
        {
            try
            {
                model.StudentId = Convert.ToInt32(User.Identity.GetStudentId());
                var studenttest = _studentService.GetStudentTests(model.StudentTestId);
                var studentstats = new StudentTestStats
                {
                   MinimumMarkScored=model.Score,
                   MaximumMarkScored=model.Score,
                   Testid=studenttest.TestId,
                };
                studentstats.UpdatedAt = DateTime.Now;
                var studentTestHistory = _studentService.InsertStudentTestResult(model);
                _studentService.UpsertStudentTestStats(studentstats);
                return new JsonResult(new { studentTestHistoryId = studentTestHistory });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
         [HttpPost]
         public JsonResult SaveStudentCalculatedResult(CalculatedResult result)
        {
            result.StudentId = Convert.ToInt32(User.Identity.GetStudentId());
            return new JsonResult(new { studentCalculatedResultId = _studentService.InsertCalculatedResults(result) });
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
        public JsonResult GetQuestionsByTestId(int testid, int? groupby)
        {
            if (testid > 0 )

            {

                switch (groupby)
                {
                    case 1:
                        var query = _tutorService.GetQuestionsByTestId(testid).OrderBy(s=>s.SectionId);
                        var result = new List<object>();
                        query.Select(s => s.SectionId).Distinct().ToList().ForEach(section =>
                            {
                                result.Add(new { section = section, Questions = query.Where(s => s.SectionId == section).ToList() });
                            });
                        
                        return new JsonResult(new { Questions = result });

                    default:
                        return new JsonResult(new { Questions = _tutorService.GetQuestionsByTestId(testid) });
                }
            }
            else
            {
                return new JsonResult(new { Questions = new QuestionViewModel() });
            }

        }
        public JsonResult GetQuestionsByTestId(List<int> testIds,int?groupby)
        {
            if (testIds.Any())

            {

                switch (groupby)
                {
                    case 1:
                        var query = _tutorService.GetQuestionsByTestId(testIds).OrderBy(s => s.SectionId);
                        var result = new List<object>();
                        query.Select(s => s.SectionId).Distinct().ToList().ForEach(section =>
                        {
                            result.Add(new { section = section, Questions = query.Where(s => s.SectionId == section).ToList() });
                        });

                        return new JsonResult(new { Questions = result });

                    default:
                        return new JsonResult(new { Questions = _tutorService.GetQuestionsByTestId(testIds) });
                }
            }
            else
            {
                return new JsonResult(new { Questions = new QuestionViewModel() });
            }
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
        [AllowAnonymous, HttpGet]
        public JsonResult GetGradeWiseSubjects(string gradeids)
        {
            var grades = new List<int>();
            if (!string.IsNullOrEmpty(gradeids))
                gradeids.Split(",").ToList().ConvertAll(int.Parse);
            return new JsonResult(new { subjects = _studentService.GetTestSubjectViewModels(grades) });
        }
        public JsonResult GetSubjectWiseGrades(string subjects)
        {
            var subjectIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(subjects))
                subjectIds.AddRange(subjects.Split(",").ToList().ConvertAll(int.Parse));
            return new JsonResult(_studentService.TestGradeViewModels(subjectIds));
        }
        [HttpPost]
        public JsonResult UpdateStudentTestStatus(StudentTestStatusPartialModel[] partialModels)
        {
           return new JsonResult(_studentService.UpdateTestStatus(partialModels.ToList()));
        }                                     
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

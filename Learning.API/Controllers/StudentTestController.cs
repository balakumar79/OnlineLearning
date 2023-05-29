using Learning.Auth;
using Learning.Entities;
using Learning.Student;
using Learning.Student.Abstract;
using Learning.Student.ViewModel;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Learning.API.Controllers
{


    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]

    public class StudentTestController : ControllerBase
    {
        #region variables
        readonly IStudentService _studentService;
        readonly ITutorService _tutorService;
        #endregion

        #region ctor
        public StudentTestController(IStudentService studentService, ITutorService tutorService)
        {
            _tutorService = tutorService;
            this._studentService = studentService;
        }
        #endregion

        #region methods
        [HttpGet]
        public IEnumerable<StudentTestViewModel> GetStudentTest()
        {
            List<int> userids = new List<int>();
            if (User.IsInRole(Utils.Enums.Roles.Parent.ToString()))
                userids = User.Identity.GetChildIds();
            if (User.IsInRole(Utils.Enums.Roles.Major.ToString()) || User.IsInRole(Utils.Enums.Roles.Minor.ToString()))
                userids = new List<int> { Convert.ToInt32(HttpContext.User.Identity.GetUserID()) };
            return _studentService.GetStudentTestByStudentIDs(userids).OrderByDescending(p => p.Modified).ToList();
        }


        [HttpPost]
        [Authenticate(Permissions.Student.Examination)]
        public JsonResult SaveStudentAnswerLog(StudentAnswerLog log)
        {
            log.StudentId = Convert.ToInt32(User.Identity.GetStudentId());
            return new JsonResult(new { studentAnswerLogId = _studentService.InsertStudentAnswerLog(log) });
        }

        [HttpPost]
        public JsonResult SaveStudentTest(List<StudentTest> studentTests)
        {
            try
            {
                foreach (var studentTest in studentTests)
                {


                    if (User.IsInRole(Utils.Enums.Roles.Minor.ToString()) || User.IsInRole(Utils.Enums.Roles.Major.ToString()))
                        studentTest.StudentId = Convert.ToInt32(User.Identity.GetStudentId());
                    else
                    {
                        if (studentTest.StudentId == 0)
                            return new JsonResult(new { studentTestId = 0, error = "Please enter the valid student id/s to proceed" });
                        var studentids = User.Identity.GetChildIds().Where(p => p.ToString().Contains(studentTest.StudentId.ToString()));
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
                        UpdatedAt = DateTime.Now
                    };
                    _studentService.UpsertStudentTestStats(studentstats);
                }
                return new JsonResult(new { studentTestId = _studentService.InsertStudentTest(studentTests) });
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
                if (model.StudentId == 0)
                    return ResponseFormat.JsonResult("No student account has been found.", false);
                var studenttest = _studentService.GetStudentTests(model.StudentTestId);
                var studentstats = new StudentTestStats
                {
                    MinimumMarkScored = model.Score,
                    MaximumMarkScored = model.Score,
                    Testid = studenttest.TestId,
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
        [AllowAnonymous, Authorize]
        public JsonResult GetTest(int? testid = 0, bool isTutorviewOnly = false)
        {
            if ((User.IsInRole(Utils.Enums.Roles.Tutor.ToString()) || User.IsInRole(Utils.Enums.Roles.Teacher.ToString())) && isTutorviewOnly)
            {
                if (User.Identity.GetTutorId() > 0)
                    return new JsonResult(Ok(new { test = _tutorService.GetTestByUserID(User.Identity.GetTutorId()) }));
                if (User.Identity.GetTeacherId() > 0)
                    return new JsonResult(Ok(new { test = _tutorService.GetTestByUserID(User.Identity.GetTeacherId()) }));
            }
            if (testid == 0)
                return new JsonResult(Ok(new
                {
                    test = _studentService.GetAllTest(Convert.ToInt32(User.Identity.GetStudentId())).OrderByDescending(s => s.SubjectID == 4)
                    .ThenByDescending(t => t.Created).ThenByDescending(t => t.Modified).ToList()
                }));
            else
                return new JsonResult(Ok(new { test = _studentService.GetTestById(testid) }));

        }

        [HttpGet]
        public JsonResult GetQuestionsByQuestionId(int questionid)
        {
            return new JsonResult(new { Question = _studentService.GetQuestionDetails(questionid) });
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetQuestionsByTestId(int testid, int? groupby)
        {
            if (testid > 0)

            {

                switch (groupby)
                {
                    case 1:
                        var query = _studentService.GetQuestionsByTestId(testid).OrderBy(s => s.SectionId);
                        var result = new List<object>();
                        query.Select(s => s.SectionId).Distinct().ToList().ForEach(section =>
                            {
                                result.Add(new { section = section, Questions = query.Where(s => s.SectionId == section).ToList() });
                            });

                        return new JsonResult(new { Questions = result });

                    default:
                        return new JsonResult(new { Questions = _studentService.GetQuestionsByTestId(testid) });
                }
            }
            else
            {
                return new JsonResult(new { Questions = new QuestionViewModel() });
            }

        }

        [AllowAnonymous, HttpGet]
        public JsonResult GetQuestionsByTestIds(List<int> testIds, int? groupby)
        {
            if (testIds.Any())

            {

                switch (groupby)
                {
                    case 1:
                        var query = _studentService.GetQuestionsByTestId(testIds).OrderBy(s => s.SectionId);
                        var result = new List<object>();
                        query.Select(s => s.SectionId).Distinct().ToList().ForEach(section =>
                        {
                            result.Add(new { section = section, Questions = query.Where(s => s.SectionId == section).ToList() });
                        });

                        return new JsonResult(new { Questions = result });

                    default:
                        return new JsonResult(new { Questions = _studentService.GetQuestionsByTestId(testIds) });
                }
            }
            else
            {
                return new JsonResult(new { Questions = new QuestionViewModel() });
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetSubjects(int? subjectId)
        {
            var sub = await _tutorService.GetTestSubject();
            return new JsonResult(new { subject = subjectId == null ? sub : sub.Where(o => o.Id == subjectId) });

        }
        [HttpGet]
        public async Task<JsonResult> GetTestSectionByTestId(int testid) => new JsonResult(Ok(new { sections = (await _tutorService.GetTestSectionByTestId(testid)) }));
        [HttpGet]
        public JsonResult GetGradeLevels(int? gradeid)
        {
            return new JsonResult(Ok(new { grades = gradeid == null ? _tutorService.GetGradeLevels() : _tutorService.GetGradeLevels().Where(s => s.Id == gradeid) }));
        }
        [AllowAnonymous, HttpGet]
        public JsonResult GetGradeWiseSubjects(string gradeids)
        {
            var grades = new List<int>();
            if (!string.IsNullOrEmpty(gradeids))
                grades.AddRange(gradeids.Split(",").ToList().ConvertAll(int.Parse));
            return new JsonResult(new { subjects = _studentService.GetTestSubjectViewModels(grades).OrderBy(s => s.Language) });
        }

        [AllowAnonymous, HttpGet]
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
        [AllowAnonymous, HttpGet]
        public JsonResult GetLoggedInUser()
        {
            return ResponseFormat.JsonResult(new { User.Identity, TokenBasedUser = HttpContext.Items["User"] });
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<bool> SendEmailAsync(SendEmailRequestModel requestModel)
        {
            if (requestModel.FormFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    requestModel.FormFile.CopyTo(ms);
                    return await _studentService.SendEmailAsync(requestModel.toEmail, requestModel.Subject, requestModel.Body, requestModel.CC, ms, requestModel.FileName);
                }
            }
            else
            {
                return await _studentService.SendEmailAsync(requestModel.toEmail, requestModel.Subject, requestModel.Body, requestModel.CC);
            }
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

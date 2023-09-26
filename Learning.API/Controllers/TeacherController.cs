using Learning.Auth;
using Learning.Entities;
using Learning.Entities.Enums;
using Learning.LogMe;
using Learning.Student.Abstract;
using Learning.Teacher.Services;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Learning.Entities;
using Learning.Entities.Config;
using Learning.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.API.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Tutor,Major")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class TeacherController : ControllerBase
    {
        readonly IStudentService _studentService;
        readonly ITeacherService _teacherService;
        readonly UserManager<AppUser> _userManager;
        readonly IEmailService _emailService;
        readonly EncryptionKey _encryptionKey;
        readonly ILoggerRepo _logger;


        readonly ITutorService _tutorService;
        public TeacherController(IStudentService studentTestService, ITutorService tutorService, IEmailService emailService, ILoggerRepo logger, UserManager<AppUser> userManager, ITeacherService teacherService, EncryptionKey encryptionKey)
        {
            _studentService = studentTestService;
            _teacherService = teacherService;
            _tutorService = tutorService;
            _emailService = emailService;
            _userManager = userManager;
            _encryptionKey = encryptionKey;
            _logger = logger;
        }
        [AcceptVerbs("Post", "GET")]
        public JsonResult SearchStudent(StudentSearchRequest searchRequest)
        {
            return new JsonResult(new
            {
                students = _teacherService.SearchStudent(searchRequest.FirstName, searchRequest.LastName, searchRequest.UserName,
               searchRequest.Gender, searchRequest.Grades, searchRequest.Districts, searchRequest.Institution, User.Identity.GetTeacherId())
            });
        }

        [HttpGet]
        public async Task<JsonResult> SendStudentInvitationAsync(int studentId, int TeacherId)
        {
            try
            {
                var student = _studentService.GetStudentByStudentId(studentId);
                if (student == null)
                    return ResponseFormat.JsonResult("No student found.", false);
                var appUser = await _userManager.FindByIdAsync(student.UserID.ToString());
                if (appUser == null && student.RoleId != (int)Roles.Major)
                    return ResponseFormat.JsonResult("No parent account found !!!", false);
                var existinginvite = _teacherService.GetStudentInvitations(new List<int> { studentId }, 2).Where(s => s.TeacherId == TeacherId);
                if (existinginvite.Any())
                    return ResponseFormat.JsonResult("Invite already sent !!!", false);

                var body = await _emailService.GetEmailTemplateContent(EmailTemplate.SendStudentInvitationLinkByTeacher);
                var invite = _teacherService.StudentInvitationUpsert(new StudentInvitation
                {
                    Parentid = appUser.Id,
                    TeacherId = TeacherId,
                    StudentId = studentId,
                    SentOn = DateTime.Now,
                    AcceptedOn = null,
                    Response = 0
                });
                var idInBytes = Encoding.UTF8.GetBytes(CommonData.EncryptString(invite.Id.ToString(), _encryptionKey.Key));
                var idEncoded = WebEncoders.Base64UrlEncode(idInBytes);
                body = body.Replace("$Teacher", User.Identity.Name)
                    .Replace("$ParentFirstName", appUser.FirstName)
                    .Replace("$ParentLastName", appUser.LastName)
                    .Replace("$StudentId", studentId.ToString())
                    .Replace("$StudentId", studentId.ToString())
                      .Replace("$Id", idEncoded)
                      .Replace("$Id", idEncoded)
                    .Replace("$Link", "https://domockexam.com/#/studentinvitationresponse")
                    .Replace("$Link", "https://domockexam.com/#/studentinvitationresponse");
                var email = appUser?.Email ?? student.UserName;
                await _emailService.SendStudentInvitationLink(email, body);
                return ResponseFormat.JsonResult(response: "Invitation send successfully !!!");
            }
            catch (Exception ex)
            {
                _logger.InsertLogger(ex);
                return ResponseFormat.JsonResult(ex.InnerException == null ? ex.Message : ex.InnerException.Message, false);
            }
        }
        [AllowAnonymous, HttpGet]
        public JsonResult StudentInvitationResponse([FromQuery] string id, int res)
        {
            var idInBytes = WebEncoders.Base64UrlDecode(id);
            var idDecoded = Encoding.UTF8.GetString(idInBytes);
            if (int.TryParse(CommonData.DecryptString(idDecoded, _encryptionKey.Key), out int idValue))
            {
                var invite = _teacherService.GetStudentInvitations(new List<int> { idValue }, 4).FirstOrDefault();
                if (invite == null)
                    return ResponseFormat.JsonResult("No invitation found !!!", false);

                invite.Response = res;
                invite.AcceptedOn = DateTime.Now;
                _teacherService.StudentInvitationUpsert(invite);
                return ResponseFormat.JsonResult("Response saved successfully.", true);
            }
            else
                return ResponseFormat.JsonResult("No invite found.", false);

        }

        [HttpPost]
        public async Task<JsonResult> SaveRandomTest(TestViewModel model)
        {
            var userid = 0;
            if (model.TopicId == 0)
                return ResponseFormat.JsonResult(false, "TopicId is Required !!!");
            if (model.SubTopicId == 0)
                return ResponseFormat.JsonResult(false, "SubTopicId is Required !!!");
            if (model.RoleId == 0)
                return ResponseFormat.JsonResult(false, "RoleId is Required !!!");
            if (User.IsInRole(Roles.Teacher.ToString()))
            {
                if (User.Identity.GetTeacherId() > 0)
                    userid = User.Identity.GetTeacherId();
            }
            else if (User.IsInRole(Roles.Parent.ToString()) && userid == 0)
                userid = Convert.ToInt32(User.Identity.GetUserID());
            else if (User.IsInRole(Roles.Tutor.ToString()) && userid == 0)
                userid = User.Identity.GetTutorId();
            var result = await _teacherService.RandomTestUpsert(userid, model.Title, model.SubjectID, model.TopicId, model.SubTopicId, model.RoleId, model.GradeID, model.Language, model.StartDate, model.EndDate, model.Duration, model.PassingMark,
                model.Description, model.Id);

            return ResponseFormat.JsonResult(

                result: result > 0,
                message: result == -1 ? "Test name already exists !!!" : null,
                description: result
            );
        }

        [HttpGet]
        public IActionResult GenerateRandomQuestions(int testid, int randomNumber)
        {
            return ResponseFormat.JsonResult(_teacherService.GenerateRandomQuestions(testid, randomNumber).ToList());
        }
    }
}

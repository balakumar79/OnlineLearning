using Learning.Auth;
using Learning.Entities;
using Learning.Student.Abstract;
using Learning.Teacher.Services;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.Abstract;
using Learning.Utils;
using Learning.Utils.Config;
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
    [Authorize(Roles = "Admin,Teacher,Tutor")]
    [ApiController]
    [Route("[controller]/[action]/{id?}")]
    public class TeacherController : ControllerBase
    {
        readonly IStudentService _studentService;
        readonly ITeacherService _teacherService;
        readonly UserManager<AppUser> _userManager;
        readonly IEmailService _emailService;
        readonly EncryptionKey  _encryptionKey;
        readonly LoggerRepo _logger;

        readonly ITutorService _tutorService;
        public TeacherController(IStudentService studentTestService,ITutorService tutorService,IEmailService emailService,LoggerRepo logger,UserManager<AppUser> userManager,ITeacherService teacherService,EncryptionKey encryptionKey)
        {
            _studentService = studentTestService;
            _teacherService = teacherService;
            _tutorService = tutorService;
            _emailService = emailService;
            _userManager = userManager;
            _encryptionKey = encryptionKey;
            _logger = logger;
        }
             [AcceptVerbs("Post","GET")]
        public JsonResult SearchStudent(StudentSearchRequest searchRequest)
        {
            return new JsonResult(new
            {
                students = _teacherService.SearchStudent(searchRequest.FirstName, searchRequest.LastName, searchRequest.UserName,
               searchRequest.Gender, searchRequest.Grades, searchRequest.Districts, searchRequest.Institution, User.Identity.GetTeacherId())
            });
        }

        public async Task<JsonResult> SendStudentInvitationAsync(int studentId,int TeacherId)
        {
            try
            {
                var student = _studentService.GetStudentByStudentId(studentId);
                if (student == null)
                    return new JsonResult(new ResponseFormat { Result = false, Message = "No student found." });
                var appUser= await _userManager.FindByIdAsync(student.UserID.ToString());
                if (appUser == null&&student.RoleId!=(int)Utils.Enums.Roles.Major)
                    return new JsonResult(new ResponseFormat { Result = false, Message = "No parent account found !!!" });
                var existinginvite = _teacherService.GetStudentInvitations(new List<int> { studentId }, 2).Where(s=>s.TeacherId==TeacherId);
                if (existinginvite.Any())
                    return new JsonResult(new ResponseFormat { Result = false, Message = "Invite already sent !!!" });

                var body = await _emailService.GetEmailTemplateContent(Utils.Config.EmailTemplate.SendStudentInvitationLinkByTeacher);
                var invite = _teacherService.StudentInvitationUpsert(new StudentInvitation
                {
                    Parentid = appUser.Id,
                    TeacherId = TeacherId,
                    StudentId = studentId,
                    SentOn = DateTime.Now,
                    AcceptedOn=null,
                    Response = 0
                });
                var idInBytes = Encoding.UTF8.GetBytes(CommonData.EncryptString(invite.Id.ToString(), _encryptionKey.Key));
                var idEncoded = WebEncoders.Base64UrlEncode(idInBytes);
                body = body.Replace("$Teacher", User.Identity.Name)
                    .Replace("$ParentFirstName", appUser.FirstName)
                    .Replace("$ParentLastName", appUser.LastName)
                    .Replace("$StudentId",studentId.ToString())
                    .Replace("$StudentId",studentId.ToString())
                      .Replace("$Id",idEncoded)
                      .Replace("$Id",idEncoded)
                    .Replace("$Link", "https://domockexam.com/#/studentinvitationresponse")
                    .Replace("$Link", "https://domockexam.com/#/studentinvitationresponse");
                var email = appUser?.Email ?? student.UserName;  
                await _emailService.SendStudentInvitationLink(email, body);
                return new JsonResult(new ResponseFormat { Result = true, Message = "Invitation send successfully !!!" });
            }
            catch (Exception ex)
            {
                _logger.InsertLogger(ex);                                                                                                                                          
                return new JsonResult(new ResponseFormat { Result = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
              [AllowAnonymous]
        public JsonResult StudentInvitationResponse([FromQuery] string id, int res)
        {
            var idInBytes = WebEncoders.Base64UrlDecode(id);
            var idDecoded = Encoding.UTF8.GetString(idInBytes);
            if (int.TryParse(CommonData.DecryptString(idDecoded,_encryptionKey.Key), out int idValue))
            {
                var invite = _teacherService.GetStudentInvitations(new List<int> { idValue }, 4).FirstOrDefault();
                if (invite == null)
                    return new JsonResult(new ResponseFormat { Message = "No invitation found !!!", Result = false });
                
                invite.Response = res;
                invite.AcceptedOn = DateTime.Now;
                _teacherService.StudentInvitationUpsert(invite);
                return new JsonResult(new ResponseFormat { Message = "Response saved successfully.", Result = true });
            }
            else
                return new JsonResult(new ResponseFormat { Message = "No invite found.", Result = true });

        }

    }
}

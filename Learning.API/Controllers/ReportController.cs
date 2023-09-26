using Learning.Auth;
using Learning.Student.Abstract;
using Learning.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]/{id?}")]
    public class ReportController : ControllerBase
    {
        readonly IStudentService _studentService;
        public ReportController(IStudentService studentTestService)
        {
            _studentService = studentTestService;

        }
        [HttpGet]
       public object GetStudentTestReport(int ? filterId=0, int? filterValue=0,DateTime ? fromDate=null,DateTime ? toDate=null)
        {

            var userids = new List<int>();
            if (User.IsInRole(Entities.Enums.Roles.Minor.ToString()) || User.IsInRole(Entities.Enums.Roles.Major.ToString()))
                userids.Add(Convert.ToInt32(User.Identity.GetStudentId()));
            else
                userids.AddRange(User.Identity.GetChildIds());
            var reports = _studentService.GetStudentTestByStudentIDs(userids);
            switch (filterId)
            {
                case 1:
                    reports = reports.Where(p => p.TestId == filterValue).ToList();
                    break;
                case 2:
                    reports = reports.Where(p => p.StudentId == filterValue).ToList();
                    break;
                case 3:
                    reports = reports.Where(p => p.Assigner == filterValue).ToList();
                    break;
                case 4:
                    reports = reports.Where(p => p.SubjectID == filterValue).ToList();
                    break;
                case 5:
                    reports = reports.Where(p => p.StatusID == filterValue).ToList();
                    break;
                case 6:
                    reports = reports.Where(p => p.Id == filterValue).ToList();
                    break;
                case 7:
                    reports = reports.Where(p => p.StartDate > fromDate && p.EndDate < toDate).ToList();
                    break;
                default:
                    break;
            }
           
            return new JsonResult(Ok(new { report =reports.OrderByDescending(s=>s.Modified)  }));
        }
        [HttpGet]
        public object GetCalculatedResults(int ? filterId=0,int ? filterValue = 0)
        {
            var studentid = User.Identity.GetStudentId();
            var result = _studentService.GetCalculatedResults(Convert.ToInt32(studentid));
            var filter = (SearchFilterEnums)filterId;
            switch (filter)
            {
                case SearchFilterEnums.Testid:
                    result = result.Where(p => p.StudentId == filterValue).ToList();
                    break;
                case SearchFilterEnums.StudentId:
                    result = result.Where(o => o.StudentId == filterValue).ToList();
                    break;
                case SearchFilterEnums.Id:
                    result = result.Where(k => k.Id == filterValue).ToList();
                    break;
                default:
                    break;
            }
            return new JsonResult(Ok(result));
        }
        
    }
}

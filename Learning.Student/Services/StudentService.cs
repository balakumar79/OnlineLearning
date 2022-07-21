using Learning.Entities;
using Learning.Student.Abstract;
using Learning.Student.ViewModel;
using Learning.Tutor.ViewModel;
using Learning.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Student.Services
{
    public class StudentService : IStudentService
    {
        readonly IStudentRepo _studentTestRepo;
        readonly IEmailService _emailService;
        public StudentService(IStudentRepo studentTestRepo,IEmailService emailService)
        {
            _studentTestRepo = studentTestRepo;
            _emailService = emailService;
        }

        public List<StudentTestViewModel> GetAllStudentTest()
        {
            return _studentTestRepo.GetAllStudentTest();
        }

        public StudentTest GetStudentTests(int studenttestid = 0)
        {
            return _studentTestRepo.GetStudentTests(studenttestid);
        }
        public List<StudentTest> GetStudentTests(List<int> testIds)
        {
            return _studentTestRepo.GetStudentTests(testIds);
        }

        public List<StudentTestViewModel> GetStudentTestByStudentID(List<int> studentIds)
        {
            return _studentTestRepo.GetStudentTestByStudentIDs(studentIds);
        }

        public int InsertStudentTest(StudentTest studentTest)
        {
            return _studentTestRepo.InsertStudentTest(studentTest);
        }
        public List<int> InsertStudentTest(List<StudentTest> studentTest)
        {
            return _studentTestRepo.InsertStudentTest(studentTest);
        }

        public int InsertStudentAnswerLog(StudentAnswerLog log)
        {
            return _studentTestRepo.InsertStudentAnswerLog(log);
        }

        public int InsertStudentTestResult(StudentTestHistory studentTestHistory)
        {
            return _studentTestRepo.InsertStudentTestResult(studentTestHistory);
        }

        public int InsertCalculatedResults(CalculatedResult result)
        {
            return _studentTestRepo.InsertCalculatedResults(result);
        }

        public List<CalculatedResult> GetCalculatedResults(int studentid)
        {
            return _studentTestRepo.GetCalculatedResults(studentid);
        }
        public int UpsertStudentTestStats(StudentTestStats stats)
        {
            return _studentTestRepo.UpsertStudentTestStats(stats);
        }
        public StudentTestStats GetStudentTestStatsByTestid(int testid)
        {
            return _studentTestRepo.GetStudentTestStatsByTestid(testid);
        }
        public List<Languages> GetTestSubjectViewModels(List<int> gradeIds = null)
        {
            return _studentTestRepo.GetTestSubjectViewModels(gradeIds);
        }
        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject)
        {
            return _studentTestRepo.TestGradeViewModels(subject);
        }
        public int UpdateTestStatus(List<StudentTestStatusPartialModel> statusPartialModels)
        {
            return _studentTestRepo.UpdateTestStatus(statusPartialModels);
        }

        public TestViewModel GetTestById(int? id)
        {
            return _studentTestRepo.GetTestById(id);
        }

        public List<TestViewModel> GetAllTest()
        {
            return _studentTestRepo.GetAllTest();
        }

        public List<QuestionViewModel> GetQuestionsByTestId(List<int> TestId)
        {
            return _studentTestRepo.GetQuestionsByTestId(TestId);
        }

        public List<QuestionViewModel> GetQuestionsByTestId(int TestId)
        {
            return _studentTestRepo.GetQuestionsByTestId(TestId);
        }

        public List<StudentTestViewModel> GetStudentTestByStudentIDs(List<int> studentid)
        {
            return _studentTestRepo.GetStudentTestByStudentIDs(studentid);
        }
       public QuestionViewModel GetQuestionDetails(int QuestionId)
        {
            return _studentTestRepo.GetQuestionDetails(QuestionId);
        }
        public async Task<bool> SendEmailAsync(string toEmail,string subject, string body,List<string> ? cc=null,System.IO.Stream ? stream=null,string filename=null)
        {
           return await _emailService.SendEmailAsync(toEmail, subject, body, cc, stream,filename);
        }

        public Entities.Student GetStudentByStudentId(int studentId)
        {
            return _studentTestRepo.GetStudentByStudentId(studentId);
        }
        public AppUser GetParentByStudentId(int studentId)
        {
            return _studentTestRepo.GetParentByStudentId(studentId);
        }
    }
}

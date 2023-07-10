using Learning.Entities;
using Learning.Student.ViewModel;
using Learning.Tutor.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learning.Student.Abstract
{
    public interface IStudentService
    {
        public TestViewModel GetTestById(int? id);
        Task<List<TestViewModel>> GetAllTest(int? studentId = 0);
        List<QuestionViewModel> GetQuestionsByTestId(List<int> TestId);
        List<QuestionViewModel> GetQuestionsByTestId(int TestId);
        List<StudentTestViewModel> GetStudentTestByStudentIDs(List<int> studentid);
        QuestionViewModel GetQuestionDetails(int QuestionId);
        int InsertStudentTest(StudentTest studentTest);
        List<int> InsertStudentTest(List<StudentTest> studentTests);
        int InsertStudentAnswerLog(StudentAnswerLog log);
        int InsertStudentTestResult(StudentTestHistory studentTestHistory);
        int InsertCalculatedResults(CalculatedResult result);
        StudentTest GetStudentTests(int studenttestid = 0);
        List<StudentTest> GetStudentTests(List<int> studenttestid);
        List<CalculatedResult> GetCalculatedResults(int studentid);
        public StudentTestStats GetStudentTestStatsByTestid(int testid);
        /// <summary>
        /// Insert or update student stats.  If total registration passed as 1 means new registration for the exam.
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public int UpsertStudentTestStats(StudentTestStats stats);
        public List<Languages> GetTestSubjectViewModels(List<int> gradeIds = null);
        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject);
        public int UpdateTestStatus(List<StudentTestStatusPartialModel> statusPartialModels);
        Task<bool> SendEmailAsync(string toEmail, string subject, string body, List<string>? cc = null, System.IO.Stream? stream = null, string filename = null);
        Entities.Student GetStudentByStudentId(int studentId);
        AppUser GetParentByStudentId(int studentId);
    }
}

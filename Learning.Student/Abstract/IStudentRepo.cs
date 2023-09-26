using Learning.Entities;
using Learning.Entities.Domain;
using Learning.Student.ViewModel;
using Learning.Tutor.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learning.Student.Abstract
{
    public interface IStudentRepo
    {
        Task<List<TestViewModel>> GetAllTest(PaginationQuery pagination, int? studentId = 0, int subjectId = 0, int gradeId = 0);
        TestViewModel GetTestById(int? id);
        List<QuestionViewModel> GetQuestionsByTestId(int TestId);
        List<QuestionViewModel> GetQuestionsByTestId(List<int> TestId);
        List<StudentTestViewModel> GetStudentTestByStudentIDs(List<int> studentid);
        QuestionViewModel GetQuestionDetails(int QuestionId);
        int InsertStudentTest(StudentTest studentTest);
        List<int> InsertStudentTest(List<StudentTest> studentTests);
        int InsertStudentAnswerLog(StudentAnswerLog log);
        int InsertStudentTestResult(StudentTestHistory studentTestHistory);
        int InsertCalculatedResults(CalculatedResult result);
        List<StudentTestViewModel> GetAllStudentTest();
        StudentTest GetStudentTests(int studenttestid = 0);
        List<CalculatedResult> GetCalculatedResults(int studentid);
        int UpsertStudentTestStats(StudentTestStats stats);
        public StudentTestStats GetStudentTestStatsByTestid(int testid);
        public List<StudentTest> GetStudentTests(List<int> studenttestid);
        public List<Languages> GetTestSubjectViewModels(List<int> gradeIds = null);
        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject);
        public Entities.Student GetStudentByStudentId(int studentId);
        public int UpdateTestStatus(List<StudentTestStatusPartialModel> statusPartialModels);
        public AppUser GetParentByStudentId(int studentId);
    }
}

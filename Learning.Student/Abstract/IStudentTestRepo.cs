using Learning.Entities;
using Learning.Student.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Abstract
{
   public interface IStudentTestRepo
    {
        List<StudentTestViewModel> GetStudentTestByStudentIDs(List<int> studentid);
        int InsertStudentTest(StudentTest studentTest);
        int InsertStudentAnswerLog(StudentAnswerLog log);
        int InsertStudentTestResult(StudentTestHistory studentTestHistory);
        int InsertCalculatedResults(CalculatedResult result);
        List<StudentTestViewModel> GetAllStudentTest();
        StudentTest GetStudentTests(int studenttestid = 0);
        List<CalculatedResult> GetCalculatedResults(int studentid);
        int UpsertStudentTestStats(StudentTestStats stats);
       public StudentTestStats GetStudentTestStatsByTestid(int testid);
        public List<StudentTest> GetStudentTests(List<int> studenttestid);
       public List<TestSubjectViewModel> GetTestSubjectViewModels(List<int> gradeIds = null);
       public List<TestGradeViewModel> TestGradeViewModels(List<int> subject);

        public int UpdateTestStatus(List<StudentTestStatusPartialModel> statusPartialModels);
    }
}

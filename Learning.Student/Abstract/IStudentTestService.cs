using Learning.Entities;
using Learning.Student.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Abstract
{
    public interface IStudentTestService
    {
        List<StudentTestViewModel> GetStudentTestByStudentID(List<int> studentid);
        int InsertStudentTest(StudentTest studentTest);
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
       public List<TestSubjectViewModel> GetTestSubjectViewModels(List<int> gradeIds = null);
        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject);
       public int UpdateTestStatus(List<StudentTestStatusPartialModel> statusPartialModels);
    }
}

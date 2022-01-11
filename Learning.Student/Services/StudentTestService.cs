using Learning.Entities;
using Learning.Student.Abstract;
using Learning.Student.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Services
{
    public class StudentTestService : IStudentTestService
    {
        readonly IStudentTestRepo _studentTestRepo;
        public StudentTestService(IStudentTestRepo studentTestRepo)
        {
            _studentTestRepo = studentTestRepo;
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
        public List<TestSubjectViewModel> GetTestSubjectViewModels(List<int> gradeIds = null)
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

    }
}

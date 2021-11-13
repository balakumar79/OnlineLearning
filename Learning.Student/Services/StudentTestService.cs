using Learning.Entities;
using Learning.Student.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Services
{
   public class StudentTestService:IStudentTestService
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

        public List<StudentTest> GetStudentTests(int studenttestid = 0)
        {
            return _studentTestRepo.GetStudentTests(studenttestid);
        }

        public List<StudentTestViewModel> GetTestByUserID(int userid)
        {
            return _studentTestRepo.GetTestByUserID(userid);
        }

        public int InsertStudentAnswerLog(StudentAnswerLog log)
        {
            return _studentTestRepo.InsertStudentAnswerLog(log);
        }

        public int InsertStudentTestResult(StudentTestHistory studentTestHistory)
        {
            return _studentTestRepo.InsertStudentTestResult(studentTestHistory);
        }
    }
}

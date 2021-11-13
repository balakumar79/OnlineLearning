using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Abstract
{
    public interface IStudentTestService
    {
        List<StudentTestViewModel> GetTestByUserID(int userid);
        int InsertStudentAnswerLog(StudentAnswerLog log);
        int InsertStudentTestResult(StudentTestHistory studentTestHistory);
        List<StudentTestViewModel> GetAllStudentTest();
        List<StudentTest> GetStudentTests(int studenttestid = 0);
    }
}

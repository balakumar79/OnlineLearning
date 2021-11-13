using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Learning.Student.Abstract;

namespace Learning.Student.Repos
{
  
    public class StudentTestRepo:IStudentTestRepo
    {
        #region variables
        readonly AppDBContext _dBContext;
        #endregion

        #region ctor
        public StudentTestRepo(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        #endregion

        public List<StudentTestViewModel> GetTestByUserID(int userid)
        {
           
                return (from test in _dBContext.Tests
                        join studentTest in _dBContext.StudentTests on test.Id equals studentTest.TestId
                        join subject in _dBContext.TestSubjects on test.SubjectID equals subject.Id
                        join grade in _dBContext.GradeLevels on test.GradeID equals grade.Id
                        where studentTest.StudentId == userid
                        select new StudentTestViewModel
                        {
                            StatusID = test.StatusID,
                            StartDate = test.StartDate,
                            SubjectID = test.SubjectID,
                            SubjectName = subject.SubjectName,
                            SubTopics = test.SubTopics,
                            AssignedOn = studentTest.AssignedOn,
                            Assigner = studentTest.Assigner,
                            Created = test.Created,
                            Description = test.TestDescription,
                            Duration = test.Duration,
                            EndDate = test.EndDate,
                            GradeID = test.GradeID,
                            Id = studentTest.Id,
                            Language = test.Language,
                            GradeName = grade.Grade,
                            Modified = test.Modified,
                            TestId = test.Id,
                            Title = test.Title,
                            StudentId = studentTest.StudentId,
                            StudentTestHistories = _dBContext.StudentTestHistories.Where(p => p.Id == studentTest.Id).ToList()
                        }).ToList();

           
        }

        public List<StudentTestViewModel> GetAllStudentTest()
        {

            return (from test in _dBContext.Tests
                    join studentTest in _dBContext.StudentTests on test.Id equals studentTest.TestId
                    join subject in _dBContext.TestSubjects on test.SubjectID equals subject.Id
                    join grade in _dBContext.GradeLevels on test.GradeID equals grade.Id
                    select new StudentTestViewModel
                    {
                        StatusID = test.StatusID,
                        StartDate = test.StartDate,
                        SubjectID = test.SubjectID,
                        SubjectName = subject.SubjectName,
                        SubTopics = test.SubTopics,
                        AssignedOn = studentTest.AssignedOn,
                        Assigner = studentTest.Assigner,
                        Created = test.Created,
                        Description = test.TestDescription,
                        Duration = test.Duration,
                        EndDate = test.EndDate,
                        GradeID = test.GradeID,
                        Id = studentTest.Id,
                        Language = test.Language,
                        GradeName = grade.Grade,
                        Modified = test.Modified,
                        TestId = test.Id,
                        Title = test.Title,
                        StudentId = studentTest.StudentId,
                        StudentTestHistories = _dBContext.StudentTestHistories.Where(p => p.Id == studentTest.Id).ToList()
                    }).ToList();


        }

        public int InsertStudentAnswerLog(StudentAnswerLog log)
        {
            try
            {
                _dBContext.StudentAnswerLogs.Add(log);
                return _dBContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int InsertStudentTestResult(StudentTestHistory studentTestHistory)
        {
            try
            {
                if (studentTestHistory != null)
                {
                    var db = _dBContext.StudentTestHistories.Add(studentTestHistory);
                    return _dBContext.SaveChanges();
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<StudentTest> GetStudentTests(int studenttestid = 0)
        {
            if (studenttestid > 0)
            {
                return _dBContext.StudentTests.Where(s => s.Id == studenttestid).ToList();
            }
            else
                return _dBContext.StudentTests.ToList();
        }

    }
}

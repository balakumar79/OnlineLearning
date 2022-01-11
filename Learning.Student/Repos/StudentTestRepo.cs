using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Learning.Student.Abstract;
using Learning.Utils.Enums;
using Learning.Student.ViewModel;

namespace Learning.Student.Repos
{

    public class StudentTestRepo : IStudentTestRepo
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
        /// <summary>
        /// Get student test by parentid/userid
        /// </summary>
        /// <param name="userid">ParentId/UserId</param>
        /// <returns></returns>
        public List<StudentTestViewModel> GetStudentTestByStudentIDs(List<int> studentid)
        {
            return (from test in _dBContext.Tests
                    join studentTest in _dBContext.StudentTests on test.Id equals studentTest.TestId
                    join subject in _dBContext.TestSubjects on test.SubjectID equals subject.Id
                    join grade in _dBContext.GradeLevels on test.GradeID equals grade.Id
                    join student in _dBContext.Students on studentTest.StudentId equals student.Id
                    join g_studentstats in _dBContext.StudentTestStats on test.Id equals g_studentstats.Testid into grpstats
                    from  studentstats in grpstats.DefaultIfEmpty()
                    where  studentid.Contains(studentTest.StudentId)

                    select new StudentTestViewModel
                    {
                        StatusID = test.StatusID,
                        StartDate = studentTest.StartDate,
                        SubjectID = test.SubjectID,
                        SubjectName = subject.SubjectName,
                        SubTopics = test.SubTopics,
                        AssignedOn = studentTest.AssignedOn,
                        Assigner = studentTest.Assigner,
                        Active = studentTest.Active,
                        Created = test.Created,
                        Description = test.TestDescription,
                        Duration = test.Duration,
                        StatusName = ((StudentTestStatus)studentTest.StatusId).ToString(),
                        EndDate = studentTest.EndDate,
                        Topics = test.Topics,
                        TutorId = studentTest.Assigner.ToString(),
                        GradeID = test.GradeID,
                        Id = studentTest.Id,
                        Language = test.Language,
                        GradeName = grade.Grade,
                        Modified = test.Modified,
                        TestId = test.Id,
                        Title = test.Title,
                        StudentTestStats=studentstats,
                        StudentId = studentTest.StudentId,
                        StudentTestHistories = _dBContext.StudentTestHistories.Where(p => studentid.Contains(p.StudentId) && p.StudentTestId == studentTest.Id).ToList()
                    }).ToList();
        }

        public int UpsertStudentTestStats(StudentTestStats stats)
        {
            var db = _dBContext.StudentTestStats.FirstOrDefault(s => s.Testid == stats.Testid);
            if (db == null)
            {
                stats.CreatedAt = DateTime.Now;
                _dBContext.StudentTestStats.Add(stats);
            }
            else
            {
                db.NumberOfAttempts += 1;
                db.UpdatedAt = DateTime.Now;
                db.AverageMarkScored = (db.AverageMarkScored + stats.MaximumMarkScored) / (db.NumberOfAttempts);
                if (stats.MaximumMarkScored > db.MaximumMarkScored && stats.MaximumMarkScored > 0 && stats.TotalRegistration == 0)
                    db.MaximumMarkScored = stats.MaximumMarkScored;
                if (stats.MinimumMarkScored < db.MinimumMarkScored && stats.MinimumMarkScored > 0 && stats.TotalRegistration == 0)
                    db.MinimumMarkScored = stats.MinimumMarkScored;
                _dBContext.StudentTestStats.Update(db);
            }
            _dBContext.SaveChanges();
            return stats.Id;
        }
        public StudentTestStats GetStudentTestStatsByTestid(int testid) => _dBContext.StudentTestStats.FirstOrDefault(p => p.Testid == testid);

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
        public int InsertStudentTest(StudentTest studentTest)
        {
            _dBContext.StudentTests.Add(studentTest);
             _dBContext.SaveChanges();
            return studentTest.Id;
        }

        public int InsertStudentAnswerLog(StudentAnswerLog log)
        {
            try
            {
                _dBContext.StudentAnswerLogs.Add(log);
                _dBContext.SaveChanges();
                return log.LogId;
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
                     _dBContext.SaveChanges();
                    return studentTestHistory.Id;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int InsertCalculatedResults(CalculatedResult result)
        {
            _dBContext.CalculatedResults.Add(result);
            _dBContext.SaveChanges();
            return result.Id;
        }
        public List<CalculatedResult> GetCalculatedResults(int studentid)
        {
            return _dBContext.CalculatedResults.ToList();
        }
        public StudentTest GetStudentTests(int studenttestid = 0)
        {
            if (studenttestid > 0)
            {
                return _dBContext.StudentTests.FirstOrDefault(s => s.Id == studenttestid);
            }
            else
                return new StudentTest();
        }
        public List<StudentTest> GetStudentTests()
        {
            return _dBContext.StudentTests.ToList();
        }
        public List<StudentTest> GetStudentTestsByTestIds(List<int> testids)
        {
            if (testids.Any())
                return _dBContext.StudentTests.Where(p => testids.Contains(p.TestId)).ToList();
            else
                return new List<StudentTest>();
        }
        public List<StudentTest> GetStudentTests(List<int> studenttestid)
        {
            if (studenttestid.Any())
            {
                return _dBContext.StudentTests.Where(s => studenttestid.Contains(s.TestId)).ToList();
            }
            else
                return new List<StudentTest>();
        }
        public List<TestSubjectViewModel> GetTestSubjectViewModels(List<int> gradeIds = null)
        {
            gradeIds = gradeIds ?? new List<int>();
            var model = new List<TestSubjectViewModel>();


            var subjects = _dBContext.TestSubjects.ToList();
            if (gradeIds.Any())
                subjects = subjects.Where(g => gradeIds.Contains(g.Id)).ToList();
            subjects.ToList().ForEach(subj =>
             {
                 var test = _dBContext.Tests.Where(p => p.SubjectID == subj.Id);
                 var grades = test.Select(g => g.GradeID).ToList();
                 model.Add(new TestSubjectViewModel { GradeLevels = _dBContext.GradeLevels.Where(g => grades.Contains(g.Id)).ToList(), SubjectId = subj.Id, SubjectName = subj.SubjectName });

             });
            return model;

        }
        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject)
        {
            subject = subject ?? new List<int>();
            var model = new List<TestGradeViewModel>();
            var grades = _dBContext.GradeLevels.ToList();
            grades.ForEach(grade =>
            {
                var test = _dBContext.Tests.Where(t => t.GradeID == grade.Id);
                if (subject.Any())
                    test = test.Where(s => subject.Contains(s.Id));
                var subjectIds = test.Select(t => t.SubjectID).Distinct();
                model.Add(new TestGradeViewModel
                {
                    GradeId = grade.Id,
                    TestSubjects = _dBContext.TestSubjects.Where(sub => subjectIds.Contains(sub.Id)).ToList(),
                    GradeName = grade.Grade
                });
            });
            return model;
        }

        public int UpdateTestStatus(List<StudentTestStatusPartialModel> statusPartialModels)
        {
            var db = _dBContext.StudentTests.Where(p => statusPartialModels.Select(s => s.StudentTestId).Contains(p.Id));    
            statusPartialModels.ForEach(model =>
             {
                 var studentTest = db.FirstOrDefault(g => g.Id == model.StudentTestId);
                 if (studentTest != null)
                 {
                     studentTest.StatusId = model.StatusId;
                     studentTest.StartDate = DateTime.Now;
                 }
             });
            _dBContext.StudentTests.UpdateRange(db);
            return _dBContext.SaveChanges();
        }
    }
}

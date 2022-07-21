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

    public class StudentRepo : IStudentRepo
    {
        #region variables
        readonly AppDBContext _dBContext;
        #endregion

        #region ctor
        public StudentRepo(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        #endregion

        public TestViewModel GetTestById(int? id)
        {
            return _dBContext.Tests.Where(p => p.Id == id&&p.StatusID==3&&p.IsActive).Select(p => new TestViewModel
            {
                Id = p.Id,
                Created = p.Created,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                StatusID = p.StatusID,
                SubjectID = p.SubjectID,
                //SubTopics = p.SubTopics,
                Duration = p.Duration,
                Description = p.TestDescription,
                Language = p.Language,
                GradeID = p.GradeID,
                Modified = p.Modified,
                Title = p.Title,
                TutorId = p.TutorId,
                //Topics = p.Topics
            }).FirstOrDefault();
        }
        public List<TestViewModel> GetAllTest()
        {
            return (from test in _dBContext.Tests
                    join sub in _dBContext.TestSubjects on test.SubjectID equals sub.Id
                    join teststatus in _dBContext.TestStatuses on test.StatusID equals teststatus.Id
                    join grade in _dBContext.GradeLevels on test.GradeID equals grade.Id
                    where test.StatusID == 3 &&test.IsActive
                    select new TestViewModel
                    {
                        Created = test.Created,
                        SubjectName = sub.SubjectName,
                        StatusID = test.StatusID,
                        StatusName = teststatus.Status,
                        Duration = test.Duration,
                        StartDate = test.StartDate,
                        EndDate = test.EndDate,
                        GradeID = test.GradeID,
                        GradeName = grade.Grade,
                        Language = test.Language,
                        Id = test.Id,
                        Modified = test.Modified,
                        Title = test.Title,
                        TutorId = test.TutorId,
                    }).ToList();
        }
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
                        //SubTopics = test.SubTopics,
                        AssignedOn = studentTest.AssignedOn,
                        Assigner = studentTest.Assigner,
                        Active = studentTest.Active,
                        Created = test.Created,
                        Description = test.TestDescription,
                        Duration = test.Duration,
                        StatusName = ((StudentTestStatus)studentTest.StatusId).ToString(),
                        EndDate = studentTest.EndDate,
                        //Topics = test.Topics,
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
        public List<QuestionViewModel> GetQuestionsByTestId(int TestId)
        {
            var model = (from qus in _dBContext.Questions.Where(p => p.TestId == TestId)
                         join status in _dBContext.TestStatuses on qus.StatusId equals status.Id
                         join test in _dBContext.Tests on qus.TestId equals test.Id
                         join qustype in _dBContext.QuestionTypes on qus.QusType equals qustype.Id
                         where qus.StatusId==3 && test.StatusID==3
                         select new QuestionViewModel
                         {
                             TestSection = _dBContext.TestSections.Where(s => s.Id == qus.SectionId).Select(sec => new TestSectionViewModel
                             {
                                 SectionName = sec.SectionName ?? "N/A",
                                 SubTopic = qus.SubTopics ?? "N/A",
                                 AddedQuestions = sec.AddedQuestions,
                                 AdditionalInstruction = sec.AdditionalInstruction,
                                 Created = sec.Created,
                                 Id = sec.Id,
                                 TestId = sec.TestId,
                                 IsActive = sec.IsActive,
                                 IsOnline = sec.IsOnline,
                                 Modified = sec.Modified,
                                 TestName = test.Title,
                                 Topic = qus.Topics,
                                 TotalMarks = sec.TotalMarks,
                                 TotalQuestions = sec.TotalQuestions
                             }).FirstOrDefault() ?? new TestSectionViewModel
                             {
                                 SectionName = "No section",
                                 Id = 0,
                             },
                             SectionId = qus.SectionId,
                             Created = qus.Created,
                             Modified = qus.Modified,
                             QuestionName = qus.QuestionName,
                             QusID = qus.QusID,
                             StatusId = qus.StatusId,
                             Mark = qus.Mark,
                             TestName = test.Title,
                             TestId = test.Id,Topic=qus.Topics,SubTopic=qus.SubTopics,
                             QusType = qustype.QustionTypeName,
                             StatusName = status.Status,
                             CorrectOption = qus.CorrectOption,
                             QuestionTypeId = qus.QusType,
                             Options = _dBContext.Options.Where(p => p.QuestionId == qus.QusID).Select(p => new OptionsViewModel
                             {
                                 Id = p.Id,
                                 IsCorrect = p.IsCorrect,
                                 Option = p.Answer,
                                 Position = p.Position
                             }).ToList(),
                             Language = test.Language
                         }).ToList();
            return model;
        }
        public List<QuestionViewModel> GetQuestionsByTestId(List<int> TestId)
        {
            var model = (from qus in _dBContext.Questions.Where(p => TestId.Contains(p.TestId))
                         join status in _dBContext.TestStatuses on qus.StatusId equals status.Id
                         join test in _dBContext.Tests on qus.TestId equals test.Id
                         join qustype in _dBContext.QuestionTypes on qus.QusType equals qustype.Id
                         join gsec in _dBContext.TestSections on qus.SectionId equals gsec.Id into grp_sec
                         from sec in grp_sec.DefaultIfEmpty()
                         where qus.StatusId==3&&test.StatusID==3
                         select new QuestionViewModel
                         {
                             TestSection = new TestSectionViewModel
                             {
                                 SubTopic = qus.SubTopics,
                                 AddedQuestions = sec.AddedQuestions,
                                 AdditionalInstruction = sec.AdditionalInstruction,
                                 Id = sec.Id,
                                 IsActive = sec.IsActive,
                                 IsOnline = sec.IsOnline,
                                 Topic = qus.Topics,
                                 TotalMarks = sec.TotalMarks,
                                 TotalQuestions = sec.TotalQuestions
                             },
                             SectionName = sec.SectionName,
                             Created = qus.Created,
                             Modified = qus.Modified,
                             QuestionName = qus.QuestionName,
                             QusID = qus.QusID,
                             StatusId = qus.StatusId,
                             Mark = qus.Mark,
                             TestName = test.Title,
                             TestId = test.Id,
                             QusType = qustype.QustionTypeName,
                             StatusName = status.Status,
                             CorrectOption = qus.CorrectOption,
                             QuestionTypeId = qus.QusType,
                             Options = _dBContext.Options.Where(p => p.QuestionId == qus.QusID).Select(p => new OptionsViewModel
                             {
                                 Id = p.Id,
                                 IsCorrect = p.IsCorrect,
                                 Option = p.Answer,
                                 Position = p.Position
                             }).ToList(),
                             Language = test.Language
                         }).ToList();
            return model;
        }

        public QuestionViewModel GetQuestionDetails(int QuestionId)
        {
            var qusDb = (from qus in _dBContext.Questions
                         join qustype in _dBContext.QuestionTypes on qus.QusType equals qustype.Id
                         join test in _dBContext.Tests on qus.TestId equals test.Id
                         join g_sec in _dBContext.TestSections on qus.SectionId equals g_sec.Id into grpsec
                         from sec in grpsec.DefaultIfEmpty()
                         where qus.QusID == QuestionId &&qus.StatusId==3
                         select new { qus, qustype, sec,test }
                       );
            var options = _dBContext.Options.Where(p => p.QuestionId == QuestionId);
            if (!qusDb.Any())
                return null;
            var ques = qusDb.FirstOrDefault(q => q.qus.QusID == QuestionId);
            var model = new QuestionViewModel
            {
                TestSection = new TestSectionViewModel
                {
                    SubTopic = ques?.qus?.SubTopics ?? "N/A",
                    AddedQuestions = ques?.sec?.AddedQuestions ?? 0,
                    AdditionalInstruction = ques?.sec?.AdditionalInstruction,
                    Id = ques?.sec?.Id ?? 0,
                    IsActive = ques?.sec?.IsActive ?? false,
                    IsOnline = ques?.sec?.IsOnline ?? false,
                    SectionName = ques?.sec?.SectionName ?? "No section",
                    Topic = ques.qus?.Topics ?? "N/A",
                    TotalMarks = ques?.sec?.TotalMarks ?? 0,
                    TotalQuestions = ques?.sec?.TotalQuestions ?? 0
                },
                Created = ques.qus.Created,
                QusID = ques.qus.QusID,
                Mark = ques.qus.Mark,
                QuestionTypeId = ques.qus.QusType,
                QuestionName = ques.qus.QuestionName,
                TestId = ques.qus.TestId,
                QusType = ques.qustype.QustionTypeName,
                CorrectOption = ques.qus.CorrectOption,
                Modified=ques.qus.Modified,
                StatusId=ques.qus.StatusId,
                SectionId=ques.qus.SectionId,
                Topic=ques.qus.Topics,
                SubTopic=ques.qus.SubTopics ,
                SectionName=ques?.sec?.SectionName,
                Language=ques.test.Language,
                //Options = options.Select(l => new OptionsViewModel
                //{
                //    IsCorrect = l.IsCorrect,
                //    Option = l.Answer,
                //    Id = l.Id,
                //    Position = l.Position
                //}).ToList()
            };

            return model;
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
                        //SubTopics = test.SubTopics,
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
        public List<int> InsertStudentTest(List<StudentTest> studentTests)
        {
            _dBContext.StudentTests.AddRange(studentTests);
            _dBContext.SaveChanges();
            return studentTests.Select(s=>s.Id).ToList();
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
        public List<Languages> GetTestSubjectViewModels(List<int> gradeIds = null)
        {
            List<Languages> languages = new List<Languages>();
            var langs = _dBContext.Languages.Where(l =>_dBContext.Tests.Select(lang=>lang.Language).Contains(l.Id)).ToList();
            langs.ForEach(lang =>
            {
                var grads = new List<Grades>();
                var grades = _dBContext.GradeLevels.Where(g => _dBContext.Tests.Where(s => s.Language == lang.Id)
                 .Select(s => s.GradeID).Distinct().Contains(g.Id)).ToList();
                
                grades.ForEach(grade =>
                {

                    grads.Add( new Grades
                    {      Grade=grade.Grade,
                    GradeID=grade.Id,
                        TestSubjects = _dBContext.TestSubjects.Where(s =>
                    _dBContext.Tests.Where(g => g.GradeID == grade.Id).Distinct().Select(s => s.SubjectID).Contains(s.Id)).Select(sub => new TestSubject
                    {
                        SubjectName = sub.SubjectName, 
                        Id = sub.Id
                    }).ToList()

                    });

                });
                if (grads != null)
                    languages.Add(new Languages { Grades = grads ,LangId=lang.Id,Language=lang.Name });
            });

            //gradeIds = gradeIds ?? new List<int>();
            //var model = new List<TestSubjectViewModel>();


            //var subjects = _dBContext.TestSubjects.ToList();
            //if (gradeIds.Any())
            //    subjects = subjects.Where(g => gradeIds.Contains(g.Id)).ToList();
            //subjects.ToList().ForEach(subj =>
            // {
            //     var test = _dBContext.Tests.Where(p => p.SubjectID == subj.Id&&p.StatusID==3);
            //     var grades = test.Select(g => g.GradeID).ToList();
            //     model.Add(new TestSubjectViewModel { GradeLevels = _dBContext.GradeLevels.Where(g => grades.Contains(g.Id)).ToList(), SubjectId = subj.Id, SubjectName = subj.SubjectName });

            // });
            return languages;

        }
        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject)
        {
            subject = subject ?? new List<int>();
            var model = new List<TestGradeViewModel>();
            var grades = _dBContext.GradeLevels.ToList();
            grades.ForEach(grade =>
            {
                var test = _dBContext.Tests.Where(t => t.GradeID == grade.Id&&t.StatusID==3);
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

        public Entities.Student GetStudentByStudentId(int studentId)
        {                                                             
                return _dBContext.Students.FirstOrDefault(s => s.Id == studentId);
           
        }
        public AppUser GetParentByStudentId(int studentId)
        {
            var userid = _dBContext.Students.FirstOrDefault(s => s.Id == studentId)?.UserID;
            if (userid == null)
                return null;
            return _dBContext.Users.FirstOrDefault(u => u.Id == userid);

        }

       

       
    }
}

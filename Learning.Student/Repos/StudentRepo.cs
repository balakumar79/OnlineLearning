﻿using Learning.Entities;
using Learning.Entities.Config;
using Learning.Entities.Domain;
using Learning.Entities.Enums;
using Learning.Entities.Extension;
using Learning.Student.Abstract;
using Learning.Student.ViewModel;
using Learning.Tutor.ViewModel;
using Learning.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Student.Repos
{

    public class StudentRepo : IStudentRepo
    {
        #region variables
        readonly AppDBContext _dBContext;
        readonly ConnectionString _connectionString;
        #endregion

        #region ctor
        public StudentRepo(AppDBContext dBContext, ConnectionString connectionString)
        {
            _dBContext = dBContext;
            _connectionString = connectionString;
        }
        #endregion

        #region methods

        public TestViewModel GetTestById(int? id)
        {
            return _dBContext.Tests
                .Include(t => t.TestSubject)
                .Include(t => t.StudentTestStats)
                .Include(t => t.GradeLevels)
                .Include(t => t.TestStatus)

                .Where(p => p.Id == id && p.TestStatusId == 3 && p.IsActive).Select(p => new TestViewModel
                {
                    Id = p.Id,
                    Created = p.Created,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    StatusID = p.TestStatusId,
                    SubjectID = p.TestSubjectId,
                    //SubTopics = p.SubTopics,
                    Duration = p.Duration,
                    Description = p.TestDescription,
                    Language = p.LanguageId,
                    GradeID = p.GradeLevelsId,
                    Modified = p.Modified,
                    Title = p.Title,
                    CreatedBy = p.CreatedBy,
                    MaximumMarkScored = p.StudentTestStats.MaximumMarkScored,
                    MinimumMarkScored = p.StudentTestStats.MinimumMarkScored,
                    AverageScore = p.StudentTestStats.AverageMarkScored,
                    IsActive = p.IsActive,
                    IsPublished = p.IsPublished,
                    SubjectName = p.TestSubject.SubjectName,
                    GradeName = p.GradeLevels.Grade,
                    StatusName = p.TestStatus.Status,
                    PassingMark = p.PassingMark,
                    RoleId = p.RoleId,
                    TestTypeId = p.TestType,
                    ShuffleTypeId = p.ShuffleTypeId,
                    //Topics = p.Topics
                }).FirstOrDefault();
        }

        public async Task<PaginationResult<TestViewModel>> GetAllTest(GetTestRequestModel requestModel)
        {
            #region old
            //using (IDbConnection db = new SqlConnection(_connectionString.ConnectionStr))
            //{
            //    db.Open();
            //    var reader = await db.QueryMultipleAsync("sp_GetAllTests", new { studentId, subjectId, gradeId, pageSize, pageNumber },
            //        commandType: CommandType.StoredProcedure);
            //    var result =await reader.ReadAsync<TestViewModel>();

            //    return result.ToList();
            //}
            //var result = (from test in _dBContext.Tests.Include(s => s.StudentTests)
            //              join sub in _dBContext.TestSubjects on test.TestSubjectId equals sub.Id
            //              join teststatus in _dBContext.TestStatuses on test.TestStatusId equals teststatus.Id
            //              join grade in _dBContext.GradeLevels on test.GradeLevelsId equals grade.Id
            //              where test.TestStatusId == 3
            //              && test.IsActive
            //              && test.IsPublished
            //              && (test.TestType == ((int)TestTypeEnum.Public) || test.StudentTests.Any(ts => ts.StudentId == requestModel.StudentId))
            //              && (grade.Id == ((requestModel.GradeId > 0) ? requestModel.GradeId : grade.Id))
            //              && (test.LanguageId == ((requestModel.LanguageId > 0) ? requestModel.LanguageId : test.LanguageId))
            //              && (test.TestSubjectId == ((requestModel.SubjectId > 0) ? requestModel.SubjectId: test.TestSubjectId))
            //              && (test.Id == ((requestModel.TestId > 0) ? requestModel.TestId: test.Id))

            //              orderby test.Id descending, test.Created descending, test.Modified descending
            //              select new TestViewModel
            //              {
            //                  Created = test.Created,
            //                  SubjectName = sub.SubjectName,
            //                  SubjectID = sub.Id,
            //                  StatusID = test.TestStatusId,
            //                  StatusName = teststatus.Status,
            //                  Duration = test.Duration,
            //                  StartDate = test.StartDate,
            //                  EndDate = test.EndDate,
            //                  GradeID = test.GradeLevelsId,
            //                  GradeName = grade.Grade,
            //                  Language = test.LanguageId,
            //                  Id = test.Id,
            //                  Modified = test.Modified,
            //                  Title = test.Title,
            //                  CreatedBy = test.CreatedBy,
            //              }).Paginate(requestModel.PaginationQuery);
            #endregion

            string searchString = requestModel.PaginationQuery.SearchString;
            var result = _dBContext.Tests
                .Include(s => s.StudentTests)
                .Include(s => s.TestSubject)
                          .Include(s => s.TestStatus)
                          .Include(s => s.GradeLevels)
                          .Include(s => s.RandomTest)
                          .Where(test =>
                          test.TestStatusId == 3 &&
                          test.IsActive &&
                          test.IsPublished &&
                          (test.TestType == ((int)TestTypeEnum.Public) || test.StudentTests.Any(ts => ts.StudentId == requestModel.StudentId)) &&
                          (requestModel.GradeId <= 0 || test.GradeLevelsId == requestModel.GradeId) &&
                          (requestModel.LanguageId <= 0 || test.LanguageId == requestModel.LanguageId) &&
                          (requestModel.SubjectId <= 0 || test.TestSubjectId == requestModel.SubjectId) &&
                          (requestModel.TestId <= 0 || test.Id == requestModel.TestId))
                          .OrderByDescending(test => test.Id)
                          .ThenByDescending(test => test.Created)
                          .ThenByDescending(test => test.Modified)
                          .Select(test => new TestViewModel
                          {
                              Created = test.Created,
                              SubjectName = test.TestSubject.SubjectName,
                              SubjectID = test.TestSubject.Id,
                              StatusID = test.TestStatusId,
                              StatusName = test.TestStatus.Status,
                              Duration = test.Duration,
                              StartDate = test.StartDate,
                              EndDate = test.EndDate,
                              GradeID = test.GradeLevelsId,
                              GradeName = test.GradeLevels.Grade,
                              Language = test.LanguageId,
                              Id = test.Id,
                              ShuffleTypeId = test.ShuffleTypeId,
                              TestTypeId = test.TestType,
                              PassingMark = test.PassingMark,
                              Description = test.TestDescription,
                              TopicId = test.RandomTest == null ? 0 : test.RandomTest.TopicId,
                              SubTopicId = test.RandomTest == null ? 0 : test.RandomTest.SubTopicId,
                              Modified = test.Modified,
                              Title = test.Title,
                              CreatedBy = test.CreatedBy,
                          }).Paginate(requestModel.PaginationQuery);

            return result;
        }

        /// <summary>
        /// Get student test by parentid/userid
        /// </summary>
        /// <param name="userid">ParentId/UserId</param>
        /// <returns></returns>
        /// 
        public List<StudentTestViewModel> GetStudentTestByStudentIDs(List<int> studentid)
        {
            return (from test in _dBContext.Tests
                    join studentTest in _dBContext.StudentTests on test.Id equals studentTest.TestId
                    join subject in _dBContext.TestSubjects on test.TestSubjectId equals subject.Id
                    join grade in _dBContext.GradeLevels on test.GradeLevelsId equals grade.Id
                    join student in _dBContext.Students on studentTest.StudentId equals student.Id
                    join g_studentstats in _dBContext.StudentTestStats on test.Id equals g_studentstats.TestId into grpstats
                    from studentstats in grpstats.DefaultIfEmpty()
                    where studentid.Contains(studentTest.StudentId)

                    select new StudentTestViewModel
                    {
                        StatusID = test.TestStatusId,
                        StartDate = studentTest.StartDate,
                        SubjectID = test.TestSubjectId,
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
                        CreatedBy = studentTest.Assigner,
                        GradeID = test.GradeLevelsId,
                        Id = studentTest.Id,
                        Language = test.LanguageId,
                        GradeName = grade.Grade,
                        Modified = test.Modified,
                        TestId = test.Id,
                        Title = test.Title,
                        ShuffleTypeId = test.ShuffleTypeId,
                        StudentTestStats = studentstats,
                        StudentId = studentTest.StudentId,
                        StudentTestHistories = _dBContext.StudentTestHistories.Where(p => studentid.Contains(p.StudentId) && p.StudentTestId == studentTest.Id).ToList()
                    }).ToList();
        }

        public List<QuestionViewModel> GetQuestionsByTestId(int TestId)
        {
            var model = new List<QuestionViewModel>();
            var test = _dBContext.Tests.Include(t => t.Language).Include(t => t.Questions).ThenInclude(t => t.QuestionType).Include(t => t.TestStatus)
                .Include(t => t.RandomQuestions).FirstOrDefault(t => t.Id == TestId && t.TestStatusId == 3);

            if (test == null)
                return model;
            if (test.TestType == ((int)TestTypeEnum.Public) && test.Questions.Count == 0)
                return model;
            if (test.TestType == ((int)TestTypeEnum.Random) && !test.RandomQuestions.Any())
                return model;

            var t = _dBContext.RandomQuestions.Where(r => r.TestId == TestId).Include(s => s.Question).Select(s => s.Question);
            var e = test.Questions.AsQueryable();

            model = (from qus in test.TestType == ((int)TestTypeEnum.Random) ? t : test.Questions.AsQueryable()

                     where (qus.TestStatusId == 3 || qus.TestStatusId == 2)
                     select new QuestionViewModel
                     {
                         TestSection = _dBContext.TestSections.Where(s => s.Id == qus.SectionId).Select(sec => new TestSectionViewModel
                         {
                             SectionName = sec.SectionName,
                             SubTopic = qus.SubTopicId,
                             AddedQuestions = sec.AddedQuestions,
                             AdditionalInstruction = sec.AdditionalInstruction,
                             Created = sec.Created,
                             Id = sec.Id,
                             TestId = sec.TestId,
                             IsActive = sec.IsActive,
                             IsOnline = sec.IsOnline,
                             Modified = sec.Modified,
                             TestName = test.Title,
                             Topic = qus.TopicId,
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
                         StatusId = qus.TestStatusId,
                         Mark = qus.Mark,
                         TestName = test.Title,
                         TestId = test.Id,
                         Topic = qus.TopicId,
                         SubTopic = qus.SubTopicId,
                         QusType = qus.QuestionType == null ? "" : qus.QuestionType.QustionTypeName,
                         StatusName = test.TestStatus.Status,
                         CorrectOption = qus.CorrectOption,
                         QuestionTypeId = qus.QuestionTypeId,
                         Options = _dBContext.Options.Where(p => p.QuestionId == qus.QusID).Select(p => new OptionsViewModel
                         {
                             Id = p.Id,
                             IsCorrect = p.IsCorrect,
                             Option = p.Answer,
                             Position = p.Position
                         }).ToList(),
                         Language = test.LanguageId
                     }).ToList();
            return model;
        }

        public List<QuestionViewModel> GetQuestionsByTestId(List<int> TestId)
        {
            var model = (from qus in _dBContext.Questions.Where(p => TestId.Contains(p.TestId))
                         join status in _dBContext.TestStatuses on qus.TestStatusId equals status.Id
                         join test in _dBContext.Tests.Include(t => t.LanguageId) on qus.TestId equals test.Id
                         join qustype in _dBContext.QuestionTypes on qus.QuestionTypeId equals qustype.Id
                         join gsec in _dBContext.TestSections on qus.SectionId equals gsec.Id into grp_sec
                         from sec in grp_sec.DefaultIfEmpty()
                         where qus.TestStatusId == 3 && test.TestStatusId == 3 && !qus.Deleted
                         select new QuestionViewModel
                         {
                             TestSection = new TestSectionViewModel
                             {
                                 SubTopic = qus.SubTopicId,
                                 AddedQuestions = sec.AddedQuestions,
                                 AdditionalInstruction = sec.AdditionalInstruction,
                                 Id = sec.Id,
                                 IsActive = sec.IsActive,
                                 IsOnline = sec.IsOnline,
                                 Topic = qus.TopicId,
                                 TotalMarks = sec.TotalMarks,
                                 TotalQuestions = sec.TotalQuestions
                             },
                             SectionName = sec.SectionName,
                             Created = qus.Created,
                             Modified = qus.Modified,
                             QuestionName = qus.QuestionName,
                             QusID = qus.QusID,
                             StatusId = qus.TestStatusId,
                             Mark = qus.Mark,
                             TestName = test.Title,
                             TestId = test.Id,
                             QusType = qustype.QustionTypeName,
                             StatusName = status.Status,
                             CorrectOption = qus.CorrectOption,
                             QuestionTypeId = qus.QuestionTypeId,
                             Options = _dBContext.Options.Where(p => p.QuestionId == qus.QusID).Select(p => new OptionsViewModel
                             {
                                 Id = p.Id,
                                 IsCorrect = p.IsCorrect,
                                 Option = p.Answer,
                                 Position = p.Position
                             }).ToList(),
                             Language = test.LanguageId,
                         }).ToList();
            return model;
        }

        public QuestionViewModel GetQuestionDetails(int QuestionId)
        {
            var qusDb = (from qus in _dBContext.Questions
                         join qustype in _dBContext.QuestionTypes on qus.QuestionTypeId equals qustype.Id
                         join test in _dBContext.Tests.Include(t => t.TestSubject) on qus.TestId equals test.Id
                         join g_sec in _dBContext.TestSections on qus.SectionId equals g_sec.Id into grpsec
                         from sec in grpsec.DefaultIfEmpty()
                         join g_comp in _dBContext.Comprehensions on qus.QusID equals g_comp.QusId into gcomp
                         from comp in gcomp.DefaultIfEmpty()
                         where qus.QusID == QuestionId && (qus.TestStatusId == 3 || qus.TestStatusId == 2)
                         select new { qus, qustype, sec, test, comp }
                       );
            var options = _dBContext.Options.Where(p => p.QuestionId == QuestionId);
            if (!qusDb.Any())
                return null;
            var ques = qusDb.FirstOrDefault(q => q.qus.QusID == QuestionId);
            var model = new QuestionViewModel
            {
                TestSection = new TestSectionViewModel
                {
                    SubTopic = ques.qus.SubTopicId,
                    AddedQuestions = ques?.sec?.AddedQuestions ?? 0,
                    AdditionalInstruction = ques?.sec?.AdditionalInstruction,
                    Id = ques?.sec?.Id ?? 0,
                    IsActive = ques?.sec?.IsActive ?? false,
                    IsOnline = ques?.sec?.IsOnline ?? false,
                    SectionName = ques?.sec?.SectionName ?? "No section",
                    Topic = ques.qus.TopicId,
                    TotalMarks = ques?.sec?.TotalMarks ?? 0,
                    TotalQuestions = ques?.sec?.TotalQuestions ?? 0
                },
                Created = ques.qus.Created,
                QusID = ques.qus.QusID,
                Mark = ques.qus.Mark,
                QuestionTypeId = ques.qus.QuestionTypeId,
                QuestionName = ques.qus.QuestionName,
                TestId = ques.qus.TestId,
                QusType = ques.qustype.QustionTypeName,
                CorrectOption = ques.qus.CorrectOption,
                Modified = ques.qus.Modified,
                StatusId = ques.qus.TestStatusId,
                SectionId = ques.qus.SectionId,
                Topic = ques.qus.TopicId,
                SubTopic = ques.qus.SubTopicId,
                SectionName = ques?.sec?.SectionName,
                Language = ques.test.LanguageId,
                AnswerExplanation = ques.qus.AnswerExplantion
                //ComprehensionModels = new ComprehensionModel
                //{
                //    QusId = ques.qus.QusID,
                //    SectionId = ques.comp?.SectionId,
                //    CompQusId = ques.comp?.CompQusId,
                //    TestId = ques.test.Id,
                //    Id = ques.comp.Id,
                //    Question = _dBContext.Questions.FirstOrDefault(q => q.QusID == ques.comp.CompQusId)?.QuestionName
                //},
            };

            return model;
        }

        public int UpsertStudentTestStats(StudentTestStats stats)
        {
            var db = _dBContext.StudentTestStats.FirstOrDefault(s => s.TestId == stats.TestId);
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

        public StudentTestStats GetStudentTestStatsByTestid(int testid) => _dBContext.StudentTestStats.FirstOrDefault(p => p.TestId == testid);

        public List<StudentTestViewModel> GetAllStudentTest()
        {

            return (from test in _dBContext.Tests
                    join studentTest in _dBContext.StudentTests on test.Id equals studentTest.TestId
                    join subject in _dBContext.TestSubjects.Include(p => p.SubjectLanguageVariants) on test.TestSubjectId equals subject.Id
                    join grade in _dBContext.GradeLevels on test.GradeLevelsId equals grade.Id
                    select new StudentTestViewModel
                    {
                        StatusID = test.TestStatusId,
                        StartDate = test.StartDate,
                        SubjectID = test.TestSubjectId,
                        SubjectName = subject.SubjectName,
                        //SubTopics = test.SubTopics,
                        AssignedOn = studentTest.AssignedOn,
                        Assigner = studentTest.Assigner,
                        Created = test.Created,
                        Description = test.TestDescription,
                        Duration = test.Duration,
                        EndDate = test.EndDate,
                        GradeID = test.GradeLevelsId,
                        Id = studentTest.Id,
                        Language = test.LanguageId,
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
            return studentTests.Select(s => s.Id).ToList();
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
            //List<Languages> languages = new List<Languages>();

            var langs = _dBContext.Languages
    .Where(lang => _dBContext.Tests.Any(test => test.LanguageId == lang.Id))
    .Select(lang => new Languages
    {
        LangId = lang.Id,
        Language = lang.Name,
        Grades = _dBContext.GradeLevels
            .Where(grade => _dBContext.Tests.Any(test => test.LanguageId == lang.Id && test.GradeLevelsId == grade.Id))
            .Where(grade => gradeIds.Count == 0 || gradeIds.Contains(grade.Id))
            .Select(grade => new Grades
            {
                Grade = grade.Grade,
                GradeID = grade.Id,
                TestSubjects = _dBContext.TestSubjects
                    .Where(subject => _dBContext.Tests.Any(test => test.GradeLevelsId == grade.Id && test.TestSubjectId == subject.Id))
                    .Select(subject => new TestSubjectViewModel
                    {
                        SubjectName = subject.SubjectName,
                        Id = subject.Id
                    })
                    .ToList()
            })
            .ToList()
    })
    .ToList();


            #region oldlogic
            //var langs = _dBContext.Languages.Where(l => _dBContext.Tests.Select(lang => lang.LanguageId).Contains(l.Id)).ToList();
            //langs.ForEach(lang =>
            //{
            //    var grads = new List<Grades>();
            //    var grades = _dBContext.GradeLevels.Where(g => _dBContext.Tests.Where(s => s.LanguageId == lang.Id)
            //     .Select(s => s.GradeLevelsId).Distinct().Contains(g.Id)).ToList();
            //    if (gradeIds.Count > 0)
            //        grades = grades.Where(g => gradeIds.Contains(g.Id)).ToList();

            //    grades.ForEach(grade =>
            //    {

            //        grads.Add(new Grades
            //        {
            //            Grade = grade.Grade,
            //            GradeID = grade.Id,
            //            TestSubjects = _dBContext.TestSubjects.Where(s =>
            //        _dBContext.Tests.Where(g => g.GradeLevelsId == grade.Id).Distinct().Select(s => s.TestSubjectId).Contains(s.Id)).Select(sub => new TestSubjectViewModel
            //        {
            //            SubjectName = sub.SubjectName,
            //            Id = sub.Id
            //        }).ToList()

            //        });

            //    });
            //    if (grads != null)
            //        languages.Add(new Languages { Grades = grads, LangId = lang.Id, Language = lang.Name });
            //});
            #endregion

            //var test = _dBContext.Tests.Include(l => l.Language).Include(s => s.TestSubject).Include(g => g.GradeLevels).AsEnumerable()
            //.GroupBy(t => t.LanguageId)
            //.Select(n => new Languages
            //{
            //    Grades = n.AsEnumerable().GroupBy(g => g.GradeLevels).Select(grade =>
            //    new Grades
            //    {
            //        Grade = grade.Key.Grade,
            //        GradeID = grade.Key.Id,
            //        TestSubjects = n.AsEnumerable().GroupBy(s => new { s.GradeLevels, s.TestSubject }).Select(ts => new TestSubjectViewModel
            //        {
            //            Id = ts.Key.GradeLevels.Id,
            //            SubjectName = ts.Key.TestSubject.SubjectName,
            //            //GradeLevels=ts.GradeLevels
            //        }).ToList()
            //    }).Distinct().ToList(),
            //    LangId = n.Key,
            //    Language = n.FirstOrDefault().Language.Name
            //}).Distinct();

            //return languages.ToList();
            return langs;

        }

        public List<TestGradeViewModel> TestGradeViewModels(List<int> subject)
        {
            subject = subject ?? new List<int>();
            var model = new List<TestGradeViewModel>();
            var grades = _dBContext.GradeLevels.ToList();
            grades.ForEach(grade =>
            {
                var test = _dBContext.Tests.Where(t => t.GradeLevelsId == grade.Id && t.TestStatusId == 3);
                if (subject.Any())
                    test = test.Where(s => subject.Contains(s.Id));
                var subjectIds = test.Select(t => t.TestSubjectId).Distinct();
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

        #endregion


    }
}

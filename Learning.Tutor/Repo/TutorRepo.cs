﻿using Learning.Entities;
using Learning.Tutor.ViewModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Learning.Tutor.Abstract;

using Microsoft.EntityFrameworkCore;
using Learning.ViewModel.Tutor;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Learning.Utils.Config;

namespace Learning.Tutor.Repo
{
    public class TutorRepo : ITutorRepo
    {
        private readonly AppDBContext _dBContext;
        private ConnectionString  _connectionString;
        public TutorRepo(AppDBContext dBContext, ConnectionString connectionString)
        {
            this._dBContext = dBContext;
            _connectionString = connectionString;
        }

        public async Task<TutorDashboardViewModel> GetTutorDashboardModel(int userid)
        {
            var dashboard = new TutorDashboardViewModel();
            using (IDbConnection db = new SqlConnection(_connectionString.ConnectionStr))
            {
                db.Open();
                var reader=await db.QueryMultipleAsync("sp_GetTutorDashBoard",new {@UserId=userid }, commandType: CommandType.StoredProcedure);
                dashboard = reader.Read<TutorDashboardViewModel>().FirstOrDefault();
                //dashboard.NotificationPartialModels = reader.Read<NotificationPartialModel>().ToList();
                db.Close();
                return dashboard;
            }
        }
        public List<TestViewModel> GetTestByUserID(int tutorid)
        {
            return (from test in _dBContext.Tests
                    join sub in _dBContext.TestSubjects on test.TestSubjectId equals sub.Id
                    join teststatus in _dBContext.TestStatuses on test.TestStatusId equals teststatus.Id
                    join grade in _dBContext.GradeLevels on test.GradeLevelsId equals grade.Id
                    where test.TutorId == tutorid && test.IsActive
                    select new TestViewModel
                    {
                        Created = test.Created,
                        SubjectName = sub.SubjectName,
                        StatusID = test.TestStatusId,
                        StatusName = teststatus.Status,
                        Duration = test.Duration,
                        StartDate = test.StartDate,
                        EndDate = test.EndDate,
                        GradeID = test.GradeLevelsId,
                        GradeName = grade.Grade,
                        Id = test.Id,
                        Modified = test.Modified,
                        Title = test.Title,
                        IsActive = test.IsActive,
                        IsPublished = test.IsPublished,
                        TutorId = test.TutorId
                    }).ToList();
        }
        public TestViewModel GetTestById(int? id)
        {
            var t = _dBContext.Tests.Include(t => t.TestSubject).Include(t => t.TestSubject.SubjectLanguageVariants);
            return t.Where(p => p.Id == id).Select(p => new TestViewModel
            {
                Id = p.Id,
                Created = p.Created,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                StatusID = p.TestStatusId,
                SubjectID = p.TestSubjectId,

                Duration = p.Duration,
                Description = p.TestDescription,
                GradeID = p.GradeLevelsId,
                Modified = p.Modified,
                Title = p.Title,
                TutorId = p.TutorId,
                IsActive = p.IsActive,
                IsPublished = p.IsPublished,
                Language = p.LanguageId,
                LanguageVariants = p.TestSubject.SubjectLanguageVariants.Count > 0 ? p.TestSubject.SubjectLanguageVariants.Select(l => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = l.Language.Name,
                    Value = l.LanguageId.ToString()
                }).ToList() :
                _dBContext.Languages.Select(lang => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = lang.Name, Value = lang.Id.ToString() }).ToList()

            }).FirstOrDefault();
        }
        public List<TestViewModel> GetAllTest()
        {
            return (from test in _dBContext.Tests
                    join sub in _dBContext.TestSubjects on test.TestSubjectId equals sub.Id
                    join teststatus in _dBContext.TestStatuses on test.TestStatusId equals teststatus.Id
                    join grade in _dBContext.GradeLevels on test.GradeLevelsId equals grade.Id
                    join tutor in _dBContext.Tutors on test.TutorId equals tutor.TutorId


                    select new TestViewModel
                    {
                        Created = test.Created,
                        SubjectName = sub.SubjectName,
                        StatusID = test.TestStatusId,
                        StatusName = teststatus.Status,
                        Duration = test.Duration,
                        StartDate = test.StartDate,
                        EndDate = test.EndDate,
                        GradeID = test.GradeLevelsId,
                        GradeName = grade.Grade,
                        Id = test.Id,
                        IsPublished = test.IsPublished,
                        IsActive = test.IsActive,
                        Modified = test.Modified,
                        Title = test.Title,
                        TutorId = test.TutorId,
                        TutorUserName = tutor.UserName
                    }).ToList();
        }
        public List<QuestionViewModel> GetQuestionsByTestId(int TestId)
        {
            var model = (from qus in _dBContext.Questions.Where(p => p.TestId == TestId)
                         join status in _dBContext.TestStatuses on qus.TestStatusId equals status.Id
                         join test in _dBContext.Tests.Include(t => t.TestSubject.SubjectLanguageVariants) on qus.TestId equals test.Id
                         join qustype in _dBContext.QuestionTypes on qus.QuestionTypeId equals qustype.Id
                         where !qus.Deleted && test.IsActive
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
                             Language = test.LanguageId
                         }).ToList();
            return model;
        }
        public List<QuestionViewModel> GetQuestionsByTestId(List<int> TestId)
        {
            var model = (from qus in _dBContext.Questions.Where(p => TestId.Contains(p.TestId))
                         join status in _dBContext.TestStatuses on qus.TestStatusId equals status.Id
                         join test in _dBContext.Tests.Include(sub => sub.TestSubject) on qus.TestId equals test.Id
                         join qustype in _dBContext.QuestionTypes on qus.QuestionTypeId equals qustype.Id
                         join gsec in _dBContext.TestSections on qus.SectionId equals gsec.Id into grp_sec
                         from sec in grp_sec.DefaultIfEmpty()
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
                             Language = test.LanguageId
                         }).ToList();
            return model;
        }
        public QuestionViewModel GetQuestionDetails(int QuestionId)
        {
            var qusDb = (from qus in _dBContext.Questions
                         join qustype in _dBContext.QuestionTypes on qus.QuestionTypeId equals qustype.Id
                         join g_sec in _dBContext.TestSections on qus.SectionId equals g_sec.Id into grpsec
                         from sec in grpsec.DefaultIfEmpty()
                         where qus.QusID == QuestionId
                         select new { qus, qustype, sec }
                       );
            var options = _dBContext.Options.Where(p => p.QuestionId == QuestionId);
            if (!qusDb.Any())
                return null;
            var ques = qusDb.FirstOrDefault(q => q.qus.QusID == QuestionId);
            var model = new QuestionViewModel
            {
                TestSection = new TestSectionViewModel
                {
                    //SubTopic = ques.qus?.SubTopics??"N/A",
                    AddedQuestions = ques.sec?.AddedQuestions ?? 0,
                    AdditionalInstruction = ques.sec?.AdditionalInstruction,
                    Id = ques.sec?.Id ?? 0,
                    IsActive = ques.sec?.IsActive ?? false,
                    IsOnline = ques.sec?.IsOnline ?? false,
                    SectionName = ques.sec?.SectionName ?? "No section",
                    //Topic =ques.qus?.Topics??"N/A",
                    TotalMarks = ques.sec?.TotalMarks ?? 0,
                    TotalQuestions = ques.sec?.TotalQuestions ?? 0
                },
                ComprehensionModels = _dBContext.Comprehensions.Where(c =>
                c.QusId == ques.qus.QusID).Select(q => new ComprehensionModel
                {
                    SectionId = q.SectionId,
                    CompQusId = q.CompQusId,
                    Id = q.Id,
                    QusId = q.QusId,
                    TestId = q.TestId
                }).FirstOrDefault(),
                Created = ques.qus.Created,
                QusID = ques.qus.QusID,
                Mark = ques.qus.Mark,
                QuestionTypeId = ques.qus.QuestionTypeId,
                QuestionName = ques.qus.QuestionName,
                TestId = ques.qus.TestId,
                Topic = ques.qus.TopicId,
                SubTopic = ques.qus.SubTopicId,
                SectionId = ques.qus.SectionId,
                StatusId = ques.qus.TestStatusId,
                Modified = ques.qus.Modified,
                QusType = ques.qustype.QustionTypeName,
                CorrectOption = ques.qus.CorrectOption,
                Options = options.Select(l => new OptionsViewModel
                {
                    IsCorrect = l.IsCorrect,
                    Option = l.Answer,
                    Id = l.Id,
                    Position = l.Position
                }).OrderBy(p => p.Position).ToList()
            };

            return model;
        }
        public async Task<List<QuestionType>> GetQuestionTypes()
        {
            return await _dBContext.QuestionTypes.ToListAsync();
        }

        public TutorViewModel GetTutorProfile(int UserId)
        {

            return (from tutor in _dBContext.Tutors
                    join user in _dBContext.Users on tutor.UserId equals user.Id
                    join lg in _dBContext.Languages on tutor.PreferredLanguage equals lg.Id into lng
                    from lang in lng.DefaultIfEmpty()
                    where tutor.UserId == UserId
                    select new TutorViewModel
                    {

                        CreatedAt = tutor.CreatedAt,
                        Educations = tutor.Educations,
                        GradesTaken = _dBContext.TutorGradesTakens.Where(l => l.TutorID.ToString().Contains(tutor.TutorId.ToString())).Select(l => l.GradeID).ToList(),
                        LanguageOfInstruction = _dBContext.TutorLanguageOfInstructions.Where(p => tutor.TutorId == p.TutorID).Select(k => k.Language).ToList(),
                        LanguagePreference = lang.Name,
                        LastLoggedIn = tutor.LastLoggedIn,
                        TutorID = tutor.TutorId,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        //Gender =(Learning.ViewModel.Enums.Genders)Enum.Parse(typeof(Learning.ViewModel.Enums.Genders), user.Gender),
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserID = user.Id,
                        HasUserAccess = user.HasUserAccess,
                        TutorType = tutor.TutorType.ToString(),
                        Organization = tutor.Organization,
                    }).FirstOrDefault();

        }
        public async Task<int> TestUpsert(TestViewModel model)
        {
            Test test = _dBContext.Tests.FirstOrDefault(p => p.Id == model.Id);
            if (test == null)
                test = new Test();
            else
                _dBContext.Tests.Attach(test);
            test.Created = DateTime.Now;
            test.TestStatusId = 1;
            test.IsPublished = model.IsPublished;
            test.TestSubjectId = model.SubjectID;
            test.TestDescription = model.Description;
            test.IsActive = true;
            test.Duration = model.Duration;
            test.StartDate = model.StartDate;
            test.EndDate = model.EndDate;
            test.GradeLevelsId = model.GradeID;
            test.Modified = DateTime.Now;
            test.Title = model.Title;
            test.LanguageId = model.Language;
            test.TutorId = model.TutorId;
            if (model.Id == 0)
                _dBContext.Tests.Add(test);
            await _dBContext.SaveChangesAsync();
            return test.Id;
        }

        public async Task<bool> SetTestIsPublished(int id, bool isChecked)
        {
            var entity = _dBContext.Tests.Find(id);
            if (entity == null)
                return false;
            entity.IsPublished = isChecked;
            _dBContext.Tests.Update(entity);
            return await _dBContext.SaveChangesAsync() > 0;
        }

        public async Task<int> TestUpsert(IList<Test> entity)
        {
            if (entity == null)
                throw new Exception(nameof(Test));
            _dBContext.Tests.UpdateRange(entity);
            return await _dBContext.SaveChangesAsync();
        }
        public async Task<bool> CreateTestSection(TestSectionViewModel model)
        {
            var section = new TestSection
            {
                Created = DateTime.Now,
                Modified = DateTime.Now,
                SectionName = model.SectionName,
                //Topic = model.Topic,
                //SubTopic = model.SubTopic,
                IsActive = true,
                IsOnline = model.IsOnline,
                TestId = model.TestId,
                TotalMarks = model.TotalMarks,
                TotalQuestions = model.TotalQuestions
            };
            _dBContext.TestSections.Add(section);
            await _dBContext.SaveChangesAsync();
            return true;

        }
        public async Task<bool> UpsertQuestion(QuestionViewModel model)
        {
            if (model == null)
                throw new Exception("Model cannot be null.");
            if (model.Options == null && model.QuestionTypeId != 9 && model.QuestionTypeId != 8)
                throw new Exception("Options cannot be null.");
            model.Options = model.Options ?? new List<OptionsViewModel>();

            Question question = new Question();
            if (model.QusID > 0)
            {
                question = _dBContext.Questions.FirstOrDefault(m => m.QusID == model.QusID);
                question.SectionId = model.SectionId;
                question.QuestionTypeId = model.QuestionTypeId;
                question.TestId = model.TestId;
                question.Modified = DateTime.Now;
                question.Mark = model.Mark;
                question.QuestionName = model.QuestionName;
                question.TestStatusId = model.StatusId;
                question.CorrectOption = model.CorrectOption;
                question.TopicId = model.Topic;
                question.SubTopicId = model.SubTopic;
                _dBContext.Questions.Update(question);

                //delete options by question id for update
                var optiontoremove = _dBContext.Options.Where(p => p.QuestionId == model.QusID);
                _dBContext.Options.RemoveRange(optiontoremove);
                _dBContext.SaveChanges();

                var opts = new List<Options>();

                foreach (var item in model.Options)
                {
                    opts.Add(new Options
                    {
                        Answer = item.Option,
                        IsCorrect = item.IsCorrect,
                        QuestionId = question.QusID,
                        Position = item.Position,

                    });
                }
                if (opts.Count > 0)
                    _dBContext.Options.AddRange(opts);
                _dBContext.SaveChanges();
            }
            else
            {
                question = new Question
                {
                    SectionId = model.SectionId,
                    Created = DateTime.Now,
                    TestStatusId = model.StatusId,
                    Modified = DateTime.Now,
                    QuestionName = model.QuestionName,
                    QuestionTypeId = model.QuestionTypeId,
                    TestId = model.TestId,
                    CorrectOption = model.CorrectOption,
                    Mark = model.Mark,
                    TopicId = model.Topic,
                    SubTopicId = model.SubTopic
                };
                _dBContext.Questions.Add(question);
                _dBContext.SaveChanges();
                var mcq = new List<Options>();
                foreach (var item in model?.Options)
                {
                    mcq.Add(new Options
                    {
                        Answer = item.Option,
                        IsCorrect = item.IsCorrect,
                        QuestionId = question.QusID,
                        Position = item.Position,

                    });
                }
                _dBContext.Options.AddRange(mcq);
                await _dBContext.SaveChangesAsync();
            }

            await _dBContext.SaveChangesAsync();

            //UPDATE ADDED QUESTIONS COUNT IN SECTIONS
            if (model.SectionId > 0)
            {
                var section = _dBContext.TestSections.FirstOrDefault(p => p.Id == model.SectionId);
                section.AddedQuestions += 1;
                _dBContext.TestSections.Update(section);
                await _dBContext.SaveChangesAsync();
            }

            if (model.ComprehensionModels != null && model.QuestionTypeId == 8)
            {
                var comp = _dBContext.Comprehensions.FirstOrDefault(com => com.QusId == model.ComprehensionModels.QusId);
                if (comp != null)
                {
                    comp.CompQusId = model.ComprehensionModels.CompQusId;
                    comp.SectionId = model.ComprehensionModels.SectionId;
                    comp.TestId = model.ComprehensionModels.TestId;
                    comp.ModifiedAt = DateTime.Now;
                    _dBContext.Comprehensions.Update(comp);
                }
                else
                {
                    comp = new Comprehension
                    {
                        SectionId = model.ComprehensionModels.SectionId,
                        CompQusId = model.ComprehensionModels.CompQusId,
                        ModifiedAt = DateTime.Now,
                        TestId = model.ComprehensionModels.TestId,
                        QusId = question.QusID
                    };
                    _dBContext.Comprehensions.Add(comp);
                }
                _dBContext.SaveChanges();
            }
            //update/insert language variant


            return true;
        }

        public List<Test> GetTests(int? testid)
        {
            if (testid == null)
                return _dBContext.Tests.ToList();
            else
                return _dBContext.Tests.Where(t => t.Id == testid).ToList();
        }

        public List<TestSection> GetTestSections(int testsectionid) => _dBContext.TestSections.Where(t => t.Id == testsectionid).ToList();
        public async Task<bool> UpdateTutor(TutorViewModel model)
        {
            var tutor = _dBContext.Tutors.FirstOrDefault(p => p.TutorId == model.TutorID);
            tutor.ModifiedAt = DateTime.Now;
            tutor.Organization = model.Organization;
            tutor.PreferredLanguage = Convert.ToInt32(model.LanguagePreference);
            tutor.TutorType = Convert.ToInt32(model.TutorType);
            _dBContext.Tutors.Update(tutor);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsTestNameExists(string testname, int? id, int tutorId)
        {
            return id == null ? await _dBContext.Tests.AnyAsync(p => p.Title == testname && p.TutorId == tutorId) : await _dBContext.Tests.AnyAsync(p => p.Title == testname & p.Id != id && p.TutorId == tutorId);
        }
        public async Task<bool> IsSectionExists(string sectionname, int? id)
        {
            return id == null ? await _dBContext.TestSections.AnyAsync(p => p.SectionName == sectionname) : await _dBContext.TestSections.AnyAsync(p => p.SectionName == sectionname & p.TestId == id);
        }
        public async Task<int> DeleteTest(int id)
        {
            var test = _dBContext.Tests.FirstOrDefault(p => p.Id == id);
            test.IsActive = false;
            _dBContext.Tests.Update(test);
            return await _dBContext.SaveChangesAsync();
        }
        public bool DeleteQuestion(List<int> questionIds)
        {
            var question = _dBContext.Questions.Where(m => questionIds.Contains(m.QusID));
            question.ToList().ForEach(qus => qus.Deleted = true);
            _dBContext.Questions.UpdateRange(question);
            _dBContext.SaveChanges();
            return true;
        }

        public int DeleteSection(List<int> sectionid)
        {

            var db = _dBContext.TestSections.Where(s => sectionid.Contains(s.Id));
            db.ToList().ForEach(sec => sec.IsActive = false);
            _dBContext.TestSections.UpdateRange(db);
            return _dBContext.SaveChanges();
        }

        public int SetQuestionStatus(int questionid, int status)
        {
            var question = _dBContext.Questions.FirstOrDefault(k => k.QusID == questionid);
            question.TestStatusId = status;
            _dBContext.Questions.Update(question);
            return _dBContext.SaveChanges();
        }
        public int SetOnlineStatus(int sectionid, bool status)
        {
            try
            {
                var section = _dBContext.TestSections.FirstOrDefault(s => s.Id == sectionid);
                if (section != null)
                {
                    section.IsOnline = status;
                    _dBContext.TestSections.Update(section);
                    return _dBContext.SaveChanges();
                }
                return 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<Language> GetLanguages() => _dBContext.Languages.OrderBy(p => p.Name).ToList();
        public List<GradeLevels> GetGradeLevels() => _dBContext.GradeLevels.ToList();
        public async Task<List<TestSubject>> GetTestSubject()
        {
            var subject = await _dBContext.TestSubjects.OrderBy(p => p.SubjectName).ToListAsync();
            if (subject.Any(sub => sub.SubjectName.ToLower() == "other"))
            {
                var item = subject.Remove(subject.FirstOrDefault(p => p.SubjectName.ToLower() == "other"));
                subject.Add(new TestSubject { Id = subject.Count + 1, SubjectName = "Other" });
            }
            return subject;

        }
        public async Task<List<TestSection>> GetTestSectionByTestId(int testid) => await _dBContext.TestSections.Where(l => l.TestId == testid && l.IsActive).OrderBy(p => p.SectionName).ToListAsync();

        public async Task<List<QuestionType>> GetTestType() => await _dBContext.QuestionTypes.OrderBy(s => s.QustionTypeName).ToListAsync();

        //DB test function not for internal use
        public int savetrueorfalse()
        {
            var entity = new TrueOrFalse
            {
                QuestionId = 23,
                Position = 3,
                IsCorrect = true,
                Options = DateTime.Now.ToString()
            };
            _dBContext.TrueOrFalses.Add(entity);
            return _dBContext.SaveChanges();
        }

        public List<ComprehensionModel> GetComprehensionQuestionModels(int? testiD = 0)
        {
            var query = _dBContext.Questions.Where(comp => comp.QuestionTypeId == 9)
                        .Select(comp => new ComprehensionModel
                        {
                            SectionId = comp.SectionId,
                            CompQusId = comp.QusID,
                            QusId = comp.QusID,
                            Question = comp.QuestionName,
                            TestId = comp.TestId
                        });
            if (testiD > 0)
                query = query.Where(t => t.TestId == testiD);
            return query.ToList();

        }

        public List<SubjectTopic> GetSubjectTopics(int? id = 0)
        {
            if (id > 0)
                return _dBContext.SubjectTopics.Where(t => t.Id == id && !t.IsDeleted).ToList();
            else
                return _dBContext.SubjectTopics.Where(l => !l.IsDeleted).ToList();
        }

        public List<SubjectTopic> GetTopicsByTestId(int testid)
        {
            if (testid > 0)
            {
                var subjectid = _dBContext.Tests.FirstOrDefault(t => t.Id == testid);
                return _dBContext.SubjectTopics.Where(t => t.TestSubjectId == subjectid.TestSubjectId && !t.IsDeleted).ToList();
            }
            else
            {
                return new List<SubjectTopic>();
            }
        }
        public List<SubjectSubTopic> GetSubTopics(int? topidId = null)
        {
            if (topidId > 0)
            {
                return _dBContext.SubjectSubTopics.Where(s => s.SubjectTopicId == topidId).ToList();
            }
            return _dBContext.SubjectSubTopics.ToList();
        }

        public List<Language> GetLanguagesForSubject(int subjectid)
        {
            var db = _dBContext.TestSubjects.Include(sub => sub.SubjectLanguageVariants).ThenInclude(l => l.Language);
            return db.FirstOrDefault(s => s.Id == subjectid)?.SubjectLanguageVariants.Select(s => s.Language).ToList();
        }


    }
}

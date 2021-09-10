using Learning.Entities;
using Learning.Tutor.ViewModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Learning.Tutor.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Learning.Tutor.Repo
{
   public class TutorRepo:ITutorRepo
    {
        private readonly AppDBContext _dBContext;
        public TutorRepo(AppDBContext dBContext)
        {
           this._dBContext = dBContext;
        }

        public List<TestViewModel> GetTestByUserID(string tutorid)
        {
            return (from test in _dBContext.Tests
                    join sub in _dBContext.TestSubjects on test.SubjectID equals sub.Id
                    join teststatus in _dBContext.TestStatuses on test.StatusID equals teststatus.Id
                    join grade in _dBContext.GradeLevels on test.GradeID equals grade.Id
                    where test.TutorId==tutorid
                    select new TestViewModel
                    {
                        Created = test.Created,
                        SubjectName = sub.SubjectName,
                        StatusID = test.StatusID,
                        StatusName = teststatus.Status,
                        Duration = test.Duration,
                        StartDate=test.StartDate,
                        EndDate = test.EndDate,
                        GradeID = test.GradeID,
                        GradeName = grade.Grade,
                        Id = test.Id,
                        Modified = test.Modified,
                        Title = test.Title,
                        TutorId = test.TutorId
                    }).ToList();
        }
        public TestViewModel GetTestById(int ?id)
        {
            return _dBContext.Tests.Where(p => p.Id == id).Select(p => new TestViewModel
            {
                Id = p.Id,
                Created = p.Created,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                StatusID = p.StatusID,
                SubjectID = p.SubjectID,
                SubTopics = p.SubTopics,
                Duration = p.Duration,
                Description=p.TestDescription,
                Language=p.Language,
                GradeID = p.GradeID,
                Modified = p.Modified,
                Title = p.Title,
                TutorId = p.TutorId,
                Topics = p.Topics
            }).FirstOrDefault();
        }
        public List<TestViewModel> GetAllTest()
        {
            return (from test in _dBContext.Tests
                    join sub in _dBContext.TestSubjects on test.SubjectID equals sub.Id
                    join teststatus in _dBContext.TestStatuses on test.StatusID equals teststatus.Id
                    join grade in _dBContext.GradeLevels on test.GradeID equals grade.Id
                 
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
                        Id = test.Id,
                        Modified = test.Modified,
                        Title = test.Title,
                        TutorId = test.TutorId
                    }).ToList();
        }
        public List<QuestionViewModel> GetQuestionsByTestId(int TestId)
        {
            var model = (from qus in _dBContext.Questions.Where(p => p.TestId == TestId)
                         join test in _dBContext.Tests on qus.TestId equals test.Id
                         join qustype in _dBContext.QuestionTypes on qus.QusType equals qustype.Id
                         join gsec in _dBContext.TestSections on qus.SectionId equals gsec.Id into grp_sec
                         from sec in grp_sec.DefaultIfEmpty()
                         select new QuestionViewModel
                         {
                             SectionId = qus.SectionId,
                             SectionName = sec.SectionName,
                             Created = qus.Created,
                             QuestionName = qus.QuestionName,
                             QusID = qus.QusID,
                             IsActive=qus.IsActive,
                             Mark=qus.Mark,
                             TestName=test.Title,
                             TestId=test.Id,
                             QusType = qustype.QustionTypeName,
                             QuestionTypeId = qus.QusType,
                         }).ToList();
            return model;
        }
        public QuestionViewModel GetQuestionDetails(int QuestionId)
        {
            var qusDb =(from qus in _dBContext.Questions
                       join qustype in _dBContext.QuestionTypes on qus.QusType equals qustype.Id
                       where qus.QusID==QuestionId
                       select new {qus,qustype}
                       );
            var options = _dBContext.Options.Where(p => p.QuestionId == QuestionId);
            if (!qusDb.Any())
                return null;
            var ques = qusDb.FirstOrDefault(q=>q.qus.QusID==QuestionId);
            var model = new QuestionViewModel
            {
                SectionId = ques.qus.SectionId,
                Created = ques.qus.Created,
                QusID = ques.qus.QusID,
                Mark = ques.qus.Mark,
                QuestionTypeId = ques.qus.QusType,
                QuestionName = ques.qus.QuestionName,
                TestId = ques.qus.TestId,
                QusType=ques.qustype.QustionTypeName,
                Options = options.Select(l => new OptionsViewModel
                {
                    IsCorrect = l.IsCorrect,
                    Option = l.Answer,
                    Id = l.Id,
                    Position = l.Position
                }).ToList()
            };
                        
            return model;
        }
        public async Task<List<QuestionType>> GetQuestionTypes()
        {
            return await _dBContext.QuestionTypes.ToListAsync();
        }
        
        public TutorViewModel GetTutorProfile(int TutorId)
        {

            return (from tutor in _dBContext.Tutors
                    join user in _dBContext.Users on tutor.UserId equals user.Id
                    join lg in _dBContext.Languages on tutor.PreferredLanguage equals lg.Id into lng
                    from lang in lng.DefaultIfEmpty()
                    where tutor.UserId == TutorId
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
            test.StatusID = 1;
            test.SubjectID = model.SubjectID;
            test.TestDescription = model.Description;
            test.Topics = model.Topics;
            test.SubTopics = model.SubTopics;
            test.Duration = model.Duration;
            test.StartDate = model.StartDate;
            test.EndDate = model.EndDate;
            test.GradeID = model.GradeID;
            test.Modified = DateTime.Now;
            test.Title = model.Title;
            test.Language = model.Language;
            test.TutorId = model.TutorId;
            if (model.Id == 0)
                _dBContext.Tests.Add(test);
            await _dBContext.SaveChangesAsync();
            return test.Id;
        }
        public async Task<bool> CreateTestSection(TestSectionViewModel model)
        {
            var section = new TestSection
            {
                Created = DateTime.Now,
                Modified = DateTime.Now,
                SectionName = model.SectionName,
                Topic=model.Topic,
                SubTopic=model.SubTopic,
                IsActive = true,
                IsOnline = model.IsOnline,
                TestId = model.TestId,
                TotalMarks = model.TotalMarks,
                TotalQuestions=model.TotalQuestions
            };
            _dBContext.TestSections.Add(section);
           await _dBContext.SaveChangesAsync();
            return true;

        }
        public async Task<bool> CreateQuestion(QuestionViewModel model)
        {
            Question question = new Question();
            if (model.QusID > 0)
            {
                question = _dBContext.Questions.FirstOrDefault(m => m.QusID == model.QusID);
                question.SectionId = model.SectionId;
                question.QusType = model.QuestionTypeId;
                question.TestId = model.TestId;
                question.Modified = DateTime.Now;
                question.Mark = model.Mark;
                question.QuestionName = model.QuestionName;
                question.IsActive = model.IsActive;
                _dBContext.Questions.Update(question);

                //delete options by question id for update
                _dBContext.Options.RemoveRange(_dBContext.Options.Where(p => p.QuestionId == model.QusID));

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
                _dBContext.Options.AddRange(opts);
                await _dBContext.SaveChangesAsync();
                return true;
            }
            else
            {

                question = new Question
                {
                    SectionId = model.SectionId,
                    Created = DateTime.Now,
                    IsActive = model.IsActive,
                    Modified = DateTime.Now,
                    QuestionName = model.QuestionName,
                    QusType = model.QuestionTypeId,
                    TestId = model.TestId,
                    Mark = model.Mark,
                };
            }
            _dBContext.Questions.Add(question);
            await _dBContext.SaveChangesAsync();

            //UPDATE ADDED QUESTIONS IN SECTIONS
            if (model.SectionId >0)
            {
                var section = _dBContext.TestSections.FirstOrDefault(p => p.Id == model.SectionId);
                section.AddedQuestions += 1;
                _dBContext.TestSections.Update(section);
                _dBContext.SaveChanges();
            }

            //update/insert language variant
            

            //switch (model.QuestionTypeId)
            //{
            //    case 1:
            //        {
                        var mcq = new List<Options>();
                        foreach (var item in model.Options)
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
                    return true;
                    }
        //case 2:
        //    {
        //        var gapfill = new List<GapFillingAnswer>();
        //        foreach (var item in model.Options)
        //        {
        //            gapfill.Add(new GapFillingAnswer
        //            {
        //                Answer = item.Option,
        //                QuestionId=question.QusID,
        //                Position = item.Position,
        //                IsCorrect=item.IsCorrect
        //            });
        //        }
        //        _dBContext.GapFillingAnswers.AddRange(gapfill);
        //        await _dBContext.SaveChangesAsync();
        //        return true;
        //    }
        //case 3:
        //    {
        //        var match = new List<Matching>();
        //        foreach (var item in model.Options)
        //        {
        //            match.Add(new Matching
        //            {
        //                CorrectAnswer = item.CorrectAnswer,
        //                Position = item.Position,
        //                Sentence = item.Option,
        //            });

        //        }
        //        _dBContext.Matchings.AddRange(match);
        //        await _dBContext.SaveChangesAsync();
        //        return true;
        //    }
        //case 4:
        //    {
        //        var truefalse = new List<TrueOrFalse>();
        //        foreach(var item in model.Options)
        //        {
        //            truefalse.Add(new TrueOrFalse
        //            {
        //                Options = item.Option,
        //                QuestionId=question.QusID,
        //                IsCorrect = item.IsCorrect,
        //                Position = item.Position
        //            });
        //        }
        //        _dBContext.TrueOrFalses.AddRange(truefalse);
        //       await _dBContext.SaveChangesAsync();
        //        return true;
        //    }


        //}
        //}

        public List<Test> GetTests(int? testid)
        {
            if (testid == null)
                return _dBContext.Tests.ToList();
            else
                return _dBContext.Tests.Where(t => t.Id == testid).ToList();
        }

        public List<TestSection> GetTestSections(int  testsectionid) => _dBContext.TestSections.Where(t => t.Id == testsectionid).ToList();
        public async Task<bool> UpdateTutor(TutorViewModel model)
        {
            var tutor = _dBContext.Tutors.FirstOrDefault(p => p.TutorId == model.TutorID);
            tutor.ModifiedAt = DateTime.Now;
            tutor.Organization = model.Organization;
            tutor.PreferredLanguage =Convert.ToInt32( model.LanguagePreference);
            tutor.TutorType =Convert.ToInt32(model.TutorType);
            _dBContext.Tutors.Update(tutor);
            await _dBContext.SaveChangesAsync();
            return true;
        }
      
        public async Task<bool> IsTestNameExists(string testname, int ? id)
        {
            return  id == null ? await _dBContext.Tests.AnyAsync(p => p.Title == testname) :await  _dBContext.Tests.AnyAsync(p => p.Title == testname & p.Id != id);
        }
        public async Task<bool> IsSectionExists(string sectionname, int ? id)
        {
            return id == null ? await _dBContext.TestSections.AnyAsync(p => p.SectionName == sectionname) : await _dBContext.TestSections.AnyAsync(p => p.SectionName == sectionname & p.TestId == id);
        }
        public async Task<int> DeleteTest(int id)
        {
            _dBContext.Tests.Remove(_dBContext.Tests.FirstOrDefault(p => p.Id == id));
            return await _dBContext.SaveChangesAsync();
        }
        public bool DeleteQuestion(List<int> questionIds)
        {
            _dBContext.Questions.RemoveRange(_dBContext.Questions.Where(m => questionIds.Contains(m.QusID)));
            _dBContext.SaveChanges();
            return true;
        }

        public int DeleteSection(List<int> sectionid)
        {
            var db = _dBContext.TestSections.Where(s => sectionid.Contains(s.Id));
            _dBContext.TestSections.RemoveRange(db);
            return _dBContext.SaveChanges();
        }

        public int SetQuestionStatus(int questionid,bool status)
        {
            var question = _dBContext.Questions.FirstOrDefault(k => k.QusID == questionid);
            question.IsActive = status;
            _dBContext.Questions.Update(question);
            return _dBContext.SaveChanges();
        }
        public async Task<int> SetOnlineStatus(int sectionid, bool status)
        {
            try
            {
                var section = _dBContext.TestSections.FirstOrDefault(s => s.Id == sectionid);
                if (section != null)
                {
                    section.IsOnline = status;
                    _dBContext.Entry(section).State = EntityState.Modified;
                    _dBContext.TestSections.Attach(section);
                }
                return await _dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<Language> GetLanguages() => _dBContext.Languages.ToList();
        public List<GradeLevels> GetGradeLevels() => _dBContext.GradeLevels.ToList();
        public async Task<List<TestSubject>> GetTestSubject() => await _dBContext.TestSubjects.ToListAsync();
        public async Task<List<TestSection>> GetTestSectionByTestId(int testid) =>  await _dBContext.TestSections.Where(l => l.TestId == testid).ToListAsync();
        
        public async Task<List<QuestionType>> GetTestType() =>await _dBContext.QuestionTypes.ToListAsync();
        

    }
}

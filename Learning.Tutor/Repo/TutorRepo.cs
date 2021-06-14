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
                GradeID = p.GradeID,
                Modified = p.Modified,
                Title = p.Title,
                TutorId = p.TutorId,
                Topics = p.Topics
            }).FirstOrDefault();
        }
        public async Task<List<QuestionType>> GetQuestionTypes()
        {
            return await _dBContext.QuestionTypes.ToListAsync();
        }
        
        public TutorViewModel GetTutorProfile(int userid)
        {
           
            return (from tutor in _dBContext.Tutors
                    from grades in _dBContext.GradeLevels
                    from lang in _dBContext.Languages
                    where tutor.UserId==userid
                    select new TutorViewModel
                    {
                        
                        CreatedAt = tutor.CreatedAt,
                        Educations = tutor.Educations,
                        GradesTaken = _dBContext.TutorGradesTakens.Where(l => l.TutorID.ToString().Contains(tutor.TutorId.ToString())).Select(l=>l.GradeID).ToList(),
                        LanguageOfInstruction = _dBContext.TutorLanguageOfInstructions.Where(p => lang.Id.ToString().Contains(p.Id.ToString())).Select(k=>k.Language).ToList(),
                        LanguagePreference =tutor.PreferredLanguage.ToString(),
                        LastLoggedIn = tutor.LastLoggedIn,
                        TutorID=tutor.TutorId,
                        UserName = _dBContext.Users.FirstOrDefault(p=>p.Id==tutor.UserId).UserName,
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
            test.Topics = model.Topics;
            test.SubTopics = model.SubTopics;
            test.Duration = model.Duration;
            test.StartDate = model.StartDate;
            test.EndDate = model.EndDate;
            test.GradeID = model.GradeID;
            test.Modified = DateTime.Now;
            test.Title = model.Title;
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
                SubTopic=model.SubTopic,
                IsActive = true,
                IsOnline = model.IsOnline,
                TestId = model.TestId,
                TotalMarks = model.TotalMarks,
            };
            _dBContext.TestSections.Add(section);
           await _dBContext.SaveChangesAsync();
            return true;

        }
        public async Task<bool> CreateQuestion(QuestionViewModel model)
        {
            var question = new Question
            {
                SectionId = model.SectionId,
                Created = DateTime.Now,
                IsActive = true,
                Modified = DateTime.Now,
                QuestionName = model.QuestionName,
                QusType = model.QuestionTypeId,
                TestId = model.TestId
            };
            _dBContext.Questions.Add(question);
           await _dBContext.SaveChangesAsync();
            if ( model.QuestionTypeId== 1)
            {
                var mcq = new List<MCQAnswer>();
                foreach (var item in model.Options)
                {
                    mcq.Add(new MCQAnswer
                    {
                        Answer = item.Option,
                        IsCorrect = item.IsCorrect,
                        QuestionId = question.QusID,
                    });
                }
                _dBContext.MCQAnswers.AddRange(mcq);
               await _dBContext.SaveChangesAsync();
            }
            return true;
        }
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
        public async Task<List<TestSection>> GetTestSections() => await _dBContext.TestSections.ToListAsync();
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
        public List<Language> GetLanguages() => _dBContext.Languages.ToList();
        public List<GradeLevels> GetGradeLevels() => _dBContext.GradeLevels.ToList();
        public async Task<List<TestSubject>> GetTestSubject() => await _dBContext.TestSubjects.ToListAsync();
        public async Task<List<TestSection>> GetTestSections(int testid) => await _dBContext.TestSections.Where(l => l.TestId == testid).ToListAsync();
        public async Task<List<QuestionType>> GetTestType() =>await _dBContext.QuestionTypes.ToListAsync();

    }
}

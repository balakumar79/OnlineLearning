using Learning.Entities;
using Learning.Tutor.Abstract;
using Learning.Tutor.Repo;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Tutor.Service
{
   public class TutorService:ITutorService
    {
        private readonly ITutorRepo _tutorRepo;
        public TutorService(ITutorRepo tutorRepo)
        {
            this._tutorRepo = tutorRepo;

        }
        public TutorViewModel GetTutorProfile(int id) => _tutorRepo.GetTutorProfile(id);
        public Task<int> TestUpsert(TestViewModel model) => _tutorRepo.TestUpsert(model);
        public Task<bool> CreateTestSection(TestSectionViewModel model) => _tutorRepo.CreateTestSection(model);
        public Task<bool> CreateQuestion(QuestionViewModel model) => _tutorRepo.CreateQuestion(model);
        public List<TestViewModel> GetTestByUserID(string tutorid) => _tutorRepo.GetTestByUserID(tutorid);

        public Task<List<QuestionType>> GetQuestionTypes() => _tutorRepo.GetQuestionTypes();
        public Task<List<TestSection>> GetTestSections() => _tutorRepo.GetTestSections();

        public Task<bool> IsTestExists(string testname, int? id)
        {
           return _tutorRepo.IsTestNameExists(testname,id);
        }

        public Task<bool> IsSectionExists(string sectionname, int? id)
        {
            return _tutorRepo.IsSectionExists(sectionname,id);
        }

        public TestViewModel GetTestById(int? id)
        {
            return _tutorRepo.GetTestById(id);
        }

        public Task<int> DeleteTest(int id)
        {
            return _tutorRepo.DeleteTest(id);
        }

        public List<GradeLevels> GetGradeLevels()
        {
            return _tutorRepo.GetGradeLevels();
        }

        public List<Language> GetLanguages()
        {
            return _tutorRepo.GetLanguages();
        }

        public Task<List<TestSubject>> GetTestSubject()
        {
            return _tutorRepo.GetTestSubject();
        }

        public Task<List<TestSection>> GetTestSections(int testid)
        {
            return _tutorRepo.GetTestSections();
        }

        public Task<List<QuestionType>> GetTestType()
        {
           return _tutorRepo.GetTestType();
        }
    }
}

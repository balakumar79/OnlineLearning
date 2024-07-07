using Learning.Entities;
using Learning.Entities.Domain;
using Learning.Tutor.Abstract;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Test;
using Learning.ViewModel.Tutor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Tutor.Service
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepo _tutorRepo;
        public TutorService(ITutorRepo tutorRepo)
        {
            this._tutorRepo = tutorRepo;

        }
        public TutorViewModel GetTutorProfile(int id) => _tutorRepo.GetTutorProfile(id);
        public Task<int> TestUpsert(TestViewModel model) => _tutorRepo.TestUpsert(model);
        public Task<int> TestUpsert(List<Test> model) => _tutorRepo.TestUpsert(model);

        public Task<bool> CreateTestSection(TestSectionViewModel model) => _tutorRepo.CreateTestSection(model);
        public Task<bool> CreateQuestion(QuestionViewModel model) => _tutorRepo.UpsertQuestion(model);
        public PaginationResult<TestViewModel> GetTestByUserID(int tutorid, PaginationQuery query) => _tutorRepo.GetTestByUserID(tutorid, query);
        public IEnumerable<TestViewModel> GetTestByUserID(int tutorid) => _tutorRepo.GetTestByUserID(tutorid);
        public QuestionViewModel GetQuestionDetails(int QuestionId)
        {
            return _tutorRepo.GetQuestionDetails(QuestionId);
        }
        public List<QuestionViewModel> GetQuestionsByTestId(int TestId)
        {
            return _tutorRepo.GetQuestionsByTestId(TestId);
        }
        public List<QuestionViewModel> GetQuestionsByTestId(List<int> TestIds)
        {
            return _tutorRepo.GetQuestionsByTestId(TestIds);
        }
        public Task<List<QuestionType>> GetQuestionTypes() => _tutorRepo.GetQuestionTypes();
        public List<TestSection> GetTestSections(int sectionid)
        {
            return _tutorRepo.GetTestSections(sectionid);
        }
        public async Task<List<TestSection>> GetTestSectionByTestId(int testid)
        {
            return await _tutorRepo.GetTestSectionByTestId(testid);
        }
        public Task<bool> IsTestExists(string testname, int? id, int tutorId)
        {
            return _tutorRepo.IsTestNameExists(testname, id, tutorId);
        }

        public Task<bool> IsSectionExists(string sectionname, int? id)
        {
            return _tutorRepo.IsSectionExists(sectionname, id);
        }

        public TestViewModel GetTestById(int? id)
        {
            return _tutorRepo.GetTestById(id);
        }
        public int SetQuestionStatus(int questionid, int status)
        {
            return _tutorRepo.SetQuestionStatus(questionid, status);
        }
        public int SetOnlineStatus(int sectionid, bool status)
        {
            return _tutorRepo.SetOnlineStatus(sectionid, status);
        }
        public Task<int> DeleteTest(int id)
        {
            return _tutorRepo.DeleteTest(id);
        }
        public bool DeleteQuestion(List<int> questionIds)
        {
            return _tutorRepo.DeleteQuestion(questionIds);
        }
        public int DeleteSection(List<int> sectionid)
        {
            return _tutorRepo.DeleteSection(sectionid);
        }
        public IQueryable<GradeLevels> GetGradeLevels()
        {
            return _tutorRepo.GetGradeLevels();
        }

        public IQueryable<GradeLevels> GetGradeLevelTestAssociation(int? languageId, int? testId)
        {
            return _tutorRepo.GetGradeLevelTestAssociation((int)languageId, testId);
        }

        public List<Language> GetLanguages()
        {
            return _tutorRepo.GetLanguages();
        }

        public IQueryable<TestSubject> GetTestSubject()
        {
            return _tutorRepo.GetTestSubject();
        }

        public Task<List<QuestionType>> GetTestType()
        {
            return _tutorRepo.GetTestType();
        }

        public PaginationResult<TestViewModel> GetAllTest(PaginationQuery query)
        {
            return _tutorRepo.GetAllTest(query);
        }
        public int savetrueorfalse()
        {
            return _tutorRepo.savetrueorfalse();
        }
        public List<ComprehensionModel> GetComprehensionQuestionModels(int? testiD = 0)
        {
            return _tutorRepo.GetComprehensionQuestionModels(testiD);
        }
        public List<SubjectTopic> SubjectTopics(int? id = 0)
        {
            return _tutorRepo.GetSubjectTopics(id);
        }
        public List<SubjectTopic> GetTopicsByTestId(int testid)
        {
            return _tutorRepo.GetTopicsByTestId(testid);
        }
        public List<SubjectSubTopic> GetSubTopics(int? topidId = null)
        {
            return _tutorRepo.GetSubTopics(topidId);
        }
        public List<Language> GetLanguagesForSubject(int subjectid)
        {
            return _tutorRepo.GetLanguagesForSubject(subjectid);
        }
        public Task<bool> SetTestIsPublished(int id, bool isChecked)
        {
            return _tutorRepo.SetTestIsPublished(id, isChecked);
        }

        public Task<TutorDashboardViewModel> GetTutorDashboardModel(int userid)
        {
            return _tutorRepo.GetTutorDashboardModel(userid);
        }

        public IEnumerable<GradeLevelModel> GetGradeLevelsByLanguages(int[] Languages)
        {
            return _tutorRepo.GetGradeLevelsByLanguages(Languages);
        }

        public IEnumerable<SubjectModel> GetSubjectsByGrades(int[] Grades)
        {
            return _tutorRepo.GetSubjectsByGrades(Grades);
        }
    }
}

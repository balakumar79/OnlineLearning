using Learning.Entities;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Test;
using Learning.ViewModel.Tutor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Tutor.Abstract
{
    public interface ITutorService
    {
        Task<TutorDashboardViewModel> GetTutorDashboardModel(int userid);
        TutorViewModel GetTutorProfile(int id);
        Task<int> TestUpsert(TestViewModel model);
        Task<int> TestUpsert(List<Test> entity);
        Task<bool> CreateTestSection(TestSectionViewModel model);
        Task<bool> CreateQuestion(QuestionViewModel model);
        IEnumerable<TestViewModel> GetTestByUserID(int tutorid);
        QuestionViewModel GetQuestionDetails(int QuestionId);
        Task<List<QuestionType>> GetQuestionTypes();
        List<TestSection> GetTestSections(int sectionid);
        Task<bool> IsTestExists(string testname, int? id, int tutorId);
        Task<bool> IsSectionExists(string sectionname, int? id);
        TestViewModel GetTestById(int? id);
        List<QuestionViewModel> GetQuestionsByTestId(int QuestionId);
        List<QuestionViewModel> GetQuestionsByTestId(List<int> QuestionIds);
        IQueryable<GradeLevels> GetGradeLevels();
        IQueryable<GradeLevels> GetGradeLevelTestAssociation(int? languageId, int? testId);
        List<Language> GetLanguages();
        IQueryable<TestSubject> GetTestSubject();
        Task<List<TestSection>> GetTestSectionByTestId(int testid);
        Task<int> DeleteTest(int id);
        int DeleteSection(List<int> sectionid);
        int SetQuestionStatus(int questionid, int status);
        int SetOnlineStatus(int sectionid, bool status);
        bool DeleteQuestion(List<int> questionIds);
        Task<List<QuestionType>> GetTestType();
        public List<TestViewModel> GetAllTest();

        int savetrueorfalse();
        List<ComprehensionModel> GetComprehensionQuestionModels(int? testiD = 0);
        List<SubjectTopic> SubjectTopics(int? id = 0);
        List<SubjectTopic> GetTopicsByTestId(int testid);
        List<SubjectSubTopic> GetSubTopics(int? topidId = null);
        List<Language> GetLanguagesForSubject(int subjectid);
        Task<bool> SetTestIsPublished(int id, bool isChecked);
        IEnumerable<GradeLevelModel> GetGradeLevelsByLanguages(int[] Languages);
        IEnumerable<SubjectModel> GetSubjectsByGrades(int[] Grades);

    }
}

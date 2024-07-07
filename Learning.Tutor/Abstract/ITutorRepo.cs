using Learning.Entities;
using Learning.Entities.Domain;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Test;
using Learning.ViewModel.Tutor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Tutor.Abstract
{
    public interface ITutorRepo
    {
        Task<TutorDashboardViewModel> GetTutorDashboardModel(int userid);
        TutorViewModel GetTutorProfile(int id);
        Task<int> TestUpsert(TestViewModel model);
        Task<int> TestUpsert(IList<Test> entity);
        Task<bool> CreateTestSection(TestSectionViewModel model);
        Task<bool> UpsertQuestion(QuestionViewModel model);
        PaginationResult<TestViewModel> GetTestByUserID(int tutorid, PaginationQuery query);
        IEnumerable<TestViewModel> GetTestByUserID(int tutorid);
        QuestionViewModel GetQuestionDetails(int QuestionId);
        Task<List<QuestionType>> GetQuestionTypes();
        List<TestSection> GetTestSections(int sectionid);
        Task<List<TestSection>> GetTestSectionByTestId(int testid);
        Task<bool> IsTestNameExists(string testname, int? id, int tutorId);
        Task<bool> IsSectionExists(string sectionname, int? id);
        TestViewModel GetTestById(int? id);
        List<QuestionViewModel> GetQuestionsByTestId(int ExamId);
        List<QuestionViewModel> GetQuestionsByTestId(List<int> TestIds);
        Task<bool> SetTestIsPublished(int id, bool isChecked);
        Task<int> DeleteTest(int id);
        int DeleteSection(List<int> sectionid);
        int SetQuestionStatus(int questionid, int status);
        int SetOnlineStatus(int sectionid, bool status);
        bool DeleteQuestion(List<int> questionIds);
        List<Language> GetLanguages();
        IQueryable<GradeLevels> GetGradeLevels();
        IQueryable<GradeLevels> GetGradeLevelTestAssociation(int? languageId, int? testId);
        IQueryable<TestSubject> GetTestSubject();
        Task<List<QuestionType>> GetTestType();
        PaginationResult<TestViewModel> GetAllTest(PaginationQuery query);
        int savetrueorfalse();
        List<ComprehensionModel> GetComprehensionQuestionModels(int? testiD = 0);

        List<SubjectTopic> GetSubjectTopics(int? id = 0);
        List<SubjectTopic> GetTopicsByTestId(int testid);
        List<SubjectSubTopic> GetSubTopics(int? topidId = null);
        List<Language> GetLanguagesForSubject(int subjectid);
        IEnumerable<GradeLevelModel> GetGradeLevelsByLanguages(int[] Languages);
        IEnumerable<SubjectModel> GetSubjectsByGrades(int[] Grades);
    }
}

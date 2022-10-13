using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Tutor.Abstract
{
    public interface ITutorRepo
    {
        TutorViewModel GetTutorProfile(int id);
        Task<int> TestUpsert(TestViewModel model);
        Task<int> TestUpsert(IList<Test> entity);
        Task<bool> CreateTestSection(TestSectionViewModel model);
        Task<bool> UpsertQuestion(QuestionViewModel model);
        List<TestViewModel> GetTestByUserID(string tutorid);
        QuestionViewModel GetQuestionDetails(int QuestionId);
        Task<List<QuestionType>> GetQuestionTypes();
        List<TestSection> GetTestSections(int  sectionid);
        Task<List<TestSection>> GetTestSectionByTestId(int testid);
        Task<bool> IsTestNameExists(string testname, int? id,string tutorId);
        Task<bool> IsSectionExists(string sectionname, int? id);
        TestViewModel GetTestById(int? id);
        List<QuestionViewModel> GetQuestionsByTestId(int ExamId);
        List<QuestionViewModel> GetQuestionsByTestId(List<int> TestIds);
        
        Task<int> DeleteTest(int id);
        int DeleteSection(List<int> sectionid);
        int SetQuestionStatus(int questionid, int status);
       int SetOnlineStatus(int sectionid, bool status);
        bool DeleteQuestion(List<int> questionIds);
        List<Language> GetLanguages();
        List<GradeLevels> GetGradeLevels();
        Task<List<TestSubject>> GetTestSubject();
        Task<List<QuestionType>> GetTestType();
        List<TestViewModel> GetAllTest();
        int savetrueorfalse();
        List<ComprehensionModel> GetComprehensionQuestionModels(int? testiD = 0);

    }
}

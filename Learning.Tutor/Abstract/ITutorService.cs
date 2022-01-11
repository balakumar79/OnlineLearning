using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Tutor.Abstract
{
    public interface ITutorService
    {
        TutorViewModel GetTutorProfile(int id);
        Task<int> TestUpsert(TestViewModel model);
        Task<bool> CreateTestSection(TestSectionViewModel model);
        Task<bool> CreateQuestion(QuestionViewModel model);
        List<TestViewModel> GetTestByUserID(string tutorid);
        QuestionViewModel GetQuestionDetails(int QuestionId);
        Task<List<QuestionType>> GetQuestionTypes();
        List<TestSection> GetTestSections(int sectionid);
        Task<bool> IsTestExists(string testname, int? id);
        Task<bool> IsSectionExists(string sectionname, int? id);
        TestViewModel GetTestById(int? id);
        List<QuestionViewModel> GetQuestionsByTestId(int QuestionId);
        List<QuestionViewModel> GetQuestionsByTestId(List<int> QuestionIds);
         List<GradeLevels> GetGradeLevels();
        List<Language> GetLanguages();
        Task<List<TestSubject>> GetTestSubject();
        Task<List<TestSection>> GetTestSectionByTestId(int testid);
        Task<int> DeleteTest(int id);
        int DeleteSection(List<int> sectionid);
        int SetQuestionStatus(int questionid, int status);
       int SetOnlineStatus(int sectionid, bool status);
        bool DeleteQuestion(List<int> questionIds);
        Task<List<QuestionType>> GetTestType();
        public List<TestViewModel> GetAllTest();

        int savetrueorfalse();
    }
}

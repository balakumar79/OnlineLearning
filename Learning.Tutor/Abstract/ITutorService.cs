﻿using Learning.Entities;
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
        Task<List<QuestionType>> GetQuestionTypes();
        Task<List<TestSection>> GetTestSections();
        Task<bool> IsTestExists(string testname, int? id);
        Task<bool> IsSectionExists(string sectionname, int? id);
        TestViewModel GetTestById(int? id);
         List<GradeLevels> GetGradeLevels();
        List<Language> GetLanguages();
        Task<List<TestSubject>> GetTestSubject();
        Task<List<TestSection>> GetTestSections(int testid);
        Task<int> DeleteTest(int id);
        Task<List<QuestionType>> GetTestType();
    }
}
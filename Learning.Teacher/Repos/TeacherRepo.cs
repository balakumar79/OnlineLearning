﻿using Learning.Entities;
using Learning.Entities.Enums;
using Learning.Student.Abstract;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Teacher.Repos
{
    public class TeacherRepo : ITeacherRepo
    {
        private readonly AppDBContext _dBContext;
        private readonly IStudentService _studentService;
        public TeacherRepo(AppDBContext dBContext, IStudentService studentService)
        {
            _dBContext = dBContext;
            _studentService = studentService;
        }
        public List<Entities.Teacher> GetTeacher(int? userId = null)
        {
            if (userId > 0)
                return _dBContext.Teachers.Where(t => t.UserId == userId).ToList();
            else
                return _dBContext.Teachers.ToList();
        }
        public StudentInvitation StudentInvitationUpsert(StudentInvitation studentInvitation)
        {
            if (studentInvitation == null)
                return null;
            if (studentInvitation.Id == 0)
                _dBContext.StudentInvitations.Add(studentInvitation);
            else
                _dBContext.StudentInvitations.Update(studentInvitation);
            _dBContext.SaveChanges();
            return studentInvitation;
        }
        /// <summary>
        ///  Get student invitation by parentid, studentid, teacherid.  Pass 1 for parentid, pass 2 for studentid, pass 3 for teacherid, 4 for Id
        /// </summary>
        /// <param name="Id">Column Id</param>
        /// <param name="valueType">Column Name</param>
        /// <returns></returns>
        public IList<StudentInvitation> GetStudentInvitations(List<int> Id, int valueType)
        {
            switch (valueType)
            {
                case 1:
                    return _dBContext.StudentInvitations.Where(s => Id.Contains(s.Parentid)).ToList();
                case 2:
                    return _dBContext.StudentInvitations.Where(s => Id.Contains(s.StudentId)).ToList();
                case 3:
                    return _dBContext.StudentInvitations.Where(s => Id.Contains(s.TeacherId)).ToList();
                case 4:
                    return _dBContext.StudentInvitations.Where(s => Id.Contains(s.Id)).ToList();
                default:
                    return _dBContext.StudentInvitations.ToList();
            }
        }

        public List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string>? district, List<string>? instituion, int? teacherId)
        {
            district = district ?? new List<string>();
            gradeId = gradeId ?? new List<int>();
            instituion = instituion ?? new List<string>();
            var query = _dBContext.Students.AsQueryable();
            var sids = _dBContext.StudentInvitations.Where(i => i.TeacherId == teacherId && i.Response == 1).Select(st => st.StudentId);
            if (teacherId > 0)
                query = query.Where(q => !sids.Contains(q.Id)).AsQueryable();

            if (!string.IsNullOrEmpty(fname))
                query = query.Where(s => s.FirstName.ToLower().Contains(fname.ToLower()));
            if (!string.IsNullOrEmpty(lname))
                query = query.Where(s => s.LastName.ToLower().Contains(lname.ToLower()));
            if (!string.IsNullOrEmpty(userName))
                query = query.Where(s => s.UserName.ToLower().Contains(userName));
            if (district.Any())
                query = query.Where(s => district.Contains(s.StudentDistrict.ToLower()));
            if (gradeId.Any())
            {
                query = query.Where(s => gradeId.Contains(s.Grade));
            }
            if (!string.IsNullOrEmpty(gender))
                query = query.Where(s => s.Gender.ToString().ToLower().Equals(gender.ToLower()));
            if (instituion.Any())
                query = query.Where(s => instituion.Contains(s.Institution));
            return query.Join(_dBContext.GradeLevels, x => x.Grade, y => y.Id, (x, y) => new { x, y }).Select(s => new StudentModel
            {
                StudentFirstName = s.x.FirstName,
                StudentLastName = s.x.LastName,
                StudentDistrict = s.x.StudentDistrict,
                StudentGender = s.x.Gender.ToString(),
                StudentUserName = s.x.UserName,
                Grade = s.y.Grade,
                Institution = s.x.Institution,
                LanguageKnown = s.x.LanguagesKnown,
                MotherTongue = s.x.MotherTongue,
                StudentId = s.x.Id,
                UserId = s.x.UserID
            }).ToList();
        }

        public async Task<RepoResponse<int>> RandomTestUpsert(
            int userId, string title, int subjectId, int? topicId, int? subTopicId, int roleId, int gradeId, int languageId, DateTime startDate, DateTime endDate, int duration = 0, int passingMark = 0, string description = null, int? id = 0)
        {
            if (!_dBContext.Questions
                .Include(d => d.Test)
                .Any(s => s.Test.TestSubjectId == subjectId && s.Test.LanguageId == languageId
                && s.TopicId == topicId && s.SubTopicId == subTopicId && s.Test.GradeLevelsId == gradeId))
                return new RepoResponse<int>(0, ValidateMessages.NO_QUESTION_FOUND);

            if (_dBContext.Tests.Any(s => s.Title == title))
                return new RepoResponse<int>(-1, ValidateMessages.DUPLICATE_RECORD, isDuplicated: true);

            var db = _dBContext.Tests.FirstOrDefault(t => t.Id == id && t.TestType == ((int)TestTypeEnum.Random));
            if (db == null && id > 0)
                return new RepoResponse<int>(0, ValidateMessages.NO_DATA_FOUND);
            db ??= new Test();
            db.StartDate = startDate;
            db.Created = DateTime.Now;
            db.EndDate = endDate;
            db.TestDescription = description;
            db.TestSubjectId = subjectId;
            db.LanguageId = languageId;
            db.Duration = duration;
            db.CreatedBy = userId;
            db.RoleId = roleId;
            db.Title = title;
            db.GradeLevelsId = gradeId;
            db.PassingMark = passingMark;
            db.TestType = ((int)TestTypeEnum.Random);
            db.IsActive = true;
            db.IsPublished = true;
            db.TestStatusId = 3;
            if (db.Id > 0)
                _dBContext.Tests.Update(db);
            else
                _dBContext.Tests.Add(db);
            await _dBContext.SaveChangesAsync();
            try
            {
                var rExam = _dBContext.RandomTests.Find(db.Id);
                if (rExam == null)
                {
                    rExam = new RandomTest();
                    rExam.TestId = db.Id;
                    rExam.SubTopicId = subTopicId;
                    rExam.TopicId = topicId;
                    _dBContext.RandomTests.Add(rExam);
                }
                else
                {
                    rExam.TestId = db.Id;
                    rExam.SubTopicId = (int)subTopicId;
                    rExam.TopicId = (int)topicId;
                    _dBContext.RandomTests.Update(rExam);
                }
                await _dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _dBContext.Loggers.Add(new Logger { Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message, CreatedAt = DateTime.Now, Type = "Error", Description = ex.ToString(), Link = "SaveTestUpsert" });
                _dBContext.Tests.Remove(db);
                await _dBContext.SaveChangesAsync();

            }
            return new RepoResponse<int>(db.Id, ValidateMessages.RECORD_ADDED_SUCCESSFULLY, true);
        }

        public RepoListResponse<QuestionViewModel> GenerateRandomQuestions(int testId, int numberOfQuestions, int? difficultyLevel = 0)
        {
            var test = _dBContext.Tests.Find(testId);
            if (test == null) return new RepoListResponse<QuestionViewModel>();
            var model = _dBContext.Questions
                .Include(t => t.Test)
                .Include(t => t.Test.Language)
                .Include(t => t.Test.TestSubject)
                .Include(t => t.Options)
                .Include(t => t.TestSection)
                .Include(t => t.SubjectSubTopic)
                .Where(q => q.Test.TestSubjectId == test.TestSubjectId && q.Test.GradeLevelsId == test.GradeLevelsId)
                .Select(q => new QuestionViewModel
                {
                    SectionId = q.SectionId,
                    SectionName = q.TestSection.SectionName,
                    StatusId = q.TestStatusId,
                    StatusName = q.TestStatus.Status,
                    SubTopic = q.SubTopicId,
                    CorrectOption = q.CorrectOption,
                    Language = q.Test.LanguageId,
                    QusID = q.QusID,
                    QuestionTypeId = q.QuestionTypeId,
                    Mark = q.Mark,
                    Options = q.Options.Select(s => new OptionsViewModel { CorrectAnswer = s.Answer, Id = s.Id, IsCorrect = s.IsCorrect, Position = s.Position, Option = s.Answer }).ToList(),
                    TestId = q.TestId,
                    QusType = q.QuestionType.QustionTypeName,
                    QuestionName = q.QuestionName,
                    TestName = q.Test.Title,
                    Topic = q.TopicId,
                    Created = q.Created,
                    TestSection = new TestSectionViewModel
                    {
                        AddedQuestions = 0,
                        AdditionalInstruction = "",
                        Created = q.Created,
                        SectionName = q.TestSection.SectionName,
                        SubTopic = q.SubTopicId,
                        TestId = q.TestId,
                        Topic = q.TopicId,
                        TotalMarks = q.Mark,
                        Modified = q.Modified,
                    },
                    Modified = q.Modified

                });
            if (numberOfQuestions == 0 || model.Count() == 0)
                return new RepoListResponse<QuestionViewModel> { Result = new List<QuestionViewModel>(), Description = ValidateMessages.NO_QUESTION_FOUND };
            if (model.Count() < numberOfQuestions)
                numberOfQuestions = model.Count();
            var rand = new Random();
            var toSkip = rand.Next(0, model.Count());

            var result = model.Skip(toSkip).Take(numberOfQuestions).AsEnumerable();
            var randomquestions = result.Select(rnd => new RandomQuestion { QuestionId = rnd.QusID, TestId = test.Id });
            _dBContext.RandomQuestions.AddRange(randomquestions);
            _dBContext.SaveChanges();

            return new RepoListResponse<QuestionViewModel>
            {
                Result = result.ToList(),
                IsSuccess = true,
            };
        }
    }
}

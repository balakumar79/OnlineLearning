using Learning.Entities;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;

namespace Learning.Teacher.Repos
{
    public interface ITeacherRepo
    {
        List<Entities.Teacher> GetTeacher(int? userId = null);
        StudentInvitation StudentInvitationUpsert(StudentInvitation studentInvitation);
        IList<StudentInvitation> GetStudentInvitations(List<int> Id, int valueType);
        List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string>? district, List<string>? instituion, int? userId);
        IEnumerable<QuestionViewModel> GenerateRandomQuestions(int testid, int numberOfQuestions, int? difficultyLevel = 0);
        public int RandomTestUpsert(int userId, string title, int subjectId, int roleId, int gradeId, int languageId, DateTime startDate, DateTime endDate, int duration = 0, int passingMark = 0, string description = null, int? id = 0);
    }
}

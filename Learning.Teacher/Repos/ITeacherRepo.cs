using Learning.Entities;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Teacher.Repos
{
    public interface ITeacherRepo
    {
        List<Entities.Teacher> GetTeacher(int? userId = null);
        StudentInvitation StudentInvitationUpsert(StudentInvitation studentInvitation);
        IList<StudentInvitation> GetStudentInvitations(List<int> Id, int valueType);
        List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string>? district, List<string>? instituion,int? userId);
        IEnumerable<QuestionViewModel> GenerateRandomQuestions(int subjectId, int gradeId, int numberOfQuestions, int? difficultyLevel = 0);
    }
}

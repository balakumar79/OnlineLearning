using Learning.Entities;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learning.Teacher.Services
{
    public interface ITeacherService
    {
        public List<Entities.Teacher> GetTeacher(int? userId = null);
        StudentInvitation StudentInvitationUpsert(StudentInvitation studentInvitation);
        /// <summary>
        ///  Get student invitation by parentid, studentid, teacherid.  Pass 1 for parentid, pass 2 for studentid, pass 3 for teacherid, 4 for Id
        /// </summary>
        /// <param name="Id">studentid or teacherid or parentid or Id</param>
        /// <param name="valueType">Column Name</param>
        /// <returns></returns>
        IList<StudentInvitation> GetStudentInvitations(List<int> Id, int valueType);
        List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string>? district, List<string>? instituion, int? teacherid = null);
        RepoListResponse<QuestionViewModel> GenerateRandomQuestions(int subjectId, int numberOfQuestions, int? difficultyLevel = 0);

        //save random test
        public Task<RepoResponse<int>> RandomTestUpsert(int userId, string title, int subjectId, int? topicId, int? subTopicId, int roleId, int gradeId, int languageId, DateTime startDate, DateTime endDate, int duration = 0, int passingMark = 0, string description = null, int? id = 0);
    }
}

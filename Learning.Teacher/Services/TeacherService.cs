using Learning.Entities;
using Learning.Teacher.Repos;
using Learning.TeacherServ.Viewmodel;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;

namespace Learning.Teacher.Services
{

    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepo _teacherRepo;
        public TeacherService(ITeacherRepo teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }
        public List<Entities.Teacher> GetTeacher(int? userId = null)
        {
            return _teacherRepo.GetTeacher(userId);
        }

        public StudentInvitation StudentInvitationUpsert(StudentInvitation studentInvitation)
        {
            return _teacherRepo.StudentInvitationUpsert(studentInvitation);
        }
        /// <summary>
        ///  Get student invitation by parentid, studentid, teacherid.  Pass 1 for parentid, pass 2 for studentid, pass 3 for teacherid, 4 for Id
        /// </summary>
        /// <param name="Id">Column Id</param>
        /// <param name="valueType">Column Name</param>
        /// <returns>StudentInvitation list</returns>
        public IList<StudentInvitation> GetStudentInvitations(List<int> Id, int valueType)
        {
            return _teacherRepo.GetStudentInvitations(Id, valueType);
        }
        public List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string> district, List<string> instituion, int? teacherId = null)
        {
            return _teacherRepo.SearchStudent(fname, lname, userName, gender, gradeId, district, instituion, teacherId);
        }
        public IEnumerable<QuestionViewModel> GenerateRandomQuestions(int subjectId, int numberOfQuestions, int? difficultyLevel = 0)
        {
            return _teacherRepo.GenerateRandomQuestions(subjectId, numberOfQuestions, difficultyLevel);
        }

        public int RandomTestUpsert(int userId, string title, int subjectId, int roleId, int gradeId, int languageId, DateTime startDate, DateTime endDate, int duration = 0, int passingMark = 0, string description = null, int? id = 0)
        {
            return _teacherRepo.RandomTestUpsert(userId, title, subjectId, roleId, gradeId, languageId, startDate, endDate, duration, passingMark, description, id);
        }
    }
}

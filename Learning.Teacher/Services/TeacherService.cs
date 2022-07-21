﻿using Learning.Entities;
using Learning.Teacher.Repos;
using Learning.TeacherServ.Viewmodel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Teacher.Services
{
         
   public class TeacherService :ITeacherService
    {
        private readonly ITeacherRepo _teacherRepo;
        public TeacherService(ITeacherRepo teacherRepo)
        {
            _teacherRepo = teacherRepo;
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
        public List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string> district, List<string> instituion)
        {
            return _teacherRepo.SearchStudent(fname, lname, userName, gender, gradeId, district, instituion);
        }
    }
}
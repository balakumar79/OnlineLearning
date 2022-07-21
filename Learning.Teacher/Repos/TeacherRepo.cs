using Learning.Entities;
using Learning.Student.Abstract;
using Learning.TeacherServ.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Teacher.Repos
{
   public class TeacherRepo :ITeacherRepo
    {
        private readonly AppDBContext _dBContext;
        private readonly IStudentService _studentService;
        public TeacherRepo(AppDBContext dBContext,IStudentService studentService)
        {
            _dBContext = dBContext;
            _studentService = studentService;
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
        public IList<StudentInvitation> GetStudentInvitations(List<int> Id,int valueType)
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
                    return _dBContext.StudentInvitations.Where(s => s.Id == valueType).ToList();
                default:
                    return _dBContext.StudentInvitations.ToList();
            }
        }

        public List<StudentModel> SearchStudent(string fname, string lname, string userName, string gender, List<int>? gradeId, List<string>? district, List<string> ?instituion)
        {
           district= district ?? new List<string>();
            gradeId = gradeId ?? new List<int>();
            instituion = instituion ?? new List<string>();
            var query = _dBContext.Students.AsQueryable();
            if (!string.IsNullOrEmpty(fname))
                query = query.Where(s => s.FirstName.ToLower().Contains(fname.ToLower()));
            if (!string.IsNullOrEmpty(lname))
                query = query.Where(s => s.LastName.ToLower().Contains(lname.ToLower()));
            if (!string.IsNullOrEmpty(userName))
                query = query.Where(s => s.UserName.ToLower().Contains(userName.ToLower()));
            if (district.Any())
                query = query.Where(s => district.Contains(s.StudentDistrict.ToLower()));
            if (gradeId.Any())
            {
                query = query.Where(s => gradeId.Contains(s.Grade));
            }
            if (!string.IsNullOrEmpty(gender))
                query = query.Where(s => s.Gender.ToLower().Equals(gender.ToLower()));
            if (instituion.Any())
                query = query.Where(s => instituion.Contains(s.Institution));
           return query.Join(_dBContext.GradeLevels, x => x.Grade, y => y.Id, (x, y) => new { x, y }).Select(s => new StudentModel
            {
                StudentFirstName=s.x.FirstName,
                StudentLastName=s.x.LastName,
                StudentDistrict=s.x.StudentDistrict,
                StudentGender=s.x.Gender,
                StudentUserName=s.x.UserName,
                Grade=s.y.Grade,
                Institution=s.x.Institution,
                LanguageKnown=s.x.LanguagesKnown,
                MotherTongue=s.x.MotherTongue,
                StudentId=s.x.Id,
                UserId=s.x.UserID
            }).ToList();
        }
    }
}

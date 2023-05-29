using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.ViewModel.Account;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Learning.Admin.Repo
{
    public class ManageStudentRepo : IManageStudentRepo
    {
        private readonly AppDBContext _dBContext;
        public ManageStudentRepo(AppDBContext appDBContext)
        {
            _dBContext = appDBContext;
        }
        public IEnumerable<StudentModel> GetStudents()
        {
            return _dBContext.Students.Include(s => s.AppUser).Select(stud => new StudentModel
            {
                StudentFirstName = stud.FirstName,
                StudentLastName = stud.LastName,
                LanguageKnown = stud.LanguagesKnown,
                StudentDistrict = stud.StudentDistrict,
                StudentGenderId = stud.GenderId,
                GradeLevels = stud.Grade,
                Institution = stud.Institution,
                RoleRequested = stud.RoleId,
                StudentUserName = stud.UserName,
                MotherTongue = stud.MotherTongue,
                UserId = stud.UserID,
                CreatedOn = stud.CreatedOn,
                AccountUserModel = new AccountUserModel
                {
                    District = stud.AppUser.District,
                    Email = stud.AppUser.Email,
                    FirstName = stud.AppUser.FirstName,
                    LastName = stud.AppUser.LastName,
                    GenderId = stud.AppUser.Gender,
                    HasUserAccess = stud.AppUser.HasUserAccess,
                    PhoneNumber = stud.AppUser.PhoneNumber,
                    UserName = stud.AppUser.UserName,
                }
            });
        }
    }
}

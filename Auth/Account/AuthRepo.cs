using Learning.Auth;
using Learning.Entities;
using Learning.Tutor.ViewModel;
using Learning.Utils.Config;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth.Account
{
    public class AuthRepo : IAuthRepo
    {
        readonly ISecurePassword _securePassword;
        readonly UserManager<AppUser> userManager;
        readonly SecretKey _secretkey;

        private readonly AppDBContext dBContext;

        public AuthRepo(UserManager<AppUser> userManager,ISecurePassword securePassword, AppDBContext appDBContext,SecretKey secretKey)
        {

            this.dBContext = appDBContext;
            this.userManager = userManager;
            _securePassword = securePassword;
            _secretkey = secretKey;
        }
        public async Task<IdentityResult> AddUser(AppUser appUser, string password, AppRole role)
        {
            var res = await userManager.CreateAsync(appUser, password);
            if (res.Succeeded)
            {
                var roleresult = await userManager.AddToRoleAsync(appUser, role.Name);
            }
            return res;
        }
       
        public async Task<Student> AddStudent(Student student)
        {
            student.CreatedOn = DateTime.Now;
            dBContext.Students.Add(student);
           await dBContext.SaveChangesAsync();
            return student;
        }
        public Task<int> AddTeacher(Teacher teacher)
        {
            dBContext.Teachers.Add(teacher);
            return dBContext.SaveChangesAsync();
        }
        public int UpsertStudentSecretAnswer(List<AccountRecoveryAnswerModel> recoveryAnswer)
        {
            var entity = recoveryAnswer.Select(ans => new StudentAccountRecoveryAnswer
            {
                Answer = ans.Answer,
                Id = ans.Id,
                QuestionId = ans.QuestionId,
                StudentId = ans.StudentId,
            });
            if (recoveryAnswer.Where(p => p.Id > 0).Any())
            {
                entity.ToList().ForEach(item => item.Updated = DateTime.Now);
                dBContext.StudentAccountRecoveryAnswers.UpdateRange(entity.Where(s => s.Id > 0));
            }
            else
            {
                entity.ToList().ForEach(item => item.Created = DateTime.Now);
                dBContext.StudentAccountRecoveryAnswers.AddRange(entity.Where(s => s.Id == 0));
            }
            return dBContext.SaveChanges();
        }
        public List<StudentAccountRecoveryAnswer> GetStudentAccountRecoveryAnswers(int userid)
        {
            return dBContext.StudentAccountRecoveryAnswers.Where(s => s.StudentId == userid).ToList();
        }
        public int UpdateStudentPassword(int studentId, string password)
        {
            var student = dBContext.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                student.Password = _securePassword.Secure(_secretkey.StudentSaltKey, password);
                dBContext.Students.Update(student);
            }
            return dBContext.SaveChanges();

        }
      
        public async Task<int> AddTutor(Tutor entity)
        {
            dBContext.Tutors.Add(entity);
            return await dBContext.SaveChangesAsync();
        }

        public async Task<bool> IsEmailExists(string email, int? id)
        {
            if (id == null)
                return await dBContext.Users.AnyAsync(o => o.Email == email);
            else
                return await dBContext.Users.AnyAsync(k => k.Email == email && k.Id != id);

        }
        public async Task<bool> IsUserNameExists(string username, int? id)
        {
            if (id == null)
                return await dBContext.Users.AnyAsync(o => o.UserName == username);
            else
                return await dBContext.Users.AnyAsync(k => k.UserName == username && k.Id != id);

        }
        public bool IsStudentUserNameExists(string username, int ? id=0)
        {
            var entity = dBContext.Students.Where(p => p.UserName == username);
            if (id > 0)
                return entity.Any(p => p.Id != id && p.UserName == username);
            else
                return entity.Any();
        }
        public async Task<Student> GetStudentAsync(string username, string password) => await dBContext.Students.FirstOrDefaultAsync(s => s.UserName == username && s.Password == password);

       public async Task<List<Student>> GetAssociatedStudentsForParent(int parentUserId)
        {
            return await dBContext.Students.Where(p => p.UserID == parentUserId).ToListAsync();
        }
        public List<Student> GetAssociatedStudentsForTeacher(int teacherId)
        {
            return dBContext.Students.Where(s => dBContext.StudentInvitations.Where(st=>st.Response==1 & st.StudentId==s.Id).Select(s => s.StudentId).Contains(s.Id)).ToList();
        }
        public async Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    await userManager.AddToRolesAsync(user, roles);
                    if (user.HasUserAccess)
                        return dBContext.UserScreensAccess.Where(p => p.UserID == user.Id).Select(p => new ScreenFormeter { ScreenName = p.Screen }).ToList();
                    else
                        return dBContext.ScreenAccesses.Where(p => roles.Contains(p.RoleID.ToString())).Select(l => new ScreenFormeter { ScreenName = l.ScreenPermission }).ToList();
                }
                else
                    return default(List<ScreenFormeter>);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> role = null)
        {
            var screens = new List<ScreenFormeter>();
            if (dBContext.UserScreensAccess.Any(p => p.UserID == userID) && userID != null)
                screens.AddRange(await dBContext.UserScreensAccess.Where(p => p.UserID == userID).Select(o => new ScreenFormeter { ScreenName = o.Screen }).ToListAsync());

            if (role != null)
            {
                var roleid = dBContext.Roles.Where(p => role.Contains(p.Name)).Select(l => l.Id)?.ToList();
                screens.AddRange(dBContext.ScreenAccesses.Where(p => roleid.Contains(p.RoleID) && !screens.Select(s => s.ScreenName).Contains(p.ScreenPermission))
                    .Select(o => new ScreenFormeter { ScreenName = o.ScreenPermission }).ToList());
            }
            return screens;
        }
        public async Task<Student> GetStudentByUserName(string userid) => await dBContext.Students.Where(p => p.UserName == userid).FirstOrDefaultAsync();
        public async Task<Tutor> GetTutorByUserID(int userid) => await dBContext.Tutors.Where(p => p.UserId == userid).FirstOrDefaultAsync();
    }
}

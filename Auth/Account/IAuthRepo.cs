using Learning.Entities;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth.Account
{
    public interface IAuthRepo
    {
        Task<IdentityResult> AddUser(AppUser appUser, string password, AppRole app);
        Task<StudentModel> AddStudent(Student student);
        Task<int> AddTeacher(Teacher teacher);
        Task<int> AddTutor(Tutor entity);
        Task<bool> IsEmailExists(string email, int? id);
        Task<bool> IsUserNameExists(string email, int? id);
        bool IsStudentUserNameExists(string username, int? id = 0);
        Task<Student> GetStudentAsync(string username, string password);
        /// <summary>
        /// Get associated students from parent login
        /// </summary>
        /// <param name="parentUserId"></param>
        /// <returns></returns>
        Task<List<Student>> GetAssociatedStudentsForParent(int parentUserId);
        Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId = null);
        Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username);
        int UpsertStudentSecretAnswer(List<AccountRecoveryAnswerModel> recoveryAnswer);
        List<StudentAccountRecoveryAnswer> GetStudentAccountRecoveryAnswers(int userid);
        int UpdateStudentPassword(int studentId, string password);
        List<Student> GetAssociatedStudentsForTeacher(int teacherId);

    }
}

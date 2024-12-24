using Learning.Entities;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth.Account
{
    public interface IAuthService
    {
        Task<AppUser> GetUser(LoginViewModel viewModel);
        Task<AppUser> GetUserByUserId(string userid);
        Task<IdentityResult> AddUser(AppUser appUser, string password, AppRole role);
        Task<IdentityResult> UpdateUser(AppUser app);
        Task<StudentModel> AddStudent(Student student);
        Task<int> AddTeacher(Teacher teacher);
        Task<int> AddTutor(Tutor entity);
        Task<bool> IsEmailExists(string email, int? id);
        Task<bool> IsUserNameExists(string username, int? id);
        bool IsStudentUserNameExists(string username, int? id = 0);

        Task<string> SendEmailUserRegisterdNotificationBody(AppUser user, Tutor tutor);
        Task<string> EmailConfirmation(string token, string email);
        Task<bool> ForgotPassword(string email);
        Task<IdentityResult> ResetPassword(ForgotPasswordViewModel model);

        Task<Student> GetStudent(LoginViewModel viewModel);
        Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username);
        Task<List<Student>> GetAssociatedStudentsForParent(int parentUserId);
        List<Student> GetAssociatedStudentsForTeacher(int teacherId);
        Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId = null);
        int UpserStudentSecretAnswer(List<AccountRecoveryAnswerModel> recoveryAnswer);
        List<StudentAccountRecoveryAnswer> GetStudentAccountRecoveryAnswers(int userid);
        int UpdateStudentPassword(int studentId, string password);
        Task LogOut();
    }
}

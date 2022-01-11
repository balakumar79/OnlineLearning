using Learning.Entities;
using Learning.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth.Account
{
   public interface IAuthService
    {
        Task<AppUser> GetUser(LoginViewModel viewModel);
        Task<AppUser> GetUserByUserId(string userid);
        Task<IdentityResult> AddUser(AppUser appUser, string password,AppRole role);
        Task<IdentityResult> UpdateUser(AppUser app);
        Task<Student> AddStudent(Student student);
        Task<int> AddTeacher(Teacher teacher);
        Task<int> AddTutor(Tutor entity);
        Task<bool> IsEmailExists(string email, int? id);
        Task<bool> IsUserNameExists(string username, int? id);
        bool IsStudentUserNameExists(string username, int ? id=0);


        Task<string> EmailConfirmation(string token, int userid);
        Task<bool> ForgotPassword(string email);
        Task<IdentityResult> ResetPassword(ResetPasswordViewModel model);

        Task<Student> GetStudent(LoginViewModel viewModel);
        Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username);
        Task<List<Student>> GetAssociatedStudents(int parentUserId);
        Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId=null);
        
        Task LogOut();
    }   
}

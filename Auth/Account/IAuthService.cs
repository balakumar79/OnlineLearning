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
        Task<IdentityResult> AddUser(AppUser appUser, string password,AppRole role);
        Task<int> AddStudent(Student student);
        Task<bool> IsEmailExists(string email, int? id);
        Task<bool> IsUserNameExists(string username, int? id);
      
        Task<string> EmailConfirmation(string token, int userid);
        Task<bool> ForgotPassword(string email);
        Task<IdentityResult> ResetPassword(ResetPasswordViewModel model);
        Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username);
        Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId);
        
        Task LogOut();
    }   
}

using Learning.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth.Account
{
   public interface IAuthRepo
    {
        Task<IdentityResult> AddUser(AppUser appUser, string password,AppRole app);
        Task<int> AddStudent(Student student);
        Task<bool> IsEmailExists(string email, int? id);
        Task<bool> IsUserNameExists(string email, int? id);
       
        Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> roleId);
        Task<List<ScreenFormeter>> GetScreenAccessByUserName(string username);
        
    }
}

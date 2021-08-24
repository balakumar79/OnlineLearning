using Learning.Entities;
using Learning.Tutor.ViewModel;
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
   public class AuthRepo:IAuthRepo
    {
        readonly UserManager<AppUser> userManager;
        readonly RoleManager<AppRole> roleManager;
        private readonly AppDBContext dBContext;
       
        public AuthRepo(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,AppDBContext appDBContext)
        {
            
            this.dBContext = appDBContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
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
        public Task<int> AddStudent(Student student)
        {
             dBContext.Students.Add(student);
           return Task.FromResult(dBContext.SaveChangesAsync().Result);
        }
        public async Task<int> AddTutor(Tutor entity)
        {
            dBContext.Tutors.Add(entity);
           return await dBContext.SaveChangesAsync();
        }
       
        public async Task<bool> IsEmailExists(string email,int ?id)
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

        public async Task<List<ScreenFormeter>> GetScreenAccessPrivilage(int? userID, IList<string> role=default(IList<string>))
        {

            if (dBContext.UserScreensAccess.Any(p => p.UserID == userID) && userID != null)
                return await dBContext.UserScreensAccess.Where(p => p.UserID == userID).Select(o => new ScreenFormeter { ScreenName = o.Screen }).ToListAsync();
            else 
            {
            var roleid = dBContext.Roles.Where(p => role.Contains(p.Name)).Select(l=>l.Id).ToList();
                var screens=  dBContext.ScreenAccesses.Where(p => roleid.ToString().Contains(p.RoleID.ToString()))
                    .Select(o => new ScreenFormeter { ScreenName = o.ScreenPermission }).ToListAsync().Result;
                return screens;
            }
        }
        public async Task<Student> GetStudentById(int userid) => await dBContext.Students.Where(p => p.Id == userid).FirstOrDefaultAsync();
        public async Task<Tutor> GetTutorByUserID(int userid) => await dBContext.Tutors.Where(p => p.UserId == userid).FirstOrDefaultAsync();
       


    }
}

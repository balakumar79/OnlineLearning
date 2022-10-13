using Auth.Account;
using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Service
{
   public class ManageTutorService:IManageTutorService
    {
        private readonly IManageTutorRepo _manageTutorRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;
        public ManageTutorService(IManageTutorRepo manageTutorRepo,UserManager<AppUser> userManager,IAuthService authService)
        {

            this._authService = authService;
            this._userManager = userManager;
            this._manageTutorRepo = manageTutorRepo;
        }
        public async Task<IdentityResult> CreateTutor(TutorViewModel model)
        {
            var user = new AppUser
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                UserName = model.UserName,
            };
            var result = await _authService.AddUser(user, model.Password, new AppRole { Name = Utils.Enums.Roles.Tutor.ToString() });

            if (result.Succeeded)
            { 
                model.UserID = user.Id;
                await _manageTutorRepo.CreateTutor(model);
            }
            return result;
        }
       public async Task<List<TutorViewModel>> GetAllTutors()
        {
            return await _manageTutorRepo.GetAllTutors();
        }

        public bool DeleteUser(int id)
        {
            return _manageTutorRepo.DeleteUser(id);
        }

        public List<AppUser> GetAppUsers(int? id = 0)
        {
            return _manageTutorRepo.GetAppUsers(id);
        }
    }
}

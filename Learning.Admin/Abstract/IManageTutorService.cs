using Learning.Tutor.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
   public interface IManageTutorService
    {
        Task<IdentityResult> CreateTutor(TutorViewModel model);
        Task<List<TutorViewModel>> GetAllTutors();
    }
}

using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
   public interface IManageTutorRepo
    {
        Task<int> CreateTutor(TutorViewModel model);
        Task<List<TutorViewModel>> GetAllTutors();
    }
}

using Learning.Admin.Abstract;
using Learning.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Service
{
   public class ManageSubjectService :IManageSubjectService
    {
        private readonly IManageSubjectRepo _manageSubjectRepo;
        public ManageSubjectService(IManageSubjectRepo manageSubjectRepo)
        {
            _manageSubjectRepo = manageSubjectRepo;
        }
      public  IEnumerable<SubjectViewModel> GetSubjectViewModels(int? subjectId = 0)
        {
            return _manageSubjectRepo.GetSubjectViewModels(subjectId);
        }
        public Task<int> InsertSubjectLanguageVariant(SubjectViewModel model)
        {
            return _manageSubjectRepo.InsertSubjectLanguageVariant(model);
        }
    }
}

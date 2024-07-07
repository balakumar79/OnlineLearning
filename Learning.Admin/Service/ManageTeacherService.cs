using Learning.Admin.Abstract;
using Learning.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Service
{
    public class ManageTeacherService :IManageTeacherService
    {
        private readonly IManageTeacherRepo _manageTeacherRepo;
        public ManageTeacherService(IManageTeacherRepo manageTeacherRepo)
        {
            _manageTeacherRepo = manageTeacherRepo;
        }

        public async Task<IEnumerable<TeacherModel>> GetTeacherModels()
        {
            return await _manageTeacherRepo.GetTeacherModel();
        }
    }
}

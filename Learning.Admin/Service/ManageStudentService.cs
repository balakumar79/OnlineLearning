using Learning.Admin.Abstract;
using Learning.ViewModel.Account;
using System.Collections.Generic;

namespace Learning.Admin.Service
{
    public class ManageStudentService : IManageStudentService
    {
        readonly IManageStudentRepo _manageStudentRepo;
        public ManageStudentService(IManageStudentRepo manageStudentRepo)
        {
            _manageStudentRepo = manageStudentRepo;
        }

        public IEnumerable<StudentModel> GetStudents()
        {
            return _manageStudentRepo.GetStudents();
        }
    }
}

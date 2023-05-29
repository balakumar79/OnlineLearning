using Learning.ViewModel.Account;
using System.Collections.Generic;

namespace Learning.Admin.Abstract
{
    public interface IManageStudentRepo
    {
        IEnumerable<StudentModel> GetStudents();
    }
}

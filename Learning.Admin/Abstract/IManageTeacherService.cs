using Learning.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
    public interface IManageTeacherService
    {
        public Task<IEnumerable<TeacherModel>> GetTeacherModels();
    }
}

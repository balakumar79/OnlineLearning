using Learning.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.ViewModel.Admin
{
    public class ParentUserModel : AccountUserModel
    {
        public List<StudentModel> StudentModels { get; set; }
    }
}

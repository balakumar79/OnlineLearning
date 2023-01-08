using Learning.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
    public interface IManageSubjectRepo
    {
        IEnumerable<SubjectViewModel> GetSubjectViewModels(int? subjectId = 0);
        Task<int> InsertSubjectLanguageVariant(SubjectViewModel model);
    }
}

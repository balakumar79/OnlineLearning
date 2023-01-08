using Learning.Entities;
using Learning.ViewModel.Admin;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using Learning.Admin.Abstract;
using System.Threading.Tasks;

namespace Learning.Admin.Repo
{
   public class ManageSubjectRepo:IManageSubjectRepo
    {
        private readonly AppDBContext _dBContext;
        public ManageSubjectRepo(AppDBContext dBContext)
        {
            _dBContext = dBContext;

        }
        public IEnumerable<SubjectViewModel> GetSubjectViewModels(int?subjectId=0)
        {
            if (subjectId > 0)
                return _dBContext.TestSubjects.Include(l => l.SubjectLanguageVariants).Where(lang => lang.Id == subjectId).Select(lang => new SubjectViewModel
                {
                    Name = lang.SubjectName,
                    Active = lang.Active,
                    Id = lang.Id,
                    LanguageIds = lang.SubjectLanguageVariants.Select(variant => variant.LanguageId).ToArray(),
                    Languages = _dBContext.Languages.Select(l => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = l.Name,
                        Value = l.Id.ToString()
                    }).ToList()
                });

            return _dBContext.TestSubjects.Include(l => l.SubjectLanguageVariants).Select(sub => new SubjectViewModel { Id = sub.Id, Language = string.Join(", ", sub.SubjectLanguageVariants.Select(lang => lang.Language.Name)), Active = sub.Active, Name = sub.SubjectName });
        }
        public async Task<int> InsertSubjectLanguageVariant(SubjectViewModel model)
        {
            var entity = model.LanguageIds.Select(l => new SubjectLanguageVariant { SubjectId = model.Id, LanguageId = l });
            _dBContext.SubjectLanguageVariants.RemoveRange(_dBContext.SubjectLanguageVariants.Where(la => la.SubjectId == model.Id));
            _dBContext.SaveChanges();
            _dBContext.SubjectLanguageVariants.AddRange(entity);
          return await _dBContext.SaveChangesAsync();
        }
    }
}

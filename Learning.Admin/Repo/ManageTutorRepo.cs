using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Learning.ViewModel.Admin;

namespace Learning.Admin.Repo
{
   public class ManageTutorRepo:IManageTutorRepo
    {
        private readonly AppDBContext _dBContext;
        public ManageTutorRepo(AppDBContext dBContext)
        {
            this._dBContext = dBContext;
        }

        public async Task<List<TutorViewModel>> GetAllTutors()
        {
           
            return await (from tutor in _dBContext.Tutors
                    join lang in _dBContext.Languages on tutor.PreferredLanguage equals lang.Id
                    join usr in _dBContext.Users on tutor.UserId equals usr.Id
                    select new TutorViewModel
                    {
                        CreatedAt = tutor.CreatedAt,
                        Educations = tutor.Educations,
                        LanguagePreference=lang.Name,
                        Organization=tutor.Organization,
                        UserName=usr.UserName,
                        TutorID=tutor.TutorId,
                        LastLoggedIn=tutor.LastLoggedIn,
                        TutorType=tutor.TutorType.ToString(),
                        FirstName=usr.FirstName,
                        Email=usr.Email
                    }).ToListAsync();
        }

        public async Task<int> CreateTutor(TutorViewModel model)
        {
            var tutor = new Entities.Tutor
            {
                CreatedAt = DateTime.Now,
                Organization = model.Organization,
                PreferredLanguage = Convert.ToInt32(model.LanguagePreference),
                ModifiedAt=DateTime.Now,
                Educations=model.Educations,
                TutorType=Convert.ToInt32(model.TutorType),
                UserId=model.UserID
            };

            
            _dBContext.Tutors.Add(tutor);
           await _dBContext.SaveChangesAsync();
            
                var langList = new List<TutorLanguageOfInstruction>();
            if (model.LanguageOfInstruction != null)
            {
                foreach (var item in model.LanguageOfInstruction)
                {
                    langList.Add(new TutorLanguageOfInstruction
                    {
                        Language = item,
                        TutorID = tutor.TutorId
                    });
                }
                _dBContext.TutorLanguageOfInstructions.AddRange(langList);
                await _dBContext.SaveChangesAsync();
            }
            if (model.GradesTaken != null)
            {
                var gradeList = new List<TutorGradesTaken>();
                foreach (var grade in model.GradesTaken)
                {
                    gradeList.Add(new TutorGradesTaken
                    {
                        GradeID = grade,
                        TutorID = tutor.TutorId
                    });
                }
                _dBContext.TutorGradesTakens.AddRange(gradeList);
                await _dBContext.SaveChangesAsync();
            }
                return tutor.TutorId;
        }

        public List<ScreenAccessViewModel> GetScreenAccess(Utils.Enums.Roles? roles)
        {
            return _dBContext.ScreenAccesses.Select(s => new ScreenAccessViewModel
            {
                ScreenName = s.ScreenPermission,
                Roles = (ViewModel.Enums.Roles)s.RoleID
            }).ToList();
        }
                                                                                           public bool DeleteUser(int id)
        {
            _dBContext.Users.Remove(_dBContext.Users.FirstOrDefault(u => u.Id == id));
            return true;
        }
        public List<AppUser> GetAppUsers(int ? id = 0)
        {
            if (id > 0)
                return _dBContext.Users.Where(u => u.Id == id).ToList();
            return _dBContext.Users.ToList();
        }
    }
}

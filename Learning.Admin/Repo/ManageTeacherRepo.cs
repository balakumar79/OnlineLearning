using AutoMapper;
using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.ViewModel.Account;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.Repo
{
    public class ManageTeacherRepo:IManageTeacherRepo
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;

        public ManageTeacherRepo(AppDBContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
             _mapper= mapper;
        }

        public async Task<IEnumerable<TeacherModel>> GetTeacherModel()
        {
            return await _dbContext.Teachers.Include(d => d.AppUser).Select(d=>new TeacherModel
            {
                TeacherId= d.TeacherId,
                UserId= d.UserId,
                UserModel=_mapper.Map<AccountUserModel>(d.AppUser)
            }).ToListAsync();

        }
    }
}

using AutoMapper;
using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.Entities.Domain;
using Learning.Entities.Extension;
using Learning.ViewModel.Account;
using Learning.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Admin.Repo
{
    public class ManageParentRepo : IManageParentRepo
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        public ManageParentRepo(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public PaginationResult<ParentUserModel> GetParents(PaginationQuery paginationQuery)
        {
            return _dbContext.Users.Join(_dbContext.Students, s => s.Id, s => s.UserID, (u, s) => new { u, s })

                .Select(u => new ParentUserModel
                {
                    FirstName = u.u.FirstName,
                    LastName = u.u.LastName,
                    Email = u.u.Email,
                    CreatedAt = u.u.CreatedAt,
                    District = u.u.District,
                    UserID = u.u.Id,
                    UserName = u.u.UserName,
                    GenderId = u.u.Gender,
                    PhoneNumber = u.u.PhoneNumber,
                    EmailConfirmed = u.u.EmailConfirmed,
                    StudentModels = _mapper.Map<List<StudentModel>>(u.u.Students),
                }).Paginate(paginationQuery);
        }
    }
}

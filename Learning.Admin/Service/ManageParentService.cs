using Learning.Admin.Abstract;
using Learning.Entities.Domain;
using Learning.ViewModel.Account;
using Learning.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Admin.Service
{
    public class ManageParentService : IManageParentService
    {
        private readonly IManageParentRepo _manageParentRepo;
        public ManageParentService(IManageParentRepo manageParentRepo)
        {
            _manageParentRepo = manageParentRepo;
        }

        public PaginationResult<ParentUserModel> GetParents(PaginationQuery paginationQuery)
        {
            return _manageParentRepo.GetParents(paginationQuery);
        }
    }
}

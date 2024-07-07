using Learning.Entities.Domain;
using Learning.ViewModel.Admin;

namespace Learning.Admin.Abstract
{
    public interface IManageParentService
    {
        PaginationResult<ParentUserModel> GetParents(PaginationQuery paginationQuery);
    }
}

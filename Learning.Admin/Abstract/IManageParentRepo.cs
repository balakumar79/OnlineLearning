using Learning.Entities.Domain;
using Learning.ViewModel.Admin;

namespace Learning.Admin.Abstract
{
    public interface IManageParentRepo
    {
        PaginationResult<ParentUserModel> GetParents(PaginationQuery paginationQuery);
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Learning.API
{
    public interface IResponseFormatter
    {
        JsonResult JsonResult<T>(T response, string message=null, bool? result=null, object? description=null);
    }
}

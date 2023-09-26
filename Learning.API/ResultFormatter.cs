using Learning.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Learning.API
{
    public static class ResponseFormat
    {


        //public static bool? Result { get; set; }
        //public static string Message { get; set; }
        //public static object? Description { get; set; }

        public static JsonResult JsonResult<T>(T response, string message = "", bool? result = true, object? description = null, PaginationQuery pagination = null)
        {
            return new JsonResult(new { message, result, description, response, pagination });
        }
        public static JsonResult JsonResult(string message, bool? result = true, object? description = null)
        {
            return new JsonResult(new { message, result, description });
        }
    }
}

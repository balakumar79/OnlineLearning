using Microsoft.AspNetCore.Mvc;

namespace Learning.API
{
    public class ResponseFormat : IResponseFormatter
    {


        public bool? Result { get; set; }
        public string Message { get; set; }
        public object? Description { get; set; }
        public JsonResult JsonResult<T>(T response, string message, bool? result, object? description)
        {
            return new JsonResult(new { message, result, description, response = response });
        }
    }
}

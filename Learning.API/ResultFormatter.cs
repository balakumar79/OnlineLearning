using Learning.Entities.Domain;
using Learning.Entities.Extension;
using Learning.ViewModel.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Learning.API
{
    public static class ResponseFormat
    {


        //public static bool? Result { get; set; }
        //public static string Message { get; set; }
        //public static object? Description { get; set; }

        public static JsonResult JsonResult<T>(T response, string message = "", bool? result = true, object? description = null, PaginationQuery pagination = null)
        {
            if (pagination == null)
                return new JsonResult(new { message, result, description, response });
            return new JsonResult(new { message, result, description, response, pagination });
        }
        public static JsonResult JsonResult(string message, bool? result = true, object? description = null)
        {
            return new JsonResult(new { message, result, description });
        }

        public static JsonResult EncryptJsonResult<T>(T response, string message = "", bool? result = true, object? description = null, PaginationQuery pagination = null)
        {
            return new JsonResult(JsonConvert.SerializeObject(new JsonResult(new { message, result, description, response, pagination })).EncryptJson());
        }

    }

    public static class ResponseHelper
    {
        public static JsonResult UpdateResponse<T>(this RepoResponse<T> result)
        {
            if (result.IsDuplicated)
            {
                result.Message ??= ValidateMessages.DUPLICATE_RECORD;
            }
            else if (result.IsSuccess)
            {
                result.Message ??= ValidateMessages.RECORD_UPDATED_SUCCESSFULLY;
            }

            return new JsonResult(new { result });
        }
    }
}
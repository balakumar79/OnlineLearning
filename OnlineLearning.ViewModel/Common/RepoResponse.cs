using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Learning.ViewModel.Common
{
    public class RepoResponse<T>
    {
        public RepoResponse() { }

        public RepoResponse(T result, string message = null, bool isSuccess = false, bool isDuplicated = false, object description = null)
        {
            Result = result;
            Message = message;
            IsSuccess = isSuccess;
            IsDuplicated = isDuplicated;
            Description = description;
        }
        private bool _isSuccess;
        public bool IsSuccess { get { return _isSuccess; } set { _isSuccess = value; } }
        public string Message { get; set; }
        public bool IsDuplicated { get; set; }
        public object Description { get; set; }
        public HttpStatusCode StatusCode { get => _isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest; }
        public T Result { get; set; }
    }

    public class RepoListResponse<T>
    {
        private bool _isSuccess;
        public bool IsSuccess { get; set; }
        public object Description { get; set; }
        public HttpStatusCode StatusCode { get => _isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest; }
        private List<T> _result;
        public List<T> Result { get { return _result; } set { _result = value; } }
        public string Message { get => _result?.Any() ?? false ? ValidateMessages.DATA_FOUND : ValidateMessages.NO_DATA_FOUND; }

    }

    public static class ResponseHelper
    {
        public static RepoResponse<T> UpdateResponse<T>(this RepoResponse<T> result)
        {
            if (result.IsDuplicated)
            {
                result.Message ??= ValidateMessages.DUPLICATE_RECORD;
            }
            else if (result.IsSuccess)
            {
                result.Message ??= ValidateMessages.RECORD_UPDATED_SUCCESSFULLY;
            }

            return result;
        }

        public static JsonResult AddResponse<T>(this RepoResponse<T> result)
        {
            if (result.IsDuplicated)
            {
                result.Message ??= ValidateMessages.DUPLICATE_RECORD;
            }
            else if (result.IsSuccess)
            {
                result.Message ??= ValidateMessages.RECORD_ADDED_SUCCESSFULLY;
            }

            return new JsonResult(result);
        }

        public static JsonResult GetListResponse<T>(this RepoListResponse<T> result)
        {
            return new JsonResult(result);
        }
    }
}

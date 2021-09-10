using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Learning.Middleware
{
   public class GlobalExceptionMiddleware
    {
        readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public GlobalExceptionMiddleware(RequestDelegate requestDelegate,ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                await HandlerExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandlerExceptionAsync(HttpContext httpContext,Exception exception)
        {
            httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var response = new { code = httpContext.Response.StatusCode, statusText =exception.InnerException==null? exception.Message:exception.InnerException.Message };
            var json = JsonConvert.SerializeObject(response);
            await httpContext.Response.WriteAsync(json);

        }
    }
}

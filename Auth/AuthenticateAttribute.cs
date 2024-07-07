using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthenticateAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _claim;
        public AuthenticateAttribute(string claim)
        {
            _claim = claim;

        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var identity = filterContext.HttpContext.User;
            //var test = identity.FindFirst(permission);
            if (!identity.Identity.IsAuthenticated || !identity.Claims.Any(p => _claim.Split(',').Any(s => s.TrimEnd() == p.Value)))
            {
                filterContext.Result = new ForbidResult();
            }
        }
    }
}

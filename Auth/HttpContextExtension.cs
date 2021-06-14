using Learning.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Auth
{
   public static class HttpContextExtension
    {
        public static async Task RefreshLoginAsync(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                var usermanager = httpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
                var signinmanager = httpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
                AppUser user = await usermanager.GetUserAsync(httpContext.User);
               await signinmanager.RefreshSignInAsync(user);
                //if (signinmanager.IsSignedIn(httpContext.User))
                //    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //await httpContext.SignOutAsync();
            }
        }
    }
}

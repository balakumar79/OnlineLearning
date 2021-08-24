using Auth;
using Learning.Entities;

using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using static Learning.ViewModel.Account.AuthorizationModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Learning.Auth
{
    public class AuthenticationConfig
    {
        public static void LearningAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(op=> { op.Cookie.Name = "TutorCookie";
                op.ExpireTimeSpan = TimeSpan.FromDays(4);
                op.Cookie.HttpOnly = true;
                op.Cookie.IsEssential = true;
                op.Cookie.MaxAge = TimeSpan.FromDays(4);
                op.SlidingExpiration = false;
            });
        }
       
        public static void LearningAuthorization(IServiceCollection services)
        {
            services.AddAuthorization((Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>)(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
                }
            }));
        }
       

        public async static Task DoLogin(HttpContext context, List<ScreenFormeter> screenFormeters,SessionObject sessionObject)
        {

            var claims = new List<Claim>();

            if (screenFormeters != null)
            {
                foreach (var screen in screenFormeters)
                    claims.Add(new Claim(CustomClaimTypes.Permission, screen.ScreenName));
            }

            claims.Add(new Claim(CustomClaimTypes.TutorID, sessionObject.Tutor.TutorID.ToString()));

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(300),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
                RedirectUri = "/account/login"
            };
            context.Session.SetObjectAsJson("UserObj", sessionObject);
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaims(claims);
            context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties).Wait();
            await context.RequestServices.GetRequiredService<UserManager<AppUser>>().AddClaimsAsync(sessionObject.User, claims);
        }

      
    }
}

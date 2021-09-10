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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        public static void LearningAuthorization(IServiceCollection services)
        {
            services.AddAuthorization((Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>)(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); authbuilder.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme); });
                }
            }));
        }


        public async static Task DoLogin(HttpContext context, List<ScreenFormeter> screenFormeters, SessionObject sessionObject,bool isPersist)
        {

            var claims = new List<Claim>();

            if (screenFormeters != null)
            {
                foreach (var screen in screenFormeters)
                    claims.Add(new Claim(CustomClaimTypes.Permission, screen.ScreenName));
            }
            foreach (var role in sessionObject.RoleID)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(CustomClaimTypes.TutorID, sessionObject.Tutor.TutorID.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, sessionObject.User.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, sessionObject.User.UserName));
            claims.Add(new Claim(ClaimTypes.Name, sessionObject.User.FirstName + " " + sessionObject.User.LastName));
            claims.Add(new Claim(ClaimTypes.Email, sessionObject.User.Email));
            claims.Add(new Claim(CustomClaimTypes.TutorID, sessionObject.Tutor.TutorID.ToString()));

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(4),
                IsPersistent = isPersist,
                RedirectUri = "/account/login",
                AllowRefresh = true
            };
            context.Session.SetObjectAsJson("UserObj", sessionObject);
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaims(claims);
            await context.SignInAsync(new ClaimsPrincipal(claimIdentity), authProperties);
            //await context.RequestServices.GetRequiredService<UserManager<AppUser>>().AddClaimsAsync(sessionObject.User, claims);
        }
    }
}

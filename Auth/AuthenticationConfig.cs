using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using static Learning.ViewModel.Account.AuthorizationModel;
using Microsoft.AspNetCore.Authentication;
using Learning.Utils.Config;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Learning.Auth
{
    public class AuthenticationConfig
    {
       public AppConfig _appConfig { get; set; }
        public static void LearningAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(IdentityApplicationDefault).AddCookie(op=> { op.ExpireTimeSpan = TimeSpan.FromDays(5);op.Cookie.Expiration = TimeSpan.FromDays(8); });
        }

        public static void LearningAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                //foreach (var item in Enum.GetValues(typeof(Utils.Enums.Roles)))
                //{

                //    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString());
                //        authbuilder.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme); });
                //}
            });
        }

                     /// <summary>
                     /// validate using identity
                     /// </summary>
                     /// <param name="context"></param>
                     /// <param name="screenFormeters"></param>
                     /// <param name="sessionObject"></param>
                     /// <param name="isPersist"></param>
                     /// <returns></returns>
        public async static Task DoLogin(HttpContext context, List<ScreenFormeter> screenFormeters, SessionObject sessionObject,bool isPersist)
        {

            var claims = new List<Claim>();

            if (screenFormeters != null)
            {
                foreach (var screen in screenFormeters)
                    claims.Add(new Claim(CustomClaimTypes.Permission,value: screen.ScreenName));
            }
            foreach (var role in sessionObject.RoleID)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier, sessionObject.User.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, sessionObject.User.UserName));
            claims.Add(new Claim(ClaimTypes.Name, sessionObject.User.FirstName + " " + sessionObject.User.LastName));
            claims.Add(new Claim(ClaimTypes.Email, sessionObject.User.Email));
            claims.Add(new Claim(CustomClaimTypes.TutorID, sessionObject.Tutor?.TutorID == null ? string.Empty : sessionObject.Tutor.TutorID.ToString()));

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersist,
                RedirectUri = "/account/login",
                AllowRefresh = false, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1), 
                
            };
            //context.Session.SetObjectAsJson("UserObj", sessionObject);
            var claimIdentity = new ClaimsIdentity(IdentityApplicationDefault);
            claimIdentity.AddClaims(claims);
            await context.SignInAsync(IdentityApplicationDefault, new ClaimsPrincipal(claimIdentity), authProperties);
            //await context.RequestServices.GetRequiredService<UserManager<AppUser>>().AddClaimsAsync(sessionObject.User, claims);
        }

        /// <summary>
        /// valildate using JWT
        /// </summary>
        /// <param name="screenFormeters"></param>
        /// <param name="sessionObject"></param>
        /// <param name="key">JWT secret key</param>
        /// <returns></returns>
        public static JsonResult DoLogin(SessionObject sessionObject,string key, List<ScreenFormeter> screenFormeters = null)
        {

            var claims = new List<Claim>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            if (screenFormeters != null)
            {
                foreach (var screen in screenFormeters)
                    claims.Add(new Claim(CustomClaimTypes.Permission, screen.ScreenName));
            }
            foreach (var role in sessionObject.RoleID)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier, sessionObject.User.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, sessionObject.User.UserName));
            claims.Add(new Claim(ClaimTypes.Name, sessionObject.User.FirstName + " " + sessionObject.User.LastName));
            if (sessionObject.Student != null)
                claims.Add(new Claim(CustomClaimTypes.StudentId, sessionObject.Student.Id.ToString()));
            if (sessionObject.Childs!=null)
            {
                var studentids =string.Join(",", sessionObject.Childs.Select(p => p.Id));
                claims.Add(new Claim(CustomClaimTypes.ChildIds, studentids));
            }
            if(sessionObject.Tutor!=null)
                claims.Add(new Claim(CustomClaimTypes.TutorID, sessionObject?.Tutor.TutorID.ToString()));
            if (sessionObject.TeacherId > 0)
            {
                claims.Add(new Claim(CustomClaimTypes.TeacherId, sessionObject.TeacherId.ToString()));

            }

            //var token = new JwtSecurityToken(null, null, claims);

            //return new JsonResult( new JwtSecurityTokenHandler().WriteToken(token));

            //var tokenHandler = new JwtSecurityTokenHandler();

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return new JsonResult(new { token = tokenHandler.WriteToken(token), expiry = tokenDescriptor.Expires });

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(7),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JsonResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }


    }
}

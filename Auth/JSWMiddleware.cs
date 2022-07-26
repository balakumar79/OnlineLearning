using Auth.Account;
using Learning.Entities;
using Learning.Utils.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Auth
{
   public class JSWMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppConfig _appConfig;
        public JSWMiddleware(RequestDelegate requestDelegate,AppConfig appConfig)
        {
            _appConfig= appConfig;
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext context,IAuthService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
               await attachUserToContext(context, token,authService);

            await _next(context);
        }
        private async Task attachUserToContext(HttpContext context,string token,IAuthService authService)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appConfig.SecretKey.SecretKeyValue);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // attach user to context on successful jwt validation
                context.Items["User"] = await authService.GetUserByUserId(userId);
            }
            catch(Exception ex)
            {

            }
        }
    }
}

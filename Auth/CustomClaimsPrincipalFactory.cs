using Auth.Account;
using Learning.Entities;
using Learning.Tutor.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Learning.Auth
{
   public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<Entities.AppUser> 
    {
        readonly IHttpContextAccessor _httpContext;
        readonly IAuthRepo _authService;
        readonly ITutorService _tutorService;

        public CustomClaimsPrincipalFactory(UserManager<Entities.AppUser> userManager, IHttpContextAccessor contextAccessor,
        IOptions<IdentityOptions> optionsAccessor, ITutorService tutorService,IAuthRepo authRepo )
            : base(userManager, optionsAccessor)
        {
            _httpContext = contextAccessor;
            _authService = authRepo;
            _tutorService = tutorService;
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Entities.AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            var screens = await _authService.GetScreenAccessPrivilage(user.Id, roles);
            var sessionObj = new SessionObject
            {
                User = user,
                RoleID = roles.ToList(),
                Student = null,
                Tutor = _tutorService.GetTutorProfile(user.Id),
                ScreenAccess = screens
            };

            _httpContext.HttpContext.Session.SetObjectAsJson("sessionObj", sessionObj);
            sessionObj.ScreenAccess.ForEach(screen =>
            {

                identity.AddClaim(new Claim(CustomClaimTypes.Permission, screen.ScreenName));
            });
            identity.AddClaim(new Claim(ClaimTypes.Actor, user.UserProfileImage ?? "[Click to edit profile]"));
            if(sessionObj.Tutor!=null)
            identity.AddClaim(new Claim(CustomClaimTypes.TutorID, sessionObj.Tutor.TutorID.ToString()));
            return identity;
        }
    }
}


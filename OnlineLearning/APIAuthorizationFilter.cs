using Learning.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Auth.APIAuthorizationFilter;
using static Auth.PermissionAuthorizationHandler;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Auth
{
    public class APIAuthorizationFilter
    {


        public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
        {
            UserManager<AppUser> _userManager;
            RoleManager<AppRole> _roleManager;
            SignInManager<AppUser> signIn;

            public PermissionAuthorizationHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                signIn = signInManager;
            }

            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
            {
                if (context.User == null)
                {
                    return;
                }
                var testuser = new AppUser
                {
                    UserName = "balakumar",
                    Email = "balakumar56@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "985568529",
                };

                //Get all the roles the user belongs to and check if any of the roles has the permission required
                // for the authorization to succeed.
                var test = await _userManager.CreateAsync(testuser, "test2$S");

                var user = await _userManager.FindByNameAsync("balakumar");
                var tutorRole = await _roleManager.FindByNameAsync("Tutor");
                if (tutorRole == null)
                {
                    var rolelist = new List<ScreenFormeter>();
                    rolelist.Add(new ScreenFormeter { ScreenName = Permissions.Tutor.DashBoardView });
                    rolelist.Add(new ScreenFormeter { ScreenName = Permissions.Tutor.ProfileEdit });
                    await _roleManager.CreateAsync(new AppRole { Name = "Tutor", ScreenAccessPrivileges = JsonConvert.SerializeObject(rolelist) }); ;
                    tutorRole = await _roleManager.FindByNameAsync("Administrators");

                }



                await _userManager.AddToRoleAsync(user, "Administrators");
                //await signIn.SignInAsync(user, true);

                var userRoleNames = await _userManager.GetRolesAsync(user);
                var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name)).ToList();
                foreach (var role in userRoles)
                {
                    var deser = JsonConvert.DeserializeObject<List<ScreenFormeter>>(role.ScreenAccessPrivileges);
                    foreach (var perm in deser)
                    {
                        await _roleManager.AddClaimAsync(tutorRole, new Claim(CustomClaimTypes.Permission, perm.ScreenName));
                    }
                    var roleClaims = _roleManager.GetClaimsAsync(role).Result;
                    var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                            x.Value == requirement.Permission &&
                                                            x.Issuer == "LOCAL AUTHORITY")
                                                .Select(x => x.Value);

                    if (permissions.Any())
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }

            public class PermissionRequirement : IAuthorizationRequirement
            {
                public string Permission { get; private set; }

                public PermissionRequirement(string permission)
                {
                    Permission = permission;
                }
            }
        }

    }  
}

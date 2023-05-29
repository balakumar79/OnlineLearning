﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Learning.Entities.Extension
{
    public static class SeedRole
    {
        public static void SeedRoles(RoleManager<AppRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Utils.Enums.Roles)))
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {

                    AppRole appRole = new AppRole();
                    appRole.Name = role.ToString();
                    _ = roleManager.CreateAsync(appRole).Result;
                }
            }
        }
    }
}

using System;
using Learning.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using UsingIdentity.Areas.Identity.Data;
//using UsingIdentity.Data;

[assembly: HostingStartup(typeof(Learning.Auth.IdentityHostingStartup))]
namespace Learning.Auth
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
           
            builder.ConfigureServices((context, services) => {
                services.AddIdentity<AppUser, AppRole>()
       .AddEntityFrameworkStores<AppDBContext>()
       .AddSignInManager<SignInManager<AppUser>>()
       .AddDefaultTokenProviders();
            });
        }
    }
}
using Learning.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineLearning.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Infrastruct
{
   public class Infrastruct
    {
        public static void AddDatabase(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<AppDBContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DBContext")));
        }


        //public static void AddMapperProfiles(IServiceCollection services)
        //{
        //    services.AddAutoMapper(typeof(AuthMapperProfiles), typeof(SalesMapperProfiles));
        //}


        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Microsoft.AspNetCore.Identity.UserManager<AppUser>>();
            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<AppDBContext>()
               .AddSignInManager<SignInManager<AppUser>>().AddDefaultTokenProviders();
        }
    }
}

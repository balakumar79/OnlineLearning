using Learning.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Auth.Account;
using Learning.Utils;
using Learning.Utils.Config;
using Learning.Tutor.Abstract;
using Learning.Tutor.Service;
using Learning.Tutor.Repo;
using Learning.Admin.Abstract;
using Learning.Admin.Repo;
using Learning.Admin.Service;
using System;
using Microsoft.AspNetCore.Http;

namespace Learning.Infrastructure
{
    public class Infrastructure
    {


        public static void AddDataBase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DBContext")));
        }
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AppConfig, AppConfig>();
            var smtpConfig = new SMTPConfig();
            configuration.Bind("SMTPConfig", smtpConfig);
            services.AddSingleton(smtpConfig);
            
            //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthRepo, AuthRepo>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<ITutorRepo, TutorRepo>();
            services.AddTransient<ITutorService, TutorService>();

            services.AddTransient<IManageTutorRepo, ManageTutorRepo>();
            services.AddTransient<IManageTutorService, ManageTutorService>();

        }
    }
}

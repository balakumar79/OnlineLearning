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
using Learning.Student.Abstract;
using Learning.Student.Repos;
using Learning.Student.Services;
using Microsoft.AspNetCore.DataProtection;

namespace Learning.Infrastructure
{
    public class Infrastructure
    {


        public static void AddDataBase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DBContext")));
        }

        public static void AddKeyContext(IServiceCollection services,IConfiguration configuration)
        {
            // Add a DbContext to store your Database Keys
            services.AddDbContext<LearningKeyContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DBContext")));
            services.AddDataProtection()
.PersistKeysToDbContext<LearningKeyContext>().SetApplicationName("LearningCommon");
        }
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AppConfig, AppConfig>();
            var smtpConfig = new SMTPConfig();
            configuration.Bind("SMTPConfig", smtpConfig);
            services.AddSingleton(smtpConfig);
            var secretKey = new SecretKey();
            configuration.Bind("SecretKey", secretKey);
            services.AddSingleton(secretKey);

            //services.AddTransient(_ => new MySqlConnection(configuration.GetConnectionString("DBContext")));
            //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthRepo, AuthRepo>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IStudentTestRepo, StudentTestRepo>();
            services.AddTransient<IStudentTestService, StudentTestService>();
            
            services.AddTransient<ITutorRepo, TutorRepo>();
            services.AddTransient<ITutorService, TutorService>();

            services.AddTransient<IManageTutorRepo, ManageTutorRepo>();
            services.AddTransient<IManageTutorService, ManageTutorService>();

        }
    }
}

using Auth.Account;
using Learning.Admin.Abstract;
using Learning.Admin.Repo;
using Learning.Admin.Service;
using Learning.Auth;
using Learning.Entities;
using Learning.LogMe;
using Learning.LogMe.Repo;
using Learning.Student.Abstract;
using Learning.Student.Repos;
using Learning.Student.Services;
using Learning.Teacher.Repos;
using Learning.Teacher.Services;
using Learning.Tutor.Abstract;
using Learning.Tutor.Repo;
using Learning.Tutor.Service;
using Learning.Utils;
using Learning.Utils.Config;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Learning.Infrastructure
{
    public class Infrastructure
    {

        private static string conn = "";
        public static void AddDataBase(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                conn = configuration.GetConnectionString("TestDBContext");
                services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(conn));
            }
            else
            {
                conn = configuration.GetConnectionString("DBContext");
                services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(conn));
            }
        }

        public static void AddKeyContext(IServiceCollection services, IConfiguration configuration)
        {
            // Add a DbContext to store your Database Keys
            services.AddDbContext<LearningKeyContext>(options =>
                options.UseSqlServer(conn));

            services.AddDataProtection().SetDefaultKeyLifetime(TimeSpan.FromDays(11))
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
            var encryptionKey = new EncryptionKey();
            configuration.Bind("EncryptionKey", encryptionKey);
            services.AddSingleton(encryptionKey);
            var conStr = new ConnectionString();
            conStr.ConnectionStr = conn;
            //configuration.Bind(conStr);
            services.AddSingleton(conStr);


            services.AddScoped<ISecurePassword, SecurePassword>();
            //services.AddTransient(_ => new MySqlConnection(configuration.GetConnectionString("DBContext")));
            //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthRepo, AuthRepo>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IStudentRepo, StudentRepo>();
            services.AddTransient<IStudentService, StudentService>();

            services.AddTransient<ITeacherRepo, TeacherRepo>();
            services.AddTransient<ITeacherService, TeacherService>();

            services.AddTransient<ITutorRepo, TutorRepo>();
            services.AddTransient<ITutorService, TutorService>();

            services.AddTransient<IManageTutorRepo, ManageTutorRepo>();
            services.AddTransient<IManageTutorService, ManageTutorService>();

            services.AddTransient<ILoggerRepo, LoggerRepo>();


        }
    }
}

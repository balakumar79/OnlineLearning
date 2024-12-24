using Learning.Auth;
using Learning.Entities;
using Learning.Entities.Enums;
using Learning.Middleware;
using Learning.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace TutorWebUI
{
    public class Startup
    {
        //test function not for internal use
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Learning.Infrastructure.Infrastructure.AddDataBase(services, Configuration, _hostEnvironment);
            services.AddIdentity<AppUser, AppRole>(op =>
            {

            }).AddEntityFrameworkStores<AppDBContext>()
            .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

            services.AddAuthorization(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Learning.Entities.Enums.Roles)))
                {
                    option.AddPolicy(item.ToString(), authbuilder =>
                    {
                        authbuilder.RequireRole(item.ToString());
                    });
                }
                option.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(IdentityApplicationDefault)
                .RequireAuthenticatedUser()
                .RequireRole(new string[] { Roles.Tutor.ToString(), Roles.Teacher.ToString(), Roles.Admin.ToString() }).Build();
            });
            AuthenticationConfig.LearningAuthentication(services);
            services.ConfigureApplicationCookie(op =>
            {
                op.Cookie.Name = ".AspNet.SharedCookie";
                op.ExpireTimeSpan = TimeSpan.FromDays(4);
                op.Cookie.HttpOnly = false;
                op.Cookie.IsEssential = true;
                op.Cookie.MaxAge = TimeSpan.FromDays(4);
                op.SlidingExpiration = true;
                //op.Cookie.Domain = "tutor.domockexam.com";
                op.Cookie.SameSite = SameSiteMode.Lax;
            });
            services.AddSession
           (op =>
           {
               op.IdleTimeout = TimeSpan.FromDays(20);
           });

            Learning.Infrastructure.Infrastructure.AddServices(services, Configuration);
            Learning.Infrastructure.Infrastructure.AddKeyContext(services, Configuration);
            services.AddControllersWithViews()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddScoped<UserManager<AppUser>>();

        }

        private IHostEnvironment _hostEnvironment;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Tutor/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseMiddleware<GlobalExceptionMiddleware>();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tutor}/{action=Dashboard}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private DirectoryInfo GetKeyRingDirInfo()
        {
            var startupAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var applicationBasePath = System.AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var keyRingDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, "KeyRing"));
                if (keyRingDirectoryInfo.Exists)
                {
                    return keyRingDirectoryInfo;
                }
                else
                {
                    directoryInfo.Parent.Create();
                    return keyRingDirectoryInfo;
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"KeyRing folder could not be located using the application root {applicationBasePath}.");
        }
    }
}

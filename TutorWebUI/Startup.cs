using Learning.Auth;
using Learning.Entities;
using Learning.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace TutorWebUI
{
    public class Startup
    {
        //test function not for internal use
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
          
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Learning.Infrastructure.Infrastructure.AddServices(services, Configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(op =>
            {
                op.Cookie.Name = ".AspNet.SharedCookie";
                op.ExpireTimeSpan = TimeSpan.FromDays(4);
                op.Cookie.HttpOnly = false;
                op.Cookie.IsEssential = true;
                op.Cookie.MaxAge = TimeSpan.FromDays(4);
                op.SlidingExpiration = true;
                op.Cookie.SameSite = SameSiteMode.None; 
                op.Cookie.Path = "/";
                op.AccessDeniedPath = "/Account/AccessDenied";
            });
            //AuthenticationConfig.LearningAuthentication(services);
            services.AddIdentity<AppUser, AppRole>(op=>
            {
               
            })
  .AddEntityFrameworkStores<AppDBContext>() .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
  .AddDefaultTokenProviders();

            Learning.Infrastructure.Infrastructure.AddDataBase(services, Configuration);

            Learning.Infrastructure.Infrastructure.AddKeyContext(services, Configuration);


            //services.ConfigureApplicationCookie(op =>
            //{
            //    op.Cookie.Name = ".AspNet.SharedCookie";
            //    op.ExpireTimeSpan = TimeSpan.FromDays(4);
            //    op.Cookie.HttpOnly = false;
            //    op.Cookie.IsEssential = true;
            //    op.Cookie.MaxAge = TimeSpan.FromDays(4);
            //    op.SlidingExpiration = true;
            //    op.Cookie.Domain = "localhost";
            //    op.Cookie.SameSite = SameSiteMode.Lax;
            //});
            //AuthenticationConfig.LearningAuthentication(services);



            //services.AddAuthorization(option =>
            //{
            //    foreach (var item in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
            //    {

            //        option.AddPolicy(item.ToString(), authbuilder =>
            //        {
            //            authbuilder.RequireRole(item.ToString());
            //        });

            //    }
            //});
            services.AddSession
            (op => { op.IdleTimeout = TimeSpan.FromDays(20);
               
            });

            services.AddControllersWithViews();
            //services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddRazorPages();
            services.AddScoped<UserManager<AppUser>>();

            
        }



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
                app.UseHsts();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();
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

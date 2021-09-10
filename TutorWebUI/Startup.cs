using Learning.Auth;
using Learning.Entities;
using Learning.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>()
  .AddEntityFrameworkStores<AppDBContext>()
  .AddDefaultTokenProviders();

            services.AddDataProtection()
.PersistKeysToFileSystem(GetKeyRingDirInfo())
.SetApplicationName("TutorPanel");
            Learning.Infrastructure.Infrastructure.AddDataBase(services, Configuration);
         
            services.AddScoped<UserManager<AppUser>>();
          
                
            services.AddAuthorization(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
                }
            });
    

            AuthenticationConfig.LearningAuthentication(services);

            services.AddSession(op => op.IdleTimeout = TimeSpan.FromDays(20));
            //services.ConfigureApplicationCookie(op =>
            //{
            //    op.Cookie.Name = "TutorCookie";
            //    op.ExpireTimeSpan = TimeSpan.FromDays(4);
            //    op.Cookie.HttpOnly = false;
            //    op.Cookie.IsEssential = true;
            //    op.Cookie.MaxAge = TimeSpan.FromDays(4);
            //    op.SlidingExpiration = true;
            //    op.DataProtectionProvider = DataProtectionProvider.Create(GetKeyRingDirInfo().FullName);
            //    op.Cookie.Domain = ".domockexam.com";
            //    op.Cookie.SameSite = SameSiteMode.Lax;
            //    op.Events.OnValidatePrincipal = (ctx) =>
            //    {
            //        if (ctx.Principal.HasClaim(x => x.Type == CookieAuthenticationDefaults.AuthenticationScheme)) return System.Threading.Tasks.Task.CompletedTask;
            //        return SecurityStampValidator.ValidatePrincipalAsync(ctx);
            //    };
            //});

            services.ConfigureApplicationCookie(op =>
            {
                op.Cookie.Name = "LocalTutorCookie";
                op.ExpireTimeSpan = TimeSpan.FromDays(4);
                op.Cookie.HttpOnly = false;
                op.Cookie.IsEssential = true;
                op.Cookie.MaxAge = TimeSpan.FromDays(4);
                op.SlidingExpiration = true;
                op.Cookie.Domain = "localhost";
                op.Cookie.SameSite = SameSiteMode.Lax;
            });


            services.AddAntiforgery(op => { op.Cookie.SameSite = SameSiteMode.Lax; });

            services.AddControllersWithViews();
            //services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddRazorPages();
            Learning.Infrastructure.Infrastructure.AddServices(services, Configuration);
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
            app.UseSession();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
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

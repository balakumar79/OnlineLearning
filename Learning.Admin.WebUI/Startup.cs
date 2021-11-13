using Learning.Auth;
using Learning.Entities;
using Learning.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Admin.WebUI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthorization((Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>)(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
                }
            }));
            //AuthenticationConfig.LearningAuthentication(services);
            Infrastructure.Infrastructure.AddDataBase(services, Configuration);
            services.AddSession();
            services.AddScoped<UserManager<AppUser>>();
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddSignInManager<SignInManager<AppUser>>().AddDefaultTokenProviders();
            services.AddDataProtection()
.SetApplicationName("LearningCommon");
            services.AddControllersWithViews();
            services.AddRazorPages();
            Learning.Infrastructure.Infrastructure.AddServices(services, Configuration);
            services.ConfigureApplicationCookie(op =>
            {
                op.Cookie.Name = ".AspNet.SharedCookie";
                op.ExpireTimeSpan = TimeSpan.FromDays(4);
                op.Cookie.HttpOnly = false;
                op.Cookie.IsEssential = true;
                op.Cookie.MaxAge = TimeSpan.FromDays(4);
                op.SlidingExpiration = true;
                op.Cookie.Domain = "localhost";
                op.Cookie.SameSite = SameSiteMode.Lax;
                //op.LoginPath = "/tutor.domockexam.com/account/login";
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

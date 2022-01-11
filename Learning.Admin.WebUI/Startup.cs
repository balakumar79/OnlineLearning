using Learning.Admin.Abstract;
using Learning.Admin.Repo;
using Learning.Admin.Service;
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
            Learning.Infrastructure.Infrastructure.AddServices(services, Configuration);


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddAuthorization((Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>)(option =>
            //{
            //    foreach (var item in Enum.GetValues(typeof(Utils.Enums.Roles)))
            //    {

            //        option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
            //    }
            //}));
            //AuthenticationConfig.LearningAuthentication(services);

           
          
//            services.AddDataProtection()
//.SetApplicationName("LearningCommon");
          
            services.AddScoped<IManageExamService, ManageExamService>();
            services.AddScoped<IManageExamRepo, ManageExamRepo>();
            services.ConfigureApplicationCookie(op =>
            {
                op.Cookie.Name = ".AspNet.SharedCookie";
                op.ExpireTimeSpan = TimeSpan.FromDays(4);
                op.Cookie.HttpOnly = false;
                op.Cookie.IsEssential = true;
                op.Cookie.MaxAge = TimeSpan.FromDays(4);
                op.SlidingExpiration = true;
                op.Cookie.Path = "/";
                op.Cookie.SameSite = SameSiteMode.Lax;
                //op.LoginPath = "/tutor.domockexam.com/account/login";
            });

            Infrastructure.Infrastructure.AddDataBase(services, Configuration);

            Learning.Infrastructure.Infrastructure.AddKeyContext(services, Configuration);

            services.AddSession();
            services.AddIdentity<AppUser, AppRole>(op =>
            {

            })
.AddEntityFrameworkStores<AppDBContext>().AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
.AddDefaultTokenProviders();

            services.AddControllersWithViews();
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
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

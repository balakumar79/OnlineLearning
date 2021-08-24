using Learning.Auth;
using Learning.Entities;
using Learning.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace OnlineLearning
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
            AuthenticationConfig.LearningAuthentication(services);

            services.AddAuthorization((Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>)(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
                }
            }));
            
            services.AddSession(op=>op.IdleTimeout=TimeSpan.FromMinutes(200));
            Learning.Infrastructure.Infrastructure.AddDataBase(services, Configuration);
            services.AddScoped<UserManager<AppUser>>();
            services.AddIdentity<AppUser, AppRole>()
               .AddEntityFrameworkStores<AppDBContext>()
               .AddSignInManager<SignInManager<AppUser>>()
               .AddDefaultTokenProviders();
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            
            app.UseCookiePolicy();
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

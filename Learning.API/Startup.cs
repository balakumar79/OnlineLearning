using Learning.Auth;
using Learning.Entities;
using Learning.Utils.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.API
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
            services.AddSingleton(op =>

                op.GetRequiredService<IOptions<SecretKey>>().Value
            );  
            AuthenticationConfig.LearningAuthentication(services);

            Infrastructure.Infrastructure.AddDataBase(services, Configuration);

            services.AddIdentity<AppUser, AppRole>()
             .AddEntityFrameworkStores<AppDBContext>()
             .AddSignInManager<SignInManager<AppUser>>()
             .AddDefaultTokenProviders();

            services.AddCors(policy =>
            {
                policy.AddPolicy("LearningCors", builder => { builder.WithOrigins(" http://localhost:3000, https://localhost:44300, https://domockexam.com").AllowAnyMethod().AllowAnyHeader(); });
            });
            services.AddMvc();
            services.AddAuthorization(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
                }
            });
            services.AddSession(op =>
            {
                op.IdleTimeout = TimeSpan.FromDays(1);
            });
            services.AddScoped<UserManager<AppUser>>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            Infrastructure.Infrastructure.AddServices(services, Configuration);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseSession();
         
            var cookoption = new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict };
            app.UseCookiePolicy(cookoption);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("LearningCors");
            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(  
                    name:"default",
                    pattern:"{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}

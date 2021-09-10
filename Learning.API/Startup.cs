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
            //services.AddSingleton(op =>

            //    op.GetRequiredService<IOptions<SecretKey>>().Value
            //);  
            services.AddCors();
            services.AddIdentity<AppUser, AppRole>()
        .AddEntityFrameworkStores<AppDBContext>()
        .AddSignInManager<SignInManager<AppUser>>()
        .AddDefaultTokenProviders();
            AuthenticationConfig.LearningAuthentication(services);

            Infrastructure.Infrastructure.AddDataBase(services, Configuration);
            services.AddAuthorization(option =>
            {
                foreach (var item in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
                {

                    option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
                }
            });
          
            //services.AddSession(op =>
            //{
            //    op.IdleTimeout = TimeSpan.FromDays(1);
            //});
            services.AddScoped<UserManager<AppUser>>();

            //services.AddMvc();
            //services.AddControllersWithViews();
            //services.AddRazorPages();
            Infrastructure.Infrastructure.AddServices(services, Configuration);
            //services.AddCors(policy =>
            //{
            //    policy.AddPolicy("LearningCors", builder =>
            //    {
            //        builder.WithOrigins("http://localhost:3000", "https://localhost:44315", "https://domockexam.com", "https://api.domockexam.com")
            //        .AllowAnyHeader().AllowAnyMethod();
            //    });
            //});
          


          
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x=>x.WithOrigins("http://localhost:3000", "https://localhost:44315", "https://domockexam.com", "https://api.domockexam.com")
                    .AllowAnyHeader().AllowAnyMethod()) ;
            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseSession();

            var cookoption = new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict };
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
          
            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=weatherforecast}/{action=get}/{id?}");
            });
        }
    }
}

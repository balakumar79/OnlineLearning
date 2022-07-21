using Learning.Auth;
using Learning.Entities;
using Learning.Utils.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            services.AddControllers(op =>
            {
                op.RespectBrowserAcceptHeader = true;
            });
            Infrastructure.Infrastructure.AddDataBase(services, Configuration);

            //services.AddScoped<UserManager<AppUser>>();

            //services.ConfigureApplicationCookie(op =>
            //{
            //    op.Cookie.Name = ".AspNet.SharedCookie";
            //    op.ExpireTimeSpan = TimeSpan.FromDays(4);
            //    op.Cookie.HttpOnly = false;
            //    op.Cookie.IsEssential = true;
            //    op.Cookie.MaxAge = TimeSpan.FromDays(4);
            //    op.SlidingExpiration = true;
            //    op.Cookie.SameSite = SameSiteMode.None;
            //    op.Cookie.Path = "/Forbidden-AccessDenied";
            //});

            services.AddCors();
            Learning.Infrastructure.Infrastructure.AddKeyContext(services,Configuration);

            services.AddIdentity<AppUser, AppRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
                op.SignIn.RequireConfirmedAccount = true;
            })
        .AddEntityFrameworkStores<AppDBContext>()
        .AddDefaultTokenProviders();
            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.SaveToken = true;
                op.RequireHttpsMetadata = false;
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience=false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey:SecretKeyValue"]))

                };
            });
            //services.AddAuthorization(option =>
            //{
            //    foreach (var item in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
            //    {

            //        option.AddPolicy(item.ToString(), authbuilder => { authbuilder.RequireRole(item.ToString()); });
            //    }
            //});

            services.AddSession(op =>
            {
                op.IdleTimeout = TimeSpan.FromDays(2);
            });
            services.AddMvc();
            //services.AddControllersWithViews();
            //services.AddRazorPages();
            Infrastructure.Infrastructure.AddServices(services, Configuration);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin()
            //WithOrigins("http://localhost:3000", "https://domockexam.com", "https://localhost:44315", "https://api.domockexam.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            //app.UseMiddleware<JSWMiddleware>();

            app.UseAuthorization();
            app.UseSession();
            var cookoption = new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax,CheckConsentNeeded=context=>true };
           
            //app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name:"default",pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

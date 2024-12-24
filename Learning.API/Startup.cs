using Learning.Auth;
using Learning.Entities;
using Learning.Entities.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.Text;

namespace Learning.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(op =>
            {
                op.RespectBrowserAcceptHeader = true;
            });
            Infrastructure.Infrastructure.AddDataBase(services, Configuration, _hostEnvironment);

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
            Infrastructure.Infrastructure.AddKeyContext(services, Configuration);

            services.AddIdentity<AppUser, AppRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
                op.SignIn.RequireConfirmedAccount = true;
            })
        .AddEntityFrameworkStores<AppDBContext>()
        .AddDefaultTokenProviders();
            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.SaveToken = true;
                op.RequireHttpsMetadata = false;
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey:SecretKeyValue"]))

                };
            });
            services.AddSwaggerGen(setup =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
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
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();
            //services.AddControllersWithViews();
            //services.AddRazorPages();
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(option =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ta-IN"),
                };
                option.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
                option.SupportedCultures = supportedCultures;
                option.SupportedUICultures = supportedCultures;
            });

            Infrastructure.Infrastructure.AddServices(services, Configuration);
        }
        private IHostEnvironment _hostEnvironment;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _hostEnvironment = env;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors(x => x.
            WithOrigins("http://localhost:3000", "https://domockexam.com", "https://www.domockexam.com", "https://localhost:44315", "https://api.domockexam.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            app.UseMiddleware<JSWMiddleware>();

            app.UseAuthorization();
            app.UseSession();
            var cookoption = new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax, CheckConsentNeeded = context => true };
            app.UseRequestLocalization();
            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1"));
            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        public void SeedRoles(RoleManager<AppRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {

                    AppRole appRole = new AppRole();
                    appRole.Name = role.ToString();
                    _ = roleManager.CreateAsync(appRole).Result;
                }
            }
        }
    }
}

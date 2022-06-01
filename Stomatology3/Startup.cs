using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stomatology3.Context;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Stomatology3.Interfaces;
using Stomatology3.Controllers.Auth;
using Stomatology3.Repositories;

namespace Stomatology3
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

            services.AddControllersWithViews();
            //services.AddSwaggerGen();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (!options.IsConfigured) options.UseSqlServer(Configuration.GetConnectionString("Stomatology"));
            });
            services.AddIdentityCore<User>()
            //var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie("Identity.Application")
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(Configuration.GetSection("Jwt:PrivateKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            });
            

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSingleton<IJwtHandlerAuth>(new JwtHandlerAuth(Configuration.GetSection("Jwt:PrivateKey").Value));
            services.AddScoped<IAppointmentsRepository1, AppointmentsRepository>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
            //    options.AddPolicy("user", policy => policy.RequireRole("user"));
            //    options.AddPolicy("taskUser", policy => policy.RequireRole("manager", "exec"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}

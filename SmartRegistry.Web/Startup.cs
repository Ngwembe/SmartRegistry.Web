using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Data.Service;
using SmartRegistry.Web.Domain;
using SmartRegistry.Web.Integration;
using SmartRegistry.Web.Interfaces;
using SmartRegistry.Web.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace SmartRegistry.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseMySql(Configuration.GetConnectionString("MySQLConnectionLocal")));
            options.UseMySql(Configuration.GetConnectionString("MySQLConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            // Automatically perform database migration
            
            // Important line of code to be uncommented after DEVELOPMENT
            //services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();

            //********************************************

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("AzureConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            //    {
            //        config.SignIn.RequireConfirmedEmail = true;
            //    })
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            services.AddScoped<IPatientHandler, PatientHandler>();
            services.AddScoped<IApiAccessor, ApiAccessor>();
            services.AddScoped<IReportingHandler, ReportingHandler>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "controllerActionRoute",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: null,
                    dataTokens: new { NameSpace = "default" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

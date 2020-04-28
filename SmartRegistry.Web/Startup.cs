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
using Microsoft.AspNetCore.Mvc;
using SmartRegistry.Web.Hubs;

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
            //.UseMySql(Configuration.GetConnectionString("MySQLConnection")));
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Automatically perform database migration

            // Important line of code to be uncommented after DEVELOPMENT
            //services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();

            //********************************************

            //services.AddIdentityCore<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
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

            services.AddSignalR();

            services.AddMvc().AddMvcOptions(opt => {
                opt.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddMvc();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddMvc().AddRazorPagesOptions(options => {                
            //    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Login}/{id?}");
            //    //template: "{controller=Dashboard}/{action=Index}/{id?}");
            //});

            app.UseSignalR(config => {
                config.MapHub<MessageHub>("/messages");
            });

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

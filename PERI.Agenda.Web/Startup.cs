using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();
            Core.Setting.Configuration = builder.Build();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddMvc();
            services.AddAutoMapper();

            services.AddAuthentication("MyCookieMiddlewareInstance")
            .AddCookie("MyCookieMiddlewareInstance", options =>
            {
                options.AccessDeniedPath = "/Main/Forbidden/";
                options.LoginPath = "/Authentication/";
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<BLL.VerifyUserAttribute>();

            services.Configure<Core.Emailer>(options => Core.Setting.Configuration.GetSection("SmtpClient").Bind(options));
            services.Configure<Core.GoogleReCaptcha>(options => Core.Setting.Configuration.GetSection("GoogleReCaptcha").Bind(options));

            services.AddScoped<BLL.ValidateReCaptchaAttribute>();

            services.AddDbContext<EF.AARSContext>(options => options.UseSqlServer(Core.Setting.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
            services.AddScoped<BLL.IUnitOfWork, BLL.UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRHub>("/AttendanceBroadcast");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}

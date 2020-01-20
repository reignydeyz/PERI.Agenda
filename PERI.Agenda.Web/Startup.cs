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
using System.IO;
using PERI.Agenda.BLL;

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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "API", Description = "Endpoints" });

                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });
            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
            });
            // AutoMapper
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AutoMapperProfileConfiguration());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication("MyCookieMiddlewareInstance")
            .AddCookie("MyCookieMiddlewareInstance", options =>
            {
                options.AccessDeniedPath = "/Main/Forbidden/";
                options.LoginPath = "/Authentication/";
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<BLL.VerifyUserAttribute>();

            services.Configure<Core.Emailer>(options => Core.Setting.Configuration.GetSection("Emailer").Bind(options));
            services.Configure<Core.GoogleReCaptcha>(options => Core.Setting.Configuration.GetSection("GoogleReCaptcha").Bind(options));

            services.AddScoped<BLL.ValidateReCaptchaAttribute>();

            services.AddDbContext<EF.AARSContext>(options => options.UseSqlServer(Core.Setting.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
            services.AddScoped<BLL.IUnitOfWork, BLL.UnitOfWork>();

            services.AddScoped<IAttendance, Attendance>();
            services.AddScoped<ICommunity, Community>();
            services.AddScoped<IEndUser, EndUser>();
            services.AddScoped<IEvent, Event>();
            services.AddScoped<IEventCategory, EventCategory>();
            services.AddScoped<IEventCategoryReport, EventCategoryReport>();
            services.AddScoped<IFirstTimer, FirstTimer>();
            services.AddScoped<IGroup, Group>();
            services.AddScoped<IGroupCategory, GroupCategory>();
            services.AddScoped<IGroupMember, GroupMember>();
            services.AddScoped<ILocation, Location>();
            services.AddScoped<ILookUp, LookUp>();
            services.AddScoped<IMember, Member>();
            services.AddScoped<IRegistrant, Registrant>();
            services.AddScoped<IReport, Report>();
            services.AddScoped<IRole, Role>();
            services.AddScoped<IRsvp, Rsvp>();
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
            });

            app.MapWhen(r => !r.Request.Path.Value.StartsWith("/swagger"), builder => {
                builder.UseMvc(routes => {
                    routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });
        }
    }
}

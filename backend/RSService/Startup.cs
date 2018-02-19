using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using RSData.Models;
using RSRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;
using RSService.BusinessLogic;
using RSService.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Hangfire;
using RSService.Controllers;
using RSService.DTO;

namespace RSService
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
           // services.AddHangfire(x => x.UseSqlServerStorage("Server=BUSWGVMINDEV3\\MSSQLSERVER12;Database=RoomPlannerDev;User Id=roomplanner;Password=roomplanner123"));

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidatorActionFilter));
            })
            .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            var connection = @"Server=BUSWGVMINDEV3\MSSQLSERVER12;Database=RoomPlannerDev;User Id=roomplanner;Password=roomplanner123";

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IPenaltyRepository, PenaltyRepository>();
            services.AddTransient<IAvailabilityRepository, AvailabilityRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRSBusiness, RSBusiness>();
            services.AddTransient<IDbOperation, DbOperation>();
            services.AddDbContext<RoomPlannerDevContext>(options => options.UseSqlServer(connection), optionsLifetime: ServiceLifetime.Singleton);

            services.AddCors(options => options.AddPolicy("Cors",
                builder =>
                {
                    builder.
                        AllowAnyOrigin().
                        AllowAnyMethod().
                        AllowAnyHeader().
                        AllowCredentials();
                }));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                     {
                         options.LoginPath = "/api/auth/login";
                         options.LogoutPath = "/api/auth/logout";
                         //options.Cookie.Domain = "http:// fctestweb1:888";
                         options.Events.OnRedirectToLogin = context =>
                         {
                             context.Response.StatusCode = 401;
                             return Task.CompletedTask;
                         };
                         options.Events.OnRedirectToAccessDenied = context =>
                         {
                             context.Response.StatusCode = 401;
                             return Task.CompletedTask;
                         };
                     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            app.UseCors("Cors");
            app.UseAuthentication();
           

            app.UseMvc();
        // *    app.UseHangfireDashboard();
        //  *  app.UseHangfireServer();
        //   * RecurringJob.AddOrUpdate<UserController>(x => x.EventReminder(),Cron.Minutely);




            /* var trackPackageRouteHandler = new Microsoft.AspNetCore.Routing.RouteHandler(context =>
             {
                 return Task.CompletedTask;
             });

            /* var builder = new Microsoft.AspNetCore.Routing.RouteBuilder(app, trackPackageRouteHandler);
             builder.MapRoute(
                 name: "default",
                 template: "{controller=Home}/{action=Index}/{id:int}");

             builder.MapRoute("EventListByHosts", "event/list/{hostId}", new
             {
                 controller = "RoomScheduler", action = nameof(Controllers.RoomSchedulerController.GetEventsByHosts),
             });

             builder.MapRoute("EventList", "event/list/{hostId?}", new
             {
                 controller = "RoomScheduler",
                 action = nameof(Controllers.RoomSchedulerController.GetEvents),
             });

             app.UseRouter(builder.Build());*/

            Mapper.Initialize(Configuration =>
            {
                Configuration.CreateMap<AddEventDto, Event>().ReverseMap();
                Configuration.CreateMap<EditEventDto, Event>().ReverseMap();
                Configuration.CreateMap<EditUserDto, User>().ReverseMap();

            });
        }
    }
}

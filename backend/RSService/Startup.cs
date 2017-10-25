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
using RSService.ViewModels;

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
            services.AddMvc();
            var connection = @"Server=BUSWGVMINDEV3\MSSQLSERVER12;Database=RoomPlannerDev;User Id=roomplanner;Password=roomplanner123";

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IDbTransaction, DbTransaction>();
            services.AddDbContext<RoomPlannerDevContext>(options => options.UseSqlServer(connection));

            

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                     {
                         options.LoginPath = "/api/auth/login";
                     });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();

            Mapper.Initialize(Configuration =>
            {
                Configuration.CreateMap<EventViewModel, Event>().ReverseMap();
            });
        }
    }
}

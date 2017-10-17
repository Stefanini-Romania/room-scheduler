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
            services.AddDbContext<RoomPlannerDevContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<User, IdentityRole>();
                    //.AddEntityFrameworkStores<RoomPlannerDevContext>();

            services.AddAuthentication("MyCookieAuthenticationScheme")
                    .AddCookie(options =>
                     {
                         options.Events.OnRedirectToLogin = (context) =>
                         {
                             context.Response.StatusCode = 401;
                             return Task.CompletedTask;
                         };
                         options.Events.OnRedirectToAccessDenied = (context) =>
                         {
                             context.Response.StatusCode = 403;
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

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using WebApplication.Database;
using Microsoft.EntityFrameworkCore.Design;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration config)
        {
            configuration = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            
            services.AddMvc();

            services.AddScoped<IFinancialService, FinancialService>();

            ConfigureDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();    

            app.UseEndpoints(x => x.MapControllers());
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<Db>((options) =>
            {
                options.UseNpgsql(configuration["Database:ConnectionString"]);
            });
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Airport.FlightManagementAPI.DataAccess;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Airport.FlightManagementAPI.Model;
using Airport.Infrastructure.Messaging;
using System;
using Airport.FlightManagementAPI.Events;
using Airport.FlightManagementAPI.Commands;
using Airport.FlightManagementAPI.Migrations;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.HealthChecks;


namespace Airport.FlightManagementAPI
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add DBContext classes
            var sqlConnectionString = _configuration.GetConnectionString("FlightManagementCN");
            services.AddDbContext<FlightManagementDBContext>(options => options.UseSqlServer("server=sqlserver,1433;user id=sa;password=8jkGh47hnDw89Haq8LN2;database=FlightManagement;"));

            // add messagepublisher classes
            var configSection = _configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];
            services.AddTransient<IMessagePublisher>((sp) => new RabbitMQMessagePublisher(host, userName, password, "Airport"));

            // Add framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "FlightManagement API", Version = "v1" });
            });

            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
                checks.AddSqlCheck("FlightManagementCN", _configuration.GetConnectionString("FlightManagementCN"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, FlightManagementDBContext dbContext)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .Enrich.WithMachineName()
                .CreateLogger();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            SetupAutoMapper();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightManagement API - v1");
            });

            // auto migrate db
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<FlightManagementDBContext>().MigrateDB();
            }
        }

        private void SetupAutoMapper()
        {
            // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RegisterFlight, Flight>();
                cfg.CreateMap<Flight, RegisterFlight>()
                    .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
                cfg.CreateMap<RegisterFlight, FlightRegistered>()
                    .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            });
        }
    }
}

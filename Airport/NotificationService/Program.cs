﻿using Airport.Infrastructure.Messaging;
using Airport.NotificationService.NotificiationChannels;
using Airport.NotificationService.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Airport.NotificationService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddEnvironmentVariables("DOTNET_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string rabbitMQHost = rabbitMQConfigSection["Host"];
                        string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                        string rabbitMQPassword = rabbitMQConfigSection["Password"];
                        return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "Pitstop", "Notifications", ""); ;
                    });

                    services.AddTransient<INotificationRepository>((svc) =>
                    {
                        var sqlConnectionString = hostContext.Configuration.GetConnectionString("NotificationServiceCN");
                        return new SqlServerNotificationRepository(sqlConnectionString);
                    });

                    services.AddTransient<IEMailNotifier>((svc) =>
                    {
                        var mailConfigSection = hostContext.Configuration.GetSection("Email");
                        string mailHost = mailConfigSection["Host"];
                        int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
                        string mailUserName = mailConfigSection["User"];
                        string mailPassword = mailConfigSection["Pwd"];
                        return new SMTPEmailNotifier(mailHost, mailPort, mailUserName, mailPassword);
                    });

                    services.AddHostedService<NotificationManager>();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }
}

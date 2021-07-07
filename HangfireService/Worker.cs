using Hangfire;
using Hangfire.MemoryStorage;
using HangfireService.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HangfireService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration config;

        public Worker(IConfiguration config)
        {
            this.config = config;
        }

        private string ResolveCron(string jobName)
        {
            var job = config.GetSection($"CronExpressions:{jobName}").Value;
            return job;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            GlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage();
            var serverOptions = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromMilliseconds(1000)
            };

            using (var server = new BackgroundJobServer(serverOptions))
            {
                JobConfigurations();
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }

        public void JobConfigurations()
        {
            //Startup Notifier
            BackgroundJob.Enqueue(() => new ServiceStartupNotifier().Run());

            //Daily HealthCheck
            RecurringJob.AddOrUpdate(
                nameof(DailyHealthCheck),
                () => new DailyHealthCheck().Run(),
                ResolveCron(nameof(DailyHealthCheck)));

            //Weekly Report Generation
            RecurringJob.AddOrUpdate(
                nameof(WeeklyReportGeneration),
                () => new WeeklyReportGeneration().Run(),
                ResolveCron(nameof(WeeklyReportGeneration)));
        }
    }
}
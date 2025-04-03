using Infrastructure.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddQuartzServices(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(BinanceFetchJob));

            config.AddJob<BinanceFetchJob>(options => options.WithIdentity(jobKey));
            config.AddTrigger(options =>
                options.ForJob(jobKey)
                    .WithIdentity($"{jobKey.Name}Trigger")
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(1).RepeatForever())
                    .UsingJobData("symbol1", "BTCUSDT_250627")
                    .UsingJobData("symbol2", "BTCUSDT_250926"));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        return services;
    }
}
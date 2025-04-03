using DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Json;

namespace Api.Extensions;

public static class ApiExtensions
{
    public static void AddSerilogWithConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((serviceProvider, lc) =>
        {
            lc.ReadFrom.Configuration(configuration)
                .ReadFrom.Services(serviceProvider)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/errors/log-.json",
                    rollingInterval: RollingInterval.Month,
                    retainedFileCountLimit: 2,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                .WriteTo.Logger(logConf => logConf
                    .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Information)
                    .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/info/log-.json",
                        rollingInterval: RollingInterval.Month,
                        retainedFileCountLimit: 2)
                );
        });
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();

            var migrations = context.Database.GetPendingMigrations();

            if (migrations.Any())
            {
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Database migration failed.");
        }
    }
}
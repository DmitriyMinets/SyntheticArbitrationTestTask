using Api.Extensions;
using Api.Middlewares;
using Application.Services;
using Core.Interfaces;
using Core.Interfaces.Repository;
using DataAccess;
using DataAccess.Repository;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddSerilogWithConfiguration(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IBinanceClient, BinanceClient>();
        builder.Services.AddScoped<IBinanceService, BinanceService>();
        builder.Services.AddScoped<IFuturesArbitrageRepository, FuturesArbitrageRepository>();
        builder.Services.AddScoped<ExceptionHandlerMiddleware>();
        builder.Services.AddHttpClient();
        builder.Services.AddQuartzServices();
        builder.Services.AddDbContext<DataContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        
        var app = builder.Build();
        app.ApplyMigrations();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
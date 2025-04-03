using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Jobs;

public class BinanceFetchJob(IBinanceService binanceService, ILogger<BinanceFetchJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var symbol1 = GetJobValue(context, "symbol1");
        var symbol2 = GetJobValue(context, "symbol2");
        
        logger.LogInformation("Запуск Quartz Job: Получение данных с Binance");

        try
        {
            await binanceService.ProcessFuturePricesAsync(symbol1, symbol2);
            logger.LogInformation("Данные успешно обработаны и сохранены.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обработке данных");
        }
    }

    private static string GetJobValue(IJobExecutionContext context, string jobName)
    {
        return context.MergedJobDataMap.GetString(jobName) ??
               throw new ArgumentException("Значение не найдено", nameof(jobName));
    }
}
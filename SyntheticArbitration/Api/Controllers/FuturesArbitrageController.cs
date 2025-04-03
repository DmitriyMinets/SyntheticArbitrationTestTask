using Api.Contracts;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route($"{Routes.BASE_URL}/[controller]/[action]")]
[ApiController]
public class FuturesArbitrageController(IBinanceService binanceService) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<FuturesArbitrage>> Get()
    {
        return await binanceService.GetFuturesArbitrageAsync();
    }
}
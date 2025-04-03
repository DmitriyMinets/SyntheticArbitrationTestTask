using Core.Abstractions;

namespace Core.Entities;

public class FuturesArbitrage : BaseEntity
{
    /// <summary>
    /// Название актива для фьючерсного контракта.
    /// </summary>
    public string Assets { get; init; } = string.Empty;

    /// <summary>
    /// Цена фьючерса на квартальный контракт.
    /// </summary>
    public decimal QuarterPrice { get; init; }

    /// <summary>
    /// Цена фьючерса на полуквартальный контракт (би-квартальный).
    /// </summary>
    public decimal BiQuarterPrice { get; init; }

    /// <summary>
    /// Разница между ценой полуквартального и квартального фьючерса.
    /// </summary>
    public decimal PriceDifference { get; init; }

    /// <summary>
    /// Время создания записи арбитража.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
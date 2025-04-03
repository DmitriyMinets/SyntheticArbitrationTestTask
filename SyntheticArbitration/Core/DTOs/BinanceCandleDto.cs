namespace Core.DTOs;

public record BinanceCandleDto
{
    /// <summary>
    /// Время закрытия свечи
    /// </summary>
    public long EndTimestamp { get; init; }

    /// <summary>
    /// Цена закрытия
    /// </summary>
    public decimal Close { get; init; }
}
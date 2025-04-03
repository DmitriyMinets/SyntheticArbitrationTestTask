using Core.Const;

namespace Infrastructure.Helpers;

public static class IntervalHelper
{
    private static readonly Dictionary<Interval, string> _intervalMap = new()
    {
        { Interval.OneHour, "1h" },
        { Interval.OneDay, "1d" }
    };

    public static string ToStringValue(this Interval interval) =>
        _intervalMap.TryGetValue(interval, out var value) ? value : throw new ArgumentException(null, nameof(interval));
}
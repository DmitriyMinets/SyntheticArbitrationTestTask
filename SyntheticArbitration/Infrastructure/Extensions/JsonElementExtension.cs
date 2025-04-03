using System.Globalization;
using System.Text.Json;

namespace Infrastructure.Extensions;

public static class JsonElementExtension
{
    public static decimal GetDecimalInvariant(this JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.String && decimal.TryParse(element.GetString(),
                CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        throw new InvalidOperationException($"Ошибка парсинга числа: {element}");
    }
}
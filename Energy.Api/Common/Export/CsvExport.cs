using System.Reflection;
using System.Text;

namespace Energy.Api.Common.Export;

/// <summary>
/// Rapor satırlarını (projection DTO'ları) tek tip CSV'ye dönüştüren yardımcı.
/// Kolon başlıkları DTO'nun public property adlarından üretilir; böylece her rapor
/// controller'ında elle kolon listesi tutmaya gerek kalmaz ve export mantığı tek
/// yerde toplanır (controller'lar ince kalır).
/// </summary>
public static class CsvExport
{
    /// <summary>Satır koleksiyonunu CSV byte dizisine dönüştürür.</summary>
    public static byte[] ToBytes<T>(IEnumerable<T>? rows)
    {
        var items = rows as IReadOnlyList<T> ?? rows?.ToList() ?? [];
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetIndexParameters().Length == 0)
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", props.Select(p => Escape(p.Name))));
        foreach (var item in items)
        {
            sb.AppendLine(string.Join(",", props.Select(p => Escape(Format(p.GetValue(item))))));
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string Format(object? value) => value switch
    {
        null => string.Empty,
        DateTime dt => dt.ToString("o"),
        IFormattable f => f.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
        _ => value.ToString() ?? string.Empty,
    };

    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
        return value;
    }
}


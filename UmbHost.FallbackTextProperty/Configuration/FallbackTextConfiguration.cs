using System.Globalization;
using System.Text.Json;

namespace UmbHost.FallbackTextProperty.Configuration;

public class FallbackTextConfiguration
{
    public string? FallbackTemplate { get; set; }
    public int? MaxChars { get; set; }
    public bool AllowNone { get; set; }
    public int? Rows { get; set; }

    // Manifest-only schemas surface ConfigurationObject as a dictionary (or JSON).
    // Map defensively rather than relying on PublishedDataType.ConfigurationAs<T>()
    // (which throws unless ConfigurationObject is already this type).
    public static FallbackTextConfiguration From(object? configurationObject)
    {
        var config = new FallbackTextConfiguration();
        if (configurationObject is null) return config;

        // Note: IDictionary<string,object?> and IDictionary<string,object> are the SAME type
        // after nullable-reference erasure, so one arm covers both (Umbraco surfaces the config
        // as Dictionary<string,object>, which matches at runtime).
        IDictionary<string, object?> map = configurationObject switch
        {
            IDictionary<string, object?> d => d,
            JsonElement je when je.ValueKind == JsonValueKind.Object
                => je.EnumerateObject().ToDictionary(p => p.Name, p => (object?)p.Value),
            _ => new Dictionary<string, object?>(),
        };

        config.FallbackTemplate = AsString(Get(map, "fallbackTemplate"));
        config.MaxChars = AsInt(Get(map, "maxChars"));
        config.AllowNone = AsBool(Get(map, "allowNone")) ?? false;
        config.Rows = AsInt(Get(map, "rows"));
        return config;
    }

    private static object? Get(IDictionary<string, object?> map, string key)
        => map.TryGetValue(key, out var v) ? v : null;

    private static string? AsString(object? v) => v switch
    {
        null => null,
        JsonElement je => je.ValueKind == JsonValueKind.String ? je.GetString() : je.ToString(),
        _ => v.ToString(),
    };

    private static int? AsInt(object? v) => v switch
    {
        null => null,
        int i => i,
        long l => (int)l,
        JsonElement je when je.ValueKind == JsonValueKind.Number => je.GetInt32(),
        _ => int.TryParse(AsString(v), NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : null,
    };

    private static bool? AsBool(object? v) => v switch
    {
        null => null,
        bool b => b,
        JsonElement je when je.ValueKind is JsonValueKind.True or JsonValueKind.False => je.GetBoolean(),
        _ => bool.TryParse(AsString(v), out var b2) ? b2 : (AsString(v) is "1" ? true : null),
    };
}

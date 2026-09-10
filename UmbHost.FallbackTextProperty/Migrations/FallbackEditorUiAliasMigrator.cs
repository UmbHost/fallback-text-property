using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Services;

namespace UmbHost.FallbackTextProperty.Migrations;

// The re-point logic, factored out of the migration so it can be unit-tested by mocking
// IDataTypeService. Re-points any data type using our schema aliases whose EditorUiAlias is missing
// or the legacy Wholething.* value to the new UmbHost.* UI alias. EditorAlias (the schema alias)
// is get-only and unchanged, so no stored property values are affected. Uses IDataTypeService
// (not raw SQL) so the data-type cache is refreshed. Idempotent — writes only when needed.
public class FallbackEditorUiAliasMigrator
{
    // schema alias -> new UI alias
    public static readonly IReadOnlyDictionary<string, string> AliasMap = new Dictionary<string, string>
    {
        ["FallbackTextstring"] = "UmbHost.PropertyEditorUi.FallbackTextstring",
        ["FallbackTextarea"] = "UmbHost.PropertyEditorUi.FallbackTextarea",
    };

    private const string LegacyPrefix = "Wholething.PropertyEditorUi.";

    private readonly IDataTypeService _dataTypeService;
    private readonly ILogger<FallbackEditorUiAliasMigrator> _logger;

    public FallbackEditorUiAliasMigrator(
        IDataTypeService dataTypeService, ILogger<FallbackEditorUiAliasMigrator> logger)
    {
        _dataTypeService = dataTypeService;
        _logger = logger;
    }

    // Returns the number of data types re-pointed.
    public async Task<int> RunAsync()
    {
        var changed = 0;
        foreach (var (schemaAlias, newUi) in AliasMap)
        {
            var dataTypes = await _dataTypeService.GetByEditorAliasAsync(schemaAlias);
            foreach (var dt in dataTypes)
            {
                var current = dt.EditorUiAlias;
                var needs = string.IsNullOrEmpty(current)
                            || current!.StartsWith(LegacyPrefix, StringComparison.Ordinal);
                if (!needs || current == newUi)
                {
                    continue;
                }

                dt.EditorUiAlias = newUi;
                await _dataTypeService.UpdateAsync(dt, Umbraco.Cms.Core.Constants.Security.SuperUserKey);
                changed++;
                _logger.LogInformation(
                    "UmbHost fallback: re-pointed data type '{Name}' EditorUiAlias '{Old}' -> '{New}'",
                    dt.Name, current, newUi);
            }
        }

        return changed;
    }
}

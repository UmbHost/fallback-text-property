using Umbraco.Cms.Infrastructure.Migrations;

namespace UmbHost.FallbackTextProperty.Migrations;

// Migration step (run once, tracked in umbracoKeyValue via the package migration plan) that
// re-points legacy Wholething.PropertyEditorUi.* data types to UmbHost.*. The work lives in the
// injected FallbackEditorUiAliasMigrator (unit-tested); this just runs it inside the migration.
public class FallbackEditorUiAliasMigration : AsyncMigrationBase
{
    private readonly FallbackEditorUiAliasMigrator _migrator;

    public FallbackEditorUiAliasMigration(IMigrationContext context, FallbackEditorUiAliasMigrator migrator)
        : base(context)
        => _migrator = migrator;

    protected override async Task MigrateAsync() => await _migrator.RunAsync();
}

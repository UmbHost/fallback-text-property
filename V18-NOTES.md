# Umbraco 18 — scoping notes (UmbHost.FallbackTextProperty)

**Branch:** `dev/v18` (branched from the rebranded `dev/v17`). v18 code changes live here; `dev/v17`
stays the Umbraco-17 LTS line.

**Target framework:** Umbraco 18 (released 2026-06-25) is an **STS** release on **.NET 10** — the same
TFM as Umbraco 17 LTS. So **no `TargetFramework` change** was needed; the v18 build stays `net10.0`.

## Status: DONE (built + tested against Umbraco.Cms.* 18.1.1)

## Package bumps (applied)
- `Umbraco.Cms.Web.Website` `17.6.2` → `18.1.1`
- `Umbraco.Cms.Web.Common` `17.6.2` → `18.1.1`
- `Handlebars.Net` `2.1.6` — unaffected (third-party).

## Removed-in-18 APIs fixed on this branch
- `FallbackTextProperty.cs` — `SetupFallbackTextProperty` moved from `PackageMigrationBase`
  (**removed** in 18, not merely obsolete) to `AsyncPackageMigrationBase`; `Migrate()` → `MigrateAsync()`
  returning `Task.CompletedTask`. The 18 `AsyncPackageMigrationBase` ctor signature is identical to the
  old base, so the ctor was unchanged. (`FallbackEditorUiAliasMigration` already used `AsyncMigrationBase`.)
- `Services/Impl/ParentFallbackTextResolver.cs` — `context.Content?.Parent` (property removed from
  `IPublishedContent` in 18) → `context.Content?.Parent()`, the `Umbraco.Extensions` friendly extension
  (`FriendlyPublishedContentExtensions.Parent`), already imported. No ctor/DI change needed.

## Not affected (confirmed against 18.1.1)
- Value converter, services/resolvers (other than Parent), the preview API controller + its
  `[Authorize(BackOfficeAccess)]` auth, the Lit client, schema/UI aliases, App_Plugins paths.
- `.Children`/`BlockListItem.ContentUdi` — not used by this package (grep clean).

## Verification
- `dotnet build -c Release` → 0 errors (2 pre-existing `CS8714` nullability warnings in
  `DictionaryExtensions`, unrelated). `dotnet test` → 15/15 pass. Verified against the local CMS
  checkout `origin/release/18.1.1`.

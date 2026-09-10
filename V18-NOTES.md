# Umbraco 18 — scoping notes (UmbHost.FallbackTextProperty)

**Branch:** `dev/v18` (branched from the rebranded `dev/v17`). v18 code changes live here; `dev/v17`
stays the Umbraco-17 LTS line.

**Target framework:** Umbraco 18 (released 2026-06-25) is an **STS** release on **.NET 10** — the same
TFM as Umbraco 17 LTS. So **no `TargetFramework` change** is needed; the v18 build stays `net10.0`.
The work is: bump the Umbraco package refs to `18.x` and fix the APIs removed in v18.

## Package bumps
- `Umbraco.Cms.Web.Website` `17.6.2` → `18.x`
- `Umbraco.Cms.Web.Common` `17.6.2` → `18.x`
- `Handlebars.Net` `2.1.6` — unaffected (third-party).

## Removed-in-18 APIs used by this package (fix on this branch)
- `FallbackTextProperty.cs:41` — `SetupFallbackTextProperty : PackageMigrationBase`. `PackageMigrationBase`
  is obsolete (removal v18) → change base to `AsyncPackageMigrationBase` and its `Migrate()` to the
  async override. (The new `FallbackEditorUiAliasMigration` already uses the current `AsyncMigrationBase`.)
- `Services/Impl/ParentFallbackTextResolver.cs:31` — `context.Content?.Parent`. `IPublishedContent.Parent`
  is obsolete (removal v18) → use the `Umbraco.Extensions` `Parent<T>()` extension, or
  `IDocumentNavigationQueryService` for keys. The resolver would then need the nav service injected
  (it currently uses only the published content) — small ctor change + DI is already in place.

## Not affected
- Value converter, services/resolvers (other than Parent), the preview API controller + its
  `[Authorize(BackOfficeAccess)]` auth, the Lit client, schema/UI aliases, App_Plugins paths.
- `.Children`/`BlockListItem.ContentUdi` — not used by this package (grep clean).

## Verify at implementation
- ⚠ Confirm `Umbraco.Cms.Web.*` 18.x is on nuget.org and the exact `AsyncPackageMigrationBase` ctor
  signature + the `Parent<T>()` extension namespace in 18, then bump + build + run the unit tests.

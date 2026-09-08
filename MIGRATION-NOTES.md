# FallbackTextProperty — v17 / .NET 10 migration notes

Working branch: `dev/v17`. Plan: `<designinglibraries>/docs/superpowers/plans/2026-09-07-fallback-text-property-v17.md`.

## Task 1 — retarget + strip legacy (done)

- **TFM:** `net472;net50` → **`net10.0`** (package + test project). ImplicitUsings + Nullable enabled.
- **Package refs:** the plan called for `Umbraco.Cms.Web.BackOffice 17.6.2`, but **that package was discontinued after v13** (nuget.org caps it at 13.16.1 — the backoffice moved client-side in v14). Replaced with the v14+ extension-library references:
  - `Umbraco.Cms.Web.Website` 17.6.2 (Core + value-converter/composer APIs)
  - `Umbraco.Cms.Web.Common` 17.6.2 (`AuthorizationPolicies.BackOfficeAccess` for the preview endpoint)
  - `Handlebars.Net` 2.1.6
- **nuget.config:** replaced the private wholething Azure DevOps feed (401, unauthorized) with `nuget.org`.
- **Test project:** was **xUnit**; the plan's task tests are all **NUnit**. Standardised on NUnit 4 + NUnit3TestAdapter + Moq 4.20; converted the existing `FallbackTextReferenceParserTests` from xUnit → NUnit.
- **Deleted:** `Extensions/PublishedSnapshotAccessorExtensions.cs` (snapshot gone), `Controllers/TemplateDataController.cs` (`UmbracoAuthorizedApiController` removed v14 — replaced by `Api/FallbackTextController` in Task 6).
- **Stripped** every `#if NET5_0_OR_GREATER / #else / #endif` block (kept the NET5 branch) and all `using Umbraco.Core.*` / `Umbraco.Web.*` v8 usings.

## Recorded build error surface (`dotnet build`, after Task 1)

All 6 errors are the same removed type — **`IPublishedSnapshotAccessor`** (CS0246):
- `Services/Impl/FallbackTextService.cs:18,30` — field + ctor param
- `Services/Impl/RootFallbackTextResolver.cs:13,15` — field + ctor param
- `Services/Impl/UrlFallbackTextResolver.cs:13,15` — field + ctor param

## Predicted remaining errors (suppressed until the snapshot type resolves) — reconciled per task

- `FallbackTextService`: `_publishedSnapshotAccessor.GetPublishedSnapshot().Content.GetById(...)`, `_dataTypeService.GetDataType(Guid).Configuration`, `propertyType.DataType.Configuration`, `GetTemplate(object)` → **Task 3** (`IPublishedContentCache.GetById(bool,·)` + `FallbackTextConfiguration.From(...)`; `IDataTypeService.GetAsync` is async → `BuildDictionaryAsync`).
- `RootFallbackTextResolver`: `snapshot.Content.GetAtRoot().First()` → **Task 4** (`context.Content.AncestorOrSelf(1)`, drop the accessor).
- `UrlFallbackTextResolver`: `snapshot.Content.GetByRoute(args[0])` → **Task 4** (inject `IPublishedContentCache`, `GetByRoute(false, path)` — ⚠verify overload).
- `FallbackValueConverter`: `ConvertIntermediateToXPath` override no longer on `IPropertyValueConverter` → **Task 5** (delete it; subclass `PropertyValueConverterBase`).

## Reconciliations the plan's draft tests need (real code, verified reading source)

- `FallbackTextFunctionReference` shape is **`Function` (string) + `Args` (string[]) + `Key`** — NOT `Argument`/`Key` only as Task 4's draft test assumed.
- `FallbackTextResolverContext` exposes **`Element`** + derived **`Content`** (`Element as IPublishedContent`) — there is **no `Owner`** member (Task 4 draft uses `context.Owner`).
- Concrete resolvers extend abstract `FallbackTextResolver` and have **parameterised constructors** (`IFallbackTextLoggerService`, some also a cache) — Task 4's draft `new ParentFallbackTextResolver()` won't compile. `CanResolve` lives on the base and switches on `reference.Function`.

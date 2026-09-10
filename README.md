# UmbHost.FallbackTextProperty

An Umbraco **textstring / textarea** property editor with a configurable **fallback/default value**.
When an editor leaves the field empty, the property resolves to a fallback rendered from a **Mustache
template** over node properties (e.g. `{{pageTitle}}`, `{{1234:heroTitle}}`, or resolver functions
like `parent`/`ancestor`/`url`). Built for **Umbraco 17 / .NET 10** (new-backoffice Lit editor with a
live preview of the resolved fallback).

> **Note:** Wholething has merged with **UmbHost**. `UmbHost.FallbackTextProperty` is the continued,
> Umbraco 17+ version of the original `Wholething.FallbackTextProperty` package, now maintained by
> UmbHost — hence the rename.

## Install

```bash
dotnet add package UmbHost.FallbackTextProperty
```

## Usage

Create a data type using **Textstring with Fallback** (`FallbackTextstring`) or **Textarea with
Fallback** (`FallbackTextarea`). Config:

- **Fallback Template** — a Mustache template used when the value is blank. Reference node properties
  (`{{pageTitle}}`), other nodes by id (`{{1234:heroTitle}}`), or resolver functions.
- **Maximum allowed characters**, **Number of rows** (textarea), **Allow none** (adds a *None* button
  that stores an explicit empty value).

The editor shows the resolved fallback as placeholder text; typing overrides it; clearing reverts to
the fallback; *None* stores `<none>` (renders empty).

## Migrating from `Wholething.FallbackTextProperty`

1. Remove the old package: `dotnet remove package Wholething.FallbackTextProperty`.
2. Add this one: `dotnet add package UmbHost.FallbackTextProperty`.
3. On next startup a **one-time migration** (Umbraco migration plan, tracked in `umbracoKeyValue`)
   re-points affected data types' Property Editor UI from `Wholething.PropertyEditorUi.*` to
   `UmbHost.PropertyEditorUi.*`. **The schema aliases `FallbackTextstring`/`FallbackTextarea` are
   unchanged, so stored content is preserved** — no value migration.
4. Verify in **Settings → Data Types** that the editors load.

## Maintainers — publishing

Releases publish to nuget.org via GitHub Actions **trusted publishing** (OIDC, no stored API key) on
a `v*` tag. This requires a Trusted Publisher policy configured on nuget.org for
`UmbHost/fallback-text-property` + the release workflow.

## License

MIT.

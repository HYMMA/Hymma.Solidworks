# Hymma.Solidworks Agent Guide

## Scope
- Applies to the entire repository unless a nested `AGENTS.md` exists.

## Module Map
- `Interop/`: Interop library (base, no dependencies).
- `Extensions/`: Extensions library (depends on `Interop`).
- `Addins/`: Addins library (depends on `Interop`).
- `Addins.Fluent/`: Fluent addins library (depends on `Addins`).
- `Samples/`: Sample projects and installers (includes `Samples/QRifyInstaller`).
- `UnitTestProject/`: Test project (Framework tests).
- `nugets/`: CI output folder for packed NuGet artifacts.
- `docs/`: Documentation (do not read unless requested).

## Cross-Domain Workflows
- Release pipeline is tag-driven. GitHub Actions build publishes NuGet packages only when pushing tags that match `v*` (see `.github/workflows/build.yml`).
- Tag format controls versions:
  - Tag format: `v2018.MINOR.PATCH` (example `v2018.3.1`).
  - Assembly/NuGet version becomes `2018.MINOR.PATCH`.
  - MSI version derived as `1.MINOR.PATCH`.
- Package order matters: Interop → Extensions/Addins → Addins.Fluent.

## Verification
- Build: `msbuild hymma.solidworks.sln -restore -p:Configuration=Release -p:RestorePackagesConfig=true`
- Tests: `vstest.console.exe /TestCaseFilter:"TestCategory=Framework" UnitTestProject\bin\Release\UnitTestProject.dll`

## Notes
- Avoid touching `docs/` unless explicitly asked.

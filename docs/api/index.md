# API Reference

This section contains the complete API reference for all Hymma.Solidworks packages.

## Namespaces

### [Hymma.Solidworks.Addins](Hymma.Solidworks.Addins.html)

The base framework for creating SolidWorks add-ins.

Key classes:
- `AddinBase` - Base class for all add-ins
- `CommandManager` - Manages commands and menus

### [Hymma.Solidworks.Addins.Fluent](Hymma.Solidworks.Addins.Fluent.html)

Fluent API for building add-ins with a clean, chainable syntax.

Key classes:
- `AddinMaker` - Base class for fluent add-ins
- `AddinUserInterface` - Entry point for UI building
- `CommandTabBuilder` - Builds command tabs
- `CommandGroupBuilder` - Builds command groups

### [Hymma.Solidworks.Extensions](Hymma.Solidworks.Extensions.html)

Extension methods that simplify SolidWorks API interactions.

Key extensions:
- `ModelDoc2Extensions` - Document operations
- `ISldWorksExtensions` - Application operations
- `FeatureExtensions` - Feature tree operations

### [Hymma.Solidworks.Interop](Hymma.Solidworks.Interop.html)

References to SolidWorks interop assemblies.

## Getting Started

If you're new to Hymma.Solidworks, start with:

1. [Getting Started Guide](../articles/getting-started.md)
2. [Fluent API Guide](../articles/fluent-api.md)
3. [Sample Add-ins](../articles/samples/qrify.md)

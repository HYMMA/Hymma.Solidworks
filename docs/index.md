---
_layout: landing
---

# Hymma.Solidworks

A collection of .NET libraries for building professional SolidWorks add-ins and extensions.

## Quick Navigation

<div class="row">
<div class="col-md-6">

### [Getting Started](articles/getting-started.md)
Learn how to create your first SolidWorks add-in using the Hymma framework.

</div>
<div class="col-md-6">

### [API Reference](api/index.md)
Complete API documentation generated from XML comments.

</div>
</div>

<div class="row" style="margin-top: 1rem;">
<div class="col-md-6">

### [Addins.Fluent Guide](articles/fluent-api.md)
Build add-ins using the intuitive fluent API for cleaner, more readable code.

</div>
<div class="col-md-6">

### [Extensions Reference](articles/extensions.md)
Explore the extension methods that simplify SolidWorks API interactions.

</div>
</div>

## Packages

| Package | Description |
|---------|-------------|
| [Hymma.Solidworks.Interop](api/Hymma.Solidworks.Interop.html) | SolidWorks Interop library references |
| [Hymma.Solidworks.Extensions](api/Hymma.Solidworks.Extensions.html) | Extension methods for SolidWorks API |
| [Hymma.Solidworks.Addins](api/Hymma.Solidworks.Addins.html) | Framework for native-looking SolidWorks add-ins |
| [Hymma.Solidworks.Addins.Fluent](api/Hymma.Solidworks.Addins.Fluent.html) | Fluent API wrapper for building add-ins |

## Installation

```powershell
# For the fluent API (recommended)
Install-Package Hymma.Solidworks.Addins.Fluent

# Or for the base add-in framework
Install-Package Hymma.Solidworks.Addins

# For extension methods only
Install-Package Hymma.Solidworks.Extensions
```

## Quick Example

```csharp
using Hymma.Solidworks.Addins.Fluent;
using System.Runtime.InteropServices;

[Guid("YOUR-GUID-HERE")]
[ComVisible(true)]
public class MyAddin : AddinMaker
{
    public override AddinUserInterface GetUserInterFace()
    {
        return new AddinUserInterface(this)
            .AddCommandTab()
                .WithTitle("My Tab")
                .AddCommandGroup()
                    .WithTitle("My Commands")
                    .Has()
                    .Commands(cmds => cmds
                        .Command("Do Something")
                            .OnClick(() => DoSomething()))
                    .SaveCommandGroup()
                .SaveCommandTab();
    }
}
```

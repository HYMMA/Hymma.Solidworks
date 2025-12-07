# Hymma.Solidworks

[![Build Status](https://github.com/HYMMA/Hymma.Solidworks/actions/workflows/build.yml/badge.svg)](https://github.com/HYMMA/Hymma.Solidworks/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Hymma.Solidworks.Addins.svg)](https://www.nuget.org/packages/Hymma.Solidworks.Addins)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A collection of .NET libraries for building SolidWorks add-ins and extensions.

## Packages

| Package | Description | NuGet |
|---------|-------------|-------|
| [Hymma.Solidworks.Interop](./Interop) | SolidWorks Interop library references | [![NuGet](https://img.shields.io/nuget/v/Hymma.Solidworks.Interop.svg)](https://www.nuget.org/packages/Hymma.Solidworks.Interop) |
| [Hymma.Solidworks.Extensions](./Extensions) | Extension methods for SolidWorks API | [![NuGet](https://img.shields.io/nuget/v/Hymma.Solidworks.Extensions.svg)](https://www.nuget.org/packages/Hymma.Solidworks.Extensions) |
| [Hymma.Solidworks.Addins](./Addins) | Framework for native-looking SolidWorks add-ins | [![NuGet](https://img.shields.io/nuget/v/Hymma.Solidworks.Addins.svg)](https://www.nuget.org/packages/Hymma.Solidworks.Addins) |
| [Hymma.Solidworks.Addins.Fluent](./Addins.Fluent) | Fluent API wrapper for building add-ins | [![NuGet](https://img.shields.io/nuget/v/Hymma.Solidworks.Addins.Fluent.svg)](https://www.nuget.org/packages/Hymma.Solidworks.Addins.Fluent) |

## Quick Start

### Installation

```powershell
# For the fluent API (recommended)
Install-Package Hymma.Solidworks.Addins.Fluent

# Or for the base add-in framework
Install-Package Hymma.Solidworks.Addins

# For extension methods only
Install-Package Hymma.Solidworks.Extensions
```

### Using Addins.Fluent (Recommended)

```csharp
using Hymma.Solidworks.Addins.Fluent;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
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
                .That()
                .IsVisibleIn(swDocumentTypes_e.swDocPART, swDocumentTypes_e.swDocASSEMBLY)
                .AddCommandGroup()
                    .WithTitle("My Commands")
                    .WithIcon(Resources.MyIcon)
                    .Has()
                    .Commands(cmds => cmds
                        .Command("Do Something")
                            .WithIcon(Resources.CommandIcon)
                            .OnClick(() => DoSomething())
                        .Command("Another Action")
                            .WithIcon(Resources.AnotherIcon)
                            .OnClick(() => AnotherAction()))
                    .SaveCommandGroup()
                .SaveCommandTab();
    }

    private void DoSomething()
    {
        // Your code here
    }

    private void AnotherAction()
    {
        // Your code here
    }
}
```

### Using Addins (Base Framework)

```csharp
using Hymma.Solidworks.Addins;
using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;

[Guid("YOUR-GUID-HERE")]
[ComVisible(true)]
public class MyAddin : AddinBase
{
    public override bool OnConnect()
    {
        // Initialize your add-in
        return true;
    }

    public override bool OnDisconnect()
    {
        // Clean up resources
        return true;
    }
}
```

## Compatibility

| SolidWorks Version | .NET Framework | Status |
|-------------------|----------------|--------|
| 2021+ | 4.8 | ✅ Supported |
| 2019-2020 | 4.7.2+ | ✅ Supported |
| 2018 | 4.6.2+ | ✅ Supported |
| 2017 and earlier | 4.5.2+ | ⚠️ May work, not tested |

> **Note:** SolidWorks add-ins require .NET Framework (not .NET Core/.NET 5+) due to COM interop requirements.

## Sample Add-ins

This repo includes two sample add-ins to help you get started:

- **[QRify](./Samples/QRify)** - Built with `Hymma.Solidworks.Addins`
  - Converts text to QR code and copies to clipboard

- **[QRify+](./Samples/QrifyPlus)** - Built with `Hymma.Solidworks.Addins.Fluent`
  - Converts custom property values to QR codes with suffix support

Both samples are free for commercial use and include `.snk` files for immediate building after cloning.

## Documentation

- [Addins Framework Guide](./Addins/README.md)
- [Fluent API Guide](./Addins.Fluent/README.md)
- [API Documentation](https://hymma.github.io/Hymma.Solidworks/) *(coming soon)*

## Contributing

We welcome contributions! Please see our [Contributing Guide](./CONTRIBUTING.md) for details.

## License

[MIT](./LICENSE) © 2021 HYMMA

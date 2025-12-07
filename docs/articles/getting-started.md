# Getting Started

This guide will help you create your first SolidWorks add-in using Hymma.Solidworks.

## Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.8 SDK
- SolidWorks 2018 or later (for testing)

## Installation

Install the NuGet package:

```powershell
Install-Package Hymma.Solidworks.Addins.Fluent
```

Or via .NET CLI:

```bash
dotnet add package Hymma.Solidworks.Addins.Fluent
```

## Create Your First Add-in

### 1. Create a Class Library Project

Create a new **Class Library (.NET Framework)** project targeting **.NET Framework 4.8**.

### 2. Add the NuGet Package

Install `Hymma.Solidworks.Addins.Fluent` from NuGet.

### 3. Create the Add-in Class

```csharp
using Hymma.Solidworks.Addins.Fluent;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Runtime.InteropServices;

namespace MyFirstAddin
{
    [Guid("12345678-1234-1234-1234-123456789ABC")]
    [ComVisible(true)]
    public class MyAddin : AddinMaker
    {
        public override AddinUserInterface GetUserInterFace()
        {
            return new AddinUserInterface(this)
                .AddCommandTab()
                    .WithTitle("My Add-in")
                    .That()
                    .IsVisibleIn(swDocumentTypes_e.swDocPART, swDocumentTypes_e.swDocASSEMBLY)
                    .AddCommandGroup()
                        .WithTitle("Commands")
                        .WithDescription("My custom commands")
                        .Has()
                        .Commands(cmds => cmds
                            .Command("Say Hello")
                                .WithDescription("Shows a greeting message")
                                .OnClick(() => SayHello()))
                        .SaveCommandGroup()
                    .SaveCommandTab();
        }

        private void SayHello()
        {
            Solidworks.SendMsgToUser2(
                "Hello from Hymma.Solidworks!",
                (int)swMessageBoxIcon_e.swMbInformation,
                (int)swMessageBoxBtn_e.swMbOk);
        }
    }
}
```

### 4. Configure COM Registration

Add the following to your `.csproj` file:

```xml
<PropertyGroup>
    <RegisterForComInterop>true</RegisterForComInterop>
</PropertyGroup>
```

### 5. Sign the Assembly

1. Right-click the project → Properties → Signing
2. Check "Sign the assembly"
3. Create a new strong name key file

### 6. Build and Register

Build the project in Release mode. The add-in will be automatically registered for COM interop.

## Next Steps

- Learn about the [Fluent API](fluent-api.md) in detail
- Explore [Extension Methods](extensions.md)
- Check out the [Sample Add-ins](samples/qrify.md)

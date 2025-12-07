# QRify+

An advanced QR code generation add-in for SOLIDWORKS built using the **Fluent API** from [Hymma.Solidworks.Addins.Fluent](../../Addins.Fluent). This sample demonstrates how to create a feature-rich add-in with Property Manager Pages, tabs, groups, and popup dialogs.

## Features

- Generate QR codes from custom property values
- Property Manager Page with multiple tabs and controls
- WPF and WinForms popup dialog integration
- Selection box with popup menu items
- Demonstrates the fluent builder pattern for UI construction

## Screenshot

*Coming soon*

## Dependencies

- .NET Framework 4.8
- Hymma.Solidworks.Interop 2018.3.3
- Hymma.Solidworks.Addins.Fluent 2018.3.3
- QRCoder 1.4.3

## Building

1. Clone the repository
2. Open `hymma.solidworks.sln` in Visual Studio 2022
3. Build the solution in Release mode
4. Register the add-in DLL with SOLIDWORKS

## Usage

1. Open a SOLIDWORKS Drawing document
2. Click on the **QRify+** tab in the CommandManager
3. Use the Property Manager Page to configure QR code generation
4. Access help via the **Qrify+ help** tab

## Code Highlights

This sample demonstrates:

### Fluent API for Command Tabs
```csharp
builder
    .AddCommandTab()
        .WithTitle("QRify+")
        .That()
        .IsVisibleIn(new[] { swDocumentTypes_e.swDocDRAWING })
        .SetCommandGroup(5)
            .WithTitle("Qrify+", AddinConstants.SolidworksMenu.View)
            .WithIcon(Properties.Resources.qrifyPlus)
            .Has()
                .Commands(() => new AddinCommand[] { ... })
        .SaveCommandGroup()
    .SaveCommandTab()
```

### Property Manager Page with Tabs
```csharp
builder
    .AddPropertyManagerPage("QRify+", this.SolidWorks)
        .AddTab<QrPlusTab>()  // Custom tab class
        .AddTab("Settings", Properties.Resources.infoPlus)
            .AddGroup("Settings Controls")
                .That()
                .HasTheseControls(GetSettingsControls)
                .SetExpansion(true)
            .SaveGroup()
        .SaveTab()
    .OnClosing((p, r) => closeCallBackRegistry.DuringClose(r))
    .OnAfterClose((p, r) => closeCallBackRegistry.AfterClose())
    .SavePropertyManagerPage(out PropertyManagerPageX64 pmpFactory);
```

### Popup Dialogs
```csharp
// WPF Dialog
var dia = SolidWorks.HookWpfWindow(new WpfPopupApp.MainWindow());
dia.Show();

// WinForms Dialog
var dia = SolidWorks.HookWinForm(new WinFormPopupApp.Form1());
dia.Show();
```

## License

Copyright 2024 HYMMA

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# QRify Sample

QRify is a sample add-in built using `Hymma.Solidworks.Addins` that converts text to QR codes.

## Features

- Convert any text to a QR code image
- Copy QR code to clipboard
- Integrates seamlessly into SolidWorks ribbon

## Source Code

The complete source code is available in the [QRify folder](https://github.com/HYMMA/Hymma.Solidworks/tree/main/QRify) of the repository.

## How It Works

QRify demonstrates the basic add-in framework:

```csharp
[Guid("...")]
[ComVisible(true)]
public class QrifyAddin : AddinBase
{
    public override bool OnConnect()
    {
        // Initialize add-in
        CreateUI();
        return true;
    }

    public override bool OnDisconnect()
    {
        // Clean up resources
        return true;
    }

    private void CreateUI()
    {
        // Set up commands and menus
    }
}
```

## Building

1. Clone the repository
2. Open `Hymma.Solidworks.sln` in Visual Studio
3. Build the QRify project
4. The add-in will be registered automatically

## See Also

- [QRify+ (Fluent Version)](qrifyplus.md)
- [Getting Started](../getting-started.md)

# QRify+ Sample

QRify+ is an enhanced version of QRify built using `Hymma.Solidworks.Addins.Fluent`.

## Features

- Convert custom property values to QR codes
- Add custom suffixes to QR text
- Clean, fluent code structure
- Full ribbon integration

## Source Code

The complete source code is available in the [QRifyPlus folder](https://github.com/HYMMA/Hymma.Solidworks/tree/main/QRifyPlus) of the repository.

## Fluent API Example

QRify+ demonstrates the fluent API approach:

```csharp
[Guid("...")]
[ComVisible(true)]
public class QrifyPlusAddin : AddinMaker
{
    public override AddinUserInterface GetUserInterFace()
    {
        return new AddinUserInterface(this)
            .AddCommandTab()
                .WithTitle("QRify+")
                .That()
                .IsVisibleIn(swDocumentTypes_e.swDocPART)

                .AddCommandGroup()
                    .WithTitle("QR Tools")
                    .WithIcon(Resources.QrIcon)
                    .Has()
                    .Commands(cmds => cmds
                        .Command("Generate QR")
                            .WithIcon(Resources.GenerateIcon)
                            .WithDescription("Generate QR from custom property")
                            .OnClick(() => GenerateQr())

                        .Command("Settings")
                            .WithIcon(Resources.SettingsIcon)
                            .OnClick(() => ShowSettings()))
                    .SaveCommandGroup()

                .SaveCommandTab();
    }

    private void GenerateQr()
    {
        // Get custom property value
        // Generate QR code
        // Copy to clipboard
    }

    private void ShowSettings()
    {
        // Show settings dialog
    }
}
```

## Comparison with QRify

| Aspect | QRify | QRify+ |
|--------|-------|--------|
| Framework | AddinBase | AddinMaker (Fluent) |
| Code Style | Imperative | Declarative |
| Lines of Code | More | Fewer |
| Readability | Good | Excellent |

## Building

1. Clone the repository
2. Open `Hymma.Solidworks.sln` in Visual Studio
3. Build the QRifyPlus project
4. The add-in will be registered automatically

## See Also

- [QRify (Base Version)](qrify.md)
- [Fluent API Guide](../fluent-api.md)

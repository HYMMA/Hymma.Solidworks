# Fluent API Guide

The `Hymma.Solidworks.Addins.Fluent` package provides an intuitive, chainable API for building SolidWorks add-ins.

## Overview

Instead of manually configuring commands, menus, and toolbars, the Fluent API lets you describe your UI declaratively:

```csharp
new AddinUserInterface(this)
    .AddCommandTab()
        .WithTitle("My Tab")
        .AddCommandGroup()
            .WithTitle("My Commands")
            .Has()
            .Commands(cmds => cmds
                .Command("Action 1").OnClick(() => DoAction1())
                .Command("Action 2").OnClick(() => DoAction2()))
            .SaveCommandGroup()
        .SaveCommandTab();
```

## Building Command Tabs

Command tabs appear in SolidWorks' ribbon interface.

### Basic Tab

```csharp
.AddCommandTab()
    .WithTitle("My Tab")
    // ... add groups
    .SaveCommandTab()
```

### Tab Visibility

Control which document types show your tab:

```csharp
.AddCommandTab()
    .WithTitle("Part Tools")
    .That()
    .IsVisibleIn(swDocumentTypes_e.swDocPART)
    .SaveCommandTab()
```

Multiple document types:

```csharp
.IsVisibleIn(
    swDocumentTypes_e.swDocPART,
    swDocumentTypes_e.swDocASSEMBLY,
    swDocumentTypes_e.swDocDRAWING)
```

## Building Command Groups

Command groups organize related commands together.

### Basic Group

```csharp
.AddCommandGroup()
    .WithTitle("My Commands")
    .Has()
    .Commands(cmds => cmds
        .Command("Do Something")
            .OnClick(() => DoSomething()))
    .SaveCommandGroup()
```

### Group with Icon

```csharp
.AddCommandGroup()
    .WithTitle("Tools")
    .WithIcon(Properties.Resources.ToolsIcon)
    .Has()
    .Commands(/* ... */)
    .SaveCommandGroup()
```

### Group with Description

```csharp
.AddCommandGroup()
    .WithTitle("Export Tools")
    .WithDescription("Commands for exporting data")
    .Has()
    .Commands(/* ... */)
    .SaveCommandGroup()
```

## Building Commands

### Basic Command

```csharp
.Command("Export PDF")
    .OnClick(() => ExportToPdf())
```

### Command with Icon

```csharp
.Command("Export PDF")
    .WithIcon(Properties.Resources.PdfIcon)
    .OnClick(() => ExportToPdf())
```

### Command with Description

```csharp
.Command("Export PDF")
    .WithDescription("Export the current document to PDF format")
    .OnClick(() => ExportToPdf())
```

### Command with Enable Condition

```csharp
.Command("Save Selection")
    .OnClick(() => SaveSelection())
    .EnableWhen(() => HasSelection())
```

## Complete Example

```csharp
[Guid("YOUR-GUID")]
[ComVisible(true)]
public class ExportAddin : AddinMaker
{
    public override AddinUserInterface GetUserInterFace()
    {
        return new AddinUserInterface(this)
            .AddCommandTab()
                .WithTitle("Export Tools")
                .That()
                .IsVisibleIn(
                    swDocumentTypes_e.swDocPART,
                    swDocumentTypes_e.swDocASSEMBLY)

                .AddCommandGroup()
                    .WithTitle("File Export")
                    .WithIcon(Resources.ExportIcon)
                    .WithDescription("Export documents to various formats")
                    .Has()
                    .Commands(cmds => cmds
                        .Command("Export to PDF")
                            .WithIcon(Resources.PdfIcon)
                            .WithDescription("Export as PDF document")
                            .OnClick(() => ExportPdf())

                        .Command("Export to STEP")
                            .WithIcon(Resources.StepIcon)
                            .WithDescription("Export as STEP file")
                            .OnClick(() => ExportStep())

                        .Command("Export to STL")
                            .WithIcon(Resources.StlIcon)
                            .WithDescription("Export as STL for 3D printing")
                            .OnClick(() => ExportStl()))
                    .SaveCommandGroup()

                .AddCommandGroup()
                    .WithTitle("Batch Export")
                    .Has()
                    .Commands(cmds => cmds
                        .Command("Export All Configurations")
                            .OnClick(() => ExportAllConfigs())
                        .Command("Export BOM")
                            .OnClick(() => ExportBom()))
                    .SaveCommandGroup()

                .SaveCommandTab();
    }

    private void ExportPdf() { /* ... */ }
    private void ExportStep() { /* ... */ }
    private void ExportStl() { /* ... */ }
    private void ExportAllConfigs() { /* ... */ }
    private void ExportBom() { /* ... */ }
}
```

## Best Practices

1. **Organize logically**: Group related commands together
2. **Use clear titles**: Make command purposes obvious
3. **Add descriptions**: Help users understand what each command does
4. **Provide icons**: Visual cues improve usability
5. **Control visibility**: Only show commands in relevant contexts

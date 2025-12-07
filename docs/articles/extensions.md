# Extensions Reference

The `Hymma.Solidworks.Extensions` package provides extension methods that simplify common SolidWorks API operations.

## Installation

```powershell
Install-Package Hymma.Solidworks.Extensions
```

## Usage

Add the namespace to access extension methods:

```csharp
using Hymma.Solidworks.Extensions;
```

## Available Extensions

### ModelDoc2 Extensions

Extensions for working with SolidWorks documents.

```csharp
// Get the active configuration
var config = modelDoc.GetActiveConfiguration();

// Get all configurations
var configs = modelDoc.GetConfigurations();

// Get custom properties
var props = modelDoc.GetCustomProperties();
```

### ISldWorks Extensions

Extensions for the main SolidWorks application object.

```csharp
// Get active document safely
var doc = swApp.GetActiveDocument();

// Open document with options
var doc = swApp.OpenDocument(path, options);
```

### Feature Extensions

Extensions for working with features.

```csharp
// Get feature parameters
var params = feature.GetParameters();

// Traverse feature tree
feature.TraverseSubFeatures(f => ProcessFeature(f));
```

### Body Extensions

Extensions for working with solid bodies.

```csharp
// Get body faces
var faces = body.GetFaces();

// Get body edges
var edges = body.GetEdges();

// Calculate volume
var volume = body.GetVolume();
```

## Examples

### Getting Custom Properties

```csharp
public void PrintCustomProperties(ModelDoc2 doc)
{
    var props = doc.GetCustomProperties();

    foreach (var prop in props)
    {
        Console.WriteLine($"{prop.Key}: {prop.Value}");
    }
}
```

### Traversing the Feature Tree

```csharp
public void ListAllFeatures(ModelDoc2 doc)
{
    var firstFeature = doc.FirstFeature();

    while (firstFeature != null)
    {
        Console.WriteLine(firstFeature.Name);
        firstFeature = firstFeature.GetNextFeature();
    }
}
```

### Working with Configurations

```csharp
public void ExportAllConfigurations(ModelDoc2 doc, string outputDir)
{
    var configs = doc.GetConfigurations();

    foreach (var config in configs)
    {
        doc.ShowConfiguration2(config.Name);
        doc.SaveAs($"{outputDir}\\{config.Name}.step");
    }
}
```

## See Also

- [API Reference](../api/Hymma.Solidworks.Extensions.html)
- [Getting Started](getting-started.md)

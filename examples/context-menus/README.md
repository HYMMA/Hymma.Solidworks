# Context Menu Examples

These examples show how to register SolidWorks right-click context menus with selection-based predicates.

## Example A: Feature menu with predicate

Register a menu item for feature selections that only enables when the selected feature name matches a predicate.

```csharp
using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.ContextMenus;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Runtime.InteropServices;

[Guid("YOUR-GUID-HERE")]
[ComVisible(true)]
public class FeatureMenuAddin : AddinMaker
{
    private ContextMenuRegistrar _contextMenus;

    public FeatureMenuAddin()
    {
        OnStart += (s, e) =>
        {
            _contextMenus = ContextMenuRegistrar.Create(this, e.Cookie);

            var menu = new ContextMenuDefinition(
                "Feature Tools",
                new[] { ContextMenuTarget.Features },
                new[]
                {
                    new ContextMenuItem(
                        "Inspect Feature",
                        ctx =>
                        {
                            var model = ctx.Model;
                            var selectionMgr = ctx.SelectionManager;
                            var feature = selectionMgr?.GetSelectedObject6(1, -1) as Feature;
                            if (feature != null)
                                Solidworks.SendMsgToUser($"Feature: {feature.Name}");
                        },
                        ctx =>
                        {
                            var selectionMgr = ctx.SelectionManager;
                            var feature = selectionMgr?.GetSelectedObject6(1, -1) as Feature;
                            return feature != null && feature.Name.StartsWith("BOSS", StringComparison.OrdinalIgnoreCase);
                        })
                });

            _contextMenus.Register(menu);
        };

        OnExit += (s, e) =>
        {
            _contextMenus?.Dispose();
            _contextMenus = null;
        };
    }

    public override AddinUserInterface GetUserInterFace()
    {
        return new AddinUserInterface();
    }
}
```

## Example B: Sketch segment menu

Register a menu item for sketch segment selections.

```csharp
using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.ContextMenus;
using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;

[Guid("YOUR-GUID-HERE")]
[ComVisible(true)]
public class SketchMenuAddin : AddinMaker
{
    private ContextMenuRegistrar _contextMenus;

    public SketchMenuAddin()
    {
        OnStart += (s, e) =>
        {
            _contextMenus = ContextMenuRegistrar.Create(this, e.Cookie);

            var menu = new ContextMenuDefinition(
                "Sketch Tools",
                new[] { ContextMenuTarget.SketchSegments },
                new[]
                {
                    new ContextMenuItem(
                        "Show Segment Type",
                        ctx =>
                        {
                            var segment = ctx.SelectionManager?.GetSelectedObject6(1, -1) as SketchSegment;
                            if (segment != null)
                                Solidworks.SendMsgToUser($"Segment type: {segment.GetType().Name}");
                        })
                });

            _contextMenus.Register(menu);
        };

        OnExit += (s, e) =>
        {
            _contextMenus?.Dispose();
            _contextMenus = null;
        };
    }

    public override AddinUserInterface GetUserInterFace()
    {
        return new AddinUserInterface();
    }
}
```

# Harden WinForms/WPF interop in SwPopupWindow

This PR hardens `SwPopupWindow` for WinForms↔WPF interop to eliminate intermittent crashes and input issues when hosting WPF popups inside the SOLIDWORKS WinForms UI.

## What changed

- **Thread-safety**: All WPF `Window` interactions are marshaled to the window’s Dispatcher to satisfy WPF thread affinity and avoid `InvalidOperationException`.
- **Keyboard routing (modeless)**: Calls `ElementHost.EnableModelessKeyboardInterop(window)` so modeless WPF windows receive keystrokes correctly when opened from WinForms.
- **Robust positioning**: Positions the WPF window after layout using `ActualWidth/ActualHeight` (with `SourceInitialized`/`Loaded`), avoiding `NaN`/incorrect sizes. 
- **Correct `Thickness` order**: Uses `new Thickness(Left, Top, Right, Bottom)` (previously Left, Right, Top, Bottom), fixing the padding input into `PopupHelper.CalculateLocation`.
- **Bug fix (WinForms)**: `PopupWinForm.Close()` previously closed only when `isDisposed == true`. The condition is inverted and the form is disposed deterministically.

> **No public API changes** to `PopupWpfWindow` or `PopupWinForm`.

## Testing

- ✅ Modal WPF popup: `ShowDialog()` blocks SOLIDWORKS UI, window positions correctly, and no cross-thread exceptions.
- ✅ Modeless WPF popup: `Show()` allows SOLIDWORKS interaction; text inputs and accelerators work; window positions after layout.
- ✅ WinForms popup: `PopupWinForm.Show()` and `.ShowDialog()` still work; `Close()` now consistently disposes.
- ✅ No API breaks: Existing external code using `PopupWpfWindow/PopupWinForm` remains valid.

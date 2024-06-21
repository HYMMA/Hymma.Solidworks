# License
This folder is a minimalist implementation of `SwPopupWindows` in [Xcad](https://github.com/xarial/xcad/tree/master).

```
﻿//*********************************************************************
//xCAD
//Copyright(C) 2024 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************
```
## How to show a modal Winform
There are two extension methods for `ISldWorks` that allow you to hook into a `WinForm` object. As shown below:

```CSharp

//Solidworks is ISldWorks provided by AddinMaker.cs
        public void ShowWPFDia()
        {
            var dia = Solidworks.HookWpfWindow(new WpfPopupApp.MainWindow());
            dia.Show();
        }
```

```CSharp

        public void ShowWinformHelpDia()
        {
            var dia = Solidworks.HookWinForm(new WinFormPopupApp.Form1());
            dia.Show();
        }
```
Refer to `Qrifyplus.cs` in the _Samples_ folder to see an implementation.

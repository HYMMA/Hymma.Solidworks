# Overview

This monorepo consists of four different projects. Each project has its own readme file. Please refer to them for more information.
## Package descriptions

| package  | Description  |
|---|---|
| Hymma.Solidworks.Interop  | Adds references to solidworks Interop Libraries  |
| Hymma.Solidworks.Extensions  | Provides a wealth of useful extension methods to solidworks API|
| Hymma.Solidworks.Addins  | This is a framework to help with making native looking plugins for solidworks|
| Hymma.Solidworks.Addins.Fluent  | Based on Hymma.solidworks.Addins package, this framework uses fluent api to deliver same result | 

## Sample Addins
 There are two free addins in this repo.:heart:  
 - Qrify is made using [Hymma.Solidworks.Addins](https://github.com/HYMMA/Hymma.Solidworks/tree/dev/Addins)
 	- This Addin converts a random text into QR image and copies it into the clipboard.
 - Qrify+ is made using [Hymma.Solidworks.Addins.Fluent](https://github.com/HYMMA/Hymma.Solidworks/tree/dev/Addins.Fluent)
 	- This Addin converts value of a custom property into QR image. Users can add suffixes to the QR text. 

Although these are free for commercial use they exist to help you develop your own addins. 

## License
[MIT](https://opensource.org/licenses.MIT)
Copyright 2021 HYMMA

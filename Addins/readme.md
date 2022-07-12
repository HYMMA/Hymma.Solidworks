# Overview
Solidworks is knows to be easy to use. On the one hand, it is a feature-rich software that has a solution for most users requirements. On the other hand, it has a powerful UI to deliver those features. This package is focused on the UI side of things. It allows you create native looking addins for solidworks easily.

## The Issue with Solidworks API 
 Solidworks API is well documented by Dassault Systems. But since it uses COM you should use primitive types like `string` to interact with it. For example there is no `Bitmap` object, you should format, resize and save the image following strict regulations and pass the file path to solidworks.
 Fortunately `Hymma.Solidworks.Addins` wraps solidworks API functions in a proper object. To put it other words, it acts as a proxy between your code and solidworks API. It uses `.NET Framework` objects and `events`. In the example above, you would use a `Bitmap` object and the framework will format, resize and save the image and pass the address to solidworks.
 > _Note_ 
 `Hymma.Solidworks.Addins` uses `%localappdata%` as the main folder for saving and accessing files.
## Addin UIs
### Terminology
**Command** : It is a button in one of the menu bars or the tab bar. It is a signal to Solidworks to start a `Feature`. `Commands` are housed in the `Command Group` which in turn is in a `Tab` and `Menus`.
**Feature** : When users create a 3D object, cut from it or modify it in any form they have used a feature. 
**Property Manager Page** : Each feature in solidworks has its own `Property Manager Page`. A `Property Manager Page` hosts UI elements that allow users to define the feature. _For example_ when user clicks on the `Hole` button a property manager page will pop up on the left side of the screen. And they define the diameter and depth of the `Hole`. 
# Overview
Making native-looking SOLIDWORKS Addins is time consuming and has a steep learning curve. This package was originally made for internal use in HYMMA and now is available to the public. It allows creating a complex UI in a [property manager page](http://help.solidworks.com/2020/english/SolidWorks/sldworks/r_pm_overview.htm) in SOLIDWORKS with fluent API.

# Key Features
- Fluent API 
- Supports Bitmaps
- No need to create [sprites](https://en.wikipedia.org/wiki/Sprite_(computer_graphics)) for Commands 
- Each of [Property manager page controllers](http://help.solidworks.com/2017/english/api/sldworksapiprogguide/Overview/Using_PropertyManagerPage2_and_the_Related_Objects.htm?id=fd93f031fb6c4a9c935310a569d9ce45#Pg0) have separate class that supports events so you don't need to worry about [IPropertyManagerPage2Handler9](https://help.solidworks.com/2018/English/api/swpublishedapi/SolidWorks.Interop.swpublished~SolidWorks.Interop.swpublished.IPropertyManagerPage2Handler9.html).  

# How to start
Since this package is built on top of [Hymma.Solidworks.Addins](https://www.nuget.org/packages/Hymma.Solidworks.Addins/) you need to inherit from `AddinMaker.cs` and override the `GetUserInterFace()`.

```Csharp
	<================FROM QRIFYPLUS PROJECT===============>
        public override AddinUserInterface GetUserInterFace()
        {
            var builder = this.GetBuilder();
            builder
                .AddCommandTab()                                                //An Addin must have a command tab that hosts the command group which in turn hosts the commands
                    .WithTitle("QRify+")
                    .That()                                                     
                    .IsVisibleIn(new[] { swDocumentTypes_e.swDocDRAWING })      //Define which document types this command tab should be accessible from
                    .SetCommandGroup(1)                                         //Add a command group with ID 1. or else if you want. this id and the GUID of this add-in should be unique. once you updated your addin you should change this ID to hold backward compatibility
                        .WithTitle("&File\\Qrify+")                             //Define a title for command group and place the group under File menu in solidworks. for most Ui elements solidworks will not load the ui if they don't have a title
                        .WithIcon(Properties.Resources.qrifyPlus)               //An Icon for the command group
                        .Has()                                                  //change context
                            .Commands(() =>                                     //Add commands to the command group
                            {
                                return new AddinCommand[]
                                {
                                    new AddinCommand("QRify+", "QRify+", "QRify+", Properties.Resources.qrifyPlus, nameof(ShowQrifyPlusPmp), enableMethode:nameof(QrifyPlusPmpEnabler)),
                                };
                            })
                    .SaveCommandGroup()                                         //Save the command group 
                .SaveCommandTab()                                               //Save the command tab to the builder
                    .AddPropertyManagerPage("QRify+", this.Solidworks)          //Add property manager page to the list of UI that the builder will create
                        .AddTab<QrPlusTab>()                                    //Best practice to add tabs with complex Ui setup
                        .AddTab("Settings", Properties.Resources.infoPlus)      //A Property manager page can or cannot have a tab that host the groups. A group hosts the controls such as text box and selection box and ...
                            .AddGroup(caption: "Settings Controls")             //Add a group to property manager page Tab
                                .That()                                         //Just showing off
                                .HasTheseControls(GetSettingsControls)          //add Controls to the property manager page group
                                .SetExpansion(true)                             //Expanded or not expanded on display
                                .OnExpansionChange(null)                        //Event to fire once group expansion changes
                            .SaveGroup()                                        //Save the property manager page group
                        .SaveTab()                                              //Save the property manger page tab
                    .OnClosing((r)=>closeCallBackRegistry.DuringClose(r))       //Solidworks exposes this API but actually locks the UI and most of the command will have no effect. THIS IS IMPORTANT
                    .OnAfterClose(()=>closeCallBackRegistry.AfterClose())       //Once the Property Manager Page is closed for good
                    .SavePropertyManagerPage(out PmpFactoryX64 pmpFactoryX64);  //expose the object that is responsible for showing th property manager page 
            this.pmpFactory = pmpFactoryX64;
            return builder.Build();                                             //Build the UI
	    <==============CONTINUED ON QRIFYPLUS PROJECT==============>
        }
```

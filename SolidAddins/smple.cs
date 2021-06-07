using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.SolidAddins
{
    public class smple
    {
        public void AddCommandManager()
        {
            //we will need an Assembly object to create the bitmaps for the command group items
            //we will use this array to add the command tab to each document type
            int[] documentTypes = new int[]{(int)swDocumentTypes_e.swDocDRAWING
                ,(int)swDocumentTypes_e.swDocPART,(int)swDocumentTypes_e.swDocASSEMBLY};
            int commandGroupError = 0;
            bool ignorePrevious = false;

            //get the ID information stored in the registry
            object registryIDs;
            bool getDataResult = commandManager.GetGroupDataFromRegistry(mainCmdGroupID, out registryIDs);

            int[] knownIDs = new int[1] { mainItemID1 };

            //if the IDs don't match, reset the commandGroup
            if (getDataResult)
            {
                if (!CompareIDs((int[])registryIDs, knownIDs))
                {
                    ignorePrevious = true;
                }
            }

            //a command group is a button that once clicked on, shows a list of other 
            //commands in it. it also gets listed in the Tools menu if you want
            commandGroup = commandManager.CreateCommandGroup2(mainCmdGroupID, Texts.AddinTitle, Texts.AddinDesc,
                "", -1, ignorePrevious, ref commandGroupError);

            //after creating the command group you should add the bitmap photos to it. for this you should use a BitMapHandler object
            Assembly thisAssembly = Assembly.GetAssembly(this.GetType());
            var thisAssemblyInfo = new FileInfo(thisAssembly.Location);
            var icons = new string[3];
            var mainIcons = new string[3];
            icons[0] = thisAssemblyInfo.DirectoryName + "\\Assets\\SaveDxf64.png";
            icons[1] = thisAssemblyInfo.DirectoryName + "\\Assets\\SaveDxf96.png";
            icons[2] = thisAssemblyInfo.DirectoryName + "\\Assets\\SaveDxf128.png";
            mainIcons[0] = thisAssemblyInfo.DirectoryName + "\\Assets\\iProductive64.png";
            mainIcons[1] = thisAssemblyInfo.DirectoryName + "\\Assets\\iProductive96.png";
            mainIcons[2] = thisAssemblyInfo.DirectoryName + "\\Assets\\iProductive128.png";
            commandGroup.IconList = icons;
            commandGroup.MainIconList = mainIcons;

            //Here we make the buttons in the command group. They have the callback functions names as strings
            //ShowPMP is the call back function here which will use UserPMPage to create a new property manager page
            int menuToolbarOption = (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem);


            int commandIndex0 = commandGroup.AddCommandItem2(Texts.DxfTitle, -1, Texts.SaveAsDxfCmdDescription,
                Texts.DxfTitle, 0, nameof(ExtractDxf), nameof(EnableDxfExtraction), mainItemID1, menuToolbarOption);

            //with this you get the command group listed under the Tools menu
            commandGroup.HasToolbar = true;
            commandGroup.HasMenu = true;
            commandGroup.Activate();

            //create command tabs
            foreach (int type in documentTypes)
            {
                CommandTab commandTab;
                commandTab = commandManager.GetCommandTab(type, Texts.AddinTitle);
                //this code removes older tabs I had created
                try
                {
                    CommandTab commandTab2;
                    commandTab2 = commandManager.GetCommandTab(type, "HyProductive");
                    commandManager.RemoveCommandTab(commandTab2);
                    commandTab2 = null;
                }
                catch { }
                //if tab exists, but we have ignored the registry info (or changed command group ID), re-create the tab.  
                //Otherwise the ids won't match-up and the tab will be blank
                if (commandTab != null & !getDataResult | ignorePrevious)
                {
                    bool res = commandManager.RemoveCommandTab(commandTab);
                    commandTab = null;
                }
                //if cmdTab is null, must be first load (possibly after reset), add the commands to the tabs
                if (commandTab == null)
                {
                    commandTab = commandManager.AddCommandTab(type, Texts.AddinTitle);
                    CommandTabBox commandBox0 = commandTab.AddCommandTabBox();
                    //we will use these to add the command to a command box
                    // we will get the command ids from the command group items references (i.e. commnadIndex* we defined earlier)
                    int[] commandIds = new int[1];
                    int[] TextType = new int[1];

                    commandIds[0] = commandGroup.get_CommandID(commandIndex0);
                    TextType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;
                    _ = commandBox0.AddCommands(commandIds, TextType);
                }
            }
            thisAssembly = null;
        }
    }
}

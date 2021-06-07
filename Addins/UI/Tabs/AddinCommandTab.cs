using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// represents a command tab that host commands 
    /// </summary>
    public class AddinCommandTab
    {
        #region private fields

        private AddinCommandGroup commandGroup;
        #endregion

        #region public properties

        /// <summary>
        /// title of command tab 
        /// </summary>
        public string TabTitle { get; set; }

        /// <summary>
        /// document types that this should be visible in 
        /// </summary>
        public IEnumerable<swDocumentTypes_e> Types { get; set; }

        /// <summary>
        /// A command group
        /// </summary>
        public AddinCommandGroup CommandGroup
        {
            get => commandGroup;
            set
            {
                //assign separators
                commandGroup = value;
                var groups = commandGroup.Commands.GroupBy(c => c.BoxId);
                var commandsWithSpacers = new List<AddinCommand>();
                for (int i = 0; i < groups.Count(); i++)
                {
                    var group = groups.ElementAt(i);
                    commandsWithSpacers.AddRange(group.Select(cmd => cmd));
                    
                    //except for the last group ...
                    if (i + 1 < groups.Count())

                        //add a dummy command to indicate spacer
                        commandsWithSpacers.Add(new AddinCommand() { UserId = -1, IconBitmap = new Bitmap(128, 128), Index = -1, SolidworksId = -1 });
                }

                //update commads
                commandGroup.Commands = commandsWithSpacers.ToArray();
            }
        }
        #endregion
       
        /// <summary>
        /// Adds a command tab to solidworks <br/>
        /// </summary>
        /// <param name="commandManager"></param>
        /// <returns>true if successfull false otherwise</returns>
        public bool AddCommandTab(ICommandManager commandManager)
        {
            foreach (int type in Types)
            {
                #region Add Tabs

                //try to get a command tab with the current tabTitle
                CommandTab swTab = commandManager.GetCommandTab(type, TabTitle);

                //if this swTab already esists...
                if (swTab != null & !CommandGroup.IsRegistered | CommandGroup.IgnorePrevious)
                {
                    //clrear this swTab from solidworks so we can regenerate it with new commands
                    //otherwise id of commands wont match up and the tab will be blank
                    Log($"removed command tab {swTab.Name}");
                    commandManager.RemoveCommandTab(swTab);
                    swTab = null;
                }

                //if swTab is already added to this type ...
                if (swTab != null)
                {
                    Log($"tab was not null so we didnt create it");
                    continue;
                }

                //if cmdTab is null, must be first load(possibly after reset), add the commands to the tabs
                swTab = commandManager.AddCommandTab(type, TabTitle);
                Log($"tab was created {swTab.Name}");
                #endregion

                #region Add tab boxes

                var groups = CommandGroup.Commands.GroupBy(cmd => cmd.BoxId);

                CommandTabBox[] tabBoxes = new CommandTabBox[groups.Count()];
                for (int i = 0; i < groups.Count(); i++)
                {
                    Log($"creating a command for group number {i}");
                    var commandBox = groups.ElementAt(i);

                    //add a command box
                    tabBoxes[i] = swTab.AddCommandTabBox();
                    //get commands but exclude the dummy one we added to represent spacer
                    var commandsForThisBox = commandBox
                        .Select(cmd => cmd)
                        .Where(cmd => cmd.SolidworksId != -1);

                    //get command ids
                    var commandIds = commandsForThisBox
                        .Select(c => c.SolidworksId)
                        .ToArray();

                    commandIds.ToList().ForEach(id => Log($"command with id {id} is in tab box {i}"));
                    //get text types
                    var commandTextTypes = commandsForThisBox
                        .Select(cmd => cmd.CommandTabTextType)
                        .ToArray();

                    //add commands to command box
                    var result = tabBoxes[i].AddCommands(commandIds, commandTextTypes);
                    Log($"commands were added to tab box? {result}");
                }
                #endregion
            }
            return true;
        }
    }
}

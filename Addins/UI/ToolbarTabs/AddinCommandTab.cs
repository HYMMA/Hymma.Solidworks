using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Hymma.Solidworks.Addins.Logger;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// represents a command tab that host commands 
    /// </summary>
    public class AddinCommandTab : IWrapSolidworksObject<CommandTab>
    {
        #region private fields
        private AddinCommandGroupBase _commandGroup;
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
        /// Get command groups
        /// </summary>
        public AddinCommandGroupBase CommandGroup
        {
            get => _commandGroup;
            set
            {
                _commandGroup = value;

                //update the icon direcoty of the command group
                //_commandGroup.IconsDir = AddinMaker.GetIconsDir().CreateSubdirectory($"tab_{TabTitle}").CreateSubdirectory($"grp{_commandGroup.UserId}").FullName;

                var groups = _commandGroup.Commands.GroupBy(c => c.BoxId);
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
                _commandGroup.Commands = commandsWithSpacers.ToArray();
            }
        }

        ///<inheritdoc/>
        public CommandTab SolidworksObject { get; private set; }

        #endregion

        /// <summary>
        /// Adds a command tab to solidworks <br/>
        /// </summary>
        /// <param name="commandManager"></param>
        /// <returns>true if successfull false otherwise</returns>
        internal bool Register(ICommandManager commandManager)
        {
            foreach (int type in Types)
            {

                #region Add Tabs
                //try to get a command tab with the current tabTitle
                SolidworksObject = commandManager.GetCommandTab(type, TabTitle);

                //if this swTab already esists and you want to add new command groups or refresh old ones in it
                if (SolidworksObject != null & !CommandGroup.IsRegistered | CommandGroup.IgnorePrevious)
                {
                    Log($"removed command tab {SolidworksObject.Name}");
                    commandManager.RemoveCommandTab(SolidworksObject);
                    SolidworksObject = null;
                }

                //if swTab is already added to this type ...
                if (SolidworksObject != null)
                {
                    Log($"tab was not null so we didnt create it");
                    continue;
                }
                //if cmdTab is null, must be first load(possibly after reset), add the commands to the tabs
                SolidworksObject = commandManager.AddCommandTab(type, TabTitle);
                Log($"tab was created {SolidworksObject.Name}");
                #endregion

                #region Add tab boxes
                var groups = CommandGroup.Commands.GroupBy(cmd => cmd.BoxId);

                CommandTabBox[] tabBoxes = new CommandTabBox[groups.Count()];
                for (int i = 0; i < groups.Count(); i++)
                {
                    Log($"creating a command for group number {i}");
                    var commandBox = groups.ElementAt(i);
                    //add a command box
                    tabBoxes[i] = SolidworksObject.AddCommandTabBox();
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

        private void RegisterCommands(AddinCommandGroupBase CommandGroup)
        {
            if (SolidworksObject == null)
                return;
            var groups = CommandGroup.Commands.GroupBy(cmd => cmd.BoxId);

            CommandTabBox[] tabBoxes = new CommandTabBox[groups.Count()];
            for (int i = 0; i < groups.Count(); i++)
            {
                Log($"creating a command for group number {i}");
                var commandBox = groups.ElementAt(i);
                //add a command box
                tabBoxes[i] = SolidworksObject.AddCommandTabBox();
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
        }
    }
}

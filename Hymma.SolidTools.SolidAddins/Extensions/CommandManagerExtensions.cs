using Hymma.SolidTools.SolidAddins.Tools;
using SolidWorks.Interop.sldworks;
using System.Linq;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// usefull extensions for a <see cref="CommandGroup"/> object
    /// </summary>
    public static class CommandManagerExtensions
    {
        /// <summary>
        /// adds a new command group to this Add-inm
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="commandGroup"></param>
        /// <returns></returns>
        public static void AddCommandGroup(this ICommandManager commandManager, AddinCommandGroup commandGroup)
        {
            //if commandGroup with all its commands does not exist ignore previous instances re-creat the commands
            _ = commandManager.ContainsCommandGroup(commandGroup, out bool ignorePrevious);

            int errors = 0;
            //a command group is a button that once clicked on, shows a list of other 
            //commands in it. it also gets listed in the Tools menu if you want
            CommandGroup swGroup = commandManager.CreateCommandGroup2(commandGroup.GroupID
                , commandGroup.Title
                , commandGroup.ToolTip
                , commandGroup.Hint
                , commandGroup.Position
                , commandGroup.IgnorePrevious
                , ref errors);

            //update commandGroup Error propertye
            commandGroup.Errors = errors;

            //get commands icon addresses
            var icons = commandGroup.Commands.Select(cmd => cmd.Icon);

            //convert all these icons into strips of standard sizes
            var strips = ToolbarIcons.GetIcons(icons.ToArray(), "commandIcons");

            //Get main icon in all sizes
            //
            //NOTE: because main icon is actually one image we will end up just resizing it
            //
            var mainIcons = ToolbarIcons.GetIcons(new[] { commandGroup.MainIcon }, "mainIcon");

            //for each command in commands...
            for (int i = 0; i < commandGroup.Commands.Count(); i++)
            {
                //first get the currnet element in the list 
                var command = commandGroup.Commands.ElementAt(i);

                //add command to the group and update its index property
                command.Index = swGroup.AddCommandItem2(command.Name
                    , command.Position
                    , command.HintString
                    , command.ToolTip
                    , i
                    , command.CallBackFunction
                    , command.EnableMethode
                    , command.UserId
                    , command.MenueOptions);
            }

            //assign icons to the command group
            swGroup.IconList = strips.ToArray();
            swGroup.MainIconList = mainIcons.ToArray();

            //with this you get the command group listed under the Tools menu
            swGroup.HasToolbar = commandGroup.HasToolbar;
            swGroup.HasMenu = commandGroup.HasMenue;

            //activate the command group
            swGroup.Activate();

            #region Update command Ids and command.GroupId

            //at this stage we update command Ids from registry so we can call these commands later
            foreach (AddinCommand command in commandGroup.Commands)
            {
                command.SwId = swGroup.CommandID[command.Index];
                command.GroupId = commandGroup.GroupID;
            }
            #endregion
        }

        /// <summary>
        /// Adds a command tab to solidworks <br/>
        /// A <see cref="AddinCommandGroup"/> should have been added to solidworks via <see cref="AddCommandGroup(ICommandManager, AddinCommandGroup)"/> prior calling this method
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="tab"></param>
        /// <param name="group"></param>
        /// <returns>true if successfull false otherwise</returns>
        public static bool AddCommandTab(this ICommandManager commandManager, AddinCommandTab tab)
        {
            foreach (int type in tab.Types)
            {
                #region Add Tabs

                //try to get a command tab with the current tabTitle
                CommandTab swTab = commandManager.GetCommandTab(type, tab.TabTitle);

                //if this swTab already esists...
                if (swTab !=null)
                {
                    //for each command box in the proposed tab
                    foreach (var box in tab.CommandBoxes)
                    {
                        //if the command is registered...
                        if (box.Commands.Any(cmd=>commandManager.ContainsCommand(cmd)))
                        {
                            //clrear this swTab from solidworks so we can regenerate it with new commands
                            //otherwise id of commands wont match up and the tab will be blank
                            commandManager.RemoveCommandTab(swTab);
                            swTab = null;
                        }
                    }
                }

                //if swTab is already added to this type ...
                if (swTab != null)
                    //skip this type
                    continue;

                //if cmdTab is null, must be first load(possibly after reset), add the commands to the tabs
                swTab = commandManager.AddCommandTab(type, tab.TabTitle);
                #endregion

                #region Add tab boxes
                foreach (var commandBox in tab.CommandBoxes)
                {
                    //add a command box
                    CommandTabBox swTabBox = swTab.AddCommandTabBox();

                    //get command ids
                    var commandIds = commandBox.Commands
                        .Where(cmd => commandManager.ContainsCommand(cmd) == true) //only commands that are registered
                        .Select(cmd => cmd.SwId).ToArray(); //select them by their SwId and convert the result to an array of command-ids

                    //get text types
                    var commandTextTypes = commandBox.Commands.Select(cmd => cmd.CommandTabTextType).ToArray();

                    //add commands to command box
                    swTabBox.AddCommands(commandIds, commandTextTypes);
                }
                #endregion
            }
            return true;
        }

        /// <summary>
        /// determnines if a command group with its commands exists in the registry or not <br/>
        /// also update the IsNEw property in a <see cref="AddinCommandGroup"/>
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="commandGroup"></param>
        /// <param name="ignorePrevious">if command group is registered but has been modified this parameter is false</param>
        /// <returns></returns>
        public static bool ContainsCommandGroup(this ICommandManager commandManager, AddinCommandGroup commandGroup, out bool ignorePrevious)
        {
            bool commandGroupExists = false;
            //get the command IDs of this command group that were registerd in windows registry
            commandGroup.IsRegistered = commandManager.GetGroupDataFromRegistry(commandGroup.GroupID, out object registeredCommands);

            //if the IDs don't match, reset the commandGroup
            if (commandGroup.IsRegistered && registeredCommands is int[] oldIds)
            {
                //get the difference between oldIds and new ids...
                var commandIdsNotInRegistry = commandGroup.Commands
                     .Select(cmd => cmd.UserId)
                     .Except(oldIds.ToList());

                //if old command ids exitst in registry this command group is already in registry with current verstion of command ids
                commandGroupExists = commandIdsNotInRegistry.Count() == 0 ? true : false;
            }
            commandGroup.IgnorePrevious = commandGroupExists;
            ignorePrevious = commandGroupExists;
            return commandGroupExists;
        }

        /// <summary>
        /// indicates if a <see cref="AddinCommand"/> is registered in COM or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool ContainsCommand(this ICommandManager commandManager, AddinCommand command)
        {
            //check if the group this command belongs to is registered 
            var groupIsRegistered = commandManager.GetGroupDataFromRegistry(command.GroupId, out object registeredCommands);

            //if group is registred...
            if (groupIsRegistered && registeredCommands is int[] oldIds)
            {
                //check if this command is registred against that group
                return oldIds.Contains(command.UserId);
            }
            return false;
        }
    }
}

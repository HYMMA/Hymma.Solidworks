// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Hymma.Solidworks.Addins.Core
{
    internal static class ICommandMangerExtensions
    {

        #region register this to solidworks
        static void RegisterIcons(ICommandGroup swGroup, IAddinCommandGroup group)
        {
            swGroup.IconList = group.CommandIcons;
            swGroup.MainIconList = group.GroupIcon;

            int imageIndex = -1;
            foreach (var command in group.Commands)
            {
                imageIndex++;
                if (command.UserId == -1)
                {
                    //add a spacer after each group box
                    swGroup.AddSpacer2(imageIndex, (int)swCommandItemType_e.swMenuItem);
                    continue;
                }

                //add the command to command manager
                var index = swGroup.AddCommandItem2(command.Name
                    , -1
                    , command.HintString
                    , command.ToolTip
                    , imageIndex
                    , command.CallBackFunction
                    , command.EnableMethod
                    , command.UserId
                    , command.MenuOptions);

                //assign the index we got to command
                command.Index = index;
            }
        }

        /// <summary>
        /// determines if a command group with its commands exists in the registry or not <br/>
        /// also update the IsRegistered property in a <see cref="AddinCommandGroup"/>
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="group">the model to use to create the command group</param>
        /// <returns></returns>
        private static void CheckRegistryForThisGroup(ICommandManager commandManager, IAddinCommandGroup group)
        {
            //get the command IDs of this command group that were registered in windows registry
            group.IsRegistered = commandManager.GetGroupDataFromRegistry(group.UserId, out object registryIds);

            //if the IDs don't match, reset the commandGroup
            if (group.IsRegistered && IdsAreEqual((int[])registryIds, group.Commands.Select(cmd => cmd.UserId).ToArray()))
                group.IgnorePrevious = true;
        }
        private static bool IdsAreEqual(int[] storedIDs, int[] addinIDs)
        {
            List<int> storedList = new List<int>(storedIDs);
            List<int> addinList = new List<int>(addinIDs);

            addinList.Sort();
            storedList.Sort();

            if (addinList.Count != storedList.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < addinList.Count; i++)
                {
                    if (addinList[i] != storedList[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// adds a new command group to this Add-in
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        internal static void Register(this ICommandManager commandManager, IAddinCommandGroup group)
        {
            #region Create new command group
            //if commandGroup with all its commands does not exist ignore previous instances re-create the commands
            CheckRegistryForThisGroup(commandManager, group);

            int errors = 0;

            //a command group is a button that once clicked on, shows a list of other 
            //commands in it. it also gets listed in the Tools menu bu default
            ICommandGroup swGroup =
                commandManager.CreateCommandGroup2(
                    group.UserId
                    , group.Title
                    , group.ToolTip
                    , group.Hint
                    , group.Position
                    , group.IgnorePrevious
                    , ref errors);

            //stop if could not create the command group
            if (errors != (int)swCreateCommandGroupErrors.swCreateCommandGroup_Success)
                throw new System.Exception($"could not create command group {group.UserId}");
            #endregion

            RegisterIcons(swGroup, group);

            //with this you get the command group listed under the Tools menu
            swGroup.HasToolbar = group.HasToolbar;
            swGroup.HasMenu = group.HasMenu;

            //activate the command group
            swGroup.Activate();

            #region Update command Ids

            //at this stage we update command Ids from registry so we can call these commands later
            group.Commands
                //index -1 is for spacers
                .Where(c => c.Index != -1).ToList()
                .ForEach(c => c.SolidworksId = swGroup.CommandID[c.Index]);
            #endregion
        }

        #endregion

        #region register command tab

        /// <summary>
        /// Adds a command tab to solidworks <br/>
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="tab">the command tab</param>
        internal static bool Register(this ICommandManager commandManager, AddinCommandTab tab)
        {
            foreach (int doc in tab.DocTypes)
            {

                #region Add Tabs
                //try to get a command tab with the current tabTitle
                var swCmdTab = commandManager.GetCommandTab(doc, tab.Title);

                //if this swTab already exists and you want to add new command groups or refresh old ones in it
                if (swCmdTab != null & !tab.CommandGroup.IsRegistered | tab.CommandGroup.IgnorePrevious)
                {
                    commandManager.RemoveCommandTab(swCmdTab);
                    swCmdTab = null;
                }

                //if swTab is already added to this doc ...
                if (swCmdTab != null)
                {
                    continue;
                }
                //if cmdTab is null, must be first load(possibly after reset), add the commands to the tabs
                swCmdTab = commandManager.AddCommandTab(doc, tab.Title);
                #endregion

                #region Add tab boxes
                var groups = tab.CommandGroup.Commands.GroupBy(cmd => cmd.BoxId);

                CommandTabBox[] tabBoxes = new CommandTabBox[groups.Count()];
                for (int i = 0; i < groups.Count(); i++)
                {
                    var commandBox = groups.ElementAt(i);
                    //add a command box
                    tabBoxes[i] = swCmdTab.AddCommandTabBox();
                    //get commands but exclude the dummy one we added to represent spacer
                    var commandsForThisBox = commandBox
                        .Select(cmd => cmd)
                        .Where(cmd => cmd.SolidworksId != -1);

                    //get command ids
                    var commandIds = commandsForThisBox
                        .Select(c => c.SolidworksId)
                        .ToArray();

                    //get text types
                    var commandTextTypes = commandsForThisBox
                        .Select(cmd => cmd.CommandTabTextType)
                        .ToArray();

                    //add commands to command box
                    var result = tabBoxes[i].AddCommands(commandIds, commandTextTypes);
                }
                #endregion
            }
            return true;
        }
        #endregion
    }
}

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a model for <see cref="CommandGroup"/>
    /// </summary>
    public class AddinCommandGroup : AddinCommandGroupBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="userId"> 
        /// If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a new unique user-defined UserID to that CommandGroup. <br/>
        /// You must perform this action to avoid conflicts with any previously existing CommandGroupa and to allow for backward and forward compatibility of the CommandGroups in your application.<br/>
        /// The user ID and the GUID of the CoClass implementing ISwAddin are a unique pair.</param>
        /// <param name="commands"> a list of <see cref="AddinCommand"/> this group presents</param>
        /// <param name="title">To add a menu item for a CommandGroup to an existing SOLIDWORKS menu, specify the name of a parent menu here.<br/>
        /// <example><c>"&amp;Help\\MyApp Title"</c></example></param>
        /// <param name="description">Description of this AddinCommandGroup</param>
        /// <param name="tooltip">Tooltip of this AddinCommandGroup</param>
        /// <param name="hint">A Hint for this AddinCommandGroup</param>
        /// <param name="icon"><see cref="Bitmap"/> object as icon for this command group inside the command manager</param>
        /// <param name="hasToolbar">does it have toolbar?</param>
        /// <param name="hasMenue">should it be presented in a menue?</param>
        public AddinCommandGroup(int userId, AddinCommand[] commands, string title, string description, string tooltip, string hint, Bitmap icon, bool hasToolbar = true, bool hasMenue = true )
        {
            #region assing values to properties
            UserId = userId;
            Commands = commands;
            Title = title;
            Description = description;
            ToolTip = tooltip;
            Hint = hint;
            MainIconBitmap = icon;
            HasToolbar = hasToolbar;
            HasMenue = hasMenue;
            #endregion
        }
        
        #region register this to solidworks
        private  void RegisterIcons(ICommandGroup swGroup)
        {
            Log($"getting command icons from command group");
            swGroup.IconList = CommandIcons;
            swGroup.MainIconList = GroupIcon;

            int imageIndex = -1;
            foreach (var command in Commands)
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
                    , command.EnableMethode
                    , command.UserId
                    , command.MenueOptions);

                //assing the index we got to command
                command.Index = index;

                Log($"a command with index {index} is created");
            }
        }


        /// <summary>
        /// adds a new command group to this Add-inm
        /// </summary>
        /// <param name="commandManager"></param>
        /// <returns></returns>
        public void AddCommandGroup(ICommandManager commandManager)
        {
            #region Create new command group

            Log("crreating new command group ...");
            //if commandGroup with all its commands does not exist ignore previous instances re-creat the commands
            CheckRegistryForThisGroup(commandManager);

            int errors = 0;

            //a command group is a button that once clicked on, shows a list of other 
            //commands in it. it also gets listed in the Tools menu bu default
            ICommandGroup swGroup =
                commandManager.CreateCommandGroup2(
                    UserId
                    , Title
                    , ToolTip
                    , Hint
                    , Position
                    , IgnorePrevious
                    , ref errors);

            //stop if could not create the command group
            if (errors != (int)swCreateCommandGroupErrors.swCreateCommandGroup_Success)
                throw new System.Exception($"could not create command group {UserId}");
            #endregion

            Log("registering icons...");
            RegisterIcons(swGroup);

            //with this you get the command group listed under the Tools menu
            swGroup.HasToolbar = HasToolbar;
            swGroup.HasMenu = HasMenue;

            //activate the command group
            swGroup.Activate();

            #region Update command Ids

            //at this stage we update command Ids from registry so we can call these commands later
            Commands
                //index -1 is for spacers
                .Where(c => c.Index != -1).ToList()
                .ForEach(c => c.SolidworksId = swGroup.CommandID[c.Index]);
            #endregion
        }


        /// <summary>
        /// determnines if a command group with its commands exists in the registry or not <br/>
        /// also update the IsRegistered property in a <see cref="AddinCommandGroup"/>
        /// </summary>
        /// <param name="commandManager"></param>
        /// <returns></returns>
        private void CheckRegistryForThisGroup(ICommandManager commandManager)
        {
            //get the command IDs of this command group that were registerd in windows registry
            IsRegistered = commandManager.GetGroupDataFromRegistry(UserId, out object registryIds);

            Log($"command gourp with user id {UserId} was registered already ?  {IsRegistered}");
            //if the IDs don't match, reset the commandGroup
            if (IsRegistered && !CompareIDs((int[])registryIds, Commands.Select(cmd => cmd.UserId).ToArray()))
                IgnorePrevious = true;
        }

        private bool CompareIDs(int[] storedIDs, int[] addinIDs)
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
        #endregion
    }
}

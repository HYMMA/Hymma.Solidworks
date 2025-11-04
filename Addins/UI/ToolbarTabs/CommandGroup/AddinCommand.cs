// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System;
using System.Drawing;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper for a command in solidworks 
    /// </summary>
    public class AddinCommand : IDisposable
    {
        private bool disposed;
        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AddinCommand()
        {
            disposed = false;    
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">text to display next to icon</param>
        /// <param name="hint">text to display in a tool-tip</param>
        /// <param name="tooltipTitle">tool-tip title</param>
        /// <param name="icon">icon for this command</param>
        /// <param name="nameofCallBackFunc">name of function that this command calls <strong>THIS FUNCTION MUST BE DEFINED IN THE ADDIN CLASS. ADDIN CLASS IS THE ONE THAT INHERITS FROM <see cref="AddinMaker"/></strong></param>
        /// <param name="userId">a user id for this command</param>
        /// <param name="menuOption">whether this command should be in menu or toolbox or both as defined in <see cref="swCommandItemType_e"/><br/></param>
        /// <param name="tabTextStyle">text display of this command when used in a <see cref="AddinCommandTab"/> as defined by <see cref="swCommandTabButtonTextDisplay_e"/></param>
        /// <param name="enableMethod">name of optional function that controls the state of the item; if specified, then SOLIDWORKS calls this function before displaying the item 
        /// <strong>THIS FUNCTION MUST BE DEFINED IN THE ADDIN CLASS. ADDIN CLASS IS THE ONE THAT INHERITS FROM <see cref="AddinMaker"/></strong>
        /// <list type="table">
        /// <listheader>
        /// <term>values</term>
        /// <description>if returned value of enabled function is:</description>
        /// </listheader>
        /// <item>0<term></term><description>SOLIDWORKS De-selects and disables the item</description></item>
        /// <item>1<term></term><description>SOLIDWORKS De-selects and enables the item; this is the default state if no update function is specified</description></item>
        /// <item>2<term></term><description>SOLIDWORKS Selects and disables the item</description></item>
        /// <item>3<term></term><description>SOLIDWORKS Selects and enables the item</description></item>
        /// <item>4<term></term><description>Not supported</description></item>
        /// </list></param>
        public AddinCommand(string name, string hint, string tooltipTitle, Bitmap icon, string nameofCallBackFunc, short userId = 0, int menuOption = 3, int tabTextStyle = 2, string enableMethod = ""):this()
        {
            #region assign values to properties
            Name = name;
            HintString = hint;
            ToolTip = tooltipTitle;
            IconBitmap = icon;
            CallBackFunction = nameofCallBackFunc;
            UserId = userId;
            MenuOptions = menuOption;
            CommandTabTextType = tabTextStyle;
            EnableMethod = enableMethod;
            #endregion
        }
        #endregion

        /// <summary>
        /// commands that have the same box id will be grouped together and separated by a '|'
        /// </summary>
        public int BoxId { get; set; } = 0;

        /// <summary>
        /// index of this command inside its command group object
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// name as appears in SolidWORKS
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// hint for this command
        /// </summary>
        public string HintString { get; set; }

        /// <summary>
        /// tool-tip for this command
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// <see cref="Bitmap"/>  object as an icon
        /// </summary>
        public Bitmap IconBitmap { get; set; }

        /// <summary>
        /// name of the function this command will call 
        /// </summary>
        /// <remarks><strong>THIS FUNCTION MUST BE DEFINED IN THE ADDIN CLASS. ADDIN CLASS IS THE ONE THAT INHERITS FROM <see cref="AddinMaker"/></strong></remarks>
        public string CallBackFunction { get; set; }

        /// <summary>
        /// Optional function that controls the state of the item; if specified, then SOLIDWORKS calls this function before displaying the item 
        /// <list type="table">
        /// <listheader>
        /// <term>values</term>
        /// <description>if returned value of enabled function is:</description>
        /// </listheader>
        /// <item>0<term></term><description>SOLIDWORKS De-selects and disables the item</description></item>
        /// <item>1<term></term><description>SOLIDWORKS De-selects and enables the item; this is the default state if no update function is specified</description></item>
        /// <item>2<term></term><description>SOLIDWORKS Selects and disables the item</description></item>
        /// <item>3<term></term><description>SOLIDWORKS Selects and enables the item</description></item>
        /// <item>4<term></term><description>Not supported</description></item>
        /// </list>
        /// </summary>
        /// <remarks><strong>THIS FUNCTION MUST BE DEFINED IN THE ADDIN CLASS. ADDIN CLASS IS THE ONE THAT INHERITS FROM <see cref="AddinMaker"/></strong></remarks>
        public string EnableMethod { get; set; } = "";

        /// <summary>
        /// User-defined command ID or 0 if not used
        /// </summary>
        public int UserId { get; set; } = 0;

        /// <summary>
        /// Id that SolidWORKS assigns to this command once created. it then gets used by command boxes
        /// </summary>
        public int SolidworksId { get; internal set; }

        /// <summary>
        /// whether this command should be in menu or toolbox or both as defined in <see cref="swCommandItemType_e"/><br/>
        /// default is 3
        /// </summary>
        public int MenuOptions { get; set; } = 3;

        /// <summary>
        /// text display of this command when used in a <see cref="AddinCommandTab"/> as defined by <see cref="swCommandTabButtonTextDisplay_e"/><br/>
        /// default is 2
        /// </summary>
        public int CommandTabTextType { get; set; } = 2;

        /// <summary>
        /// disposes the resourses
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                IconBitmap.Dispose();
                disposed = true;
            }
        }
    }
}

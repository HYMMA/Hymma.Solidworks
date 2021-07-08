using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for a command in solidworks 
    /// </summary>
    public class AddinCommandBase
    {
        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AddinCommandBase()
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">text to display next to icon</param>
        /// <param name="hint">text to display in a tooltip</param>
        /// <param name="tooltipTitle">tooltip title</param>
        /// <param name="icon">icon for this command</param>
        /// <param name="nameofCallBackFunc">name of function that this command calls</param>
        /// <param name="userId">a user id for this command</param>
        /// <param name="menuOption">whether this command should be in menue or toolbox or both as defined in <see cref="swCommandItemType_e"/><br/></param>
        /// <param name="tabTextStyle">text dispay of this commnd when used in a <see cref="AddinCommandTab"/> as defined by <see cref="swCommandTabButtonTextDisplay_e"/></param>
        /// <param name="enableMethode">name of optional function that controls the state of the item; if specified, then SOLIDWORKS calls this function before displaying the item 
        /// <list type="table">
        /// <listheader>
        /// <term>values</term>
        /// <description>if returned value of enabled function is:</description>
        /// </listheader>
        /// <item>0<term></term><description>SOLIDWORKS Deselects and disables the item</description></item>
        /// <item>1<term></term><description>SOLIDWORKS Deselects and enables the item; this is the default state if no update function is specified</description></item>
        /// <item>2<term></term><description>SOLIDWORKS Selects and disables the item</description></item>
        /// <item>3<term></term><description>SOLIDWORKS Selects and enables the item</description></item>
        /// <item>4<term></term><description>Not supported</description></item>
        /// </list></param>
        public AddinCommandBase(string name, string hint, string tooltipTitle, Bitmap icon, string nameofCallBackFunc, short userId = 0, int menuOption = 3, int tabTextStyle = 2, string enableMethode = "")
        {
            #region assign values to properties
            Name = name;
            HintString = hint;
            ToolTip = tooltipTitle;
            IconBitmap = icon;
            CallBackFunction = nameofCallBackFunc;
            UserId = userId;
            MenueOptions = menuOption;
            CommandTabTextType = tabTextStyle;
            EnableMethode = enableMethode;
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
        public int Index { get; set; }
        /// <summary>
        /// name as appears in solidworks
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// hit for this command
        /// </summary>
        public string HintString { get; set; }

        /// <summary>
        /// tooltip for this command
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// <see cref="Bitmap"/>  object as an icon
        /// </summary>
        public Bitmap IconBitmap { get; set; }

        /// <summary>
        /// name of the function this command will call
        /// </summary>
        public string CallBackFunction { get; set; }

        /// <summary>
        /// Optional function that controls the state of the item; if specified, then SOLIDWORKS calls this function before displaying the item 
        /// <list type="table">
        /// <listheader>
        /// <term>values</term>
        /// <description>if returned value of enabled function is:</description>
        /// </listheader>
        /// <item>0<term></term><description>SOLIDWORKS Deselects and disables the item</description></item>
        /// <item>1<term></term><description>SOLIDWORKS Deselects and enables the item; this is the default state if no update function is specified</description></item>
        /// <item>2<term></term><description>SOLIDWORKS Selects and disables the item</description></item>
        /// <item>3<term></term><description>SOLIDWORKS Selects and enables the item</description></item>
        /// <item>4<term></term><description>Not supported</description></item>
        /// </list>
        /// </summary>
        public string EnableMethode { get; set; } = "";

        /// <summary>
        /// User-defined command ID or 0 if not used
        /// </summary>
        public int UserId { get; set; } = 0;

        /// <summary>
        /// Id that solidworks assigns to this command once created. it then gets used by command boxes
        /// </summary>
        public int SolidworksId { get; set; }

        /// <summary>
        /// whether this command should be in menue or toolbox or both as defined in <see cref="swCommandItemType_e"/><br/>
        /// default is 3
        /// </summary>
        public int MenueOptions { get; set; } = 3;

        /// <summary>
        /// text dispay of this commnd when used in a <see cref="AddinCommandTab"/> as defined by <see cref="swCommandTabButtonTextDisplay_e"/><br/>
        /// default is 2
        /// </summary>
        public int CommandTabTextType { get; set; } = 2;
    }
}

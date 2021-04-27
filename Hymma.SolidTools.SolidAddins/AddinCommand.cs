namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a wrapper for a command in solidworks 
    /// </summary>
    public class AddinCommand
    {
        /// <summary>
        /// id of <see cref="AddinCommandGroup"/> this command belongs to.
        /// </summary>
        public int GroupId { get; set; }
       
        /// <summary>
        /// index of this command inside a command group
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// name as appears in solidworks
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// position of this command in command group
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// hit for this command
        /// </summary>
        public string HintString { get; set; }

        /// <summary>
        /// tooltip for this command
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// icon address, should be png file 
        /// </summary>
        public string Icon { get; set; }

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
        /// solidworks id of this command that gets used by command boxes
        /// </summary>
        public int SwId { get; set; }

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

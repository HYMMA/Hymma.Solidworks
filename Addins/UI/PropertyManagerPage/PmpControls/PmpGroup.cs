using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page groups
    /// </summary>
    public class PmpGroup
    {
        #region constructors

        /// <summary>
        /// construct a property manage page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="Caption">text that appears next to a group box</param>
        /// <param name="Expanded">determines the expand state of this group box</param>
        public PmpGroup(string Caption,bool Expanded=false)
        {
            this.Caption = Caption;
            this.Expanded = Expanded;
        }

        /// <summary>
        /// construct a property manager page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="Caption">text that appears next to a group box</param>
        /// <param name="Controls">list of controls to add to this group</param>
        /// <param name="Expanded">determines the expand state of this group box</param>
        public PmpGroup(string Caption, List<IPmpControl> Controls, bool Expanded= false) : this(Caption,Expanded)
        {
            this.Controls = Controls;
        }
        #endregion

        #region Members/Properties

        /// <summary>
        /// identifier for this group box in Property manager page
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// caption
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// determines the expand state of this group box 
        /// </summary>
        public bool Expanded { get; internal set; }

        /// <summary>
        /// bitwise options as defined by <see cref="swAddGroupBoxOptions_e"/> default values correspond to a group that is expanded and is set to be visible
        /// </summary>
        public swAddGroupBoxOptions_e Options { get; set; }
            = swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded | swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;

        /// <summary>
        /// a list of solidworks controllers
        /// </summary>
        public List<IPmpControl> Controls { get; set; }
        #endregion

    }
}
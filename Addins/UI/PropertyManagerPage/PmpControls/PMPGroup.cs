using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page groups
    /// </summary>
    public class PMPGroup : IWrapSolidworksObject<IPropertyManagerPageGroup>
    {
        #region constructors

        /// <summary>
        /// construct a property manage page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="Caption">text that appears next to a group box</param>
        /// <param name="Expanded">determines the expand state of this group box</param>
        public PMPGroup(string Caption, bool Expanded = false)
        {
            this.Caption = Caption;
            this.Expanded = Expanded;
            this.Controls = new List<IPmpControl>();
        }

        /// <summary>
        /// construct a property manager page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="Caption">text that appears next to a group box</param>
        /// <param name="Controls">list of controls to add to this group</param>
        /// <param name="Expanded">determines the expand state of this group box</param>
        public PMPGroup(string Caption, List<IPmpControl> Controls, bool Expanded = false) : this(Caption, Expanded)
        {
            this.Controls = Controls;
        }
        #endregion

        #region Members/Properties

        /// <summary>
        /// adds a control to the <see cref="Controls"/>
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(IPmpControl control)
        {
            Controls.Add(control);
        }

        /// <summary>
        /// adds a list of controls to the <see cref="Controls"/>
        /// </summary>
        /// <param name="controls"></param>
        public void AddControls(IEnumerable<IPmpControl> controls)
        {
            Controls.AddRange(controls);
        }
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
        public bool Expanded { get; set; }

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


        /// <summary>
        /// method to invoke when user expands a group <br/>
        /// this delegate requires a bool variable to indicate the IsExpanded status of the group
        /// </summary>
        public Action<bool> OnGroupExpand { get; set; }

        /// <summary>
        /// method to invoke when user checks a group <br/>
        /// this delegate requires a bool variable to indicate the IsChecked status of the group
        /// </summary>
        public Action<bool> OnGroupCheck { get; set; }

        ///<inheritdoc/>
        public IPropertyManagerPageGroup SolidworksObject { get; private set; }

        /// <summary>
        /// registers this group into a property manager page
        /// </summary>
        /// <param name="propertyManagerPage"></param>
        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            SolidworksObject = (IPropertyManagerPageGroup)propertyManagerPage.AddGroupBox(Id, Caption, (int)Options);

            Controls.ForEach(c => c.Register(SolidworksObject));

            //a special rule should apply for options (radio buttons) as they require a style value of 1 to indicate the beginning of a group of options
            //any following optio without this value set are considered part of the group; the next option with this value set indicates the start of a new option group
            //we assume all the radio buttons in a PMPGroup are members of a group so we assing a value of 1 to the first one
            var firstOption = Controls.FirstOrDefault(c => c.Type == swPropertyManagerPageControlType_e.swControlType_Option)?.CastTo<PmpRadioButton>();
            if (firstOption != null)
                firstOption.SolidworksObject.Style = 1;
        }
    }
}
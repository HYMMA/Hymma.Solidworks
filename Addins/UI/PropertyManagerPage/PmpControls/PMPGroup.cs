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
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="expanded">determines the expand state of this group box</param>
        public PMPGroup(string caption = "Group", bool expanded = false)
        {
            //assign properties
            Caption = caption;
            Expanded = expanded;
            Controls = new List<IPmpControl>();
            Options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded | (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;
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

        #region Properties

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
        public int Options { get; set; }

        /// <summary>
        /// a list of solidworks controllers
        /// </summary>
        public List<IPmpControl> Controls { get; internal set; }

        ///<inheritdoc/>
        public IPropertyManagerPageGroup SolidworksObject { get; private set; }
        #endregion
        
        #region methods
        /// <summary>
        /// Registers a control to the <see cref="Controls"/> and solidworks UI
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
        /// registers this group into a property manager page
        /// </summary>
        /// <param name="propertyManagerPage"></param>
        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            Id = Counter.GetNextPmpId();
            SolidworksObject = (IPropertyManagerPageGroup)propertyManagerPage.AddGroupBox(Id, Caption, (int)Options);

            Controls.ForEach(c => c.Register(SolidworksObject));

            //a special rule should apply for options (radio buttons) as they require a style value of 1 to indicate the beginning of a group of options
            //any following optio without this value set are considered part of the group; the next option with this value set indicates the start of a new option group
            //we assume all the radio buttons in a PMPGroup are members of a group so we assing a value of 1 to the first one
            var firstOption = Controls.FirstOrDefault(c => c.Type == swPropertyManagerPageControlType_e.swControlType_Option)?.CastTo<PmpRadioButton>();
            if (firstOption != null)
                firstOption.SolidworksObject.Style = 1;
        }
        #endregion

        #region events

        /// <summary>
        /// an event that gets called right before this pmpGroup is displayed
        /// </summary>
        public event EventHandler OnDisplay;

        /// <summary>
        /// method to invoke when user expands a group <br/>
        /// this delegate requires a bool variable to indicate the IsExpanded status of the group
        /// </summary>
        public event EventHandler<bool> OnGroupExpand;

        /// <summary>
        /// method to invoke when user checks a group <br/>
        /// this delegate requires a bool variable to indicate the IsChecked status of the group
        /// </summary>
        public event EventHandler<bool> OnGroupCheck;
        #endregion

        #region call backs
        internal void GroupChecked(bool e)
        {
            OnGroupCheck?.Invoke(this,e);
        }
        internal void GroupExpand(bool e)
        {
            OnGroupExpand?.Invoke(this, e);
        }
        internal void Display()
        {
            SolidworksObject.Expanded = Expanded;
            Controls.ForEach(c => c.Display());
            OnDisplay?.Invoke(this,EventArgs.Empty);
        }
        #endregion
    }
}
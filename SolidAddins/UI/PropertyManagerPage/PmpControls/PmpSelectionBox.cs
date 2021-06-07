using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class PmpSelectionBox : PmpControl
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PmpSelectionBox(swSelectType_e[] Filter,  short Height=50) : base(swPropertyManagerPageControlType_e.swControlType_Selectionbox)
        {
            this.Height = Height;
            this.Filter = Filter;
        }
        /// <summary>
        /// array of <see cref="swSelectType_e"/> to allow selection of specific types only
        /// </summary>
        public swSelectType_e[] Filter { get; set; }

        /// <summary>
        /// height of this selection box in proerty manager page
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// SOLIDWORKS will invoke this once focus is changed from this selection box
        /// </summary>
        public Action OnFocusChanged { get; set; }

        /// <summary>
        /// SOLIDWORKS will invoke this once list changes <br/>
        /// requires an input variable as the qty of list items
        /// </summary>
        public Action<int> OnListChanged { get; set; }

        /// <summary>
        /// SOLIDWORKS will invoke this once a call-out is created for thsi selection box
        /// </summary>
        public Action OnCallOutCreated { get; set; }

        /// <summary>
        /// SOLIDWORKS will invoke this once a callout is destroyed
        /// </summary>
        public Action OnCallOutDestroyed { get; set; }
    }
}

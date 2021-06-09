using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class PmpSelectionBox : PmpControl
    {
        /// <summary>
        /// default constructor
        /// <param name="Filter">defines out type of entity in solidworks user could select</param>
        /// <param name="Height">height of selectionbox in the pmp</param>
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

        /// <summary>
        /// Called when a selection is made, which allows the add-in to accept or reject the selection. it must return true for selections to occure
        /// </summary>
        /// <param name="int">ID of the active selection box, where this selection is being made</param>
        /// <param name="Selection">Object being selected</param>
        /// <param name="SelType">Entity type of the selection as defined in<see cref="swSelectType_e"/> </param>
        /// <param name="ItemText">ItemText is returned to SOLIDWORKS and stored on the selected object and can be used by your PropertyManager page selection list boxes for the life of that selection.</param>
        /// <remarks>  This method is called by SOLIDWORKS when an add-in has a PropertyManager page displayed and a selection is made that passes the selection filter criteria set up for a selection list box. The add-in can then: 
        /// Take the Dispatch pointer and the selection type.
        ///        QueryInterface the Dispatch pointer to get the specific interface.
        ///    Use methods or properties of that interface to determine if the selection should be allowed or not.If the selection is:
        ///        accepted, return true, and processing continues normally.
        ///        - or -
        ///        rejected, return false, and SOLIDWORKS does not accept the selection, just as if the selection did not pass the selection filter criteria of the selection list box.  
        ///The add-in should not release the Dispatch pointer. SOLIDWORKS will release the Dispatch pointer upon return from this method.
        ///The method is called during the process of SOLIDWORKS selection.It is neither a pre-notification nor post-notification.The add-in should not be taking any action that might affect the model or the selection list.The add-in should only be querying information and then returning true/VARIANT_TRUE or false/VARIANT_FALSE.
        ///</remarks>
        /// <returns></returns>
        public Func<int , object , int , string ,bool> OnSubmitSelection(int Id, object Selection, int SelType, ref string ItemText) 
        
        {
            return true;
        }
    }
}

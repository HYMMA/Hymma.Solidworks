using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class PmpSelectionBox : PmpControl<T>
    {
        /// <summary>
        /// default constructor
        /// <param name="Filter">defines out type of entity in solidworks user could select</param>
        /// <param name="Height">height of selectionbox in the pmp</param>
        /// </summary>
        public PmpSelectionBox(swSelectType_e[] Filter, short Height = 50) : base(swPropertyManagerPageControlType_e.swControlType_Selectionbox)
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
        /// Gets or sets whether the same entity can be selected multiple times in this selection box
        /// </summary>
        /// <value>True if the same entity can be selected multiple times in this selection box, false if not</value>
        public bool AllowMultipleSelectOfSameEntity { get; set; }

        /// <summary>
        /// Gets or sets whether an entity can be selected in this selection box if the entity is selected elsewhere. 
        /// </summary>
        /// <value><list type="bullet"><listheader>if--------------- then</listheader>
        /// <item>true--------------- <description>When an entity is selected while this selection box is active and that entity is selected in a different selection box, then the entity is added to this selection box.</description> </item>
        /// <item>true---------------  <description>If the entity is already selected in this selection box then the entity is removed from the selection box.</description> </item>
        /// <item>false---------------   <description>When an entity is selected while this selection box is active and that entity is already selected,  then the entity is removed from the selection box. This is the default behavior of a selection box.</description> </item>
        /// </list> </value>
        public bool AllowSelectInMultipleBoxes { get; set; }


        #region even handlers

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
        /// Called when a selection is made, which allows the add-in to accept or reject the selection. <strong>it must return <c>true</c> for selections to occure</strong><br/>
        /// you should pass a delegate that accepts (object WhatIsSelected, int swSelectType_e, string tag)
        /// </summary>
        /// <remarks> 
        /// <para>This method is called by SOLIDWORKS when an add-in 
        /// has a PropertyManager page displayed and a selection is made that passes the selection 
        /// filter criteria set up for a selection list box. The add-in can then:<br/> 
        /// </para>
        /// <list type="number">
        /// <item>Take the Dispatch pointer and the selection type.</item>
        /// <item>QueryInterface the Dispatch pointer to get the specific interface.</item>
        /// <item>Use methods or properties of that interface to determine if the selection should be allowed or not.If the selection is:
        /// <list type="bullet">
        /// <item>accepted, return true, and processing continues normally.</item>
        /// <item>rejected, return false, and SOLIDWORKS does not accept the selection, just as if the selection did not pass the selection filter criteria of the selection list box.</item>
        /// </list>
        /// </item>
        /// </list>
        /// <para>
        ///The add-in should not release the Dispatch pointer. SOLIDWORKS will release the Dispatch pointer upon return from this method.
        ///The method is called during the process of SOLIDWORKS selection.It is neither a pre-notification nor post-notification. <br/>
        ///The add-in should not be taking any action that might affect the model or the selection list.The add-in should only be querying information and then returning true/VARIANT_TRUE or false/VARIANT_FALSE.
        /// </para>
        /// </remarks>
        /// <value><see cref="OnSubmitSelection_Handler"/></value>
        public OnSubmitSelection_Handler OnSubmitSelection { get; set; }
        #endregion
    }

    /// <summary>
    /// </summary>
    /// <param name="Id">ID of the active selection box, where this selection is being made</param>
    /// <param name="selection">Object being selected</param>
    /// <param name="selectType">Entity type of the selection as defined in<see cref="swSelectType_e"/> </param>
    /// <param name="tag">ItemText is returned to SOLIDWORKS and stored on the selected object and can be used by your PropertyManager page selection list boxes for the life of that selection.</param>
    /// <returns></returns>
    public delegate bool OnSubmitSelection_Handler(object selection, int selectType, string tag);
}

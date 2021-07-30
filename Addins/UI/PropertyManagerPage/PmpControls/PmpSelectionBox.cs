using Hymma.SolidTools.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class PmpSelectionBox : PmpControl<IPropertyManagerPageSelectionbox>
    {
        #region private methods

        private CalloutModel _callout;
        private string _calloutLabel;
        private short _height;
        private swSelectType_e[] _filter;
        private bool _allowMultiSelect;
        #endregion

        /// <summary>
        /// default constructor
        /// <param name="Filter">/// <list type="table"><strong><listheader>FILTER ---- RESULTS</listheader></strong>
        /// <item>swSelFACES,  swSelSOLIDBODIES<description> ----- Face<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSELCOMPSDONTOVERRIDE.</description></item>
        /// <item>swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSelCOMPSDONTOVERRIDE.<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// </list>
        /// swSelSURFACEBODIES and swSelSURFBODIESFIRST behave simliar to swSelSOLIDBODIES and swSelSOLIDBODIESFIRST. swSelEDGES and swSelVERTICES behave similar to swSelFACES. If the Filters is set to swSelNOTHING or swSelUNSUPPORTED, this the call to this method fails.</param>
        ///<param name="AllowMultiSelect">True if the same entity can be selected multiple times in this selection box, false if not</param>
        /// <param name="Height">height of selectionbox in the pmp</param>
        /// </summary>
        public PmpSelectionBox(swSelectType_e[] Filter,bool AllowMultiSelect, short Height = 50) : base(swPropertyManagerPageControlType_e.swControlType_Selectionbox)
        {
            this._height = Height;
            this._filter = Filter;
            this._allowMultiSelect = AllowMultiSelect;
            OnRegister += PmpSelectionBox_OnRegister;
        }

        private void PmpSelectionBox_OnRegister()
        {
            Height = _height;
            Filter = _filter;
            AllowMultipleSelectOfSameEntity = _allowMultiSelect;
        }

        /// <summary>
        /// array of <see cref="swSelectType_e"/> to allow selection of specific types only.You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed. 
        /// </summary>
        /// <remarks>
        /// <list type="table"><strong><listheader>FILTER ---- RESULTS</listheader></strong>
        /// <item>swSelFACES,  swSelSOLIDBODIES<description> ----- Face<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSELCOMPSDONTOVERRIDE.</description></item>
        /// <item>swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSelCOMPSDONTOVERRIDE.<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// </list>
        /// swSelSURFACEBODIES and swSelSURFBODIESFIRST behave simliar to swSelSOLIDBODIES and swSelSOLIDBODIESFIRST. swSelEDGES and swSelVERTICES behave similar to swSelFACES. If the Filters is set to swSelNOTHING or swSelUNSUPPORTED, this the call to this method fails.
        /// </remarks>
        public swSelectType_e[] Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                SolidworksObject?.SetSelectionFilters(value);
            }
        }

        /// <summary>
        /// height of this selection box in proerty manager page
        /// </summary>
        public short? Height { get => SolidworksObject?.Height; set => SolidworksObject.Height = (short)value; }

        /// <summary>
        /// Gets or sets whether the same entity can be selected multiple times in this selection box
        /// </summary>
        /// <value>True if the same entity can be selected multiple times in this selection box, false if not</value>
        /// <remarks>You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public bool? AllowMultipleSelectOfSameEntity { get => SolidworksObject?.AllowMultipleSelectOfSameEntity; set => SolidworksObject.AllowMultipleSelectOfSameEntity = (bool)value; }

        /// <summary>
        /// Gets or sets whether an entity can be selected in this selection box if the entity is selected elsewhere. 
        /// </summary>
        /// <value><list type="table"><listheader>IF--------------- THEN</listheader>
        /// <item>true------------ <description>When an entity is selected while this selection box is active and that entity is selected in a different selection box, then the entity is added to this selection box.</description> </item>
        /// <item>true------------ <description>If the entity is already selected in this selection box then the entity is removed from the selection box.</description> </item>
        /// <item>false----------- <description>When an entity is selected while this selection box is active and that entity is already selected,  then the entity is removed from the selection box. This is the default behavior of a selection box.</description> </item>
        /// </list> 
        /// </value>
        public bool? AllowSelectInMultipleBoxes { get => SolidworksObject?.AllowSelectInMultipleBoxes; set => SolidworksObject.AllowMultipleSelectOfSameEntity = (bool)value; }

        /// <summary>
        /// create a clalout for this selectionbox
        /// </summary>
        /// <remarks>you should use this property in the context of a part or assembly or drawing environment i.e you cannot use it when solidworks starts</remarks>
        public CalloutModel CalloutModel
        {
            get
            {
                return _callout;
            }
            set
            {
                CalloutLabel = string.IsNullOrWhiteSpace(_calloutLabel) ? "Default" : _calloutLabel;
                _callout = value;
                SolidworksObject.Callout = value.SwCallout;
            }
        }

        /// <summary>
        /// a lable for the callout, unless this property has a value the callout for this selection box will not be created
        /// </summary>
        public string CalloutLabel
        {
            get => _calloutLabel;
            set { _calloutLabel = value; SolidworksObject?.SetCalloutLabel(value); }
        }

        /// <summary>
        /// Sets the color for selections made in this selection box on the PropertyManager page. 
        /// </summary>
        /// <param name="color">Color to use for selections as defined by the <see cref="SysColor"/></param>
        /// <remarks>You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public void SelectionColor(SysColor color)
        {
            SolidworksObject.SetSelectionColor(true, (int)color);
        }

        /// <summary>
        /// Gets the mark used on selected items in this selection box. 
        /// </summary>
        public int Mark { get => SolidworksObject.Mark; }

        /// <summary>
        /// If the application must rely on specific mark values for specific selection boxes, then set the Mark value before the PropertyManager page is shown. <br/>
        /// In this case, ensure that each selection box contains a different value. Otherwise, the users selection will be displayed in the selection boxes that have the same Mark value.
        /// Mark values(whether set by the SolidWorks application or by your application) must be powers of two(for example, 1, 2, 4, 8)
        /// </summary>
        /// <param name="mark"></param>
        public void SetMark(uint mark)
        {
            var isPowerOfTwo = (mark != 0) && ((mark & (mark - 1)) == 0);

            if (isPowerOfTwo)
                SolidworksObject.Mark = (int)mark;
            else
                throw new ArgumentOutOfRangeException($"you assigned {mark} to a selection box mark value. But {mark} is not a power of 2");
        }

        /// <summary>
        /// adds the selections to a selectionBox
        /// </summary>
        /// <param name="model">the document as ModelDoc2 object</param>
        /// <param name="selections">array of objects that needs to be added to the selection box</param>
        /// <remarks>
        ///set selection box's IPropertyManagerPageSelectionbox::Mark to a different power of two; for example, 1, 2, 4, 8, etc.
        ///(Setting a selection box's mark to 0 causes all selections to appear in that selection box and the active selection box.)
        ///</remarks>
        public void Append(ModelDoc2 model, object[] selections)
        {
            var swModelDocExt = model.Extension;
            SelectionMgr swSelectionMgr = model.SelectionManager as SelectionMgr;
            var swSelectData = (SelectData)swSelectionMgr.CreateSelectData();
            swSelectData.Mark = Mark;
            swModelDocExt.MultiSelect2(selections, true, swSelectData);
        }

        /// <summary>
        /// gets a list of items in a selection box whether they are selected or not
        /// </summary>
        /// <param name="model">the document where selections happen</param>
        /// <returns></returns>
        public IList<object> GetItems(ModelDoc2 model)
        {
            var selMgr = (SelectionMgr)model.SelectionManager;
            var items = new List<object>();
            var count = selMgr.GetSelectedObjectCount2(Mark);
            for (int i = 1; i <= count; i++)
            {
                items.Add(selMgr.GetSelectedObject6(i, Mark));
            }
            return items;
        }

        #region event handlers

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
        /// SOLIDWORKS will invoke this once a call-out is created for this selection box<br/>
        /// allows you to collect information such as the selection type from the last selection. Next, use the <see cref="CalloutModel"/> property to get the Callout object. <br/>
        /// Then, use that object's various properties to control the callout text and display characteristics based on that selection information.
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
    /// <param name="selection">Object being selected</param>
    /// <param name="selectType">Entity type of the selection as defined in<see cref="swSelectType_e"/> </param>
    /// <param name="tag">ItemText is returned to SOLIDWORKS and stored on the selected object and can be used by your PropertyManager page selection list boxes for the life of that selection.</param>
    /// </summary>
    /// <returns></returns>
    public delegate bool OnSubmitSelection_Handler(object selection, int selectType, string tag);
}

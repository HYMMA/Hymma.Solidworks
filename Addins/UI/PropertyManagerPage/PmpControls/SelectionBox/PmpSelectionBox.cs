using Hymma.SolidTools.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class PmpSelectionBox : PmpControl<IPropertyManagerPageSelectionbox>
    {
        #region private fields

        private CalloutModel _callout;
        private string _calloutLabel;
        private bool _enableSelectIdenticalComponents;
        private short _height;
        private IEnumerable<swSelectType_e> _filters;
        private int _style;
        private bool _allowMultipleSelectOfSameEntity;
        private bool _singleItemOnly;
        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="filters">
        /// <list type="bullet"><strong><listheader>FILTER ---- RESULTS</listheader></strong>
        /// <item>swSelFACES,  swSelSOLIDBODIES<description> ----- Face<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSELCOMPSDONTOVERRIDE.</description></item>
        /// <item>swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSelCOMPSDONTOVERRIDE.<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description>
        /// </item>
        /// swSelSURFACEBODIES and swSelSURFBODIESFIRST behave simliar to swSelSOLIDBODIES and swSelSOLIDBODIESFIRST. swSelEDGES and swSelVERTICES behave similar to swSelFACES. If the Filters is set to swSelNOTHING or swSelUNSUPPORTED, this the call to this method fails.
        /// </list>
        /// </param>
        /// <param name="allowMultipleSelectOfSameEntity">True if the same entity can be selected multiple times in this selection box, false if not</param>
        /// <param name="style">style of this selection box as defined by bitwise <see cref="SelectionBoxStyles"/></param>
        /// <param name="singleItemOnly">Gets or sets whether this selection box is for single or multiple items. </param>
        /// <param name="height">height of selectionbox in the pmp</param>
        /// <param name="tip">tip for the selectionbox</param>
        public PmpSelectionBox(IEnumerable<swSelectType_e> filters, SelectionBoxStyles style = SelectionBoxStyles.Default, bool allowMultipleSelectOfSameEntity = true, bool singleItemOnly = false, short height = 50, string tip = "")
            : base(swPropertyManagerPageControlType_e.swControlType_Selectionbox, "", tip)
        {
            _height = height;
            _filters = filters;
            _style = (int)style;
            _allowMultipleSelectOfSameEntity = allowMultipleSelectOfSameEntity;
            _singleItemOnly = singleItemOnly;
            OnRegister += PmpSelectionBox_OnRegister;
            OnDisplay += PmpSelectionBox_OnDisplay;
        }

        private void PmpSelectionBox_OnDisplay(PmpSelectionBox sender, SelBox_OnDisplay_EventArgs eventArgs)
        {
            eventArgs.Style = _style;
        }
        #endregion

        #region Call Backs
        private void PmpSelectionBox_OnRegister()
        {
            SolidworksObject.AllowMultipleSelectOfSameEntity = _allowMultipleSelectOfSameEntity;
            SolidworksObject.SingleEntityOnly = _singleItemOnly;
            SolidworksObject.EnableSelectIdenticalComponents = _enableSelectIdenticalComponents;
            SolidworksObject.Height = _height;
            SolidworksObject.SetSelectionFilters(_filters.Cast<int>().ToArray());
            Mark = Counter.GetNextSelBoxMark();
        }
        internal void FocusChanged()
        {
            OnFocusChanged?.Invoke(this);
        }
        internal void CallOutCreated()
        {
            OnCallOutCreated?.Invoke(this);
        }
        internal void CallOutDestroyed()
        {
            OnCallOutDestroyed?.Invoke(this);
        }

        internal void ListChanged(int count)
        {
            OnListChanged?.Invoke(this, new SelectionBox_OnListChanged_EventArgs(count));
        }

        internal bool SubmitSelection(object selection, int selectType, string tag)
        {

            //since this must return true for selection to happen
            //if user didnt set it up we return true to make sure 
            //addin works as expected
            if (OnSubmitSelection == null)
                return true;

            //otherwise return whatever user has set up for it
            return OnSubmitSelection.Invoke(this, new SelectionBox_OnSubmitSelection_EventArgs(selection, selectType, tag));
        }

        internal override void Display()
        {
            OnDisplay?.Invoke(this, new SelBox_OnDisplay_EventArgs(this, _filters, _style, _allowMultipleSelectOfSameEntity, _singleItemOnly, _height));
        }
        #endregion

        #region public properties

        /// <summary>
        /// PropertyManager page's cursor after a user makes a selection in the SOLIDWORKS graphics area. 
        /// </summary>
        /// <remarks>allows an interactive user to either: <br/>
        ///move to the next selection box on the PropertyManager page or <br/>
        ///okay and close a PropertyManager page<br/>
        ///after making a selection in the SOLIDWORKS graphics area. </remarks>
        public PmpCursorStyles CursorStyle { get; set; } = PmpCursorStyles.None;

        /// <summary>
        /// Gets or sets whether an entity can be selected in this selection box if the entity is selected elsewhere. 
        /// </summary>
        /// <value><list type="table"><listheader>IF--------------- THEN</listheader>
        /// <item>true------------ <description>When an entity is selected while this selection box is active and that entity is selected in a different selection box, then the entity is added to this selection box.</description> </item>
        /// <item>true------------ <description>If the entity is already selected in this selection box then the entity is removed from the selection box.</description> </item>
        /// <item>false----------- <description>When an entity is selected while this selection box is active and that entity is already selected,  then the entity is removed from the selection box. This is the default behavior of a selection box.</description> </item>
        /// </list> 
        /// </value>
        public bool AllowSelectInMultipleBoxes
        {
            get => SolidworksObject != null && SolidworksObject.AllowSelectInMultipleBoxes;
            set
            {
                if (SolidworksObject != null)
                    SolidworksObject.AllowMultipleSelectOfSameEntity = value;
                else
                    OnRegister += () => { SolidworksObject.AllowMultipleSelectOfSameEntity = value; };
            }
        }

        

        /// <summary>
        /// create a clalout for this selectionbox
        /// </summary>
        /// <remarks>you should use this property in the context of a part or assembly or drawing environment i.e you cannot use it when solidworks starts</remarks>
        public CalloutModel Callout
        {
            get => _callout;
            set
            {
                CalloutLabel = string.IsNullOrWhiteSpace(_calloutLabel) ? "Default" : _calloutLabel;
                _callout = value;
                SolidworksObject.Callout = value.SolidworksObject;
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
        /// Gets or sets the index number of the currently selected item in this selection box. 
        /// </summary>
        /// <remarks>The return value Item is the item in the selection box that is selected. Only the active selection box can have a current selection. If you use this property with an inactive selection box, -1 is returned.<see cref="IsFocused"/>  to determine if a selection box is active or not.</remarks>
        public int CurrentSelection
        {
            get
            {
                if (SolidworksObject != null)
                    return SolidworksObject.CurrentSelection;
                return -1;
            }
            set
            {
                if (SolidworksObject != null)
                    SolidworksObject.CurrentSelection = value;
            }
        }

        /// <summary>
        /// Gets whether this is the active selection box.  
        /// </summary>
        /// <value>True if the selection box is active, false if not</value>
        public bool IsFocused => SolidworksObject != null && SolidworksObject.GetSelectionFocus();

        /// <summary>
        /// Gets or sets whether to enable Select Identical Components in the context menu of this PropertyManager page selection box. 
        /// </summary>
        public bool EnableSelectIdenticalComponents
        {
            get => _enableSelectIdenticalComponents;
            set
            {
                _enableSelectIdenticalComponents = value;
                if (SolidworksObject != null)
                    SolidworksObject.EnableSelectIdenticalComponents = value;
                else
                    OnRegister += () => { SolidworksObject.EnableSelectIdenticalComponents = value; };
            }
        }

        /// <summary>
        /// Gets the number of items currently in this selection box. 
        /// </summary>
        public int ItemCount
        {
            get
            {
                if (SolidworksObject != null)
                    return SolidworksObject.ItemCount;
                return 0;
            }
        }



        /// <summary>
        /// Gets the mark used on selected items in this selection box. 
        /// </summary>
        /// <value> mark value for this selectin box </value>
        /// <remarks>if called before selection box is registered returns -1</remarks>
        private int Mark
        {
            get
            {
                if (SolidworksObject == null)
                    return -1;
                return SolidworksObject.Mark;
            }

            set
            {
                // Mark values(whether set by the SolidWorks application or by your application) must be powers of two(for example, 1, 2, 4, 8)
                var isPowerOfTwo = (value != 0) && ((value & (value - 1)) == 0);

                if (isPowerOfTwo)
                    SolidworksObject.Mark = value;
                else
                    throw new ArgumentOutOfRangeException($"you assigned {value} to a selection box mark value. But {value} is not a power of 2");
            }
        }

        /// <summary>
        /// adds the selections to a selectionBox
        /// </summary>
        /// <param name="items">array of selected objects to add to the selection box</param>
        /// <remarks>
        ///use this method to add items to the selection box when selection box is not focused. for example you can add all solid bodies of an assembly to a selection box once user clicked on a button.
        ///</remarks>
        public void Append(object[] items)
        {
            var swModelDocExt = ActiveDoc.Extension;
            SelectionMgr swSelectionMgr = ActiveDoc.SelectionManager as SelectionMgr;
            var swSelectData = (SelectData)swSelectionMgr.CreateSelectData();
            swSelectData.Mark = Mark;
            swModelDocExt.MultiSelect2(items, true, swSelectData);
        }

        /// <summary>
        /// gets a dicitonary of items and their type in a selection box whether they are selected or not
        /// folow instructions in the link below to get the actual object of the selected item 
        /// </summary>
        ///<remarks> <a href="http://help.solidworks.com/2019/english/api/swconst/SOLIDWORKS.Interop.swconst~SOLIDWORKS.Interop.swconst.swSelectType_e.html">solidworks website</a>
        ///<list type="bullet">
        ///<item>IF . . . . . . . . . . . . . . .<description> . . .THEN THIS METHOD RETURNS</description></item>
        ///<item>Reference surfaces are selected. . . . . .<description>Reference surface faces instead of the entire reference surface feature.</description></item>
        ///<item>Top-level item is selected in the DimXpertManager tab . . . . . .<description><see cref="IDimXpertManager"/> object. </description></item>
        ///<item>Non-top-level items are selected in the DimXpertManager tab . . . . . .<description><see cref="IFeature"/> object. </description></item>
        ///<item>Dimensions are selected  . . . . . .<description><see cref="IDisplayDimension"/> object instead of the <see cref="IDimension"/> object.  </description></item>
        ///<item>Called in a drawing document . . . . . .<description>Selected <see cref="IDrawingComponent"/> object. </description></item>
        ///<item>Called in a part or assembly document . . . . . .<description> <see cref="IComponent2"/> object. </description></item>
        /// </list></remarks> 
        /// <returns> a dicitonary of items and their type as defined in <see cref="swSelectType_e"/> Nothing or null might be returned if the type is not supported or if nothing is selected</returns>
        public IEnumerable<KeyValuePair<object, swSelectType_e>> GetItems()
        {
            var itemsTypes = new List<KeyValuePair<object, swSelectType_e>>();
            for (int i = 0; i < ItemCount; i++)
            {
                itemsTypes.Add(GetItem((uint)i));
            }
            return itemsTypes;
        }

        /// <summary>
        /// get the specified item in the selection box <br/>
        /// folow instructions in the link below to get the actual object of the selected item 
        /// </summary>
        /// <param name="index">0-based index of the item in the selection manager</param>
        ///<remarks> <a href="http://help.solidworks.com/2019/english/api/swconst/SOLIDWORKS.Interop.swconst~SOLIDWORKS.Interop.swconst.swSelectType_e.html">solidworks website</a>
        ///<list type="bullet">
        ///<item>IF . . . . . . . . . . . . . . .<description> . . .THEN THIS METHOD RETURNS</description></item>
        ///<item>Reference surfaces are selected. . . . . .<description>Reference surface faces instead of the entire reference surface feature.</description></item>
        ///<item>Top-level item is selected in the DimXpertManager tab . . . . . .<description><see cref="IDimXpertManager"/> object. </description></item>
        ///<item>Non-top-level items are selected in the DimXpertManager tab . . . . . .<description><see cref="IFeature"/> object. </description></item>
        ///<item>Dimensions are selected  . . . . . .<description><see cref="IDisplayDimension"/> object instead of the <see cref="IDimension"/> object.  </description></item>
        ///<item>Called in a drawing document . . . . . .<description>Selected <see cref="IDrawingComponent"/> object. </description></item>
        ///<item>Called in a part or assembly document . . . . . .<description> <see cref="IComponent2"/> object. </description></item>
        /// </list></remarks> 
        /// <returns>Selected object as defined in <see cref="swSelectType_e"/> Nothing or null might be returned if the type is not supported or if nothing is selected</returns>
        public KeyValuePair<object, swSelectType_e> GetItem(uint index)
        {
            if (ActiveDoc == null || SolidworksObject == null)
                throw new ArgumentNullException("A Call to Selection manager::GetItem failed becuase ActiveDoc or Selection manager was null");
            if (index > ItemCount - 1)
                throw new ArgumentOutOfRangeException("the index provided to selection manager::GetItem was more than items in the selection box");

            SelectionMgr selMgr = (SelectionMgr)ActiveDoc.SelectionManager;

            //index to use in selection manager
            //selection manager index is 1-based
            var selIndex = SolidworksObject.SelectionIndex[(int)index];

            //get item via selection manager
            var type = (swSelectType_e)selMgr.GetSelectedObjectType3((int)selIndex, Mark);

            //get type of object
            object item = selMgr.GetSelectedObject6(selIndex, Mark);
            return new KeyValuePair<object, swSelectType_e>(item, type);
        }
        #endregion

        #region events
        /// <summary>
        /// SOLIDWORKS will invoke this once focus is changed from this selection box
        /// </summary>
        public event SelectionBox_EventHandler OnFocusChanged;

        /// <summary>
        /// SOLIDWORKS will invoke this once a call-out is created for this selection box<br/>
        /// allows you to collect information such as the selection type from the last selection. Next, use the <see cref="Callout"/> property to get the Callout object. <br/>
        /// Then, use that object's various properties to control the callout text and display characteristics based on that selection information.
        /// </summary>
        public event SelectionBox_EventHandler OnCallOutCreated;

        /// <summary>
        /// SOLIDWORKS will invoke this once a callout is destroyed
        /// </summary>
        public event SelectionBox_EventHandler OnCallOutDestroyed;

        /// <summary>
        /// Regardless of how many items the user selects, this event is called only once per interactive box selection. In other words, if the user selects six faces using a box selection, this method is called only once. <br/>
        /// </summary>
        public event SelectionBox_EventHandler<SelectionBox_OnListChanged_EventArgs> OnListChanged;

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
        public event SelectionBox_OnSubmitSelectionEventHandler OnSubmitSelection;

        /// <summary>
        /// fired just a moment before the property manager page and its controls are displayed
        /// </summary>
        public new event SelectionBox_EventHandler<SelBox_OnDisplay_EventArgs> OnDisplay;
        #endregion
    }
}

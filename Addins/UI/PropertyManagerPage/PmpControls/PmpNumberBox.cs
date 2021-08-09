using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// creates and allows you to access a number box in a property manager page
    /// </summary>
    public class PmpNumberBox : PmpTextBase<IPropertyManagerPageNumberbox>
    {
        #region constructors
        /// <summary>
        /// creates a number box in a property manager page
        /// </summary>
        /// <param name="style">style for this numberBox as defined by <see cref="NumberBoxStyle"/></param>
        public PmpNumberBox(NumberBoxStyle style) : base(swPropertyManagerPageControlType_e.swControlType_Numberbox)
        {
            Style = (int)style;
            OnRegister += PmpNumberBox_OnRegister;
            OnDisplay += PmpNumberBox_OnDisplay;
        }


        #endregion

        #region methods

        private void PmpNumberBox_OnDisplay()
        {
            PmpNumberBox_OnRegister();
        }

        private void PmpNumberBox_OnRegister()
        {
           SolidworksObject.Style = Style;
        }

        /// <summary>
        /// Sets the range and increment for the slider. 
        /// </summary>
        /// <param name="Units">Number box units as defined in <see cref="NumberBoxUnit"/> (see Remarks)</param>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        /// <param name="Increment"></param>
        /// <param name="fastIncrement">Fast increment value for scrolling and mouse-wheel</param>
        /// <param name="slowIncrement">Slow increment value for scrolling and mouse-wheel</param>
        /// <param name="Inclusive">whether the max should be inclusive in the range or not</param>
        /// <remarks>This method works while a PropertyManager page is displayed with these restrictions: <br/>
        /// You cannot change Units once the page is displayed.The Units parameter is ignored if specified while the page is displayed. <br/>
        ///If the range is changed to an invalid value by this method, then you must immediately call <see cref="Value"/> and set a valid value to prevent displaying the dialog that requests the user to enter a valid value.
        ///</remarks>
        public void SetRange(NumberBoxUnit Units, double Minimum, double Maximum, double Increment, double fastIncrement, double slowIncrement, bool Inclusive = true)
        {
            SolidworksObject?.SetRange2((int)Units, Minimum, Maximum, Inclusive, Increment, fastIncrement, slowIncrement);
        }

        /// <summary>
        /// Adds items to the attached drop-down list of a number box. 
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(string[] items)
        {
            SolidworksObject?.AddItems(items);
        }
        
        /// <summary>
        /// Clears all items from the attached drop-down list for this the number box. 
        /// </summary>
        public void Clear() => SolidworksObject?.Clear();

        /// <summary>
        /// Gets the text for an item in the attached drop-down list for this number box. 
        /// </summary>
        /// <param name="item">Position of the item where to get the text in the 0-based list or -1 to get the text of the currently selected item</param>
        /// <returns>Text for this item</returns>
        public string ItemText(short item)
        {
            return SolidworksObject?.ItemText[item];
        }

        /// <summary>
        /// Deletes an item from the attached drop-down list for this number box. 
        /// </summary>
        /// <param name="item">Index number of the item to delete from the 0-based list of items</param>
        /// <returns>Number of items remaining in the list or -1 if the item is not deleted</returns>
        public short? DeleteItem(short item)
        {
            return SolidworksObject?.DeleteItem(item);
        }

        /// <summary>
        /// Inserts an item in the attached drop-down list for this number box. 
        /// </summary>
        /// <param name="item">Position where to add the item in the 0-based list or -1 to put the item at the end of the list</param>
        /// <param name="text">Text for item</param>
        /// <returns>Position in the 0-based list where the item is added or -1 if the item is not added to the list</returns>
        public short? InsertItem(short item, string text)
        {
            return SolidworksObject?.InsertItem(item, text);
        }

        /// <summary>
        /// Sets the parameters for the slider. 
        /// </summary>
        /// <param name="positionCount">Number of discreet positions on the slider</param>
        /// <param name="divisionCount">Number of regions separated by tick marks on the slider</param>
        /// <remarks>
        /// When a user drags the slider, PositionCount defines how sensitive the slider is. Not all of the specified discreet points are displayed if the PropertyManager page is not wide enough to show them. However, if the user widens the PropertyManager page, then more points are displayed.
        ///When a user drags the slider, the user-interface tends to snap to the nearest tick mark when the drag is nearby, making it easier for the user to set whole values.
        ///</remarks>
        public void SetSliderParameters(int positionCount, int divisionCount) => SolidworksObject?.SetSliderParameters(positionCount, divisionCount);

        #endregion

        #region public properties
        /// <summary>
        /// Gets and sets the value that appears in the number box. 
        /// </summary>
        public double? Value { get => SolidworksObject?.Value; set => SolidworksObject.Value = value.GetValueOrDefault(); }

        /// <summary>
        /// gets or sets the current selection in the number box
        /// </summary>
        /// <value>0-based index of the selection</value>
        public short? CurrentSelection { get => SolidworksObject?.CurrentSelection; set => SolidworksObject.CurrentSelection = value.GetValueOrDefault(); }

        /// <summary>
        /// 	Gets or sets the maximum height of the attached drop-down list for this number box.  
        /// </summary>
        public short? Height{get => SolidworksObject?.Height; set => SolidworksObject.Height = value.GetValueOrDefault();}

        /// <summary>
        /// style for this numberBox as defined by <see cref="NumberBoxStyle"/>
        /// </summary>
        public int Style { get; set; }

        /// <summary>
        /// Gets the text that appears in the number box. 
        /// </summary>
        /// <remarks>If a user changes the value in an number box by typing in a new value, the <see cref="OnTextChanged"/> is called with the current text string.</remarks>
        public string Text => SolidworksObject?.Text;

        /// <summary>
        /// Gets or sets the unit type to display in this PropertyManager page number box. 
        /// </summary>
        /// <remarks> <see cref="DisplayedUnit "/>allows an add-in to have a number box that shows length values in inches, even though the system default units are meters.<br/>
        /// Remember that the values specified for both <see cref="Value"/> and <see cref="SetRange(NumberBoxUnit, double, double, double, double, double, bool)"/> are in system units; <br/>
        /// <see cref="DisplayedUnit "/> simply controls how that value is displayed in the PropertyManager page number box.
        ///You can call <see cref="DisplayedUnit "/> and change the units displayed in a number box while a Propertymanager page is displayed.</remarks>
        public int? DisplayedUnit
        {
            get => SolidworksObject?.DisplayedUnit;
            set => SolidworksObject.DisplayedUnit = value.GetValueOrDefault();
        }
        #endregion

        #region events

        /// <summary>
        /// called when user changes the value in an number box by typing in a new value
        /// </summary>
        /// <remarks>You can then use <see cref="Text"/> to show the text elsewhere, such as in a callout.</remarks>
        public Action<string> OnTextChanged { get; set; }

        /// <summary>
        /// fired when user changes the value via typing or clicking the up-arrow or down-arrow buttons to increment or decrement the value
        /// </summary>
        /// <remarks>solidworks will pass in the double vlue upon change</remarks>
        public Action<double> OnChange { get; set; }
        #endregion

    }

    /// <summary>
    /// units for numberbox
    /// </summary>
    public enum NumberBoxUnit
    {
        /// <summary>
        /// angle
        /// </summary>
        Angle = 4,

        /// <summary>
        /// density
        /// </summary>
        Density = 5,

        /// <summary>
        /// force
        /// </summary>
        Force = 7,
        /// <summary>
        /// frequency
        /// </summary>
        Frequency = 10,

        /// <summary>
        /// gravity
        /// </summary>
        Gravity = 8,

        /// <summary>
        /// length
        /// </summary>
        Length = 3,

        /// <summary>
        /// percent
        /// </summary>
        Percent = 11,

        /// <summary>
        /// stress
        /// </summary>
        Stress = 6,

        /// <summary>
        /// time
        /// </summary>
        Time = 9,

        /// <summary>
        /// unitless doulbe
        /// </summary>
        UnitlessDouble = 2,

        /// <summary>
        /// unitles integer
        /// </summary>
        UnitlessInteger = 1
    }

    /// <summary>
    ///PropertyManager page number box styles.Bitmask.
    /// </summary>
    /// <remarks>When the user selects an item in the attached drop-down list, SOLIDWORKS attempts to use that item as a value in the number box.<br/>
    /// Thus, the items in the attached drop-down list should be numeric values and optionally include their units. <br/>
    /// The add-in then gets a callback via <see cref="PmpNumberBox.OnChange"/> as if the user had typed a value in the number box or clicked the up-arrow or down-arrow buttons to increment or decrement the value.<br/>
    ///If you do not want your add-in to directly use items in the attached drop-down list in the number box, but instead want it to react to the user selecting a computed or linked value in the number box,<br/>
    ///then use <see cref="AvoidSelectionText"/></remarks>
    [Flags]
    public enum NumberBoxStyle
    {
        /// <summary>
        /// The item the user selects in the attached drop-down list does not appear in the number box. Instead, the user's selection causes the add-in to get a callback via <see cref="PmpComboBox.OnSelectionChanged"/> <br/>
        /// the Id argument will be the number box; the add-in is expected to respond by setting the value for the number box using IPropertyManagerPageNumberbox::Value.  
        /// </summary>
        AvoidSelectionText = 4,

        /// <summary>
        /// Enables the attached drop-down list for the number box; user can type a value or select a value from the attached drop-down list for the number box
        /// </summary>
        ComboEditBox = 1,

        /// <summary>
        /// User can only select a value from the attached drop-down list for the number box
        /// </summary>
        /// <remarks>You can set EditBoxReadOnly either before or after the PropertyManager page is displayed. <br/>
        /// If set after the PropertyManager page is displayed and the number box contains editable text, then that text cannot be edited by the user</remarks>
        EditBoxReadOnly = 2,

        /// <summary>
        /// Do not show the up and down arrows in the number box, thus, disallowing incrementing or decrementing the value in the number box
        /// </summary>
        NoScrollArrows = 8,

        /// <summary>
        /// Slider
        /// </summary>
        Slider = 16,

        /// <summary>
        /// Suppress sending multiple notifications when a user is dragging or spinning the slider of a number box on a PropertyManager page; instead, send only one notification; see IPropertyManagerPage2Handler9::OnNumberboxChanged for details
        /// </summary>
        SuppressNotifyWhileTracking = 64,

        /// <summary>
        /// Thumbwheel
        /// </summary>
        Thumbwheel = 32
    }
}

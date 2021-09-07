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
        #region fields
        private NumberBoxStyles _style;
        private NumberBoxUnit _displayUnit;
        #endregion

        #region constructors
        /// <summary>
        /// creates a number box in a property manager page
        /// </summary>
        /// <param name="style">style for this numberBox as defined by <see cref="NumberBoxStyles"/></param>
        public PmpNumberBox(NumberBoxStyles style) : base(swPropertyManagerPageControlType_e.swControlType_Numberbox)
        {
            Style = style;
        }

        #endregion

        #region methods

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
            OnRegister += () =>
            {
                SolidworksObject.SetRange2((int)Units, Minimum, Maximum, Inclusive, Increment, fastIncrement, slowIncrement);
            };
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
        public short? Height { get => SolidworksObject?.Height; set => SolidworksObject.Height = value.GetValueOrDefault(); }

        /// <summary>
        /// style for this numberBox as defined by <see cref="NumberBoxStyles"/>
        /// </summary>
        public NumberBoxStyles Style
        {
            get => _style;
            set
            {
                _style = value;

                //if add-in is loaded already
                if (SolidworksObject != null)
                    SolidworksObject.Style = (int)value;
                else
                    OnRegister += () => { SolidworksObject.Style = (int)value; };
            }
        }

        /// <summary>
        /// Gets or sets the unit type to display in this PropertyManager page number box. 
        /// </summary>
        /// <remarks> <see cref="DisplayedUnit "/>allows an add-in to have a number box that shows length values in inches, even though the system default units are meters.<br/>
        /// <see cref="DisplayedUnit "/> simply controls how that value is displayed in the PropertyManager page number box.
        ///You can call this porperty and change the units displayed in a number box while a Propertymanager page is displayed.</remarks>
        public NumberBoxUnit DisplayedUnit
        {
            get => _displayUnit;
            set
            {
                _displayUnit = value;
                //if add-in is loaded already
                if (SolidworksObject != null)
                    SolidworksObject.DisplayedUnit = (int)value;

                //otherwise update the property when the control is loaded
                else
                    OnRegister += () => { SolidworksObject.DisplayedUnit = (int)value; };
            }
        }
        #endregion

        #region Call backs
        internal void TextChanged(string text)
        {
            OnTypeIn?.Invoke(this, text);
        }

        internal void Changed(double value)
        {
            OnChange?.Invoke(this, value);
        }

        internal override void Display()
        {
            OnDisplay?.Invoke(this, new NumberBox_Ondisplay_EventArgs(this));
        }
        #endregion

        #region events

        /// <summary>
        /// called when user changes the value in an number box by typing in a new value, solidworks will pass in the text that was entered
        /// </summary>
        public event EventHandler<string> OnTypeIn;

        /// <summary>
        /// fired when user changes the value via typing or clicking the up-arrow or down-arrow buttons to increment or decrement the value
        /// </summary>
        /// <remarks>solidworks will pass in the double vlue upon change</remarks>
        public event EventHandler<double> OnChange;

        /// <summary>
        /// fired a moment before this number box is displayed in a property manager page
        /// </summary>
        public new event NumberBox_OnDisplay_EventHandler OnDisplay;
        #endregion
    }
}

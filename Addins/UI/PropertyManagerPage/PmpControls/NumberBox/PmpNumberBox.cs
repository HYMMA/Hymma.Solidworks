// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// creates and allows you to access a number box in a property manager page
    /// </summary>
    public class PmpNumberBox : PmpTextBase<IPropertyManagerPageNumberbox>
    {
        #region fields
        private NumberBoxStyles _style;
        private NumberBoxUnit _displayUnit;
        private double _value;
        private short _height;
        #endregion

        #region constructors
        /// <summary>
        /// creates a number box in a property manager page
        /// </summary>
        /// <param name="style">style for this numberBox as defined by <see cref="NumberBoxStyles"/></param>
        public PmpNumberBox(NumberBoxStyles style=NumberBoxStyles.Default) : base(swPropertyManagerPageControlType_e.swControlType_Numberbox)
        {
            Style = style;
        }

        #endregion

        #region methods
        /// <summary>
        /// Inserts an item in the attached drop-down list for this number box. 
        /// </summary>
        /// <param name="item">Position where to add the item in the 0-based list or -1 to put the item at the end of the list</param>
        /// <param name="text">Text for item</param>
        public short InsertItem(short item, string text)
        {
            short result = -1;
            if (SolidworksObject != null)
                result = SolidworksObject.InsertItem(item, text);
            else
                Registering += () =>
                {
                    result = SolidworksObject.InsertItem(item, text);
                };
            return result;
        }

        /// <summary>
        /// Sets the range and increment for the slider. 
        /// </summary>
        /// <param name="Units">Number box units as defined in <see cref="NumberBoxUnit"/> (see Remarks)</param>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        /// <param name="Inclusive">whether the max should be inclusive in the range or not</param>
        /// <param name="Increment">increment in <strong>meter</strong></param>
        /// <param name="fastIncrement">Fast increment value for scrolling and mouse-wheel</param>
        /// <param name="slowIncrement">Slow increment value for scrolling and mouse-wheel</param>
        /// <remarks>This method works while a PropertyManager page is displayed with these restrictions: <br/>
        /// You cannot change Units once the page is displayed.The Units parameter is ignored if specified while the page is displayed. <br/>
        ///If the range is changed to an invalid value by this method, then you must immediately call <see cref="Value"/> and set a valid value to prevent displaying the dialog that requests the user to enter a valid value. 
        ///<para>
        ///SolidWORKS internal units are <strong>metric</strong> it will treat this methods parameters as such. for example increment will be Meter for lengths. 
        ///</para>
        ///</remarks>
        public void SetRange(NumberBoxUnit Units, double Minimum, double Maximum, bool Inclusive, double Increment, double fastIncrement, double slowIncrement)
        {
            Registering += () =>
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
            if (SolidworksObject != null)
            {
                SolidworksObject.AddItems(items);
            }
            else
            {
                Registering += () => { SolidworksObject.AddItems(items); };
            }
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
        public void SetSliderParameters(int positionCount, int divisionCount)
        {
            if (SolidworksObject != null)
            {
                SolidworksObject?.SetSliderParameters(positionCount, divisionCount);
            }
            else
            {
                Registering += () => { SolidworksObject.SetSliderParameters(positionCount, divisionCount); };
            }
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets and sets the value that appears in the number box. 
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                if (SolidworksObject != null)
                {
                    SolidworksObject.Value = value;
                }
                else
                {
                    Registering += () => { SolidworksObject.Value = value; };
                }
            }
        }


        /// <summary>
        /// 	Gets or sets the maximum height of the attached drop-down list for this number box.  
        /// </summary>
        public short Height
        {
            get => _height;
            set
            {
                _height = value;

                //if addin is loaded
                if (SolidworksObject != null)
                    SolidworksObject.Height = value;
                else
                    Registering += () => { SolidworksObject.Height = value; };
            }
        }

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
                    Registering += () => { SolidworksObject.Style = (int)value; };
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
                    Registering += () => { SolidworksObject.DisplayedUnit = (int)value; };
            }
        }
        #endregion


        #region Call backs
        internal void TextChangedCallback(string text) => TextChanged?.Invoke(this, text);

        internal void ChangedCallback(double value) => Changing?.Invoke(this, value);

        internal override void DisplayingCallback()
           => Displaying?.Invoke(this, new PmpNumberBoxDisplayingEventArgs(this));

        internal void TrackingCompletedCallback(double val) => TrackingCompleted?.Invoke(this, val);
        internal void SelectionChangedCallback(int item)
         =>SelectionChanged?.Invoke(this, SolidworksObject.ItemText[(short)item]);
        #endregion

        #region events

        /// <summary>
        /// called when user changes the value in an number box by typing in a new value, SolidWORKS will pass in the text that was entered
        /// </summary>
        public event EventHandler<string> TextChanged;

        /// <summary>
        /// fired when user changes the value via typing or clicking the up-arrow or down-arrow buttons to increment or decrement the value
        /// </summary>
        /// <remarks>SolidWORKS will pass in the double value upon change</remarks>
        public event EventHandler<double> Changing;

        /// <summary>
        /// Called when a user finishes changing the value in the number box on a PropertyManager page. 
        /// </summary>
        public event EventHandler<double> TrackingCompleted;

        /// <summary>
        /// fired when Style has <see cref="NumberBoxStyles.AvoidSelectionText"/> | <see cref="NumberBoxStyles.ComboEditBox"/> and user selects an item from the combo box
        /// </summary>
         public event EventHandler<string> SelectionChanged;

        /// <summary>
        /// fired a moment before this number box is displayed in a property manager page
        /// </summary>
        public new event PmpNumberBoxDisplayingEventHandler Displaying;

        #endregion
    }
}

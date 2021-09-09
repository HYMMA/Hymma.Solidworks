using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// event arguments for numberbox
    /// </summary>
    public class NumberBox_Ondisplay_EventArgs : OnDisplay_EventArgs
    {
        #region fields
        private IPropertyManagerPageNumberbox SolidworksObject;
        #endregion

        #region constructors
        /// <summary>
        /// event arguments for <see cref="PmpNumberBox.OnDisplay"/>
        /// </summary>
        /// <param name="pmpNumberBox"></param>
        public NumberBox_Ondisplay_EventArgs(PmpNumberBox pmpNumberBox) : base((IPropertyManagerPageControl)pmpNumberBox.SolidworksObject)
        {
            SolidworksObject = pmpNumberBox.SolidworksObject;
        }
        #endregion

        #region methods
        /// <summary>
        /// changes the range and increment for the slider. 
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
        ///If the range is changed to an invalid value by this method, then you must immediately call <see cref="PmpNumberBox.Value"/> and set a valid value to prevent displaying the dialog that requests the user to enter a valid value.
        ///</remarks>
        public void ChangeRange(NumberBoxUnit Units, double Minimum, double Maximum, double Increment, double fastIncrement, double slowIncrement, bool Inclusive = true)
        {
            SolidworksObject?.SetRange2((int)Units, Minimum, Maximum, Inclusive, Increment, fastIncrement, slowIncrement);
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
        public short DeleteItem(short item)
        {
            return SolidworksObject.DeleteItem(item);
        }
        
        #endregion

        #region properties
        /// <summary>
        /// Gets the text that appears in the number box. 
        /// </summary>
        /// <remarks>If a user changes the value in an number box by typing in a new value, the <see cref="PmpNumberBox.OnTextChanged"/> is called with the current text string.</remarks>
        public string Text => SolidworksObject?.Text;

        /// <summary>
        /// gets or sets the current selection in the number box
        /// </summary>
        /// <value>0-based index of the selection</value>
        public short CurrentSelection
        {
            get => SolidworksObject.CurrentSelection;
            set => SolidworksObject.CurrentSelection = value;
        }
        #endregion
    }
}

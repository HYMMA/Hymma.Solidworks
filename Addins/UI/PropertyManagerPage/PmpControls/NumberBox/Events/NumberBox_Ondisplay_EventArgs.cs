using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// event arguments for numberbox
    /// </summary>
    public class NumberBox_Ondisplay_EventArgs : OnDisplay_EventArgs
    {
        private IPropertyManagerPageNumberbox SolidworksObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pmpNumberBox"></param>
        public NumberBox_Ondisplay_EventArgs(PmpNumberBox pmpNumberBox) : base((IPropertyManagerPageControl)pmpNumberBox.SolidworksObject)
        {
            SolidworksObject = pmpNumberBox.SolidworksObject;
        }

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
        /// Gets the text that appears in the number box. 
        /// </summary>
        /// <remarks>If a user changes the value in an number box by typing in a new value, the <see cref="PmpNumberBox.OnTypeIn"/> is called with the current text string.</remarks>
        public string Text => SolidworksObject?.Text;
    }
}

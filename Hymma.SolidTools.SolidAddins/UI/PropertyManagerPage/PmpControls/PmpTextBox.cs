namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a class to represent a text box inside a property manager page in solidworks
    /// </summary>
    public class PmpTextBox : PmpTextControl
    {
        /// <summary>
        /// make a text box for a property manager page in soldiworks
        /// </summary>
        /// <param name="initialValue"></param>
        public PmpTextBox(string initialValue=""):base(swPmpControlsWithText.Textbox)
        {
            this.InitialValue = initialValue;
        }

        /// <summary>
        /// initial value for this text box once generated in a porperty manager page
        /// </summary>
        public string InitialValue { get; private set; }
    }
}

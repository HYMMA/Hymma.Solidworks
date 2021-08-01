using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a class to represent a text box inside a property manager page in solidworks
    /// </summary>
    public class PmpTextBox : PmpControl<PropertyManagerPageTextbox>
    {
        private string initialValue;

        /// <summary>
        /// make a text box for a property manager page in soldiworks
        /// </summary>
        /// <param name="initialValue">initial value for this text box once generated in a porperty manager page</param>
        public PmpTextBox(string initialValue = "") : base(swPropertyManagerPageControlType_e.swControlType_Textbox)
        {
            this.initialValue = initialValue;
            OnRegister += PmpTextBox_OnRegister;
        }

        private void PmpTextBox_OnRegister()
        {
            Text = initialValue;
        }

        /// <summary>
        /// value for this text box
        /// </summary>
        public string Text { get => SolidworksObject?.Text; set => SolidworksObject.Text = value; }

        /// <summary>
        /// Styles as defined by bitmask <see cref="TexTBoxStyles"/>
        /// </summary>
        public int? Style { get =>SolidworksObject?.Style; set => SolidworksObject.Style = value.GetValueOrDefault(); }

        /// <summary>
        /// fires when user changes the text in the text box
        /// </summary>
        public Action<string> OnChange { get; set; }
    }

    /// <summary>
    /// PropertyManager page textbox styles. Bitmask. 
    /// </summary>
    [Flags]
    public enum TexTBoxStyles
    {
        /// <summary>
        /// Do not send notification every time a character in the text box changes; instead, only send notification when text box loses focus after the user has made all changes
        /// </summary>
        NotifyOnlyWhenFocusLost = 1,

        /// <summary>
        /// text box will be read only
        /// </summary>
        ReadOnly = 2,

        /// <summary>
        /// remove borders
        /// </summary>
        NoBorder = 4,


        /// <summary>
        /// multiple lines
        /// </summary>
        Multiline = 8
    }
}

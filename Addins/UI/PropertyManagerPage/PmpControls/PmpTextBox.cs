using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a class to represent a text box inside a property manager page in solidworks
    /// </summary>
    public class PmpTextBox : PmpTextBase<PropertyManagerPageTextbox>
    {
        private int _style;

        //this field will be used in event handler to update the state of the Text in this object
        //hence it is internal
        internal string textTypedByUser;

        /// <summary>
        /// make a text box for a property manager page in soldiworks
        /// </summary>
        /// <param name="initialValue">initial value for this text box once generated in a porperty manager page</param>
        /// <param name="binding">if set true, binds the value of the controller to the <see cref="Text"/></param>
        public PmpTextBox(string initialValue = "", bool binding=true) : base(swPropertyManagerPageControlType_e.swControlType_Textbox)
        {
            Text = initialValue;
            Binding = binding;
            OnRegister += PmpTextBox_OnRegister;
            OnDisplay += PmpTextBox_OnDisplay;
        }

        private void PmpTextBox_OnDisplay()
        {
            //update the text to what it was before user closed the property manager page
            SolidworksObject.Text = textTypedByUser;
        }

        private void PmpTextBox_OnRegister()
        {
            Style = _style;
            SolidworksObject.Text = Text;
        }

        /// <summary>
        /// value for this text box
        /// </summary>
        public string Text
        {
            get => textTypedByUser;
            set
            {
                textTypedByUser = value;

                //this is important. So when framework client changes the style while the pmp is displaying the state of the style in the pmp updates properly
                if (SolidworksObject != null)
                    SolidworksObject.Text = value;
            }
        }

        /// <summary>
        /// gets whether this controller is bound to the value of the <see cref="Text"/>
        /// </summary>
        public bool Binding { get; }

        /// <summary>
        /// Styles as defined by bitmask <see cref="TexTBoxStyles"/>
        /// </summary>
        public int Style
        {
            get => _style;
            set
            {
                _style = value;

                //this is important. So when framework client changes the style while the pmp is displaying the state of the style in the pmp updates properly
                if (SolidworksObject != null)
                    SolidworksObject.Style = value;
            }
        }

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

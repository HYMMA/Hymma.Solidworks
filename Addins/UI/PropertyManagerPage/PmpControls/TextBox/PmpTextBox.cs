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
        #region fields

        private TexTBoxStyles _style;
        private string _text;
        #endregion

        #region constructor

        /// <summary>
        /// make a text box for a property manager page in soldiworks
        /// </summary>
        /// <param name="initialValue">initial value for this text box once generated in a porperty manager page</param>
        public PmpTextBox(string initialValue = "") : base(swPropertyManagerPageControlType_e.swControlType_Textbox)
        {
            Text = initialValue;
        }
        #endregion

        #region properties

        /// <summary>
        /// value for this text box
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                //if add in is loaded
                if (SolidworksObject != null)
                    SolidworksObject.Text = value;
                else
                    OnRegister += () => { SolidworksObject.Text = value; };
            }
        }
        
        /// <summary>
        /// Styles as defined by bitmask <see cref="TexTBoxStyles"/>
        /// </summary>
        public TexTBoxStyles Style
        {
            get => _style;
            set
            {
                _style = value;

                //if add in is loaded
                if (SolidworksObject != null)
                    SolidworksObject.Style = (int)value;
                else
                    OnRegister += () => { SolidworksObject.Style = (int)value; };
            }
        }
        #endregion

        #region call backs

        internal void TextChanged(string e)
        {
            OnChange?.Invoke(this, e);
        }
        #endregion

        #region events

        /// <summary>
        /// fires when user changes the text in the text box
        /// </summary>
        public event EventHandler<string> OnChange;
        #endregion
    }
}

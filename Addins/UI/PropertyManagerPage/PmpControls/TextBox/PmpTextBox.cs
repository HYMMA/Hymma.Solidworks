using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a class to represent a text box inside a property manager page in solidworks
    /// </summary>
    public class PmpTextBox : PmpTextBase
    {
        #region fields

        private TexTBoxStyles _style;
        private string _text;
        private short _height;
        #endregion

        #region constructor

        /// <summary>
        /// make a text box for a property manager page in soldiworks
        /// </summary>
        /// <param name="initialValue">initial value for this text box once generated in a porperty manager page</param>
        /// <param name="tip">a tip for this text box</param>
        public PmpTextBox(string initialValue = "",string tip="") : base(swPropertyManagerPageControlType_e.swControlType_Textbox,tip:tip)
        {
            Registering += () => SolidworksObject = (PropertyManagerPageTextbox)Control;
            Value = initialValue;
        }
        #endregion

        #region properties

        /// <summary>
        /// value for this text box
        /// </summary>
        public string Value
        {
            get => _text;
            set
            {
                _text = value;

                //if add in is loaded
                if (SolidworksObject != null)
                {
                    SolidworksObject.Text = value;
                }
                else
                {
                    Registering += () => { SolidworksObject.Text = value; };
                }
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
                    Registering += () => { SolidworksObject.Style = (int)value; };
            }
        }

        /// <summary>
        /// set the height of this control
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
        /// solidworks object
        /// </summary>
        public PropertyManagerPageTextbox SolidworksObject { get; private set; }
        #endregion

        #region call backs

        internal void UserTypedCallBack(string e)
        {
            UserTyped?.Invoke(this, e);
        }

        #endregion

        #region events
        /// <summary>
        /// fires when text box is changed
        /// </summary>
        public event EventHandler<string> UserTyped;
        #endregion
    }
}

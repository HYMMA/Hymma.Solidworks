using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.Solidworks.Addins
{

    /// <summary>
    /// a lable inside a property manager page
    /// </summary>
    public class PmpLabel : PmpTextBase
    {
        private LabelStyles _style;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="text">text of this lable</param>
        /// <param name="style">style of the lable as defined by bitwie <see cref="LabelStyles"/></param>
        public PmpLabel(string text, LabelStyles style = LabelStyles.LeftText) : base(swPropertyManagerPageControlType_e.swControlType_Label, text)
        {
            Registering += () => SolidworksObject = (PropertyManagerPageLabel)Control;
            Style = style;
        }

        /// <summary>
        /// sets whether to italicize the specified range of characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="status"></param>
        public void SetItalic(short StartChar, short EndChar, bool status)
        {
            //if add in was loaded
            if (SolidworksObject != null)
                SolidworksObject.Italic[StartChar, EndChar] = status;
            //otherwise assign value upon addin startup
            else
                Registering += () => { SolidworksObject.Italic[StartChar, EndChar] = status; };
        }

        /// <summary>
        /// gets whether a pecified range of characters in this property manager label are italic or not
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        public bool GetItalic(short StartChar, short EndChar)
        {
            if (SolidworksObject != null)
                return SolidworksObject.Italic[StartChar, EndChar];
            return false;
        }

        /// <summary>
        /// Sets whether to raise or lower the specified characters above or below their baselines, relative to their heights, in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="offset">
        /// Specify:<br/>
        ///0.0 to show the character on its baseline<br/>
        ///-1.0 to show the character 1 character below its baseline<br/>
        ///+1.0 to show the character 1 character above its baseline
        ///</param>
        /// <remarks>allows you to show that character as a subscript or exponent in an equation.</remarks>
        public void SetLineOffset(short StartChar, short EndChar, double offset)
        {
            //if add-in was loaded
            if (SolidworksObject != null)
                SolidworksObject.LineOffset[StartChar, EndChar] = offset;
            else
                Registering += () => { SolidworksObject.LineOffset[StartChar, EndChar] = offset; };
        }

        
        /// <summary>
        /// Gets or sets the size of the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="ratio">Ratio for the height of the characters relative to their expected heights &gt;0 increases their heights and &lt;0 decreases their height</param>
        public void SetSizeRatio(short StartChar, short EndChar, double ratio)
        {
            if (SolidworksObject != null)
                SolidworksObject.SizeRatio[StartChar, EndChar] = ratio;
            else
                Registering += () => { SolidworksObject.SizeRatio[StartChar, EndChar] = ratio; };
        }

        /// <summary>
        /// sets the background color for the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="color">value for the text background color for the specified characters; if not specified, then the background color for this control is used</param>
        public void SetBackgroundColor(short StartChar, short EndChar, Color color)
        {
            var RGB = ColorTranslator.ToWin32(color);
            if (SolidworksObject != null)
                SolidworksObject.CharacterBackgroundColor[StartChar, EndChar] = RGB;
            else
                Registering += () => { SolidworksObject.CharacterBackgroundColor[StartChar, EndChar] = RGB; };
        }

        /// <summary>
        /// sets the color of the specified characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="color">value for the text color for the specified characters; </param>
        public void SetCharacterColor(short StartChar, short EndChar, Color color)
        {
            var RGB = ColorTranslator.ToWin32(color);
            if (SolidworksObject != null)
                SolidworksObject.CharacterColor[StartChar, EndChar] = RGB;
            else
                Registering += () => { SolidworksObject.CharacterColor[StartChar, EndChar] = RGB; };
        }

        /// <summary>
        ///sets  the font for the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="font">Name of the font to use for the specified characters</param>
        public void SetFont(short StartChar, short EndChar, string font)
        {
            if (SolidworksObject != null)
                SolidworksObject.Font[StartChar, EndChar] = font;
            else
                Registering += () => { SolidworksObject.Font[StartChar, EndChar] = font; };
        }

        /// <summary>
        /// Sets whether to apply the specified underline style to the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar"></param>
        /// <param name="EndChar"></param>
        /// <param name="style"></param>
        public void SetUnderLineStyle(short StartChar, short EndChar, UnderLineStyles style)
        {
            if (SolidworksObject != null)
                SolidworksObject.Underline[StartChar, EndChar] = (int)style;
            else
                Registering += () => { SolidworksObject.Underline[StartChar, EndChar] = (int)style; };
        }

        /// <summary>
        /// style as defined by <see cref="LabelStyles"/>
        /// </summary>
        public LabelStyles Style
        {
            get => _style;
            set
            {
                //update backing field
                _style = value;

                //if called after addin started
                if (SolidworksObject != null)
                    SolidworksObject.Style = (int)value;

                //otherwise register the value once addin is loaded
                else
                    Registering += () => { SolidworksObject.Style = (int)value; };
            }
        }
        /// <summary>
        /// Solidworks object
        /// </summary>
        public PropertyManagerPageLabel SolidworksObject { get; private set; }

        /// <summary>
        /// sets the range of specified characters to bold. 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <param name="status">bold status</param>
        public void SetBold(short start, short end, bool status)
        {
            //if addin is loaded
            if (SolidworksObject != null)
                SolidworksObject.Bold[start, end] = status;

            //otherwise do it when addin loaded (started)
            else
                Registering += () => { SolidworksObject.Bold[start, end] = status; };
        }

        #region Callbacks
        /// <summary>
        /// called <see cref="Displaying"/>
        /// </summary>
        internal override void DisplayingCallBack()
        {
            Displaying?.Invoke(this, new PmpLabelDisplayingEventArgs(this));
        }
        #endregion

        /// <summary>
        /// raised a moment before this label is displayed in the property manager page
        /// </summary>
        public new event PmpLabelDisplayingEventHandler Displaying;
    }
}

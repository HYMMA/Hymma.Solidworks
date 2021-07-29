using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// a lable inside a property manager page
    /// </summary>
    public class PmpLabel : PmpControl<PropertyManagerPageLabel>
    {
        private LabelStyles _style;
        private string _caption;
        private short _height;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption">caption for this lable</param>
        /// <param name="style">style of the lable as defined by bitwie <see cref="LabelStyles"/></param>
        /// <param name="height">Because SOLIDWORKS sizes the label appropriately based on the text it contains, you should not have to use this parameter. However, if the label does not contain text, then using this property might be useful.</param>
        public PmpLabel(string caption, LabelStyles style = LabelStyles.LeftText, short height = 8) : base(swPropertyManagerPageControlType_e.swControlType_Label)
        {
            _style = style;
            _caption = caption;
            _height = height;
            OnRegister += PmpLabel_OnRegister;
        }

        private void PmpLabel_OnRegister()
        {
            Caption = _caption;
            Style = (int)_style;
            SolidworksObject.Height = _height;
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets whether to italicize the specified range of characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="status"></param>
        public void SetItalic(short StartChar, short EndChar, bool status)
        {
            if (SolidworksObject != null)
                SolidworksObject.Italic[StartChar, EndChar] = status;
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
            if (SolidworksObject != null)
            {
                SolidworksObject.LineOffset[StartChar, EndChar] = offset;
            }
        }

        /// <summary>
        /// Gets the size of the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>Ratio for the height of the characters relative to their expected heights &gt;0 increases their heights and &lt;0 decreases their height<br/>
        /// 0 for errors</returns>
        public double GetSizeRatio(short StartChar, short EndChar)
        {
            if (SolidworksObject != null)
            {
                return SolidworksObject.SizeRatio[StartChar, EndChar];
            }
            return 0;
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
            {
                SolidworksObject.SizeRatio[StartChar, EndChar] = ratio;
            }
        }

        /// <summary>
        /// Gets whether to raise or lower the specified characters above or below their baselines, relative to their heights, in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>offset of this character or -1000 for error</returns>
        public double GetLineOffset(short StartChar, short EndChar)
        {
            if (SolidworksObject != null)
            {
                return SolidworksObject.LineOffset[StartChar, EndChar];
            }
            return -1000;
        }

        /// <summary>
        /// sets the background color for the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="RGB">RGB value for the text background color for the specified characters; if not specified, then the background color for this control is used</param>
        public void SetBackgroundColor(short StartChar, short EndChar, int RGB)
        {
            if (SolidworksObject != null)
            {
                SolidworksObject.CharacterBackgroundColor[StartChar, EndChar] = RGB;
            }
        }

        /// <summary>
        /// gets the background color for the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>integer representation of the rgb color or -1 for error</returns>
        public int GetBackgroundColor(short StartChar, short EndChar)
        {
            if (SolidworksObject != null)
                return SolidworksObject.CharacterBackgroundColor[StartChar, EndChar];
            return -1;
        }
        /// <summary>
        /// gets the color of the specified characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>RGB value for the text color for the specified characters or -1 for error</returns>
        public int GetCharacterColor(short StartChar, short EndChar)
        {
            if (SolidworksObject != null)
            {
                return SolidworksObject.CharacterColor[StartChar, EndChar];
            }
            return -1;
        }

        /// <summary>
        /// sets the color of the specified characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <param name="RGB">RGB value for the text color for the specified characters; </param>
        public void SetCharacterColor(short StartChar, short EndChar, int RGB)
        {
            if (SolidworksObject != null)
                SolidworksObject.CharacterColor[StartChar, EndChar] = RGB;
        }

        /// <summary>
        ///Gets the font for the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>Name of the font to use for the specified characters or empty string for error</returns>
        public string GetFont(short StartChar, short EndChar)
        {
            if (SolidworksObject != null)
            {
                return SolidworksObject.Font[StartChar, EndChar];
            }
            return "";
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
        }

        /// <summary>
        /// Sets whether to apply the specified underline style to the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar"></param>
        /// <param name="EndChar"></param>
        /// <param name="style"></param>
        public void SetUnderLineStyle(short StartChar, short EndChar, UnderLineStyle style)
        {
            if (SolidworksObject!=null)
            {
                SolidworksObject.Underline[StartChar, EndChar] = (int)style;
            }
        }

        /// <summary>
        /// Gets underline style to the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>int value as defined by <see cref="UnderLineStyle"/> or -1 for errors</returns>
        public int GetUnderLineStyle(short StartChar, short EndChar)
        {
            if (SolidworksObject!=null)
                return SolidworksObject.Underline[StartChar, EndChar];
            return -1;
        }

        /// <summary>
        /// style as defined by <see cref="LabelStyles"/>
        /// </summary>
        public int Style
        {
            get
            {
                if (SolidworksObject != null)
                {
                    return SolidworksObject.Style;
                }
                return 0;
            }
            set
            {
                if (SolidworksObject != null)
                {
                    SolidworksObject.Style = value;
                }
            }
        }

        /// <summary>
        /// sets the range of specified characters to bold. 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <param name="status">bold status</param>
        public void SetBold(short start, short end, bool status)
        {
            if (SolidworksObject != null)
            {
                SolidworksObject.Bold[start, end] = status;
            }
        }

        /// <summary>
        /// get the bold status of specifid characteres 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <returns></returns>
        public bool GetBold(short start, short end)
        {
            if (SolidworksObject != null)
            {
                return SolidworksObject.Bold[start, end];
            }
            return false;
        }
    }

    /// <summary>
    /// styles for a label in property manager pages
    /// </summary>
    [Flags]
    public enum LabelStyles
    {
        /// <summary>
        /// align to center
        /// </summary>
        CenterText = 2,

        /// <summary>
        /// aling to the left (the defual value)
        /// </summary>
        LeftText = 1,

        /// <summary>
        /// align to the right
        /// </summary>
        RightText = 4,

        /// <summary>
        /// with shadow
        /// </summary>
        Sunken = 8,
    }

    /// <summary>
    /// Underline style of PropertyManager page label
    /// </summary>
    public enum UnderLineStyle
    {
        /// <summary>
        /// dashed under line
        /// </summary>
        DashedUnderline = 2,

        /// <summary>
        /// remove under line
        /// </summary>
        NoUnderline = 0,

        /// <summary>
        /// solid under line
        /// </summary>
        SolidUnderline = 1
    }
}

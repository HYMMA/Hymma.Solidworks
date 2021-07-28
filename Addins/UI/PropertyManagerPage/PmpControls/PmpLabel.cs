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

        /// <summary>
        /// default constructor
        /// </summary>
        public PmpLabel(string caption, LabelStyles style = LabelStyles.LeftText) : base(swPropertyManagerPageControlType_e.swControlType_Label)
        {
            _style = style;
            _caption = caption;
            OnRegister += PmpLabel_OnRegister;
        }

        private void PmpLabel_OnRegister()
        {
            Caption = _caption;
            Style = (int)_style;
            throw new NotImplementedException();
        }
        /// <summary>
        /// sets the background color for the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndCharnd">0-based index value of end character</param>
        /// <param name="RGB">RGB value for the text background color for the specified characters; if not specified, then the background color for this control is used</param>
        public void SetBackgroundColor(short StartChar, short EndCharnd, int RGB)
        {
            if (SolidworksObject != null)
            {
                SolidworksObject.CharacterBackgroundColor[StartChar, EndCharnd] = RGB;
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
            if (SolidworksObject!=null)
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
                 SolidworksObject.CharacterColor[StartChar, EndChar]=RGB;
        }

        /// <summary>
        ///Gets the font for the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>Name of the font to use for the specified characters or empty string for error</returns>
        public string GetFont(short StartChar, short EndChar)
        {
            if (SolidworksObject!=null)
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
}

using SolidWorks.Interop.sldworks;
using System;
using System.Drawing;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event arguments for a <see cref="PmpLabel"/>
    /// </summary>
    public class PmpLabelDisplayingEventArgs : DisplayingEventArgs
    {
        #region fields
        private PropertyManagerPageLabel SolidworksObject;

        /// <summary>
        /// the label that the event arguments are related to
        /// </summary>
        private PmpLabel Lable;
        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="label"></param>
        public PmpLabelDisplayingEventArgs(PmpLabel label) : base((IPropertyManagerPageControl)label.SolidworksObject)
        {
            this.Lable = label;
            this.SolidworksObject = label.SolidworksObject;
        }
        #endregion
        
        #region properties
        /// <summary>
        /// Gets or sets the height of this label. 
        /// </summary>
        /// <remarks>Because SOLIDWORKS sizes the label appropriately based on the text it contains, you should not have to use this property. However, if the label does not contain text, then using this property might be useful.</remarks>
        public short Height { get => SolidworksObject.Height; set => SolidworksObject.Height = value; }

        #endregion

        #region methods

        /// <summary>
        /// Gets the size of the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>Ratio for the height of the characters relative to their expected heights &gt;0 increases their heights and &lt;0 decreases their height</returns>
        public double GetSizeRatio(short StartChar, short EndChar)
        {
                return SolidworksObject.SizeRatio[StartChar, EndChar];
        }

        /// <summary>
        /// Gets whether to raise or lower the specified characters above or below their baselines, relative to their heights, in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>offset of this character</returns>
        public double GetLineOffset(short StartChar, short EndChar)
        {
                return SolidworksObject.LineOffset[StartChar, EndChar];
        }

        /// <summary>
        /// Gets underline style to the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>int value as defined by <see cref="UnderLineStyles"/> or -1 for errors</returns>
        public int GetUnderLineStyle(short StartChar, short EndChar)
        {
            return SolidworksObject.Underline[StartChar, EndChar];
        }
        /// <summary>
        ///Gets the font for the specified characters in this PropertyManager label.
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>Name of the font to use for the specified characters or empty string for error</returns>
        public string GetFont(short StartChar, short EndChar)
        {
            return SolidworksObject.Font[StartChar, EndChar];
        }

        /// <summary>
        /// get the bold status of specifid characteres 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <returns></returns>
        public bool GetBold(short start, short end)
        {
            return SolidworksObject.Bold[start, end];
        }

        /// <summary>
        /// gets the background color for the specified range of characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>integer representation of the rgb color or -1 for error</returns>
        public Color GetBackgroundColor(short StartChar, short EndChar)
        {
            var rgb = SolidworksObject.CharacterBackgroundColor[StartChar, EndChar];
            return ColorTranslator.FromWin32(rgb);
        }

        /// <summary>
        /// gets the color of the specified characters in this PropertyManager label. 
        /// </summary>
        /// <param name="StartChar">0-based index value of start character</param>
        /// <param name="EndChar">0-based index value of end character</param>
        /// <returns>RGB value for the text color for the specified characters or -1 for error</returns>
        public Color GetCharacterColor(short StartChar, short EndChar)
        {
            var rgb = SolidworksObject.CharacterColor[StartChar, EndChar];
            return ColorTranslator.FromWin32(rgb);
        }
        #endregion
    }
}

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
        /// <summary>
        /// default constructor
        /// </summary>
        public PmpLabel() : base(swPropertyManagerPageControlType_e.swControlType_Label)
        {

        }

        /// <summary>
        /// style as defined by <see cref="LabelStyles"/>
        /// </summary>
        public int Style { get => SolidworksObject.Style; set => SolidworksObject.Style = value; }
        
        /// <summary>
        /// Gets or sets RGB color of the text of a label on a PropertyManager page
        /// </summary>
        public int TextColor { get; set; }

        /// <summary>
        /// sets the range of specified characters to bold. 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <param name="status">bold status</param>
        public void SetBold(short start, short end, bool status) => SolidworksObject.Bold[start, end] = status;

        /// <summary>
        /// get the bold status of specifid characteres 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <returns></returns>
        public bool GetBold(short start, short end) => SolidworksObject.Bold[start, end];


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

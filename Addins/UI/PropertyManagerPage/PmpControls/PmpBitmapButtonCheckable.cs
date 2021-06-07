using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a checkable bitmap button
    /// </summary>
    public class PmpBitmapButtonCheckable : PmpBitmapButtonCustom
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PmpBitmapButtonCheckable()
        {
            Type = swPropertyManagerPageControlType_e.swControlType_CheckableBitmapButton;
        }

        /// <summary>
        /// Gets whether the bitmap button is clickable
        /// </summary>
        public bool IsCheckable { get; set; } = true;

        /// <summary>
        /// Gets or sets the state of the bitmap button
        /// </summary>
        public bool Checked { get; set; } = false;
    }
}

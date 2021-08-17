using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a checkable bitmap button
    /// </summary>
    public class PmpBitmapButtonCheckable : PmpBitmapButton
    {
        /// <summary>
        /// define a checkable bitmap button using a standard bitmap icon
        /// </summary>
        public PmpBitmapButtonCheckable(BitmapButtons standardBitmap,string tip) : base(standardBitmap,tip)
        {
        }

        /// <summary>
        /// define a checkable bitmap button using a custom bitmap icon
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        public PmpBitmapButtonCheckable(Bitmap bitmap, string fileName, string tip) : base(bitmap, fileName,tip)
        {
        }

        /// <summary>
        /// gets or sets if this button is checked
        /// </summary>
        public bool Checked { get => SolidworksObject.Checked; set => SolidworksObject.Checked = value; }

        /// <summary>
        /// gets or sets if this button is clickable
        /// <br/>This property is only meaningful and used by the SOLIDWORKS application when the bitmap button control is of type swControlType_CheckableBitmapButton
        /// </summary>
        public bool IsCheckable { get => SolidworksObject.IsCheckable; set => SolidworksObject.IsCheckable = value; }
    }
}

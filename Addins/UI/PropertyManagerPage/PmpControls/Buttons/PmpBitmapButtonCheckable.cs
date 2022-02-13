using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a checkable bitmap button
    /// </summary>
    public class PmpBitmapButtonCheckable : PmpBitmapButton
    {
        /// <summary>
        /// define a checkable bitmap button using a standard bitmap icon
        /// </summary>
        public PmpBitmapButtonCheckable(BitmapButtons standardBitmap, string tip) : base(standardBitmap, tip)
        {
            Type = swPropertyManagerPageControlType_e.swControlType_CheckableBitmapButton;
        }

        /// <summary>
        /// define a checkable bitmap button using a custom bitmap icon
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="tip"></param>
        /// <param name="iconSizes">possible sizes for this checkable button</param>
        /// <param name="opacity">define opacity of the bitmap on the button, less values result in more transparent pictures</param>
        public PmpBitmapButtonCheckable(Bitmap bitmap,  string tip, BtnSize iconSizes, byte opacity) : base(bitmap,tip, iconSizes, opacity)
        {
            Type = swPropertyManagerPageControlType_e.swControlType_CheckableBitmapButton;
        }

        /// <summary>
        /// gets or sets if this button is checked
        /// </summary>
        public bool Checked { get => SolidworksObject.Checked; set => SolidworksObject.Checked = value; }
    }
}

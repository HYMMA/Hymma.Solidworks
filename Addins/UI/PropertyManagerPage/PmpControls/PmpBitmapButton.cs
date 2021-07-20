using SolidWorks.Interop.sldworks;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a bitmap button for property manager page
    /// </summary>
    public class PmpBitmapButton : PmpControl<PropertyManagerPageBitmapButton>
    {

        /// <summary>
        /// a customised bitmap button for property manager pages
        /// </summary>
        public PmpBitmapButton() : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_BitmapButton)
        {
        }

        /// <summary>
        /// assign a bitmap to this bitmap button
        /// </summary>
        /// <param name="bitmap"></param>
        public void SetBitmap(Bitmap bitmap)
        {
            IconGenerator.GetBtnBitmaps(bitmap, out string[] images, out string[] masked);
            SolidworksObject.SetBitmapsByName3(images, masked);
        }

        /// <summary>
        /// gets or sets if this button is checked
        /// </summary>
        bool Checked { get; set; } = false;

        /// <summary>
        /// gets or sets if this button is chekcable
        /// </summary>
        bool IsCheckable { get; set; } = false;
    }
}

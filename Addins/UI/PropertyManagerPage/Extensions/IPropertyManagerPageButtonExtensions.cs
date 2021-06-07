using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Addins

{
    public static class IPropertyManagerPageButtonExtensions
    {
        /// <summary>
        /// Displays a bubble ToolTip containing the specified title, message, and bitmap. 
        /// </summary>
        /// <param name="title"> title to display</param>
        /// <param name="message">message to display in bubble tooltip</param>
        /// <param name="bitmapFile">Path and filename of bitmap to display in bubble ToolTip</param>
        public static void AddBubbleToolTip(this IPropertyManagerPageButton button,string title, string message, string bitmapFile)
        {
            if (button is PropertyManagerPageControl control)
                control.ShowBubbleTooltip(title, message, bitmapFile);
        }
    }
}

using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.SolidTools.Extensions
{
    public static class IPropertyManagerPageSelectionboxExtensions
    {
        /// <summary>
        /// adds the selections to a selectionBox
        /// </summary>
        /// <param name="model">the document as ModelDoc2 object</param>
        /// <param name="selections">array of objects that needs to be added to the selection box</param>
        /// <remarks>
        ///set selection box's IPropertyManagerPageSelectionbox::Mark to a different power of two; for example, 1, 2, 4, 8, etc.
        ///(Setting a selection box's mark to 0 causes all selections to appear in that selection box and the active selection box.)
        ///</remarks>
        public static void Append(this IPropertyManagerPageSelectionbox selectionbox, ModelDoc2 model, object[] selections)
        {
            var swModelDocExt = model.Extension;
            SelectionMgr swSelectionMgr = model.SelectionManager;
            var swSelectData = (SelectData)swSelectionMgr.CreateSelectData();
            swSelectData.Mark = selectionbox.Mark;
            swModelDocExt.MultiSelect2(selections, true, swSelectData);
        }
        public static void Enabled(this IPropertyManagerPageSelectionbox selectionbox, bool status)
        {
            if (selectionbox is IPropertyManagerPageControl control)
                control.Enabled = status;
        }

        /// <summary>
        /// Displays a bubble ToolTip containing the specified title, message, and bitmap. 
        /// </summary>
        /// <param name="title"> title to display</param>
        /// <param name="message">message to display in bubble tooltip</param>
        /// <param name="bitmapFile">Path and filename of bitmap to display in bubble ToolTip</param>
        public static void AddBubbleToolTip(this IPropertyManagerPageSelectionbox selectionbox, string title, string message,string bitmapFile)
        {
            if (selectionbox is IPropertyManagerPageControl control)
                control.ShowBubbleTooltip(title, message, bitmapFile);

        }

        /// <summary>
        /// gets a list of items in a selection box whether they are selected or not
        /// </summary>
        /// <param name="selectionBox"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IList<object> GetItems(this IPropertyManagerPageSelectionbox selectionBox,ModelDoc2 model)
        {
            var selMgr = (SelectionMgr)model.SelectionManager;
            var items = new List<object>();
            var count = selMgr.GetSelectedObjectCount2(selectionBox.Mark);
            for (int i = 1; i <= count; i++)
            {
                items.Add(selMgr.GetSelectedObject6(i, selectionBox.Mark));
            }
            return items;
        }
    }
}

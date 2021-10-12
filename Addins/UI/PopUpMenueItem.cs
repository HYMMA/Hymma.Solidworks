using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// allows acces to a pop up menue item
    /// </summary>
    public class PopUpMenueItem
    {
        /// <summary>
        /// Adds a menu item to the pop-up menu
        /// </summary>
        /// <param name="itemText">Text for for the item</param>
        /// <param name="hint">Text displayed in the SOLIDWORKS status bar when the user moves the cursor over this pop-up menu item</param>
        /// <param name="documentTypes">Document types for which this pop-up menu item is displayed as defined in <see cref="swDocumentTypes_e"/></param>
        public PopUpMenueItem(string itemText, string hint, swDocumentTypes_e documentTypes)
        {
            this.ItemText = itemText; this.Hint = hint; this.DocumentType = documentTypes;
            this.Id = Counter.GetNextPopUpMenuItemId();
        }

        /// <summary>
        /// Determines which item was selected when the user selects a pop-up menu item. 
        /// </summary>
        public Action OnPress { get; set; }

        /// <summary>
        ///  When Windows attempts to select or deselect and enable or disable the pop-up menu item, SOLIDWORKS calls this method to get the state of the menu item from the add-in. 
        /// </summary>
        /// State of the specified unique user-defined pop-up menu item:
        /// 0 - Not selected(i.e., not checked) and disabled(i.e., grayed out)
        /// 1 - Not selected and enabled
        /// 2 - Selected(i.e., checked) and disabled
        /// 3 - Selected and enabled
        public Action<int> OnUpdate { get; set; }
        
        /// <summary>
        /// item text in the menue
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// text that appears on the status bar once user hovers over this menue item
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// document types that this item should appear in
        /// </summary>
        public swDocumentTypes_e DocumentType { get; set; }
        
        /// <summary>
        /// id of this menue pop up item that's assigned to it once the item was created
        /// </summary>
        public int Id { get; }
    }
}

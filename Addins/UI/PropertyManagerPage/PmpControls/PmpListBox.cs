using Hymma.Mathematics;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Allows you to access a PropertyManager page list box control.
    /// </summary>
    public class PmpListBox : PmpControl<PropertyManagerPageListbox>
    {
        /// <summary>
        /// make a list box in a property manager page
        /// </summary>
        public PmpListBox() : base(swPropertyManagerPageControlType_e.swControlType_Listbox)
        {

        }
        /// <summary>
        /// Adds items to the attached drop-down list for this list box.
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(string[] items)
        {
            SolidworksObject.AddItems(items);
        }

        /// <summary>
        /// Clears all items from attached drop-down list for this list box.
        /// </summary>
        public void Clear() => SolidworksObject.Clear();

        /// <summary>
        /// Gets the text for the specified item in this list box.
        /// </summary>
        /// <param name="Item">Position of the item where to get the text in the 0-based list or -1 to get the text of the currently selected item</param>
        /// <returns></returns>
        public string ItemText(short Item) => SolidworksObject.ItemText[Item];

        /// <summary>
        /// Inserts an item in the attached drop-down list of this list box.
        /// </summary>
        /// <param name="Item">Position where to add the item in the 0-based list or -1 to put the item at the end of the list</param>
        /// <param name="Text">Text for item</param>
        /// <returns>Position in the 0-based list where the item is added or -1 if the item is not added to the list</returns>
        public short InsertItem(short Item, string Text) => SolidworksObject.InsertItem(Item, Text);

        /// <summary>
        /// Removes the specified item from the attached drop-down list for this list box.  
        /// </summary>
        /// <param name="item">Index number of the item to delete from the 0-based list of items</param>
        /// <returns>Number of items remaining in the list or -1 if the item is not deleted</returns>
        public short DeleteItem(short item) => SolidworksObject.DeleteItem(item);

        /// <summary>
        /// Gets the number of items currently selected in a list box enabled for multiple selection.
        /// </summary>
        /// <returns>Number of items currently selected in this list box</returns>
        public int GetSelectedItemsCount() => SolidworksObject.GetSelectedItemsCount();

        /// <summary>
        /// Gets the items selected in a list box enabled for multiple selections.
        /// </summary>
        /// <returns>Array of  0-based index shorts of the currently selected items in this list box</returns>
        public object GetSelectedItems() => SolidworksObject.GetSelectedItems();

        /// <summary>
        /// Sets whether an item is selected or cleared in a list box enabled for multiple selection. 
        /// </summary>
        /// <param name="Item">Index of the item to select or clear</param>
        /// <param name="Selected">True to select the item, false to not</param>
        /// <returns>True if the item was selected or cleared, false if not</returns>
        /// <remarks>The value specified for Item must be a valid index number. If it is not, then this method returns false. Thus, set up your list item index before using this method.<br/>
        /// <para>
        ///If you use this method to set a selected item in a single-selection style list box and another item in the list box is already selected, then that item is automatically cleared. <br/>
        ///You can use this method to clear a selection in a single-selection style list box, which results in no current selection in that list box.
        /// </para>
        ///</remarks>
        public bool SetSelectedItem(short Item, bool Selected)
        {
            return SolidworksObject.SetSelectedItem(Item, Selected);
        }


        /// <summary>
        /// Gets and sets the item that is currently selected in this list box. 
        /// </summary>
        /// <value>Index number of the item in the 0-based list</value>
        /// <remarks>If you use this property with a list box enabled for multiple selections, then this method returns -1 and does not affect the list box.</remarks>
        public short CurrentSelection { get => SolidworksObject.CurrentSelection; set => SolidworksObject.CurrentSelection = value; }

        /// <summary>
        /// gets and sets the attached drop down list in this list box
        /// </summary>
        /// <value>
        /// 0 	Default height with no scrolling<br/>
        ///1 &lt; 30 	Specified height and no scrolling<br/>
        ///&gt;30  	    Specified height and scrolling, but no auto sizing<br/>
        ///</value>
        ///<remarks>The height is in dialog-box units. You can convert these values to screen units (pixels) by using the Windows MapDialogRect function.</remarks>
        public short Height { get => SolidworksObject.Height; set => SolidworksObject.Height = value; }

        /// <summary>
        /// get or set style as defined in <see cref="ListBoxStyle"/>
        /// </summary>
        /// <remarks>By default, only one list item can be selected at a time. When another list item is selected, that item becomes the active item and the previously selected item is cleared. </remarks>
        public int Style { get => SolidworksObject.Style; set => SolidworksObject.Style = value; }

        /// <summary>
        /// Gets the number of items in the attached drop-down list for this list box. 
        /// </summary>
        public int ItemCount => SolidworksObject.ItemCount;

        /// <summary>
        /// Called when the right-mouse button is released in a list box on this PropertyManager page.<br/>
        /// <see cref="Point"/> is the coordinate of the right-mouse button menu
        /// </summary>
        public Action<Point> OnRightMouseBtnUp { get; set; }
    }

    /// <summary>
    /// PropertyManager page list box styles. Bitmask. 
    /// </summary>
    [Flags]
    public enum ListBoxStyle
    {
        /// <summary>
        /// allow multiple selection in the list box
        /// </summary>
        AllowMultiSelect = 4,

        /// <summary>
        /// no integral height
        /// </summary>
        NoIntegralHeight = 2,

        /// <summary>
        /// Sort the list in alphabetically
        /// </summary>
        SortAlphabetically = 1
    }
}

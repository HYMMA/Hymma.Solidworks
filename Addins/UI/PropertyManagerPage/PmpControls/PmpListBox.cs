using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System;

namespace Hymma.SolidTools.Addins
{
    public class PmpListBox : PmpControl<PropertyManagerPageListbox>
    {
        public PmpListBox() : base(swPropertyManagerPageControlType_e.swControlType_Listbox)
        {

        }
        public IEnumerable<string> Items { get; set; }

        public void AddItems(object Texts);
        public void IAddItems(short TextCount, ref string Texts);
        public void Clear();
        public string get_ItemText(short Item);
        public short InsertItem(short Item, string Text);
        public short DeleteItem(short Item);
        public int GetSelectedItemsCount();
        public object GetSelectedItems();
        public short IGetSelectedItems(int Count);
        public bool SetSelectedItem(short Item, bool Selected);

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

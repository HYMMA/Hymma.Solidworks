using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.SolidTools.Addins
{
    public class PmpListBox : PmpControl<PropertyManagerPageListbox>
    {
        public PmpListBox():base(swPropertyManagerPageControlType_e.swControlType_Listbox)
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
        public short CurrentSelection { get; set; }
        public short Height { get; set; }
        public int Style { get; set; }
        public int ItemCount { get; }
    }
}

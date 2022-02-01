using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Allows you to access a PropertyManager page list box control.
    /// </summary>
    public class PmpListBox : PmpTextBase
    {
        #region private fields

        private readonly string[] _items;
        private short _height;
        private int _style;
        #endregion

        #region constructor

        /// <summary>
        /// make a list box in a property manager page
        /// </summary>
        /// <param name="items">items to add to this list box</param>
        /// <param name="caption">caption for this list box</param>
        /// <param name="tip">tooltip text for this list box</param>
        /// <param name="height">        
        /// 0 	Default height with no scrolling<br/>
        /// &lt; 30 	Specified height and no scrolling<br/>
        ///&gt;30  	    Specified height and scrolling, but no auto sizing<br/>
        ///</param>
        /// <param name="style">style of this list box as defined in bitwise <see cref="ListboxStyles"/></param>
        public PmpListBox(string[] items,string caption, string tip, short height = 0, ListboxStyles style = ListboxStyles.SortAlphabetically) : base(swPropertyManagerPageControlType_e.swControlType_Listbox,caption,tip)
        {
            _items = items;
            _height = height==0 ? (short)(5+items.Length*15) : height;
            _style = (int)style;
            Registering += PmpListBox_OnRegister;
            OnDisplay += PmpListBox_OnDisplay;
        }
        #endregion

        #region Call backs

        private void PmpListBox_OnDisplay(object sender, OnDisplay_EventArgs e)
        {
            SolidworksObject.Style = Style;
            SolidworksObject.Height = _height;
        }

        private void PmpListBox_OnRegister()
        {
            SolidworksObject = (PropertyManagerPageListbox)Control;
            AddItems(_items);
        }

        internal void RightMouseBtnUp(Tuple<double, double, double> point)
        {
            OnRightMouseBtnUp?.Invoke(this, point);
        }

        internal void SelectionChange(int count)
        {
            OnSelectionChange?.Invoke(this, count);
        }

        internal override void Display()
        {
            OnDisplay?.Invoke(this, new Listbox_OnDisplay_EventArgs(this, _height));
        }
        #endregion

        #region public methods


        /// <summary>
        /// Adds items to the attached drop-down list for this list box.
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(string[] items) => SolidworksObject?.AddItems(items);

        /// <summary>
        /// Clears all items from attached drop-down list for this list box.
        /// </summary>
        public void Clear() => SolidworksObject?.Clear();

        /// <summary>
        /// Gets the text for the specified item in this list box.
        /// </summary>
        /// <param name="Item">Position of the item where to get the text in the 0-based list or -1 to get the text of the currently selected item</param>
        /// <returns></returns>
        public string ItemText(short Item) => SolidworksObject?.ItemText[Item];

        /// <summary>
        /// Inserts an item in the attached drop-down list of this list box.
        /// </summary>
        /// <param name="Item">Position where to add the item in the 0-based list or -1 to put the item at the end of the list</param>
        /// <param name="Text">Text for item</param>
        /// <returns>Position in the 0-based list where the item is added or -1 if the item is not added to the list</returns>
        public short? InsertItem(short Item, string Text) => SolidworksObject?.InsertItem(Item, Text);

        /// <summary>
        /// Removes the specified item from the attached drop-down list for this list box.  
        /// </summary>
        /// <param name="item">Index number of the item to delete from the 0-based list of items</param>
        /// <returns>Number of items remaining in the list or -1 if the item is not deleted</returns>
        public short? DeleteItem(short item) => SolidworksObject?.DeleteItem(item);

        /// <summary>
        /// Gets the number of items currently selected in a list box enabled for multiple selection.
        /// </summary>
        /// <returns>Number of items currently selected in this list box</returns>
        public int? GetSelectedItemsCount() => SolidworksObject?.GetSelectedItemsCount();

        /// <summary>
        /// Gets the items selected in a list box enabled for multiple selections.
        /// </summary>
        /// <returns>Array of  0-based index shorts of the currently selected items in this list box</returns>
        public object GetSelectedItems() => SolidworksObject?.GetSelectedItems();

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
        public bool? SetSelectedItem(short Item, bool Selected) => SolidworksObject?.SetSelectedItem(Item, Selected);

        /// <summary>
        /// Gets and sets the item that is currently selected in this list box. 
        /// </summary>
        /// <value>Index number of the item in the 0-based list</value>
        /// <remarks>If you use this property with a list box enabled for multiple selections, then this method returns -1 and does not affect the list box.</remarks>
        #endregion
        
        #region public properties

        public short? CurrentSelection
        {
            get => SolidworksObject?.CurrentSelection;
            set
            {
                if (SolidworksObject != null)
                    SolidworksObject.CurrentSelection = value.GetValueOrDefault();
            }
        }

        /// <summary>
        /// get or set style as defined in <see cref="ListboxStyles"/>
        /// </summary>
        /// <remarks>By default, only one list item can be selected at a time. When another list item is selected, that item becomes the active item and the previously selected item is cleared. </remarks>
        public int Style
        {
            get => _style;
            set
            {
                _style = value;
                if (SolidworksObject != null)
                    SolidworksObject.Style = value;
            }
        }

        /// <summary>
        /// Gets the number of items in the attached drop-down list for this list box. 
        /// </summary>
        public int? ItemCount => SolidworksObject?.ItemCount;

        /// <summary>
        /// solidworks object
        /// </summary>
        public PropertyManagerPageListbox SolidworksObject { get; private set; }

        #endregion

        #region events
        /// <summary>
        /// Called when the right-mouse button is released in a list box on this PropertyManager page.<br/>
        /// </summary>
        public event Listbox_EventHandler_OnRMB OnRightMouseBtnUp;

        /// <summary>
        /// Called when a user changes the selected item in a list box or selection list box on this PropertyManager page. <br/>
        /// solidowrks will pass in the id of item
        /// </summary>
        public event Listbox_EventHandler_SelectionChanged OnSelectionChange;

        /// <summary>
        /// will be fired a moment before this Lisbox is displayed in a property manager page. 
        /// </summary>
        public new event Listbox_EventHandler_Display OnDisplay;
        #endregion
    }
}

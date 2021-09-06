using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Addins
{

    public class ComboBox_OnDisplay_EventArgs : EventArgs
    {
        #region fields
        private PropertyManagerPageCombobox _solidwroksObj;
        private List<string> _items;
        private ComboBoxStyles _style;
        private short _currentSelection;
        #endregion
    
        #region constructor
        public ComboBox_OnDisplay_EventArgs(PmpComboBox combobBox, List<string> items, ComboBoxStyles style)
        {
            this._solidwroksObj = combobBox.SolidworksObject;
            this._items = items;
            this.Style = style;
        }
        #endregion

        #region mehods
        /// <summary>
        /// indicates if an item is in the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(string item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Adds items to the attached drop-down list for this combo box. 
        /// </summary>
        public void AddItems(List<string> items)
        {
            //update the backing field
            _items.AddRange(items);
            _solidwroksObj.AddItems(items.ToArray());
        }
        /// <summary>
        /// Clears all items from the attached drop-down list for this combo box.  
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            _solidwroksObj.Clear();
        }
        /// <summary>
        /// deletes an item from the attached drop-down list for this combo box
        /// </summary>
        /// <param name="index"></param>
        public void Delete(short index)
        {
            //update the backing field
            _items.RemoveAt(index);
            _solidwroksObj.DeleteItem(index);
        }
        /// <summary>
        /// Gets the text from the attached drop-down list for this combo box. 
        /// </summary>
        /// <param name="index">Position of the item where to get the text in the 0-based list of items or -1 to get the text of the currently selected item</param>
        /// <returns>text of the specified item</returns>
        public string GetItem(short index)
        {
            return _solidwroksObj.ItemText[index];
        }
        /// <summary>
        /// Inserts an item in the attached drop-down list of this combo box. 
        /// </summary>
        /// <param name="index">Position where to add the item in the 0-based list or -1 to put the item at the end of the list</param>
        /// <param name="item">item to add</param>
        public void InsertItem(short index, string item)
        {
            //update the backing field
            _items.Insert(index, item);
            _solidwroksObj.InsertItem(index, item);
        }
        #endregion

        #region properties

        /// <summary>
        ///  Gets or sets the text in the combo box. 
        /// </summary>
        public string EditText
        {
            get
            {
                return _solidwroksObj.EditText;
            }
            set
            {
                //unless style is editable no effect will take place
                Style = ComboBoxStyles.EditableText;
                _solidwroksObj.EditText = value;
            }
        }

        /// <summary>
        /// gets or sets the style for the attached drop down list for this combobox
        /// </summary>
        /// <remarks>Style is a combination of Boolean values, each represented by a bit in this long value. The different Boolean values are represented in the swPropMgrPageComboBoxStyle_e enumeration. <br/>
        /// For example, to set the attached drop-down list of a combo box so that the items are sorted, set Style to <see cref="ComboBoxStyles.Sorted"/>.
        ///<para>The control style must be set before the PropertyManager page is displayed except if setting Style <see cref="ComboBoxStyles.EditBoxReadOnly"/>. You can set style either before or after the PropertyManager page is displayed.
        ///</para>
        ///</remarks>
        ///<value>
        ///Combo-box style as defined in <see cref="ComboBoxStyles"/></value>
        public ComboBoxStyles Style
        {
            get => _style;

            set
            {
                //assign value to the backign field
                _style = value;
                _solidwroksObj.Style = (int)value;
            }
        }

        /// <summary>
        /// Gets and sets the item that is currently selected for this combo box. 
        /// </summary>
        /// <remarks>0-based index</remarks>
        public short CurrentSelection
        {
            get => _currentSelection;
            set
            {
                //assign value to the backign field
                _currentSelection = value;
                _solidwroksObj.CurrentSelection = value;
            }
        }
        #endregion
    }
}

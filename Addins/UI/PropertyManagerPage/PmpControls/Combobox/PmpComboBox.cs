﻿using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a combo box with multiple text values
    /// </summary>
    public class PmpComboBox : PmpControl
    {
        #region private fields

        private List<string> _items;
        private ComboBoxStyles _style;
        private short _height;
        private short _currentSelection;

        #endregion

        #region construtor

        /// <summary>
        /// create combo box for property manager page
        /// </summary>
        /// <param name="items">list of items in the combox box</param>
        /// <param name="style">style of the combo box as defined by <see cref="ComboBoxStyles"/></param>
        /// <param name="height">height of the combo box</param>
        public PmpComboBox(List<string> items, ComboBoxStyles style, short height = 50) : base(swPropertyManagerPageControlType_e.swControlType_Combobox,"","")
        {
            Registering += PmpComboBox_OnRegister;
            _style = style;
            _height = height;
            _items = new List<string>();
            AddItems(items);
            Style = _style;
            Height = _height;
            OnDisplay += PmpComboBox_OnDisplay;
        }

        private void PmpComboBox_OnRegister()
        {
            SolidworksObject = (PropertyManagerPageCombobox)Control;
            throw new NotImplementedException();
        }

        private void PmpComboBox_OnDisplay(PmpControl sender, OnDisplay_EventArgs eventArgs)
        {
            SolidworksObject.Clear();
            _items.Sort();
            SolidworksObject.AddItems(_items.ToArray());
        }
        #endregion

        #region mehods

        /// <summary>
        /// Adds items to the attached drop-down list for this combo box. 
        /// </summary>
        public void AddItems(IEnumerable<string> items)
        {
            //update the backing field
            _items.AddRange(items);
            _items.Sort();
            //if add in is loaded update the solidworks object
            //otherwise update the property after display
            if (SolidworksObject != null)
            {
                SolidworksObject.Clear();
                SolidworksObject.AddItems(_items.ToArray());
            }
            else
                Registering += () =>
                {
                    SolidworksObject.Clear();
                    SolidworksObject.AddItems(_items.ToArray());
                };
        }

        /// <summary>
        /// Gets the text from the attached drop-down list for this combo box. 
        /// </summary>
        /// <param name="index">Position of the item where to get the text in the 0-based list of items or -1 to get the text of the currently selected item</param>
        /// <returns>text of the specified item</returns>
        public string GetItem(short index)
        {
            if (SolidworksObject != null)
                return SolidworksObject.ItemText[index];
            return "";
        }

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
        /// Clears all items from the attached drop-down list for this combo box.  
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            if (SolidworksObject != null)
                SolidworksObject.Clear();
            else
                OnDisplay += (s, e) => SolidworksObject?.Clear();
        }

        /// <summary>
        /// deletes an item from the attached drop-down list for this combo box
        /// </summary>
        /// <param name="index"></param>
        public void Delete(short index)
        {
            //update the backing field
            _items.RemoveAt(index);

            //if add in is loaded update the solidworks object
            //otherwise update the property after display
            if (SolidworksObject != null)
                SolidworksObject.DeleteItem(index);
            else
                Registering += () => { SolidworksObject.DeleteItem(index); };
        }
        /// <summary>
        /// Inserts an item in the attached drop-down list of this combo box. 
        /// </summary>
        /// <param name="item">item to add</param>
        public void AddItem(string item)
        {
            //update the backing field
            if (_items.Contains(item))
                return;
            _items.Add(item);
            _items.Sort();
            //if add in is loaded update the solidworks object
            //otherwise update the property after display
            if (SolidworksObject != null)
            {
                SolidworksObject.Clear();
                SolidworksObject.AddItems(_items.ToArray());
            }
            else
            {

                Registering += () =>
                {
                    SolidworksObject.Clear();
                    SolidworksObject.AddItems(_items.ToArray());
                };
            }
        }


        #endregion

        #region public properties
        /// <summary>
        /// solidworks object
        /// </summary>
        public PropertyManagerPageCombobox SolidworksObject { get; private set; }
        /// <summary>
        /// items in the combobox the index of these items is not the same as solidworks Ui and is not reliable
        /// </summary>
        public ReadOnlyCollection<string> Items => _items.AsReadOnly();

        /// <summary>
        /// height of the combo box
        /// </summary>
        public short Height
        {
            get => _height;
            set
            {
                //assign value to the backing field
                _height = value;

                //if add in is loaded update the solidworks object
                //otherwise update the property after display
                if (SolidworksObject != null)
                    SolidworksObject.Height = value;
                else
                    Registering += () => { SolidworksObject.Height = value; };
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

                //if add in is loaded update the solidworks object
                //otherwise update the property after display
                if (SolidworksObject != null)
                    SolidworksObject.Style = (int)value;
                else
                    Registering += () => { SolidworksObject.Style = (int)value; };

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

                //if add in is loaded update the solidworks object
                //otherwise update the property after display
                if (SolidworksObject != null)
                    SolidworksObject.CurrentSelection = value;
                else
                    Registering += () => { SolidworksObject.CurrentSelection = value; };

            }
        }

        /// <summary>
        ///  Gets or sets the text in the combo box. 
        /// </summary>
        public string EditText
        {
            get
            {
                if (SolidworksObject != null)
                    return SolidworksObject.EditText;
                return "";
            }
            set
            {
                //unless style is editable no effect will take place
                Style = ComboBoxStyles.EditableText;

                if (SolidworksObject != null)
                    SolidworksObject.EditText = value;
                else
                    //if this property is assigned prior to registration 
                    Registering += () => { SolidworksObject.EditText = value; };
            }
        }
        #endregion

        #region call backs
        internal void PmpComboBoxSelectionChangedCallBack(int id)
        {
            PmpComboBoxSelectionChangedEventHandler?.Invoke(this, id);
        }

        internal void PmpComboBoxEditChangedCallBack(string val)
        {
            PmpComboBoxEditChangedEventHandler?.Invoke(this, val);
        }

        #endregion

        #region events

        /// <summary>
        /// Called when a user changes the selected item in a combo box on this PropertyManager page. 
        /// </summary>
        /// <remarks>solidworks will passs int the id of the selected item</remarks>
        public event EventHandler<int> PmpComboBoxSelectionChangedEventHandler;

        /// <summary>
        /// Called when a user changes the text string in the text box of a combo box on this PropertyManager page. solidworsk will pass in the text string
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only called if the combo box was set up as an editable text box. If the combo box is set up to as a static text box, then this method is not called.
        ///<para>
        /// If the user can edit the text in the text box, then use this method with <see cref="PmpComboBoxSelectionChangedEventHandler"/> to find out what is in the text box of the combo box.
        ///</para>
        ///<para>
        ///When this method is called, the control may not yet be updated with the current selection, so the <see cref="CurrentSelection"/> property is not reliable. The text passed into this method is the up-to-date text.
        ///</para>
        /// </para></remarks>
        public event EventHandler<string> PmpComboBoxEditChangedEventHandler;
        #endregion
    }
}

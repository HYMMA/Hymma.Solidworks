using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a combo box with multiple text values
    /// </summary>
    public class PmpComboBox : PmpControl<PropertyManagerPageCombobox>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PmpComboBox() : base(swPropertyManagerPageControlType_e.swControlType_Combobox)
        {

        }

        /// <summary>
        /// items to display
        /// </summary>
        public string[] Items { get; set; }

        /// <summary>
        /// height of the combo box
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// gets or sets the style for the attached drop down list for this combobox
        /// </summary>
        /// <remarks>Style is a combination of Boolean values, each represented by a bit in this long value. The different Boolean values are represented in the swPropMgrPageComboBoxStyle_e enumeration. <br/>
        /// For example, to set the attached drop-down list of a combo box so that the items are sorted, set Style to swPropMgrPageComboBoxStyle_Sorted.
        ///<para>The control style must be set before the PropertyManager page is displayed except if setting Style to swPropMgrPageComboBoxStyle_EditBoxReadOnly. You can set swPropMgrPageComboBoxStyle_EditBoxReadOnly either before or after the PropertyManager page is displayed.
        ///</para>
        ///<para>
        ///Combo-box style as defined in swPropMgrPageComboBoxStyle_e</para>
        ///</remarks>
        public ComboBoxStyles Style { get; set; }

        ///<inheritdoc/>
        public override void Register(IPropertyManagerPageGroup group)
        {
            base.Register(group);
            SolidworksObject.AddItems(Items);
            SolidworksObject.Height = Height;
            SolidworksObject.Style = (int)Style;
        }

        /// <summary>
        /// Gets and sets the item that is currently selected for this combo box. 
        /// </summary>
        /// <remarks>0-based index</remarks>
        public short CurrentSelection { get => SolidworksObject.CurrentSelection; set => SolidworksObject.CurrentSelection = value; }
        
        /// <summary>
        ///  Gets or sets the text in the combo box. 
        /// </summary>
        public string EditText { get => SolidworksObject.EditText; set { Style = ComboBoxStyles.EditableText; SolidworksObject.EditText = value; } }

        /// <summary>
        /// Called when a user changes the selected item in a combo box on this PropertyManager page. 
        /// </summary>
        /// <remarks>solidworks will passs int the id of the selected item</remarks>
        public Action<int> OnSelectionChanged { get; set; }

        /// <summary>
        /// Called when a user changes the text string in the text box of a combo box on this PropertyManager page. solidworsk will pass in the text string
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only called if the combo box was set up as an editable text box. If the combo box is set up to as a static text box, then this method is not called.
        ///<para>
        /// If the user can edit the text in the text box, then use this method with <see cref="OnSelectionChanged"/> to find out what is in the text box of the combo box.
        ///</para>
        ///<para>
        ///When this method is called, the control may not yet be updated with the current selection, so the <see cref="CurrentSelection"/> property is not reliable. The text passed into this method is the up-to-date text.
        ///</para>
        /// </para></remarks>
        public Action<string> OnSelectionEdit { get; set; }
    }

    /// <summary>
    /// styles that can be used on a combobox
    /// </summary>
    public enum ComboBoxStyles
    {
        /// <summary>
        /// Sort the items in the attached drop-down list of the combo box in alphabetical order
        /// </summary>
        Sorted = 1,
        
        /// <summary>
        /// Allows editing of the text in the combo box
        /// </summary>
        EditableText = 2,
        
        /// <summary>
        /// User can only select a value from the attached drop-down list for the combo box
        /// </summary>
        /// <remarks>You can set EditBoxReadOnly either before or after the PropertyManager page is displayed. If set after the PropertyManager page is displayed and the box contains editable text, then that text cannot be edited by the user.<br/>
        /// However, you can use <see cref="PmpComboBox.EditText"/> to edit the text in the combo box.</remarks>
        EditBoxReadOnly = 4,
        
        /// <summary>
        /// The item the user selects in the attached drop-down list does not appear in the combo box. Instead, the user's selection causes the add-in to get a callback via <see cref="PmpComboBox.OnSelectionChanged"/>
        /// </summary>
        AvoidSelectionText = 8
    }
}

namespace Hymma.Solidworks.Addins
{
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

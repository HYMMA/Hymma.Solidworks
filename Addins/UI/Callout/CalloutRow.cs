namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a row in a <see cref="SwCallout"/>
    /// </summary>
    public class CalloutRow
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="swCallout"> the container of the row</param>
        /// <param name="value"> the value of the row</param>
        /// <param name="label"> assign a label for this row</param>
        public CalloutRow(string value, string label="")
        {
            Value = value;
            Label = label;
            ValueInactive = false;
        }
        /// <summary>
        /// Gets or sets the value in for the specified row in this callout.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// /// <summary>
        /// Gets or sets the text for the label in the specified row of this callout1
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// id of this row in the callout
        /// </summary>
        public int RowId { get;internal set; }

        /// <summary>
        /// Gets or sets the color of the text in the specified row in this callout.
        /// </summary>
        public int TextColor { get; set; }

        /// <summary>
        /// Gets or sets whether the user can edit the value in the specified row in this callout. 
        /// </summary>
        public bool ValueInactive { get; set; }
    }
}

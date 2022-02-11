using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    ///PropertyManager page number box styles.Bitmask.
    /// </summary>
    /// <remarks>When the user selects an item in the attached drop-down list, SOLIDWORKS attempts to use that item as a value in the number box.<br/>
    /// Thus, the items in the attached drop-down list should be numeric values and optionally include their units. <br/>
    /// The add-in then gets a callback via <see cref="PmpNumberBox.Change"/> as if the user had typed a value in the number box or clicked the up-arrow or down-arrow buttons to increment or decrement the value.<br/>
    ///If you do not want your add-in to directly use items in the attached drop-down list in the number box, but instead want it to react to the user selecting a computed or linked value in the number box,<br/>
    ///then use <see cref="AvoidSelectionText"/></remarks>
    [Flags]
    public enum NumberBoxStyles
    {
        /// <summary>
        /// the number box appears with two scrow arrows on the right side
        /// </summary>
        Default=0,

        /// <summary>
        /// The item the user selects in the attached drop-down list does not appear in the number box. Instead, the user's selection causes the add-in to get a callback via <see cref="PmpNumberBox.SelectionChanged"/> <br/>
        /// the Id argument will be the number box; the add-in is expected to respond by setting the value for the number box using <see cref="PmpNumberBox.Value"/>.  
        /// </summary>
        AvoidSelectionText = 4,

        /// <summary>
        /// Enables the attached drop-down list for the number box; user can type a value or select a value from the attached drop-down list for the number box
        /// </summary>
        ComboEditBox = 1,

        /// <summary>
        /// User can only select a value from the attached drop-down list for the number box
        /// </summary>
        /// <remarks>You can set EditBoxReadOnly either before or after the PropertyManager page is displayed. <br/>
        /// If set after the PropertyManager page is displayed and the number box contains editable text, then that text cannot be edited by the user</remarks>
        EditBoxReadOnly = 2,

        /// <summary>
        /// Do not show the up and down arrows in the number box, thus, disallowing incrementing or decrementing the value in the number box
        /// </summary>
        NoScrollArrows = 8,

        /// <summary>
        /// Slider
        /// </summary>
        Slider = 16,

        /// <summary>
        /// Suppress sending multiple notifications when a user is dragging or spinning the slider of a number box on a PropertyManager page; instead, send only one notification; see IPropertyManagerPage2Handler9::OnNumberboxChanged for details
        /// </summary>
        SuppressNotifyWhileTracking = 64,

        /// <summary>
        /// Thumbwheel
        /// </summary>
        Thumbwheel = 32
    }
}

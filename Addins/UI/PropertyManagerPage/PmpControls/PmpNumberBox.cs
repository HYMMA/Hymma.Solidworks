using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    public class PmpNumberBox : PmpControl<IPropertyManagerPageNumberbox>
    {
        public PmpNumberBox(double initialValue = 0) : base(swPmpControlsWithText.Numberbox)
        {
            this.InitialValue = initialValue;
        }

        /// <summary>
        /// style for this numberBox as defined by <see cref="NumberBoxStyle"/>
        /// </summary>
        public int Style { get => SolidworksObject.Style; set => SolidworksObject.Style = value; }

        /// <summary>
        /// initial value of the number box when loaded firt time
        /// </summary>
        public double InitialValue { get; set; }

        /// <summary>
        /// unit or type of number as defined by <see cref="swNumberboxUnitType_e"/> input
        /// </summary>
        public swNumberboxUnitType_e Unit { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        /// <summary>
        /// whether the max should be inclusive in the range or not
        /// </summary>
        public bool Inclusive { get; set; } = true;
        public double Increment { get; set; }




        /// <summary>
        /// Sets the range and increment for the slider. 
        /// </summary>
        /// <param name="Units">Number box units as defined in <see cref="NumberBoxUnit"/> (see Remarks)</param>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        /// <param name="Increment"></param>
        /// <param name="fastIncrement">Fast increment value for scrolling and mouse-wheel</param>
        /// <param name="slowIncrement">Slow increment value for scrolling and mouse-wheel</param>
        /// <param name="Inclusive">whether the max should be inclusive in the range or not</param>
        /// <remarks>This method works while a PropertyManager page is displayed with these restrictions: <br/>
        /// You cannot change Units once the page is displayed.The Units parameter is ignored if specified while the page is displayed. <br/>
        ///If the range is changed to an invalid value by this method, then you must immediately call <see cref="Value"/> and set a valid value to prevent displaying the dialog that requests the user to enter a valid value.
        ///</remarks>
        void SetRange(NumberBoxStyle Units, double Minimum, double Maximum, double Increment, double fastIncrement, double slowIncrement, bool Inclusive = true)
        {
            SolidworksObject.SetRange2((int)Units, Minimum, Maximum, Inclusive, Increment, fastIncrement, slowIncrement);
        }
        [DispId(196612)]
        void AddItems(object Texts);
        [DispId(196613)]
        void IAddItems(short TextCount, ref string Texts);
        [DispId(196614)]
        void Clear();
        [DispId(196617)]
        string get_ItemText(short Item);
        [DispId(196618)]
        short InsertItem(short Item, string Text);
        [DispId(196619)]
        short DeleteItem(short Item);
        [DispId(196621)]
        void SetRange2(int Units, double Minimum, double Maximum, bool Inclusive, double Increment, double FastIncr, double SlowIncr);
        [DispId(196622)]
        void SetSliderParameters(int PositionCount, int DivisionCount);

        [DispId(196609)]
        double Value { get; set; }
        [DispId(196611)]
        int Style { get; set; }
        [DispId(196615)]
        short CurrentSelection { get; set; }
        [DispId(196616)]
        short Height { get; set; }
        [DispId(196620)]
        string Text { get; }
        [DispId(196623)]
        int DisplayedUnit { get; set; }











    }

    /// <summary>
    /// units for numberbox
    /// </summary>
    public enum NumberBoxUnit
    {
        /// <summary>
        /// angle
        /// </summary>
        Angle = 4,

        /// <summary>
        /// density
        /// </summary>
        Density = 5,

        /// <summary>
        /// force
        /// </summary>
        Force = 7,
        /// <summary>
        /// frequency
        /// </summary>
        Frequency = 10,

        /// <summary>
        /// gravity
        /// </summary>
        Gravity = 8,

        /// <summary>
        /// length
        /// </summary>
        Length = 3,

        /// <summary>
        /// percent
        /// </summary>
        Percent = 11,

        /// <summary>
        /// stress
        /// </summary>
        Stress = 6,

        /// <summary>
        /// time
        /// </summary>
        Time = 9,

        /// <summary>
        /// unitless doulbe
        /// </summary>
        UnitlessDouble = 2,

        /// <summary>
        /// unitles integer
        /// </summary>
        UnitlessInteger = 1
    }

    /// <summary>
    ///PropertyManager page number box styles.Bitmask.
    /// </summary>
    /// <remarks>When the user selects an item in the attached drop-down list, SOLIDWORKS attempts to use that item as a value in the number box.<br/>
    /// Thus, the items in the attached drop-down list should be numeric values and optionally include their units. <br/>
    /// The add-in then gets a callback via IPropertyManagerPage2Handler9::OnNumberboxChanged as if the user had typed a value in the number box or clicked the up-arrow or down-arrow buttons to increment or decrement the value.<br/>
    ///If you do not want your add-in to directly use items in the attached drop-down list in the number box, but instead want it to react to the user selecting a computed or linked value in the number box,<br/>
    ///then use <see cref="AvoidSelectionText"/></remarks>
    [Flags]
    public enum NumberBoxStyle
    {
        /// <summary>
        /// The item the user selects in the attached drop-down list does not appear in the number box. Instead, the user's selection causes the add-in to get a callback via <see cref="PmpComboBox.OnSelectionChanged"/> <br/>
        /// the Id argument will be the number box; the add-in is expected to respond by setting the value for the number box using IPropertyManagerPageNumberbox::Value.  
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

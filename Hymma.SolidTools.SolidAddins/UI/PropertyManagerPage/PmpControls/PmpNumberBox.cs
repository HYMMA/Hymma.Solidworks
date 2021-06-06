using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    public class PmpNumberBox : PmpTextControl
    {
        public PmpNumberBox() : base(swPmpControlsWithText.Numberbox)
        {

        }
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
    }
}

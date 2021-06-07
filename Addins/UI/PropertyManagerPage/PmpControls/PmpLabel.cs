namespace Hymma.SolidTools.Addins
{
    public class PmpLabel : PmpTextControl
    {
        public PmpLabel():base(swPmpControlsWithText.Label)
        {

        }

        /// <summary>
        /// Gets or sets RGB color of the text of a label on a PropertyManager page
        /// </summary>
        public int TextColor { get; set; }
    }
}

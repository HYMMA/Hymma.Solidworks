namespace Hymma.SolidTools.SolidAddins
{
    public class SwLabel : SwPMPTextControl
    {
        public SwLabel():base(swPmpControlsWithText.Label)
        {

        }

        /// <summary>
        /// Gets or sets RGB color of the text of a label on a PropertyManager page
        /// </summary>
        public int TextColor { get; set; }
    }
}

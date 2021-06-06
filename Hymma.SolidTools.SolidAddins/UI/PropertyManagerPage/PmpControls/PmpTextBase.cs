using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// solidworks property manager page controls that contain text
    /// </summary>
    public enum swPmpControlsWithText
    {
        /// <summary>
        /// a solidworks property manager page label equivalent to <see cref="swPropertyManagerPageControlType_e.swControlType_Label"/>
        /// </summary>
        Label = swPropertyManagerPageControlType_e.swControlType_Label,

        /// <summary>
        /// a solidworks property maanger page text box equivalent to <see cref="swPropertyManagerPageControlType_e.swControlType_Textbox"/>
        /// </summary>
        Textbox = swPropertyManagerPageControlType_e.swControlType_Textbox,

        /// <summary>
        /// a solidworks property manager page number box equivalent to <see cref="swPropertyManagerPageControlType_e.swControlType_Numberbox"/>
        /// </summary>
        Numberbox = swPropertyManagerPageControlType_e.swControlType_Numberbox
    }

    /// <summary>
    /// a class that represents property manager page controls that contain texts
    /// </summary>
    public abstract class PmpTextControl : PmpControl
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PmpTextControl(swPmpControlsWithText controlsWithText) : base((swPropertyManagerPageControlType_e)controlsWithText)
        {

        }
        /// <summary>
        /// Gets or sets the background RGB color of an edit box or label on the PropertyManager page. 
        /// </summary>
        public virtual int BackgroundColor { get; set; }
    }
}

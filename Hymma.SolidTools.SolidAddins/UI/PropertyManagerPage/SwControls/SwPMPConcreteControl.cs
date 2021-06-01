using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// SOLIDWORKS will call this once the controller is clicked on
    /// </summary>
    /// <param name="status">checked or un-checked status</param>
    public delegate void OnClicked(bool status);

    /// <summary>
    /// a SOLIDWORKS property manager page control
    /// </summary>
    public class SwPMPConcreteControl : SwPMPControl
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type"></param>
        public SwPMPConcreteControl(swPropertyManagerPageControlType_e type) : base(type)
        {

        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string BitmapBubble { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override string Caption { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override string Tip { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override short Id { get;internal set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override string BubbleMessage { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override string ColorBitmap { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override string MaskBitmap { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override short Left { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override short Width { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override short Top { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public override int OptionsForResize { get; set; }
    }
}

using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Addins
{
    
    /// <summary>
    /// a SOLIDWORKS property manager page control
    /// </summary>
    public class PmpControl : IPmpControl
    {
        #region private fields

        private swPropertyManagerPageControlType_e _type;
        #endregion
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type">type of this controller as per <see cref="swPropertyManagerPageControlType_e"/></param>
        public PmpControl(swPropertyManagerPageControlType_e type)
        {
            this._type = type;
            LeftIndet = 1;
            Options = 3;
            Enabled = Visible = true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string BitmapBubble { get; set; }

        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        public swPropertyManagerPageControlType_e Type { get { return _type; } set { _type = value; } }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public short LeftIndet { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Options { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string BubbleMessage { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ColorBitmap { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string MaskBitmap { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public short Left { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public short Top { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int OptionsForResize { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// gets or sets the visibility of thei control
        /// </summary>
        public bool Visible { get; set; }
    }
}

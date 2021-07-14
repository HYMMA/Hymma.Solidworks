using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// a SOLIDWORKS property manager page control
    /// </summary>
    public class PmpControl<T>: IPmpControl where T: IPropertyManagerPageControl
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
            if (typeof(T) is IPropertyManagerPageControl pmpControl)
                this.SolidWorksObj = pmpControl;
            
            this._type = type;
            LeftIndet = 1;
            Options = 3;
            Enabled = Visible = true;
        }

        /// <inheritdoc/>
        public string BitmapBubble { get; set; }

        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        public swPropertyManagerPageControlType_e Type { get { return _type; } set { _type = value; } }

        /// <inheritdoc/>
        public short LeftIndet { get; set; }

        /// <inheritdoc/>
        public int Options { get; set; }

        /// <inheritdoc/>
        public string Caption { get; set; }

        /// <inheritdoc/>
        public string Tip { get; set; }

        /// <inheritdoc/>
        public short Id { get; set; }

        /// <inheritdoc/>
        public string BubbleMessage { get; set; }

        /// <inheritdoc/>
        public string ColorBitmap { get; set; }

        /// <inheritdoc/>
        public string MaskBitmap { get; set; }

        /// <inheritdoc/>
        public short Left { get; set; }

        /// <inheritdoc/>
        public short Width { get; set; }

        /// <inheritdoc/>
        public short Top { get; set; }

        /// <inheritdoc/>
        public int OptionsForResize { get; set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; }

        /// <summary>
        /// gets or sets the visibility of thei control
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// gets solidworks base object 
        /// </summary>
        public IPropertyManagerPageControl SolidWorksObj { get; }
    }
}

using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a slider in property manager page
    /// </summary>
    public class PmpSlider : PmpControl<IPropertyManagerPageSlider>
    {
        #region fields

        private SliderStyles _style;
        private short _height;
        #endregion

        #region constructors

        /// <summary>
        /// make a slider in the property manager page control
        /// </summary>
        /// <param name="styles">defines type of slider</param>
        /// <param name="tip"></param>
        public PmpSlider(SliderStyles styles, string tip = "") : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_Slider, "", tip)
        {
            Style = styles;
        }
        #endregion

        #region properties

        /// <summary>
        /// bitwise style as defined by <see cref="SliderStyles"/>
        /// </summary>
        public SliderStyles Style
        {
            get => _style;
            set
            {
                _style = value;
                if (SolidworksObject != null)
                    SolidworksObject.Style = ((int)value);
                else
                    OnRegister += () =>
                    {
                        SolidworksObject.Style = ((int)value);
                    };
            }
        }

        /// <summary>
        /// Height of slider control
        /// </summary>
        /// <remarks>Normally you should not use this property because SOLIDWORKS will size the slider appropriately based on its orientation and display properties. However, the complexity of the PropertyManager page's layout may make it necessary to use this property with IPropertyManagerPageControl::Left, IPropertyManagerPageControl::Top, and IPropertyManagerPageControl::Width to get the desired look.<br/>
        /// this property is also for creating a small slider by clearing the IPropertyManagerPageSlider::Style swPropMgrPageSliderStyle_AutoTicks bit and setting Height to a value less than the default. A horizontal slider without tick marks has a default height of 16. This height is in dialog-box units. You can convert this value to screen units (pixels) by using the Windows MapDialogRect function.</remarks>
        public short Height
        {
            get => _height;

            set
            {
                _height = value;
                OnRegister += () => { SolidworksObject.Height = value; };
            }
        }

        #endregion

        #region methods

        #endregion

        #region call backs
        internal void PositionChanged(double value)
        {
            OnPositionChange?.Invoke(this, value);
        }

        internal void TrackingComplete(double value)
        {
            OnPositionChangeFinished?.Invoke(this, value);
        }

        #endregion

        #region events
        /// <summary>
        /// fires while the user changing the position of the slider
        /// </summary>
        public event EventHandler<double> OnPositionChange;

        /// <summary>
        /// fires after the user has finished changing the position of the slider
        /// </summary>
        public event EventHandler<double> OnPositionChangeFinished;

        #endregion
    }
}

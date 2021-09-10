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
        private int _lineSize;
        private double _position;
        private int _rangeMin;
        private int _rangeMax;
        private int _pageSize;
        private int _tickMark;

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
            _pageSize = 2;
            _lineSize = 1;
            _position = 5;
            _rangeMin = 0;
            _rangeMax = 10;
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

        /// <summary>
        /// Gets or sets how much the slider moves when the arrow keys are used to move the slider.
        /// </summary>
        /// <value>How much the slider moves when arrow keys are used to move the slider</value>
        /// <remarks>default is 1</remarks>
        public int LineSize
        {
            get => _lineSize;
            set
            {
                _lineSize = value;
                if (SolidworksObject != null)
                    SolidworksObject.LineSize = value;
                else
                    OnRegister += () =>
                    {
                        SolidworksObject.LineSize = value;
                    };
            }
        }

        /// <summary>
        /// Gets or sets how much the slider moves when the Page Up or Page Down keys are used to move the slider.
        /// </summary>
        /// <remarks>If this property is not used to specified how the slider moves when the Page Up or Page Down keys are used to move the slider, the value defaults to 2.</remarks>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                if (SolidworksObject != null)
                    SolidworksObject.PageSize = value;
                else
                    OnRegister += () => { SolidworksObject.PageSize = value; };
            }
        }

        /// <summary>
        /// Gets or sets the current position of the slider.
        /// </summary>
        /// <remarks>If you do not set the initial position of a slider, then the value defaults to a position of 5.</remarks>
        public int Position
        {
            get => ((int)_position);
            set
            {
                if (value < _rangeMin || value > _rangeMax)
                {
#if DEBUG
                    throw new Exception("position of the slider cannot be less than min or max range of slider");
#else
                    return;
#endif
                }
                _position = value;
                if (SolidworksObject != null)
                    SolidworksObject.Position = value;
                else
                    OnRegister += () => { SolidworksObject.Position = value; };

            }
        }
        /// <summary>
        /// Gets or sets the frequency of tick marks on a slider.
        /// </summary>
        public int TickFrequency
        {
            get => _tickMark;
            set
            {
#if DEBUG
                if (!Style.HasFlag(SliderStyles.AutoTicks))
                    throw new Exception("to set the tick mark for a slider the style of the slider must be set to SliderStyles.AutoTicks");
#else
                return;
#endif
                _tickMark = value;
                if (SolidworksObject != null)
                    SolidworksObject.TickFrequency = value;
                else
                    OnRegister += () => { SolidworksObject.TickFrequency = value; };

            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Sets the range of a slider.
        /// </summary>
        /// <param name="min">Minimum range of slider</param>
        /// <param name="max">Maximum range of slider</param>
        /// <returns>True if range is set false if not</returns>
        public bool SetRange(int min, int max)
        {
#if DEBUG
            if (max < min)
                throw new Exception("min value cannot be more than max value");
#endif
            _rangeMin = (int)min; _rangeMax = ((int)max);
            var results = false;
            if (SolidworksObject != null)
                results = SolidworksObject.SetRange(((int)min), ((int)max));
            else
                OnRegister += () => { results = SolidworksObject.SetRange(((int)min), ((int)max)); };
            return results;
        }

        /// <summary>
        /// Gets the range of the slider.
        /// </summary>
        /// <param name="min">Minimum range of slider</param>
        /// <param name="max">Maximum range of slider</param>
        public void GetRange(out int min, out int max)
        {
            min = _rangeMin;
            max = _rangeMax;
        }
        #endregion

        #region call backs
        internal void PositionChanged(double value)
        {
            OnPositionChange?.Invoke(this, value);
            _position = value;
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

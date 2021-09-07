using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page controllers
    /// </summary>
    public abstract class IPmpControl
    {
        #region constructor
        internal IPmpControl(swPropertyManagerPageControlType_e type, string caption, string tip)
        {
            Id = (short)Counter.GetNextPmpId();
            Type = type;
            Caption = caption;
            Tip = tip;
        }
        #endregion

        #region properties
        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// tip for this controller
        /// </summary>
        public string Tip { get; }

        /// <summary>
        /// property manager page control as an object
        /// </summary>
        protected object ControlObject;

        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        public swPropertyManagerPageControlType_e Type { get; internal set; }

        /// <summary>
        /// id of this controller which is used by SOLIDWORKS to identify it
        /// </summary>
        public short Id { get; }

        /// <summary>
        /// Left alignment of this control as defined in <see cref="swPropertyManagerPageControlLeftAlign_e"/>
        /// </summary>
        /// <remarks>this property will be used when the page is displayed or while it is closed</remarks>
        private short LeftAlignment { get; set; } = (short)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value coresponds to a visible and enabled control
        /// </summary>
        private int Options { get; set; } = (int)swAddControlOptions_e.swControlOptions_Enabled | (int)swAddControlOptions_e.swControlOptions_Visible;

        /// <summary>
        /// the solidworks document where the property manager page is displayed in. you can use this proeprty before the property manager page is displayed
        /// </summary>
        public ModelDoc2 ActiveDoc { get; internal set; }
        #endregion

        #region methods

        /// <summary>
        /// Add this control to a group in a property manager page 
        /// </summary>
        /// <param name="group">the group that this contorl should be registerd to</param>
        internal void Register(IPropertyManagerPageGroup group)
        {

            ControlObject = group.AddControl2(Id, (short)Type, Caption, LeftAlignment, Options, Tip);

            //we raise this event here to give multiple controls set-up their initial state. some of the proeprties of a controller has to be set prior a property manager page is displayed or after it's closed
            OnRegister?.Invoke();
        }
        #endregion

        #region call backs
        /// <summary>
        /// will be called just before this property manager page is displayed inside solidworks 
        /// </summary>
        internal abstract void Display();

        internal abstract void GainedFocus();

        internal abstract void LostFocus();
        #endregion

        #region events
        /// <summary>
        /// fired when this controller is registerd in a property manager page which is when the add-in is loaded. Either when solidworks starts or when user re-loads the addin
        /// </summary>
        internal event Action OnRegister;
        #endregion
    }
}
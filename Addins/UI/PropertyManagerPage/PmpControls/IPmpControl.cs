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
        /// <summary>
        /// property manager page control as an object
        /// </summary>
        protected object ControlObject;

        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        internal swPropertyManagerPageControlType_e Type { get; set; }

        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// tip for this controller
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// id of this controller which gets used in command box
        /// </summary>
        internal short Id { get; set; }

        /// <summary>
        /// Left alignment of this control as defined in <see cref="swPropertyManagerPageControlLeftAlign_e"/>
        /// </summary>
        /// <remarks>this property will be used when the page is displayed or while it is closed <br/>
        /// By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        public short LeftAlignment { get; set; } = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value coresponds to a visible and enabled control
        /// </summary>
        public int Options { get; set; } = (int)swAddControlOptions_e.swControlOptions_Enabled | (int)swAddControlOptions_e.swControlOptions_Visible;

        /// <summary>
        /// Add this control to a group in a property manager page 
        /// </summary>
        /// <param name="group">the group that this contorl should be registerd to</param>
        internal void Register(IPropertyManagerPageGroup group)
        {
            Id = (short)PmpConstants.GetNextId();

            ControlObject = group.AddControl2(Id, (short)Type, Caption, LeftAlignment, Options, Tip);

            //we raise this event here to give multiple controls set-up their initial state. some of the proeprties of a controller has to be set prior a property manager page is displayed or after it's closed
            OnRegister();
        }

        /// <summary>
        /// will be called just before this property manager page is displayed inside solidworks 
        /// </summary>
        internal void Display()
        {
            OnDisplay?.Invoke();
        }

        internal void GainedFocus()
        {
            OnGainedFocus?.Invoke();
        }

        internal void LostFocus()
        {
            OnLostFocus?.Invoke();
        }
        /// <summary>
        /// the solidworks document where the property manager page is displayed in. you can use this proeprty before the property manager page is displayed
        /// </summary>
        internal ModelDoc2 ActiveDoc { get; set; }

        /// <summary>
        /// fired when this controller is registerd in a property manager page which is when the add-in is loaded. Either when solidworks starts or when user re-loads the addin
        /// </summary>
        /// <remarks>Almost all of registration tasks are handled by the framework <br/>Use <see cref="OnDisplay"/> to invoke methods right before property manager is displayed</remarks>
        public event Action OnRegister;

        /// <summary>
        /// fired a moment before property manager page is displayed
        /// </summary>
        public event Action OnDisplay;

        /// <summary>
        /// fired when user starts interacting with this control, such as start of typing in a text box
        /// </summary>
        public event Action OnGainedFocus;

        /// <summary>
        /// fires when user browses away from this control
        /// </summary>
        public event Action OnLostFocus;
    }
}
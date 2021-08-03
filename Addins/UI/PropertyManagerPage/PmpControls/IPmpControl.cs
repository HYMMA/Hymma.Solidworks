using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page controllers
    /// </summary>
    public interface IPmpControl 
    {
        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        swPropertyManagerPageControlType_e Type { get; set; }

        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        string Caption { get; set; }

        /// <summary>
        /// tip for this controller
        /// </summary>
        string Tip { get; set; }

        /// <summary>
        /// id of this controller which gets used in command box
        /// </summary>
        short Id { get; set; }

        /// <summary>
        /// Left alignment of this control as defined in swPropertyManagerPageControlLeftAlign_e
        /// </summary>
        /// <remarks>this property will be used when the page is displayed or while it is closed <br/>
        /// By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        short LeftAlignment { get; set; }

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value coresponds to a visible and enabled control
        /// </summary>
        int Options { get; set; }

        /// <summary>
        /// Add this control to a group in a property manager page 
        /// </summary>
        /// <param name="group">the group that this contorl should be registerd to</param>
        void Register(IPropertyManagerPageGroup group);

        /// <summary>
        /// will be called just before this property manager page is displayed inside solidworks 
        /// </summary>
        void Display();
        /// <summary>
        /// the solidworks document where the property manager page is displayed in. you can use this proeprty before the property manager page is displayed
        /// </summary>
        ModelDoc2 ActiveDoc { get; set; }
    }
}
using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// provides useful arguments and parameters for <see cref="PmpControl{T}.OnDisplay"/>
    /// </summary>
    public class OnDisplay_EventArgs : EventArgs
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="control"></param>
        public OnDisplay_EventArgs(IPropertyManagerPageControl control)
        {
            _control = control;
        }

        /// <summary>
        /// Left edge of the control <br/>
        /// Use this proeprty and <see cref="PmpControl{T}.Top"/> to palce controls side by side<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        /// <remarks>By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        public short Left
        {
            get => _left;
            set => _left = _control.Left = value;
        }

        /// <summary>
        /// By default, the width of the control is usually set so that it extends to the right edge of its group box (not for buttons). Using this API overrides that default.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The width of the group box is 100
        /// </summary>
        public short Width
        {
            get => _width;
            set => _width = _control.Width = value;
        }

        private short _width;
        private short _left;
        private IPropertyManagerPageControl _control;
    }

}

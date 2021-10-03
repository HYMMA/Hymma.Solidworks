﻿using SolidWorks.Interop.sldworks;
using System;
using System.Drawing;
using System.IO;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// provides useful arguments and parameters for <see cref="PmpControl{T}.OnDisplay"/>
    /// </summary>
    public class OnDisplay_EventArgs : EventArgs
    {
        #region fields
        private short _width;
        private short _left;
        private IPropertyManagerPageControl _control;
        private int _optionForResize;
        #endregion

        #region constructor

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="control"></param>
        public OnDisplay_EventArgs(IPropertyManagerPageControl control)
        {
            _control = control;
        }
        #endregion

        #region methods

        /// <summary>
        /// Sets the bitmap label for this control that appears next to it on the left hand side.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <remarks>
        /// You can only use this method on a PropertyManager page before the page is displayed, while it is displayed, or when it is closed. <br/>
        /// The image will be resized to 18 x 18
        /// </remarks>
        public virtual void SetPictureLabel(Bitmap bitmap, string fileName)
        {
            if (_control == null) return;
            if (string.IsNullOrEmpty(fileName))
                return;
            var fullFileName = Path.Combine(IconGenerator.GetDefaultIconFolder(), fileName);
            MaskedBitmap.SaveAsPng(new Bitmap(bitmap, 18, 18), ref fullFileName);
            _control.SetPictureLabelByName(fullFileName, "");
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets or sets how to override the SOLIDWORKS default behavior when changing the width of a PropertyManager page. <br/>
        /// Resize the PropertyManager page as defined in <see cref="ControlResizeStyles"/>
        /// you can use ths porperty only before the control is displayed or while it is closed
        /// </summary>
        public int OptionsForResize
        {
            get => _optionForResize;
            set => _optionForResize = _control.OptionsForResize = value;
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
        #endregion
    }
}

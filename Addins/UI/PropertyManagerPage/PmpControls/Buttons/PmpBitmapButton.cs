﻿using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a bitmap button for property manager page
    /// </summary>
    public class PmpBitmapButton : PmpButtonBase<PropertyManagerPageBitmapButton>
    {
        #region private fields

        private Bitmap _bitmap;
        private string _fileName;
        private BtnSize _iconSize;
        private byte _opacity;
        private BitmapButtons _standardIcon;
        #endregion

        #region constructors

        /// <summary>
        /// generate a button with specified <see cref="Bitmap"/>
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="tip">text for this button tooltip</param>
        /// <param name="iconSizes">possible icons sizes for this button</param>
        /// <param name="opacity">define opacity of the bitmap on the button, less values result in more transparent pictures. If the format of the <see cref="Bitmap"/> provided in .png no transparency will be applied on the image</param>
        public PmpBitmapButton(Bitmap bitmap, string tip, BtnSize iconSizes, byte opacity) : base(swPropertyManagerPageControlType_e.swControlType_BitmapButton, "", tip)
        {
            _bitmap = bitmap;
            _fileName = "Btn" + Id;
            _iconSize = iconSizes;
            _opacity = opacity;
            Registering += PmpBitmapButton_OnRegister;
        }

        /// <summary>
        /// generate a button with standard icons
        /// </summary>
        /// <param name="standardIcon"></param>
        /// <param name="tip">text for this button tooltip</param>
        public PmpBitmapButton(BitmapButtons standardIcon, string tip) : base(swPropertyManagerPageControlType_e.swControlType_BitmapButton, "", tip)
        {
            _standardIcon = standardIcon;
            Registering += PmpBitmapButton_OnRegister;
        }
        #endregion

        #region call backs
        private void PmpBitmapButton_OnRegister()
        {
            if (_bitmap != null && _fileName != "")
            {
                SetButtonIcon(_bitmap, _fileName, _iconSize, _opacity);
            }
            else if (_standardIcon != 0)
            {
                SetButtonIcon(_standardIcon);
            }
            SolidworksObject.Checked = false;
        }
        #endregion

        #region methods

        /// <summary>
        /// assign a bitmap to this bitmap button that appears on the button
        /// Images should use: <br/>
        /// 256-color palette.<br/>
        ///margin of at least 3 blank pixels on all sides of the image because this is where the button borders are drawn; i.e., any pixels in the image in these 3 outer rows and columns of pixels are obscured by the button borders.
        /// </summary>
        /// <remarks>The rest of the images (selected, highlight regular, highlight selected, and disabled) for this button are automatically generated by SOLIDWORKS</remarks>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <param name="size">size of the button</param>
        /// <param name="opacity">define opacity of the bitmap on the button, less values result in more transparent pictures</param>
        public void SetButtonIcon(Bitmap bitmap, string fileName, BtnSize size, byte opacity)
        {
            if (SolidworksObject == null)
                return;

            //possible sizes for a button bitmap in solidworks
            var images = new List<string>();
            var masks = new List<string>();
            
            using (bitmap)
            {
                var fullFileName = Path.Combine(SharedIconsDir.CreateSubdirectory(Id.ToString()).FullName, new StringBuilder().Append(fileName).Append(size).Append(".png").ToString());
                MaskedBitmap.SaveAsPng(bitmap,new Size(((int)size),((int)size)), ref fullFileName, false, opacity);
                images.Add(fullFileName);
                masks.Add("");
            }

            SolidworksObject.SetBitmapsByName3(images.ToArray(), masks.ToArray());
        }

        /// <summary>
        /// assign an icon to this bitmap button from a list of standard solidworks icons <br/>
        /// The not-clicked, clicked, and disabled states for the control are automatically set by the SOLIDWORKS application.
        /// </summary>
        /// <param name="standardIcon">standard icon as defined by <see cref="BitmapButtons"/></param>
        public void SetButtonIcon(BitmapButtons standardIcon)
        {
            if (SolidworksObject != null)
                SolidworksObject.SetStandardBitmaps((int)standardIcon);
        }

        #endregion
    }
}

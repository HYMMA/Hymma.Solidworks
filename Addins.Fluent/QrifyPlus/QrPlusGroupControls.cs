// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using QRCoder;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace QrifyPlus
{
    //this class could be inside a different library
    public class QrPlusGroupControls : PmpGroup
    {
        public QrPlusGroupControls()
        {
            GenerateControlsForTheGroup();
        }

        private void GenerateControlsForTheGroup()
        {
            //a label on top of the combo box
            var label = new PmpLabel("List of Custom Properties");

            //an editable combo box that allows users to select name of the propertyNames in this drawing
            var drawingProertyNames = new PmpComboBox(new List<string>() { "Test1", "Test2" }, ComboBoxStyles.EditableText);

            //update the list on each time property manager page is shown
            drawingProertyNames.Displaying += drawingPropertyNames_Displaying;

            //GenerateControlsForTheGroup a button under the list to initiate the QR generations
            var qrifyButton = new PmpBitmapButton(Properties.Resources.qrifyPlus, "Generate QR code for the value of the property", BtnSize.thirtyTwo, opacity: byte.MaxValue);

            //put the combo box and the button on the same line
            //this value is relative to the PmpGroup. the controls with higher top value go to the bottom. because solidworks.
            drawingProertyNames.Top = qrifyButton.Top = 300;
            //Width is the percentage relative to the width of the property manager page itself
            drawingProertyNames.Width = 80;
            //Left is the position of the control from left of the property manger page as a percentage of the width of the property manger page itself
            qrifyButton.Left = 85;

            qrifyButton.Clicked += (s, e) =>
            {
                var btn = s as PmpBitmapButton;

                //get current selection of the combo box
                short index = drawingProertyNames.CurrentSelection;
                string propertyName = drawingProertyNames.GetItem(index);

                //get value of property
                var drawing = btn.ActiveDoc;
                var propertyValue = drawing.GetCustomProperty(propertyName);

                //generate qr code and save it in clipboard
                SaveQrToClipboard(propertyValue);

                btn.ShowBubleTooltip("Success", "Copied into clipboard, use Ctrl+v to paste", Properties.Resources.infoPlus, "successImageFileName");
            };

            //register the controls
            base.AddControls(new IPmpControl[] { label, drawingProertyNames, qrifyButton });
        }

        private void SaveQrToClipboard(string value)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
            var qrImage = ArtQRCodeHelper.GetQRCode(value, 5, System.Drawing.Color.Black, System.Drawing.Color.White, System.Drawing.Color.Gray, QRCodeGenerator.ECCLevel.L);
            using (qrImage)
            {
                var src = Imaging.CreateBitmapSourceFromHBitmap(qrImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Clipboard.SetImage(src);
            }
        }

        //call back
        private void drawingPropertyNames_Displaying(IPmpControl sender, PmpControlDisplayingEventArgs eventArgs)
        {
            //this would be the drawing document, Note that this addin is available in a drawing only
            ModelDoc2 drawing = sender.ActiveDoc;

            //cast sender to the actual type
            var textList = sender as PmpComboBox;

            //clear the content
            textList.Clear();

            //get propertyNames in the document
            string[] propertyNames = (string[])GetAllPropertiesOfDocument(drawing);

            //add propertyNames to the list
            textList.AddItems(propertyNames);
        }

        private object[] GetAllPropertiesOfDocument(ModelDoc2 model)
        {

            /*var customPropertyManager = drawing.Extension.CustomPropertyManager[""];
            var names= customPropertyManager.IGetNames(customPropertyManager.Count); */

            //configuration should be empty for a drawing as it does not support it
            //get custom property names
            var customPropertyNames = (object[])model.GetCustomInfoNames2("");
            return customPropertyNames;
        }

    }
}

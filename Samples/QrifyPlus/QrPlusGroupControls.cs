// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using QRCoder;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
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
            var message = new PmpLabel("Choose a property to convert to QR. You can assign suffix to the generated code using '<' and '>'. For example: <part number is:> PartNo will suffix PartNo value with part number is:");
            message.TextColor = Color.Gray;

            //a label on top of the combo box
            var label = new PmpLabel("List of Custom Properties");

            //an editable combo box that allows users to select name of the propertyNames in this drawing
            var drawingPropertyNames = new PmpComboBox(new List<string>() { "Test1", "Test2" }, ComboBoxStyles.EditableText);

            //update the list on each time property manager page is shown
            drawingPropertyNames.Displaying += drawingPropertyNames_Displaying;

            //GenerateControlsForTheGroup a button under the list to initiate the QR generations
            var qrifyButton = new PmpBitmapButton(Properties.Resources.qrifyPlus, "Generate QR code for the value of the property", BtnSize.sixteen, opacity: byte.MaxValue);

            //put the combo box and the button on the same line
            //this value is relative to the PmpGroup. the controls with higher top value go to the bottom of their group. because solidworks.
            drawingPropertyNames.Top = qrifyButton.Top = 300;

            //Width is the percentage relative to the width of the property manager page itself
            drawingPropertyNames.Width = 80;

            //Left is the position of the control from left of the property manger page as a percentage of the width of the property manger page itself
            qrifyButton.Left = 90;

            qrifyButton.Clicked += (s, e) =>
            {
                var btn = s as PmpBitmapButton;

                //get current selection of the combo box
                short index = drawingPropertyNames.CurrentSelection;
                string dropDownItem = drawingPropertyNames.GetItem(index);

                //filter out suffix if existed 
                //filter out property name
                GetConstructs(dropDownItem, out string suffix, out string property);

                //get value of property
                var drawing = btn.ActiveDoc;
                var propertyValue = drawing.GetCustomProperty(property);

                //if property is empty bypass the QR coder generation
                if (string.IsNullOrWhiteSpace(propertyValue))
                {
                    btn.ShowBubbleTooltip("Error", "The value of this property is empty", null, "");
                    return;
                }

                //generate qr code and save it in clipboard
                SaveQrToClipboard($"{suffix}{propertyValue}");

                btn.ShowBubbleTooltip("Success", $"{suffix}{propertyValue} Copied into clipboard, use Ctrl+v to paste its QR representation", Properties.Resources.infoPlus, "successImageFileName");
            };

            var selBox1 = new PmpSelectionBox(new[] { swSelectType_e.swSelDRAWINGVIEWS });
            var selBox2 = new PmpSelectionBox(new[] { swSelectType_e.swSelDRAWINGVIEWS }, SelectionBoxStyles.UpAndDownButtons | SelectionBoxStyles.MultipleItemSelect)
            {
                PopUpMenuItems = new List<PopUpMenuItem>()
                    {
                       new PopUpMenuItem("popup item","hint for the item",swDocumentTypes_e.swDocDRAWING),
                       new PopUpMenuItem("popup item2","hint for the item2",swDocumentTypes_e.swDocDRAWING),
                    }
            };
            //register the controls
            base.AddControls(new IPmpControl[] { message, label, drawingPropertyNames, qrifyButton, selBox1, selBox2 });
        }

        private void GetConstructs(string text, out string suffix, out string property)
        {
            //(<(?'suffix'[^<>]+)?>)?\s?(?'property'.+)
            var pattern = @"(<                                      # start a group that starts with <
                                (?'suffix'                          #inside this group define a group and call it 'suffix'
                                        [^<>]+)?>                   #group suffix contains characters inside < > , unless they are < or >
                            )?                                      #close parent group 1, by the way this group could be non-existent
                            \s?                                     #allow spaces
                            (?'property'.+)                         #define another group called property that contains one or many of any type of character                         
                            ";
            var match = Regex.Match(text, pattern, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            suffix = match.Groups["suffix"].Value;
            property = match.Groups["property"].Value;
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

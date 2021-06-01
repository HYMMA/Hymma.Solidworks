using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using Hymma.SolidTools.Extensions;
using System.Linq;
using System.Dynamic;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// provides useful extension methods for making controls for a property manager page in SOLIDWORKS         
    /// </summary>
    public static class IPropertyManagerPageExtensions2
    {
        #region private methods
        private static void UpdatePmpControl(IPropertyManagerPageControl t, SwPMPControl control)
        {
            t.SetPictureLabelByName(control.ColorBitmap, control.MaskBitmap);
            t.OptionsForResize = control.OptionsForResize;
            t.Top = control.Top;
            t.Left = control.Left;
            t.Width = control.Width;
            t.Visible = control.Visible;
            t.Enabled = control.Enabled;

        }

        #endregion

        #region extension methods

        /// <summary>
        /// creates a group of type <see cref="SwGroupBox"/>in solidworks proeprty maanger page and adds all controls inside that group 
        /// </summary>
        /// <param name="pmp"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static void AddSwBox(this IPropertyManagerPage2 pmp, SwGroupBox box, List<object> controls)
        {
            if (box is null || box.Controls.Count == 0)
                throw new ArgumentNullException(nameof(box));
            
            //assing solidworks groups
            var group = pmp.AddGroup(box.Id, box.Caption, (short)box.Options);
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            controls.Add( group);
            
            //add controls of each box to solidworks pmp 
            foreach (var control in box.Controls)
            {
                switch (control.Type)
                {
                    //label
                    case swPropertyManagerPageControlType_e.swControlType_Label:
                        //cast into propert ojbect
                        var c = control as SwLabel;

                        //add object to property manager page
                        _ = pmp.AddLabel(group, c);
                        break;

                    //checkboxes
                    case swPropertyManagerPageControlType_e.swControlType_Checkbox:
                        var b = control as SwCheckBox;
                        var id = b.Id;
                        var checkbox = pmp.AddCheckBox(group, b);
                        controls.Add(checkbox);
                        break;

                    //buttons
                    case swPropertyManagerPageControlType_e.swControlType_Button:
                        var d = control as SwButton;
                        _ = pmp.AddButton(group, d);
                        break;

                    //radio buttons
                    case swPropertyManagerPageControlType_e.swControlType_Option:
                        var e = control as SwRadioButton;
                        _ = pmp.AddRadioButton(group, e);
                        break;

                    //textboxes
                    case swPropertyManagerPageControlType_e.swControlType_Textbox:
                        var f = control as SwTextBox;
                        _ = pmp.AddTextBox(group, f);
                        break;

                    //list boxes
                    case swPropertyManagerPageControlType_e.swControlType_Listbox:
                        var g = control as SwListBox;
                        _ = pmp.AddListbox(group, g);
                        break;

                    //combo box
                    case swPropertyManagerPageControlType_e.swControlType_Combobox:
                        var com = control as SwComboBox;
                        _ = pmp.AddCombobox(group, com);
                        break;

                    //number box
                    case swPropertyManagerPageControlType_e.swControlType_Numberbox:
                        var num = control as SwNumberBox;
                        _ = pmp.AddNumberbox(group, num);
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_Selectionbox:
                        var sel = control as SwSelectionBox;
                        _ = pmp.AddSelectionbox(group, sel);
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_ActiveX:
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_BitmapButton:
                        var bbutton = control as SwBitmapButtonCustom;
                        _ = pmp.AddBitmapButton(group, bbutton);
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_CheckableBitmapButton:
                        _ = pmp.AddBitmapButton(group, control as SwBitmapButtonCheckable);
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_Slider:
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_Bitmap:
                        _ = pmp.AddBitmap(group, control as SwBitmap);
                        break;
                    case swPropertyManagerPageControlType_e.swControlType_WindowFromHandle:
                        _ = pmp.AddWindowFromHandle(group, control as SwWindowHandler);
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// adds a checkbox aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageCheckbox"/></returns>
        public static IPropertyManagerPageCheckbox AddCheckBox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwCheckBox checkBox)
        {
            var result = pmp.AddCheckBox(group, checkBox.Id, checkBox.Caption, checkBox.Tip, checkBox.LeftIndet, checkBox.Options);
            //UpdatePmpControl(result as PropertyManagerPageControl, checkBox);
            return result;
        }

        /// <summary>
        /// adds a Text-Box aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageTextbox"/></returns>
        public static IPropertyManagerPageTextbox AddTextBox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwTextBox textBox)
        {
            var result = pmp.AddTextBox(group, textBox.Id, textBox.Caption, textBox.Tip, textBox.LeftIndet, textBox.Options);
            UpdatePmpControl(result as PropertyManagerPageControl, textBox);
            return result;
        }

        /// <summary>
        /// adds a button aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageButton"/></returns>
        public static IPropertyManagerPageButton AddButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwButton button)
        {
            var result = pmp.AddButton(group, button.Id, button.Caption, button.Tip, button.LeftIndet, button.Options);
            UpdatePmpControl(result as PropertyManagerPageControl, button);
            return result;
        }

        /// <summary>
        /// adds a Label aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageLabel"/></returns>
        public static IPropertyManagerPageLabel AddLabel(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwLabel Swlabel)
        {
            //make a label
            var result = pmp.AddLabel(group, Swlabel.Id, Swlabel.Caption, Swlabel.Tip, Swlabel.LeftIndet, Swlabel.Options);

            //set label specific properties
            UpdatePmpControl(result as PropertyManagerPageControl, Swlabel);

            //return label
            return result;
        }

        /// <summary>
        /// adds a radio-button (option) aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageOption"/></returns>
        public static IPropertyManagerPageOption AddRadioButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwRadioButton radioButton)
        {
            var result = pmp.AddRadioButton(group, radioButton.Id, radioButton.Caption, radioButton.Tip, radioButton.LeftIndet, radioButton.Options);
            UpdatePmpControl(result as PropertyManagerPageControl, radioButton);
            return result;
        }

        /// <summary>
        /// adds a Listbox aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageListbox"/></returns>
        public static IPropertyManagerPageListbox AddListbox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwListBox listBox)
        {
            //set up list box
            var result = pmp.AddListbox(group, listBox.Id, listBox.Caption, listBox.Tip, listBox.Items.ToArray(), listBox.Height, listBox.LeftIndet, listBox.Options);
            result.Height = listBox.Height;
            result.AddItems(listBox.Items.ToArray());
            UpdatePmpControl(result as PropertyManagerPageControl, listBox);
            return result;
        }

        /// <summary>
        /// adds a Combobox aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageCombobox"/></returns>
        public static IPropertyManagerPageCombobox AddCombobox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwComboBox comboBox)
        {
            //setup combo box
            var result = pmp.AddCombobox(group, comboBox.Id, comboBox.Caption, comboBox.Tip, comboBox.Height, comboBox.Items.ToArray(), comboBox.LeftIndet, comboBox.Options);

            //setup combo box properties
            UpdatePmpControl(comboBox as PropertyManagerPageControl, comboBox);
            return result;
        }

        /// <summary>
        /// adds a numberbox to the property manager page
        /// </summary>
        /// <param name="group">group to add this controlt to</param>
        /// <param name="numberBox"></param>
        /// <returns></returns>
        public static IPropertyManagerPageNumberbox AddNumberbox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwNumberBox numberBox)
        {
            var result = pmp.AddNumberbox(group, numberBox.Id, numberBox.Caption, numberBox.Tip, numberBox.InitialValue,
                (int)numberBox.Unit, numberBox.Min, numberBox.Max, numberBox.Increment, numberBox.Inclusive, numberBox.LeftIndet, numberBox.Options);
            UpdatePmpControl(result as IPropertyManagerPageControl, numberBox);
            return result;
        }

        /// <summary>
        /// add a selection box to property manager page
        /// </summary>
        /// <param name="group">the group this control gets added to</param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static IPropertyManagerPageSelectionbox AddSelectionbox(this IPropertyManagerPage2 pmp,
            IPropertyManagerPageGroup group,
            SwSelectionBox box)
        {
            var result = pmp.AddSelectionbox(group, box.Id, box.Caption, box.Tip, box.Height, box.Filter.Select(b => (int)b).ToArray(), box.LeftIndet, box.Options);
            UpdatePmpControl(result as IPropertyManagerPageControl, box);
            return result;
        }


        /// <summary>
        /// adds a bitmap of type <see cref="SwBitmapButtonCustom"/> to proerty manager page
        /// </summary>
        /// <returns></returns>
        public static IPropertyManagerPageBitmapButton AddBitmapButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwBitmapButtonCustom button)
        {
            var swButton = pmp.AddBitmapButton(group, button.Id, button.Caption, button.Tip, button.LeftIndet, button.Options);
            UpdatePmpControl(swButton as IPropertyManagerPageControl, button);
            swButton.SetBitmapsByName3(button.ImageList, button.MaskedImageList);
            return swButton;
        }


        /// <summary>
        /// adds a bitmap of type <see cref="SwBitmapButtonCustom"/> to proerty manager page
        /// </summary>
        /// <returns></returns>
        public static IPropertyManagerPageBitmapButton AddBitmapButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwBitmapButtonCheckable button)
        {
            var swButton = pmp.AddBitmapButton(group, button.Id, button.Caption, button.Tip, button.LeftIndet, button.Options);
            UpdatePmpControl(swButton as IPropertyManagerPageControl, button);
            swButton.SetBitmapsByName3(button.ImageList, button.MaskedImageList);
            swButton.Checked = button.Checked;
            swButton.IsCheckable = button.IsCheckable;
            return swButton;
        }

        /// <summary>
        /// adds a bitmap of type <see cref="SwBitmapButtonStandard"/> to proerty manager page
        /// </summary>
        /// <returns></returns>
        public static IPropertyManagerPageBitmapButton AddBitmapButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwBitmapButtonStandard button)
        {
            var swButton = pmp.AddBitmapButton(group, button.Id, button.Caption, button.Tip, button.LeftIndet, button.Options);
            UpdatePmpControl(swButton as IPropertyManagerPageControl, button);
            swButton.SetStandardBitmaps((int)button.Image);
            return swButton;
        }

        /// <summary>
        /// adds a bitmap of type <see cref="SwBitmap"/> to this property manager page
        /// </summary>
        public static IPropertyManagerPageBitmap AddBitmap(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwBitmap bitmap)
        {
            var swBitmap = pmp.AddBitmap(group, bitmap.Id, bitmap.Caption, bitmap.Tip, bitmap.LeftIndet, bitmap.Options);
            UpdatePmpControl(swBitmap as IPropertyManagerPageControl, bitmap);
            swBitmap.SetBitmapByName(bitmap.ColorBitmap, bitmap.MaskBitmap);
            return swBitmap;
        }

        /// <summary>
        /// adds a WindowFromHandle aligned to the left
        /// <br/> use this to add WPF or windows form controls to PMP
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageWindowFromHandle"/></returns>
        public static IPropertyManagerPageWindowFromHandle AddWindowFromHandle(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, SwWindowHandler windowHandler)
        {
            var result = pmp.AddWindowFromHandle(group, windowHandler.Id, windowHandler.Caption, windowHandler.Tip, windowHandler.LeftIndet, windowHandler.Options);
            windowHandler.ProperptyManagerPageHandle = result;
            UpdatePmpControl(result as IPropertyManagerPageControl, windowHandler);
            return result;
        }
        #endregion
    }
}

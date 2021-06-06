using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Extensions
{
    public static class IPropertyManagerPageExtensions
    {
        /// <summary>
        /// creats a group 
        /// </summary>
        /// <param name="id">id of this group, this will get passed to handler for even handling</param>
        /// <param name="options">bitwise options as defined by <see cref="swAddGroupBoxOptions_e"/> default values correspond to a group that is expanded and is set to be visible</param>
        /// <returns><see cref="IPropertyManagerPageGroup "/></returns>
        public static IPropertyManagerPageGroup AddGroup(this IPropertyManagerPage2 pmp, int id,string caption, short options = 12)
        {
            return (IPropertyManagerPageGroup)pmp.AddGroupBox(id, caption, options);
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
        public static IPropertyManagerPageCheckbox AddCheckBox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Checkbox;
            var result = group != null ?
             (IPropertyManagerPageCheckbox)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageCheckbox)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
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
        public static IPropertyManagerPageTextbox AddTextBox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Textbox;
            var result = group != null ?
             (IPropertyManagerPageTextbox)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageTextbox)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
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
        public static IPropertyManagerPageButton AddButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Button;
            var result = group != null ?
             (IPropertyManagerPageButton)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageButton)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
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
        public static IPropertyManagerPageLabel AddLabel(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Label;
            var result = group != null ?
             (IPropertyManagerPageLabel)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageLabel)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
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
        public static IPropertyManagerPageOption AddRadioButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Option;
            var result =(IPropertyManagerPageOption)group.AddControl2(id, controlType, caption, leftIndent, options, tip);
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
        public static IPropertyManagerPageListbox AddListbox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, string[] items, short height, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Listbox;
            var result = group != null ?
             (IPropertyManagerPageListbox)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageListbox)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            if (result != null)
            {
                result.Height = height;
                result.AddItems(items);
            }
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
        public static IPropertyManagerPageCombobox AddCombobox(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short height, string[] items, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Combobox;
            var result = group != null ?
             (IPropertyManagerPageCombobox)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageCombobox)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            if (result != null)
            {
                result.AddItems(items);
                result.Height = height;
            }
            return result;
        }

        /// <summary>
        /// adds a Numberbox aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="initilaValue">initial value of the number box when loaded firt time</param>
        /// <param name="unit">unit or type of number as defined by <see cref="swNumberboxUnitType_e"/> input</param>
        /// <param name="inclusive">whether the max should be inclusive in the range or not</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageNumberbox"/></returns>
        public static IPropertyManagerPageNumberbox AddNumberbox(this IPropertyManagerPage2 pmp,
            IPropertyManagerPageGroup group,
            int id,
            string caption,
            string tip,
            double initilaValue,
            int unit,
            double min,
            double max,
            double increment,
            bool inclusive = true,
            short leftIndent = 1,
            int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Numberbox;
            var result = group != null ?
             (IPropertyManagerPageNumberbox)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageNumberbox)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            if (result != null)
            {
                result.Value = initilaValue;
                result.SetRange(unit, min, max, increment, inclusive);
            }
            var t = result as IPropertyManagerPageControl;
            return result;
        }

        /// <summary>
        /// adds a Selectionbox aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="filter">array of integers as defined by <see cref="swSelectType_e"/> cast into (int)</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageSelectionbox"/></returns>
        public static IPropertyManagerPageSelectionbox AddSelectionbox(this IPropertyManagerPage2 pmp,
            IPropertyManagerPageGroup group,
            int id,
            string caption,
            string tip,
            short height,
            int[] filter,
            short leftIndent = 1,
            int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Selectionbox;
            var result = group != null ?
             (IPropertyManagerPageSelectionbox)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageSelectionbox)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            if (result != null)
            {
                result.Height = height;
                result.SetSelectionFilters(filter);
            }
            return result;
        }

        /// <summary>
        /// adds a ActiveX aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageActiveX"/></returns>
        public static IPropertyManagerPageActiveX AddActiveX(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_ActiveX;
            var result = group != null ?
             (IPropertyManagerPageActiveX)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageActiveX)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            return result;
        }

        /// <summary>
        /// adds a BitmapButton aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageBitmapButton"/></returns>
        public static IPropertyManagerPageBitmapButton AddBitmapButton(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_BitmapButton;
            var result = group != null ?
             (IPropertyManagerPageBitmapButton)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageBitmapButton)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            return result;
        }

        /// <summary>
        /// adds a Slider aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageSlider"/></returns>
        public static IPropertyManagerPageSlider AddSlider(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Slider;
            var result = group != null ?
             (IPropertyManagerPageSlider)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageSlider)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            return result;
        }

        /// <summary>
        /// adds a Bitmap aligned to the left
        /// </summary>
        /// <param name="group">group to add the control to, will add the control to Property manager page if this parameter is null</param>
        /// <param name="id">id of this control, this will get passed to handler for event handling</param>
        /// <param name="caption">caption of this control</param>
        /// <param name="leftIndent">indent from left as defined by <see cref="swPropertyManagerPageControlLeftAlign_e"/></param>
        /// <param name="options">bitwise options as defined in <see cref="swAddControlOptions_e"/> default value corresponds to an enabled and visible control</param>
        /// <returns><see cref="IPropertyManagerPageBitmap"/></returns>
        public static IPropertyManagerPageBitmap AddBitmap(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_Bitmap;
            var result = group != null ?
             (IPropertyManagerPageBitmap)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageBitmap)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            return result;
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
        public static IPropertyManagerPageWindowFromHandle AddWindowFromHandle(this IPropertyManagerPage2 pmp, IPropertyManagerPageGroup group, int id, string caption, string tip, short leftIndent = 1, int options = 3)
        {
            short controlType = (int)swPropertyManagerPageControlType_e.swControlType_WindowFromHandle;
            var result = group != null ?
             (IPropertyManagerPageWindowFromHandle)group.AddControl2(id, controlType, caption, leftIndent, options, tip) :
             (IPropertyManagerPageWindowFromHandle)pmp.AddControl2(id, controlType, caption, leftIndent, options, tip);
            return result;
        }
    }

}

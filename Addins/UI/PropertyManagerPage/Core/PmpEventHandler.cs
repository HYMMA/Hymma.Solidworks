using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Hymma.Solidworks.Addins.Logger;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// handles property manager page events
    /// </summary>
    [ComVisible(true)]
    public class PmpEventHandler : PropertyManagerPage2Handler9
    {
        private IEnumerable<PopUpMenueItem> popUpItems;


        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="uiModel"></param>
        public PmpEventHandler(PmpUiModel uiModel)
        {
            this.UiModel = uiModel ?? throw new Exception();
            UiModel.OnRegister += () =>
            {
                popUpItems = UiModel.GetControls<PmpSelectionBox>().SelectMany(sb => sb.PopUpMenueItems);
                if (UiModel.PopUpMenueItems != null)
                {
                    popUpItems.Concat(UiModel.PopUpMenueItems);
                }
            };
        }

        /// <summary>
        /// wrapper for constrols in this property manager page
        /// </summary>
        public PmpUiModel UiModel { get; private set; }


        /// <summary>
        /// fires after user opens a pmp
        /// </summary>
        public void AfterActivation()
        {
            Log("After activaiton event handler");
            UiModel?.OnAfterActivation?.Invoke();
        }

        /// <summary>
        /// fired when user closes the pmp
        /// </summary>
        /// <param name="Reason"></param>
        public void OnClose(int Reason)
        {
            Log("onCLose event handler");
            UiModel?.OnClose?.Invoke((PmpCloseReason)Reason);
        }

        /// <summary>
        /// fires after users closes the pmp
        /// </summary>
        public void AfterClose()
        {
            Log("After close event handler");
            UiModel?.OnAfterClose?.Invoke();

        }

        /// <summary>
        /// fires once user clicks on help btn
        /// </summary>
        /// <returns></returns>
        public bool OnHelp()
        {
            if (UiModel.OnHelp == null) return false;
            return UiModel.OnHelp.Invoke();
        }

        /// <summary>
        /// fires when users selects on previous page button in pmp
        /// </summary>
        /// <returns></returns>
        public bool OnPreviousPage()
        {

            Log("on previous page event handling...");
            if (UiModel.OnPreviousPage == null) return false;
            return UiModel.OnPreviousPage.Invoke();
        }

        /// <summary>
        /// fires when users selects on next page button in pmp
        /// </summary>
        /// <returns></returns>
        public bool OnNextPage()
        {
            Log("on next page event handling ...");
            if (UiModel.OnNextPage == null) return false;
            return UiModel.OnNextPage.Invoke();
        }

        /// <summary>
        /// fires when users previews results
        /// </summary>
        /// <returns></returns>
        public bool OnPreview()
        {
            Log("on preview event handling...");
            if (UiModel.OnPreview == null) return false;
            return UiModel.OnPreview.Invoke();
        }

        /// <summary>
        /// firest when users press on whats new button in pmp
        /// </summary>
        public void OnWhatsNew()
        {
            Log("on what's new event handling...");
            UiModel?.OnWhatsNew?.Invoke();
        }

        /// <summary>
        /// fires when user presses undo in a pmp
        /// </summary>
        public void OnUndo()
        {
            Log("on undo event handling ... ");
            UiModel.OnUndo?.Invoke();
        }

        /// <summary>
        /// fires when user re does something
        /// </summary>
        public void OnRedo()
        {
            Log("on redo event handling ...");
            UiModel.OnRedo?.Invoke();
        }

        /// <summary>
        /// fires when usrs clicks on a tab in pmp
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool OnTabClicked(int Id)
        {
            Log("on tab clicked event handling");
            var tab = UiModel.PmpTabs.FirstOrDefault(t => t.Id == Id);
            tab?.OnPress?.Invoke();
            return true;
        }

        /// <summary>
        /// fires when usres expands a group
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Expanded"></param>
        public void OnGroupExpand(int Id, bool Expanded)
        {
            Log("onGroupExpand event handling ...");
            if (UiModel.AllGroups.FirstOrDefault(g => g.Id == Id) is PmpGroup group)
            {
                group.Expanded = Expanded;
                group.GroupExpand(Expanded);
            }
        }

        /// <summary>
        /// on checkable groups, this method responds to check event 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Checked"></param>
        public void OnGroupCheck(int Id, bool Checked)
        {
            Log($"onGroupCheck event handling int id={Id} int bool={Checked}");
            if (UiModel.AllGroups.FirstOrDefault(g => g.Id == Id) is PmpGroupCheckable group)
            {
                group.IsChecked = Checked;
                group.GroupChecked(Checked);
            }
        }

        /// <summary>
        /// fires when check box status is changed
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Checked"></param>
        public void OnCheckboxCheck(int Id, bool Checked)
        {
            Log($"onCheckboxCheck event handling int id ={Id} bool Checked={Checked}");

            //get the check box with id from UiModel
            if ((UiModel.GetControl(Id) is PmpCheckBox checkBox))
            {
                //call on checked delegate on the check box
                checkBox.IsChecked = Checked;
                checkBox.PmpCheckBoxCheckedCallBack(Checked);
            }
        }

        /// <summary>
        /// fires when radio button is checked
        /// </summary>
        /// <param name="Id">id of the clicked on radio button in the property manager page</param>
        public void OnOptionCheck(int Id)
        {
            Log($"onOptionCheck evnet handling int id ={Id}");

            //get the radio button from ui model
            if (!(UiModel.GetControl(Id) is PmpRadioButton radioBtn)) return;

            //get the group of radio button
            var group = UiModel.AllGroups.FirstOrDefault(g => g.Controls.Contains(radioBtn));

            //get all radio buttons ...
            var groupOptions = group.Controls.Where(c => c.Type == swPropertyManagerPageControlType_e.swControlType_Option).Cast<PmpRadioButton>().ToList();

            //set the IsChecked property of radio buttons to false
            groupOptions.ForEach(option => option.IsChecked = false);

            //set the IsChecked property of clicked radio button to true
            radioBtn.IsChecked = true;

            //invoke any function assigned to it
            radioBtn?.Checked();
        }

        /// <summary>
        /// fires whena button is pressed
        /// </summary>
        /// <param name="Id">id of the button in pmp</param>
        public void OnButtonPress(int Id)
        {
            Log("event handling button press...");
            var button = UiModel.GetControl(Id);
            switch (button.Type)
            {
                case swPropertyManagerPageControlType_e.swControlType_Button:
                    button.CastTo<PmpButton>()?.ClickedCallBack();
                    break;
                case swPropertyManagerPageControlType_e.swControlType_BitmapButton:
                    button.CastTo<PmpBitmapButton>()?.ClickedCallBack();
                    break;
                case swPropertyManagerPageControlType_e.swControlType_CheckableBitmapButton:
                    button.CastTo<PmpBitmapButtonCheckable>()?.ClickedCallBack();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// fires when user changes the text in a text box or a numberbox
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Text"></param>
        public void OnTextboxChanged(int Id, string Text)
        {
            var control = UiModel.GetControl(Id);

            //if control is a number box
            if (control.Type == swPropertyManagerPageControlType_e.swControlType_Numberbox
                && control is PmpNumberBox numberBox)
            {
                numberBox?.TextChanged(Text);
            }

            //if control is a text box
            if (control.Type == swPropertyManagerPageControlType_e.swControlType_Textbox
                && control is PmpTextBox txtBox)
            {
                txtBox?.TextChanged(Text);
            }
        }

        /// <summary>
        /// when value of a number box is changed by user
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Value">new value</param>
        public void OnNumberboxChanged(int Id, double Value)
        {
            var numberBox = UiModel.GetControl(Id);
            numberBox.CastTo<PmpNumberBox>()?.Changed(Value);
        }

        /// <summary>
        /// solidworks calls this everytim combox text is edited 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Text"></param>
        public void OnComboboxEditChanged(int Id, string Text)
        {
            PmpComboBox pmpComboBox = UiModel.GetControl(Id) as PmpComboBox;
            pmpComboBox?.SelectionEdit(Text);
        }

        /// <summary>
        /// solidworks calls this evertime combo box selection is changed
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Item"></param>
        public void OnComboboxSelectionChanged(int Id, int Item)
        {
            var control = UiModel.GetControl(Id);
            if (control.Type == swPropertyManagerPageControlType_e.swControlType_Combobox && control is PmpComboBox pmpCombo)
            {
                pmpCombo.SelectionChanged(Item);
            }
            else if (control.Type == swPropertyManagerPageControlType_e.swControlType_Numberbox && control is PmpNumberBox pmpNumber)
            {
                pmpNumber.SelectionChanged(Item);
            }

        }

        /// <summary>
        /// Called when a user changes the selected item in a list box or selection list box on this PropertyManager page. 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Item"></param>
        public void OnListboxSelectionChanged(int Id, int Item)
        {
            PmpListBox pmpList = UiModel.GetControl(Id) as PmpListBox;
            pmpList?.SelectionChange(Item);
        }

        /// <summary>
        /// when focus of a seleciton box is changed this methode will fire
        /// </summary>
        /// <param name="Id"></param>
        public void OnSelectionboxFocusChanged(int Id)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            selectionBox?.FocusChanged();
        }

        /// <summary>
        /// when selection box list is changed this methode will fire
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Count"></param>
        public void OnSelectionboxListChanged(int Id, int Count)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            //update the cursor if user has assigned value to it
            if (selectionBox.CursorStyle != PmpCursorStyles.None)
                UiModel.SetCursor(selectionBox.CursorStyle);

            //invoke delegate
            selectionBox?.ListChanged(Count);
        }

        /// <summary>
        /// when selection box call out is created this method is called
        /// </summary>
        /// <param name="Id">id of the selectionbox</param>
        public void OnSelectionboxCalloutCreated(int Id)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            //invoke delegate
            selectionBox?.CallOutCreated();
        }

        /// <summary>
        /// when slection box callout is destroyed this methode will get called
        /// </summary>
        /// <param name="Id"></param>
        public void OnSelectionboxCalloutDestroyed(int Id)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            //invoke delegate
            selectionBox?.CallOutDestroyed();
        }

        /// <summary>
        /// Called when a selection is made, which allows the add-in to accept or reject the selection. 
        /// </summary>
        /// <param name="Id">ID of the active selection box, where this selection is being made</param>
        /// <param name="Selection">Object being selected</param>
        /// <param name="SelType">Entity type of the selection as defined in<see cref="swSelectType_e"/> </param>
        /// <param name="ItemText">ItemText is returned to SOLIDWORKS and stored on the selected object and can be used by your PropertyManager page selection list boxes for the life of that selection.</param>
        /// <remarks>This method is called by SOLIDWORKS when an add-in has a PropertyManager page displayed and a selection is made that passes the selection filter criteria set up for a selection list box. The add-in can then: 
        /// Take the Dispatch pointer and the selection type.
        ///        QueryInterface the Dispatch pointer to get the specific interface.
        ///    Use methods or properties of that interface to determine if the selection should be allowed or not.If the selection is:
        ///        accepted, return true, and processing continues normally.
        ///        - or -
        ///        rejected, return false, and SOLIDWORKS does not accept the selection, just as if the selection did not pass the selection filter criteria of the selection list box.  
        ///The add-in should not release the Dispatch pointer. SOLIDWORKS will release the Dispatch pointer upon return from this method.
        ///The method is called during the process of SOLIDWORKS selection.It is neither a pre-notification nor post-notification.The add-in should not be taking any action that might affect the model or the selection list.The add-in should only be querying information and then returning true/VARIANT_TRUE or false/VARIANT_FALSE.
        ///</remarks>
        /// <returns></returns>
        public bool OnSubmitSelection(int Id, object Selection, int SelType, ref string ItemText)
        {
            if (UiModel.GetControl(Id) is PmpSelectionBox selectionBox)
            {
                // otherwise return what user ahs defined
                return selectionBox.SubmitSelection(Selection, SelType, ItemText);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int OnActiveXControlCreated(int Id, bool Status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// fires when the position of a slider is changing
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Value"></param>
        public void OnSliderPositionChanged(int Id, double Value)
        {
            var slider = UiModel.GetControl(Id) as PmpSlider;
            slider?.PositionChanged(Value);
        }

        /// <summary>
        /// fires when the user finishes changes on a slider
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Value"></param>
        public void OnSliderTrackingCompleted(int Id, double Value)
        {
            var slider = UiModel.GetControl(Id) as PmpSlider;
            slider?.TrackingComplete(Value);
        }

        /// <summary>
        ///  Processes a keystroke that occurred on this PropertyManager page. 
        /// </summary>
        /// <param name="Wparam">wparam argument from Windows processing; indicates the keystroke that occurred</param>
        /// <param name="Message">Message being processed by Windows; one of these values:</param>
        /// <param name="Lparam">lparam argument from Windows processing; bitmask containing various pieces of information; dependent on specific message</param>
        /// <param name="Id">ID of the control that has focus when the keystroke was made; this is the ID specified when the control was created in IPropertyManagerPage2::AddControl or IPropertyManagerPage2::IAddControl or IPropertyManagerPage2::AddGroupBox or IPropertyManagerPage2::IAddGroupBox.</param>
        /// <returns>True indicates that the keystroke has been handled by the add-in and SOLIDWORKS should not continue to try to process it, false indicates that the keystroke has not been handled by the add-in and SOLIDWORKS will continue to try to process it</returns>
        public bool OnKeystroke(int Wparam, int Message, int Lparam, int Id)
        {
            return UiModel.KeyStroke(Wparam, Message, Lparam);
        }

        /// <summary>
        /// Determines which item was selected when the user selects a pop-up menu item. 
        /// </summary>
        /// <param name="Id"></param>
        public void OnPopupMenuItem(int Id)
        {

            popUpItems.FirstOrDefault(item => item.Id == Id)?.OnPress?.Invoke();
        }

        /// <summary>
        ///  When Windows attempts to select or deselect and enable or disable the pop-up menu item, SOLIDWORKS calls this method to get the state of the menu item from the add-in. 
        /// </summary>
        /// <param name="Id">Unique user-defined ID for a pop-up menu item</param>
        /// <param name="retval">State of the specified unique user-defined pop-up menu item:
        /// 0 - Not selected(i.e., not checked) and disabled(i.e., grayed out)
        /// 1 - Not selected and enabled
        /// 2 - Selected(i.e., checked) and disabled
        /// 3 - Selected and enabled</param>
        public void OnPopupMenuItemUpdate(int Id, ref int retval)
        {
            popUpItems?.FirstOrDefault(item => item.Id == Id)?.OnUpdate?.Invoke(retval);
        }

        /// <summary>
        /// fired when user start interacting with a control
        /// </summary>
        /// <param name="Id"></param>
        public void OnGainedFocus(int Id)
        {
            var control = UiModel.GetControl(Id);
            control?.GainedFocus();
        }

        /// <summary>
        /// fired when user browses away from a control
        /// </summary>
        /// <param name="Id"></param>
        public void OnLostFocus(int Id)
        {
            var control = UiModel.GetControl(Id);
            control?.LostFocus();
        }

        /// <summary>
        /// fired after a window form handler control is displayed in the property manager page 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int OnWindowFromHandleControlCreated(int Id, bool Status)
        {
            Log("on window form handler control created fired from solidworks event handler");
            if (Status == false)
                return (int)swHandleWindowFromHandleCreationFailure_e.swHandleWindowFromHandleCreationFailure_Cancel;
            return -1;
        }

        /// <summary>
        /// fired when user right clicks on one of list box items
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        public void OnListboxRMBUp(int Id, int PosX, int PosY)
        {
            var listbox = UiModel.GetControl(Id) as PmpListBox;
            listbox?.RightMouseBtnUp(Tuple.Create<double, double, double>(PosX, PosY, 0));
        }

        /// <summary>
        /// Called when a user finishes changing the value in the number box on a PropertyManager page. 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Value"></param>
        public void OnNumberBoxTrackingCompleted(int Id, double Value)
        {
            Log("on numberbox tracking complete event fired");
            var numBox = UiModel.GetControl(Id) as PmpNumberBox;
            numBox?.TrackComplete(Value);
        }
    }
}

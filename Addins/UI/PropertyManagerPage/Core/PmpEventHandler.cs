using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// handles property manager page events
    /// </summary>
    [ComVisible(true)]
    public class PmpEventHandler : PropertyManagerPage2Handler9
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="uiModel"></param>
        public PmpEventHandler(PropertyManagerPageUIBase uiModel)
        {
            this.UiModel = uiModel ?? throw new Exception();
        }

        /// <summary>
        /// wrapper for constrols in this property manager page
        /// </summary>
        public PropertyManagerPageUIBase UiModel { get; private set; }

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
            UiModel?.OnClose?.Invoke((PMPCloseReason)Reason);
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
            if (UiModel.OnTabClicked == null) return false;
            return UiModel.OnTabClicked.Invoke(Id);
        }

        /// <summary>
        /// fires when usres expands a group
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Expanded"></param>
        public void OnGroupExpand(int Id, bool Expanded)
        {
            Log("onGroupExpand event handling ...");
            var group = UiModel.PmpGroups.FirstOrDefault(g => g.Id == Id);
            group.Expanded = Expanded;
            group?.GroupExpand(Expanded);
        }

        /// <summary>
        /// on checkable groups, this method responds to check event 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Checked"></param>
        public void OnGroupCheck(int Id, bool Checked)
        {
            Log($"onGroupCheck event handling int id={Id} int bool={Checked}");
            var group = UiModel.PmpGroups.Where(g => g.Id == Id).FirstOrDefault();
            group.GroupChecked(Checked);
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
            if (!(UiModel.GetControl(Id) is PmpCheckBox checkBox))
                return;

            //call on checked delegate on the check box
            checkBox.IsChecked = Checked;
            checkBox.Checked(Checked);
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
            var group = UiModel.PmpGroups.FirstOrDefault(g => g.Controls.Contains(radioBtn));

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
                    button.CastTo<PmpButton>()?.Press();
                    break;
                case swPropertyManagerPageControlType_e.swControlType_BitmapButton:
                    button.CastTo<PmpBitmapButton>()?.Press();
                    break;
                case swPropertyManagerPageControlType_e.swControlType_CheckableBitmapButton:
                    button.CastTo<PmpBitmapButtonCheckable>()?.Press();
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
            PmpComboBox pmpComboBox = UiModel.GetControl(Id) as PmpComboBox;
            pmpComboBox?.SelectionChanged(Item);
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

        public int OnActiveXControlCreated(int Id, bool Status)
        {
            throw new NotImplementedException();
        }

        public void OnSliderPositionChanged(int Id, double Value)
        {
            throw new NotImplementedException();
        }

        public void OnSliderTrackingCompleted(int Id, double Value)
        {
            throw new NotImplementedException();
        }

        public bool OnKeystroke(int Wparam, int Message, int Lparam, int Id)
        {
            throw new NotImplementedException();
        }

        public void OnPopupMenuItem(int Id)
        {
            throw new NotImplementedException();
        }

        public void OnPopupMenuItemUpdate(int Id, ref int retval)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// fired when user start interacting with a control
        /// </summary>
        /// <param name="Id"></param>
        public void OnGainedFocus(int Id)
        {
            var control = UiModel.GetControl(Id);
            control.GainedFocus();
        }

        /// <summary>
        /// fired when user browses away from a control
        /// </summary>
        /// <param name="Id"></param>
        public void OnLostFocus(int Id)
        {
            var control = UiModel.GetControl(Id);
            control.LostFocus();
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
            return 0;
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
            listbox?.RightMouseBtnUp(new Mathematics.Point(PosX, PosY, 0));
        }

        public void OnNumberBoxTrackingCompleted(int Id, double Value)
        {
            throw new NotImplementedException();
        }
    }
}

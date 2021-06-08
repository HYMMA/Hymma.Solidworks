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
        public PmpEventHandler(PmpUiModel uiModel)
        {
            this.UiModel = uiModel;
        }

        /// <summary>
        /// wrapper for constrols in this property manager page
        /// </summary>
        public PmpUiModel UiModel { get; private set; }

        public void AfterActivation()
        {
            Log("After activaiton event handler");]\
        }

        public void OnClose(int Reason)
        {

            Log("onCLose event handler");
        }

        public void AfterClose()
        {
            Log("After close event handler");
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

        public bool OnPreviousPage()
        {
            throw new NotImplementedException();
        }

        public bool OnNextPage()
        {
            throw new NotImplementedException();
        }

        public bool OnPreview()
        {
            throw new NotImplementedException();
        }

        public void OnWhatsNew()
        {
            throw new NotImplementedException();
        }

        public void OnUndo()
        {
            throw new NotImplementedException();
        }

        public void OnRedo()
        {
            throw new NotImplementedException();
        }

        public bool OnTabClicked(int Id)
        {
            throw new NotImplementedException();
        }

        public void OnGroupExpand(int Id, bool Expanded)
        {
            Log("onGroupExpand event handling ...");
        }

        public void OnGroupCheck(int Id, bool Checked)
        {
            Log($"onGroupCheck event handling int id={Id} int bool={Checked}");
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
            var checkBox = UiModel.GetControl(Id) as PmpCheckBox;
            if (checkBox == null) return;
            checkBox.IsChecked = Checked;

            //call on checked delegate on the check box
            checkBox?.OnChecked?.Invoke(Checked);
        }

        /// <summary>
        /// fires when radio button is checked
        /// </summary>
        /// <param name="Id">id of the clicked on radio button in the property manager page</param>
        public void OnOptionCheck(int Id)
        {
            Log($"onOptionCheck evnet handling int id ={Id}");

            //get the radio button from ui model
            var radioBtn = UiModel.GetControl(Id) as PmpRadioButton;

            //null check
            if (radioBtn == null) return;

            //get expanded groups
            //solidworks will treat all radio buttons as memebers of the same group if different groups are expanded
            //its only when a group is retracted that the radio buttons will behave differently
            var groups = UiModel.PmpGroups.Where(g => g.Expanded/*Controls.Contains(radioBtn)*/);

            //get all radio buttons ...
            var groupOptions = groups
                .SelectMany(g=>g.Controls)
                .Where(c => c is PmpRadioButton)
                .Cast<PmpRadioButton>().ToList();

            //set the IsChecked property of radio buttons to false
            groupOptions.ForEach(groupOption => groupOption.IsChecked = false);

            //set the IsChecked property of clicked radio button to true
            radioBtn.IsChecked = true;

            //invoke any function assigned to it
            radioBtn?.OnChecked?.Invoke();
        }

        public void OnButtonPress(int Id)
        {
            throw new NotImplementedException();
        }

        public void OnTextboxChanged(int Id, string Text)
        {
            throw new NotImplementedException();
        }

        public void OnNumberboxChanged(int Id, double Value)
        {
            throw new NotImplementedException();
        }

        public void OnComboboxEditChanged(int Id, string Text)
        {
            throw new NotImplementedException();
        }

        public void OnComboboxSelectionChanged(int Id, int Item)
        {
            throw new NotImplementedException();
        }

        public void OnListboxSelectionChanged(int Id, int Item)
        {
            throw new NotImplementedException();
        }


        public void OnSelectionboxFocusChanged(int Id)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            selectionBox?.OnFocusChanged?.Invoke();
        }

        public void OnSelectionboxListChanged(int Id, int Count)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            //invoke delegate
            selectionBox?.OnListChanged?.Invoke(Count);
        }

        public void OnSelectionboxCalloutCreated(int Id)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;
            
            //invoke delegate
            selectionBox?.OnCallOutCreated?.Invoke();
        }

        public void OnSelectionboxCalloutDestroyed(int Id)
        {
            //get selection box
            PmpSelectionBox selectionBox = UiModel.GetControl(Id) as PmpSelectionBox;

            //invoke delegate
            selectionBox?.OnCallOutDestroyed?.Invoke();
        }

        public bool OnSubmitSelection(int Id, object Selection, int SelType, ref string ItemText)
        {
            throw new NotImplementedException();
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

        public void OnGainedFocus(int Id)
        {
            throw new NotImplementedException();
        }

        public void OnLostFocus(int Id)
        {
            throw new NotImplementedException();
        }

        public int OnWindowFromHandleControlCreated(int Id, bool Status)
        {
            throw new NotImplementedException();
        }

        public void OnListboxRMBUp(int Id, int PosX, int PosY)
        {
            throw new NotImplementedException();
        }

        public void OnNumberBoxTrackingCompleted(int Id, double Value)
        {
            throw new NotImplementedException();
        }
    }
}

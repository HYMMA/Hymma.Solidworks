using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hymma.SolidTools.SolidAddins.UI.PropertyManagerPage
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
        public PmpUiModel UiModel { get;private set; }
        
        public void AfterActivation()
        {
            throw new NotImplementedException();
        }

        public void OnClose(int Reason)
        {
            throw new NotImplementedException();
        }

        public void AfterClose()
        {
            throw new NotImplementedException();
        }

        public bool OnHelp()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void OnGroupCheck(int Id, bool Checked)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// fires when check box status is changed
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Checked"></param>
        public void OnCheckboxCheck(int Id, bool Checked)
        {
            var checkBoxes = UiModel.SwBoxes
                .SelectMany(g => g.Controls)
                .Where(c => c.Type == swPropertyManagerPageControlType_e.swControlType_Checkbox)
                .Where(ch => ch.Id == Id).Cast<SwCheckBox>();
            foreach (var checkBox in checkBoxes)
            {
                checkBox.OnClicked.Invoke(Checked);
            }
        }

        public void OnOptionCheck(int Id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void OnSelectionboxListChanged(int Id, int Count)
        {
            throw new NotImplementedException();
        }

        public void OnSelectionboxCalloutCreated(int Id)
        {
            throw new NotImplementedException();
        }

        public void OnSelectionboxCalloutDestroyed(int Id)
        {
            throw new NotImplementedException();
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

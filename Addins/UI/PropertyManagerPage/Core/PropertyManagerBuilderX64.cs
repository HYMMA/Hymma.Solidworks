using SolidWorks.Interop.sldworks;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Generates Property Manager Page for a Solidworks Addin of type <see cref="AddinMaker"/> that runs on X64 base CPU<br/>
    /// <c>since 2015 solidworks supports X64 CPU only </c>
    /// </summary>
    public class PropertyManagerBuilderX64 : PmpBase
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="addin">the addin of type <see cref="AddinMaker"/> to add thie property manger page to</param>
        /// <param name="uiModel">an object that hosts differet inheritances of <see cref="IPmpControl"/> </param>
        public PropertyManagerBuilderX64(AddinMaker addin, PmpUiModel uiModel)
            : base(addin, new PmpEventHandler(uiModel), uiModel)
        {
        }

        /// <summary>
        /// SOLIDWORKS calls this method to show the UI. the PMP gets generated once at start up
        /// </summary>
        public override void Show()
        {
            if (PMP == null)
            {
                Solidworks.SendMsgToUser("Could not create this property manager page");
                return;
            }

            #region show window forms
            //this needs to be called every time pmp is shown
            if (winFormHandlers != null)
            {
                foreach (var handler in winFormHandlers)
                {
                    if (handler.ElementHost == null || !handler.WindowsControl.HasContent)
                        continue;
                    handler.ElementHost.Child = handler.WindowsControl;
                    handler.ProperptyManagerPageHandle.SetWindowHandlex64(handler.ElementHost.Handle.ToInt64());
                }
            }
            PMP.Show();
            #endregion
            
            #region update checkboxes state
            //whenever propety manager page is shown the checkbox state should reflect
            //the previous setup. previous setup is the last time property manager page was shown
            //this way users will get a consistent experience
            var checkBoxes = uiModel
              .GetControls<PmpCheckBox>()
              .ToList();
            checkBoxes
                .ForEach(ch => Controls[ch.Id]
                    .CastTo<IPropertyManagerPageCheckbox>()
                    .Checked = ch.IsChecked);

            #endregion

            #region update radio button state
            //whenever property manager page is shown the state of radio buttons should reflect that of 
            //previous run of the pmp
            var radioButtons = uiModel.GetControls<PmpRadioButton>().ToList();
            radioButtons.ForEach(rb => Controls[rb.Id]
                .CastTo<IPropertyManagerPageOption>().Checked = rb.IsChecked);
            #endregion

            #region Update group box expanded status
            uiModel.PmpGroups.ForEach(group => 
                Controls[group.Id].CastTo<IPropertyManagerPageGroup>()
                    .Expanded = group.Expanded);
            #endregion
        }
    }
}

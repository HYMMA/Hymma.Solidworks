//using Hymma.Mathematics;
using Hymma.SolidTools.Addins;
using Hymma.SolidTools.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class PropertyManagerPageUI : PmpUiModel
    {
        public PropertyManagerPageUI(ISldWorks Solidworks)
        {
            this.Solidworks = Solidworks;
            Title = "title of property manager page";
            
            var group1 = new PmpGroup("group box caption");
            group1.AddControls(GetControlSet1());

            var group2 = new PmpGroup("radio buttons");
            group2.AddControls(GetControlSet2());
            //group2.AddControl(GetSelectionBoxWithCallout((SldWorks)Solidworks));
            //PmpGroups.Add(new PmpGroup("radio buttons", GetControlSet2()));

            OnHelp = () => { return false; };
            OnAfterActivation = () => { Solidworks.SendMsgToUser("pmp activated"); };
        }
        public ISldWorks Solidworks { get; }
        private List<IPmpControl> GetControlSet1()
        {
            var controls = new List<IPmpControl>
            {
                new PmpCheckBox(true) { Tip = "a tip for a checkbox", Caption = "caption for chckbox" },

            

                new PmpRadioButton(true)
                {
                    Tip = "radio button on group 1",
                    Caption = "caption for radio button on group 1",
                    OnChecked = () => { Solidworks.SendMsgToUser("first radio button clicked on"); }

                },
                new PmpRadioButton(false)
                {
                    Tip = "radio button on group 1",
                    Caption = "caption for radio button on group 1",
                    OnChecked = () => { Solidworks.SendMsgToUser("first radio button clicked on"); }
                }
            };

            return controls;
        }
        private List<IPmpControl> GetControlSet2()
        {
            var controls = new List<IPmpControl>();
            controls.Add(new PmpRadioButton(true)
            {
                Tip = "a tip for this radio button",
                Caption = "caption for this radio button",
                OnChecked = () => { Solidworks.SendMsgToUser($"radio button is checked"); }
            });

            controls.Add(new PmpRadioButton()
            {
                Tip = "another tip for this radio button",
                Caption = "yet another caption for a radio button",
                OnChecked = () => { Solidworks.SendMsgToUser($"radio button is checked"); }
            });

            controls.Add(
                new PmpSelectionBox(new swSelectType_e[] { swSelectType_e.swSelEDGES })
                {
                    Tip = "a tip for this selection box",
                    Caption = "Caption for this selectionbox",
                    OnSubmitSelection = (selection, type, tag) => 
                    {
                        if (type != (int)swSelectType_e.swSelEDGES)
                        {
                            Solidworks.SendMsgToUser("only edges are allowed to select");
                            return false;
                        }
                        return true;
                    }
                    
                });
            return controls;
        }

        private PmpSelectionBox GetSelectionBoxWithCallout(SldWorks solidworks)
        {
            var selBox = new PmpSelectionBox(new swSelectType_e[] { swSelectType_e.swSelEDGES })
            {
                Caption = "caption for selection box with callout"
            };
            var rows = new List<CalloutRow>
            {
               // new CalloutRow("value 1", "row 1") { Target = new Point(0.1, 0.1, 0.1), TextColor = SysColor.Highlight },
                // new CalloutRow("value 2", "row 2") { Target = new Point(0, 0, 0), TextColor = SysColor.AsmInterferenceVolume }
            };
            selBox.CalloutHelper = new CalloutHelper(rows, solidworks, (ModelDoc2)solidworks.ActiveDoc);
            return selBox;
        }
    }
}

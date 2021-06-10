﻿using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace SampleAddin
{
    public class PropertyManagerPageUI : PmpUiModel
    {
        public PropertyManagerPageUI(ISldWorks Solidworks)
        {
            this.Solidworks = Solidworks;
            Title = "title of property manager page";
            PmpGroups.Add(new PmpGroup("group box caption") { Controls = GetControlSet1() });
            PmpGroups.Add(new PmpGroup("radio buttons", GetControlSet2()));
            OnHelp = () => { return false; };
            OnAfterActivation = () => { Solidworks.SendMsgToUser("pmp activated"); };
        }

        public ISldWorks Solidworks { get; }
        private List<IPmpControl> GetControlSet1()
        {
            var controls = new List<IPmpControl>();

            controls.Add(new PmpCheckBox(true) { Tip = "a tip for a checkbox", Caption = "caption for chckbox" });

            controls.Add(new PmpCheckBox(Properties.Settings.Default.ChkBoxChkd)
            {
                Tip = "a tip for a checkbox 2 ",
                Caption = "caption for chckbox 2",
                OnChecked = (isChecked) =>
                {
                    Solidworks.SendMsgToUser($"you have clicked on check box  {isChecked}");
                    Properties.Settings.Default.ChkBoxChkd = isChecked;
                }
            });

            controls.Add(new PmpRadioButton(true)
            {
                Tip = "radio button on group 1",
                Caption = "caption for radio button on group 1",
                OnChecked = () => { Solidworks.SendMsgToUser("first radio button clicked on"); }

            });
            controls.Add(new PmpRadioButton(false)
            {
                Tip = "radio button on group 1",
                Caption = "caption for radio button on group 1",
                OnChecked = () => { Solidworks.SendMsgToUser("first radio button clicked on"); }
            });

            controls.Add(
                new PmpSelectionBox(new swSelectType_e[] { swSelectType_e.swSelEDGES })
                {
                    Tip = "a tip for this selection box",
                    Caption = "Caption for this selectionbox",
                    OnSubmitSelection = (selection,type,tag) => {
                        if (type != (int)swSelectType_e.swSelEDGES)
                        {
                            Solidworks.SendMsgToUser( "only edges are allowed to select");
                            return false;
                        }

                        return true; }
                });

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

            return controls;
        }
    }
}

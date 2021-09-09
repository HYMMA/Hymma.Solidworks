using Hymma.SolidTools.Addins;
using Hymma.SolidTools.Core;
using Hymma.SolidTools.Fluent.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Point = Hymma.Mathematics.Point;

namespace Butter
{
    [ComVisible(true)]
    [Guid("7EF4A3B3-534A-4E20-BF10-F41B9D722E05")]
    [Addin(Title = "Butter", Description = "Smooth As butter", LoadAtStartup = true, AddinIcon = "butter")]
    public class Butter : AddinMaker
    {
        private PropertyManagerPageX64 _pmp;
        public Butter() : base(typeof(Butter))
        {
        }

        public override AddinUserInterface GetUserInterFace()
        {
            var builder = new AddinFactory().GetUiBuilder();

            #region Command Tab

            builder.AddCommandTab()
                .WithTitle("Butter")
                .That()
                .IsVisibleIn(new[] { swDocumentTypes_e.swDocASSEMBLY, swDocumentTypes_e.swDocDRAWING, swDocumentTypes_e.swDocPART })
                .AddCommandGroup(1)
                    .WithTitle("Butter")
                    .WithDescription("smooth as butter")
                    .WithToolTip("Melt a butter")
                    .WithHint("Melt a butter")
                    .WithIcon(Properties.Resources.xtractred)
                    .Has()
                    .Commands(() =>
                    {
                        var command1 = new AddinCommand("Melt the butter", "easily melt a butter", "Melt the butter", Properties.Resources.butter, nameof(ShowPMP));
                        return new[] { command1 };
                    })
                    .SaveCommnadGroup().
                SaveCommandTab();
            #endregion

            #region Butter PMP
            builder.AddPropertyManagerPage("Butter", Solidworks)
            .WithOptions(PmpOptions.OkayButton | PmpOptions.LockedPage | PmpOptions.DisablePageBuildDuringHandlers | PmpOptions.CancelButton | PmpOptions.PushpinButton)
            .WhileClosing((reason) =>
            {
                if (reason != PMPCloseReason.Okay)
                    return;
                ModelDoc2 Part = (ModelDoc2)this.Solidworks.ActiveDoc;

                var boolstatus = Part.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0, true, 0, null, 0);
                var myRefPlane = (RefPlane)Part.FeatureManager.InsertRefPlane(8, 0.01, 0, 0, 0, 0);
                boolstatus = Part.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0, true, 0, null, 0);
                myRefPlane = (RefPlane)Part.FeatureManager.InsertRefPlane(8, 0.02, 0, 0, 0, 0);

                boolstatus = Part.Extension.SelectByID2("Plane2", "PLANE", 0, 0, 0, false, 0, null, 0);
                object vSkLines = null;
                vSkLines = Part.SketchManager.CreateCornerRectangle(-0.0250462141853123, 0.0157487558892494, 0, 0.0275128867944718, -0.015559011842391, 0);

                Part.SketchManager.InsertSketch(true);

                // Sketch to extrude
                boolstatus = Part.Extension.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                // Start condition reference
                boolstatus = Part.Extension.SelectByID2("Plane2", "PLANE", 0.00105020593408751, -0.00195369982668282, 0.0248175428318827, true, 32, null, 0);
                // End condition reference
                boolstatus = Part.Extension.SelectByID2("Plane1", "PLANE", 0.0068370744701368, -0.004419862088339, 0.018892268568016, true, 1, null, 0);

                // Boss extrusion start condition reference is Plane2, and the extrusion end is offset 3 mm from the end condition reference, Plane1
                var myFeature = (Feature)Part.FeatureManager.FeatureExtrusion3(true, false, true, (int)swEndConditions_e.swEndCondOffsetFromSurface, 0, 0.003, 0.003, false, false, false,
                    false, 0.0174532925199433, 0.0174532925199433, false, false, false, false, true, true, true,
                    (int)swStartConditions_e.swStartSurface, 0, false);
            })
            #region Group 1
                    //.AddGroup("Group Caption")
                    //    .IsExpanded()
                    //    .HasTheseControls(GetControlSet2())
                    //.SaveGroup()
                    .AddGroup("Group 1")
                    .IsExpanded()
                    .HasTheseControls(GetControlSet3())
                .SaveGroup()
            #endregion

            .SavePropertyManagerPage(out PropertyManagerPageX64 pmp);
            _pmp = pmp;

            #endregion

            return (AddinUserInterface)builder;
        }

        private IEnumerable<IPmpControl> GetControlSet3()
        {
            var controls = new List<IPmpControl>();
            var selBox = new PmpSelectionBox(new[] { swSelectType_e.swSelCOMPONENTS },
                                             0,
                                             true,
                                             false,
                                             50,
                                             "tip for selection box")
            {
                AllowSelectInMultipleBoxes = true,
                EnableSelectIdenticalComponents = true
            };
            var selBox2 = new PmpSelectionBox(
                new[] { swSelectType_e.swSelSOLIDBODIES },
                SelectionBoxStyles.HScroll | SelectionBoxStyles.MultipleItemSelect | SelectionBoxStyles.UpAndDownButtons);

            var checkableBtnBtimap = new PmpBitmapButtonCheckable(Properties.Resources.xtractOrange,
                                                                  "xtractOrange23",
                                                                  "tip for checkable with bitmap",
                                                                  new[] { BtnSize.nintySix, BtnSize.hundredTwentyEight },
                                                                  50);
            var pmpBitmap = new PmpBitmap(Properties.Resources.hymma_logo_small, "hymma", opacity: 2);

            var bitmapBtn = new PmpBitmapButton(Properties.Resources.butter,
                                                "bitmapBtn2",
                                                "tip"
                                                , new[] { BtnSize.forty, BtnSize.sixtyFour }
                                                , 50);

            bitmapBtn.OnPress += (sender, e) =>
            {
                var btn = sender as PmpBitmapButton;
                var rows = new List<CalloutRow>
                {
                    new CalloutRow("label 1", "lael 1 value"){ IgnoreValue=true },
                    new CalloutRow("label 2", "row 2 value") { TextColor = SysColor.SelectedItem2 , ValueInactive=true, Target=new Point(0.2,0.2,0.2)},
                    new CalloutRow("label 3", "row 3 value") { TextColor = SysColor.Highlight, Target = new Point(0.1, 0.1, 0.1) },
                    new CalloutRow("label 4", "row 4 value") { TextColor = SysColor.Highlight, Target = new Point(0.1, 0, 0) }
                };
                var callout = new CalloutModel(rows, Solidworks, selBox.ActiveDoc)
                {
                    HasTextBox = false,
                    SkipColon = false,
                    TargetStyle = swCalloutTargetStyle_e.swCalloutTargetStyle_Arrow,
                    Position = new Point(0.1, 0.2, 0.3),
                    EnablePushPin = false,
                    MultipleLeaders = false,
                    OpaqueColor = SysColor.Background
                };
                //selBox.CalloutLabel = "my callout label";
                selBox.Callout = callout;
            };
            var comboBox = new PmpComboBox(new List<string> { "item 1", "item 2", "item 3" }, ComboBoxStyles.EditableText, 90)
            {
                EditText = "please enter..."
            };
            var label = new PmpLabel("caption of the label as it appears in the property manager page", LabelStyles.LeftText);

            label.SetBold(0, 10, true);
            label.SetBackgroundColor(0, 8, System.Drawing.Color.Red);
            label.SetCharacterColor(0, 8, System.Drawing.Color.Blue);
            label.BackGroundColor = System.Drawing.Color.Transparent;

            var radio = new PmpRadioButton("radio button caption") { MaintainState = true };
            var radio2 = new PmpRadioButton("radio button caption number 2");
            var checkbox = new PmpCheckBox("caption", false) { MaintainState = true };
            var checkbox2 = new PmpCheckBox("caption2", false);
            var numBox = new PmpNumberBox(NumberBoxStyles.Thumbwheel)
            {
            };
            numBox.AddItems(new[] { "1", "2" });
            numBox.Height = 30;

            numBox.OnDisplay += NumBox_OnDisplay;
            numBox.OnTextChanged += NumBox_OnTypeIn;
            numBox.OnChange += NumBox_OnChange;
            numBox.OnSelectionChanged += NumBox_OnSelectionChanged;
            numBox.OnTrackingComplete += NumBox_OnTrackingComplete;

            var slider = new PmpSlider(SliderStyles.AutoTicks, "caption", "tip");
            //checkbox.OnChecked += (sender, e) =>
            //{
            //    if (e)
            //    {
            //        var assembly = selBox2.ActiveDoc as AssemblyDoc;
            //        selBox.Append(assembly.GetDistictParts().ToList().Where(c => c.GetModelDoc2() is PartDoc).ToArray());
            //        var testRow = selBox.Callout.GetRows().FirstOrDefault(row => row.Label == "label 1");
            //        testRow.Value = "row 1 value";
            //    }
            //    else
            //    {
            //        comboBox.Clear();
            //    }
            //};
            //controls.Add(label);
            //controls.Add(pmpBitmap);
            //controls.Add(selBox);
            //controls.Add(selBox2);
            //controls.Add(checkbox);
            //controls.Add(bitmapBtn);
            //controls.Add(checkableBtnBtimap);
            //controls.Add(comboBox);
            controls.Add(radio);
            controls.Add(radio2);
            controls.Add(checkbox);
            controls.Add(checkbox2);
            controls.Add(numBox);
            controls.Add(slider);
            return controls;
        }

        private void NumBox_OnSelectionChanged(object sender, string e)
        {
            Solidworks.SendMsgToUser(e);
        }

        private void NumBox_OnTrackingComplete(object sender, double e)
        {
            var numbox = sender as PmpNumberBox;
            if (e==100)
            {
                numbox.TextColor = Color.Red;
                numbox.BackGroundColor = Color.Yellow;
            }
        }

        private void NumBox_OnDisplay(PmpNumberBox sender, NumberBox_Ondisplay_EventArgs eventArgs)
        {
            sender.InsertItem(-1, "9");
            sender.InsertItem(3, "second item");
        }

        private void NumBox_OnTypeIn(object sender, string e)
        {
            var numbox = sender as PmpNumberBox;
        }

        private void NumBox_OnChange(object sender, double e)
        {
            var numbox = sender as PmpNumberBox;
        }

        public void ShowPMP()
        {
            _pmp.Show();
        }

        public int EnableMethode()
        {
            if (Solidworks.ActiveDoc != null)
                return 1;
            return 0;
        }
    }
}

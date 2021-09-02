using Hymma.Mathematics;
using Hymma.SolidTools.Addins;
using Hymma.SolidTools.Core;
using Hymma.SolidTools.Fluent.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            selBox2.OnDisplay += SelBox2_OnDisplay;
            var checkbox = new PmpCheckBox("caption", false, true)
            {
            };
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
            checkbox.OnChecked += (sender, e) =>
            {
                if (e)
                {
                    var assembly = selBox2.ActiveDoc as AssemblyDoc;
                    selBox.Append(assembly.GetDistictParts().ToList().Where(c => c.GetModelDoc2() is PartDoc).ToArray());
                    var testRow = selBox.Callout.GetRows().FirstOrDefault(row => row.Label == "label 1");
                    testRow.Value = "row 1 value";
                }
                else
                {
                }
            };
            var comboBox = new PmpComboBox(new List<string> { "item 1", "item 2", "item 3" }, ComboBoxStyles.EditableText, 90)
            {
                EditText = "please enter..."
            };
            comboBox.OnLostFocus += ComboBox_OnLostFocus;
            controls.Add(pmpBitmap);
            controls.Add(selBox);
            controls.Add(selBox2);
            controls.Add(checkbox);
            controls.Add(bitmapBtn);
            controls.Add(checkableBtnBtimap);
            controls.Add(comboBox);
            return controls;
        }

        private void ComboBox_OnLostFocus(object sender, EventArgs e)
        {
            var combo = sender as PmpComboBox;
            if (!combo.Contains(combo.EditText))
                combo.InsertItem(2, combo.EditText);
        }

        private void SelBox2_OnDisplay(PmpSelectionBox sender, SelBox_OnDisplay_EventArgs eventArgs)
        {
            eventArgs.Style = ((int)SelectionBoxStyles.UpAndDownButtons);
            eventArgs.SelectionColor = SysColor.ActiveSelectionListBox;
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

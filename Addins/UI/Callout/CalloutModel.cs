using Hymma.Solidworks.Extensions;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Allows add-in applications to manipulate single and multi-row callouts
    /// </summary>
    public class CalloutModel : IWrapSolidworksObject<Callout>
    {
        #region private fields
        private List<CalloutRow> _rows = new List<CalloutRow>();
        private int _rowId;
        #endregion

        #region constructors
        /// <summary>
        /// private constructor
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="solidworks"></param>
        private CalloutModel(List<CalloutRow> rows, ISldWorks solidworks)
        {
            //assign event handler that solidworks will use upon creation of callout
            this.Handler = new SolidworksCalloutHandler(this);
            this.Solidworks = (SldWorks)solidworks;
            _rowId = -1;
        }

        /// <summary>
        /// create a callout 
        /// </summary>
        /// <param name="rows">list of string in this callout. will be adde to the UI in the same order added to this list</param>
        /// <param name="solidworks">solidworks object</param>
        /// <param name="model">model to add selection </param>
        /// <param name="updateWithSelection">will make callout dependent on selection if set to true.</param>
        public CalloutModel(List<CalloutRow> rows, ISldWorks solidworks, ModelDoc2 model, bool updateWithSelection = true) : this(rows, solidworks)
        {
            if (updateWithSelection)
            {
                var selectionMgr = model.SelectionManager as SelectionMgr;
                SolidworksObject = selectionMgr.CreateCallout2(rows.Count, Handler);
            }
            else
            {
                var modelExtension = model.Extension;
                SolidworksObject = modelExtension.CreateCallout(rows.Count, Handler);
            }
            rows.ForEach(row => AddRow(row));
        }

        /// <summary>
        /// Creates a callout independent of a selection in a <see cref="ModelView"/>
        /// </summary>
        /// <param name="rows">list of string in this callout. will be adde to the UI in the same order added to this list</param>
        /// <param name="solidworks">solidworks object</param>
        /// <param name="modelView">model view to creat the callout in</param>
        public CalloutModel(List<CalloutRow> rows, ISldWorks solidworks, ModelView modelView) : this(rows, solidworks)
        {
            SolidworksObject = modelView.CreateCallout(rows.Count, Handler);
            rows.ForEach(row => AddRow(row));
        }
        #endregion

        #region Methods
        /// <summary>
        /// get the row id for the value provided
        /// </summary>
        /// <param name="value"></param>
        public List<int> GetRowIds(string value)
        {
            return _rows.Where(r => r.Value == value).Select(r => r.Id).ToList();
        }
        #endregion

        #region properties

        /// <summary>
        /// each row in a callout has an id and a text value
        /// </summary>
        /// <value>list of rows in a callout </value>
        public List<CalloutRow> GetRows()
        {
            return _rows;
        }

        /// <summary>
        /// adds a new row to this callout
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(CalloutRow row)
        {
            row.Id = ++_rowId;
            row.Callout = SolidworksObject;
            //map to solidworks callout
            SolidworksObject.Value[row.Id] = row.Value;
            SolidworksObject.ValueInactive[row.Id] = row.ValueInactive;
            SolidworksObject.Label2[row.Id] = row.Label;
            SolidworksObject.TextColor[row.Id] = (int)row.TextColor;
            SolidworksObject.IgnoreValue[row.Id] = row.IgnoreValue;
            SolidworksObject.SetTargetPoint(row.Id, row.Target.Item1, row.Target.Item2, row.Target.Item3);

            row.OnTargetChanged += (id, target) =>
            {
                SolidworksObject.SetTargetPoint(id, target.Item1, target.Item2, target.Item3);
            };

            //when Value property of the row is changed this gets called
            row.OnValueChanged += (sender, newValue) =>
            {
                //assign the value to solidworks callout object
                SolidworksObject.Value[sender.Id] = newValue;
            };
            _rows.Add(row);
        }

        /// <summary>
        /// actual solidworks <see cref="ICallout"/> object assigned to by addin
        /// </summary>
        public Callout SolidworksObject { get; internal set; }

        /// <summary>
        /// solidworks event handler for a callout
        /// </summary>
        public SolidworksCalloutHandler Handler { get; }

        /// <summary>
        /// solidowrks object
        /// </summary>
        public SldWorks Solidworks { get; }

        /// <summary>
        /// Gets or sets the font size for this callout.
        /// </summary>
        public int FontSize { get => SolidworksObject.FontSize; set => SolidworksObject.FontSize = value; }

        /// <summary>
        /// Gets or sets the text format for this callout.
        /// </summary>
        public TextFormat TextFormat { get => SolidworksObject.TextFormat; set => SolidworksObject.TextFormat = value; }


        /* 
         * 
         <<<<<<<<<<
          this is not available in SOLIDWORKS API 2018
         >>>>>>>>>>

         /// <summary>
         /// Gets or sets whether to enable the pushpin for this callout. 
         /// </summary>
         public bool EnablePushPin { get => SolidworksObject.EnablePushPin; set => SolidworksObject.EnablePushPin = value; }*/

        /// <summary>
        /// Gets or sets whether the callout text is enclosed within a box
        /// </summary>
        public bool HasTextBox { get => SolidworksObject.TextBox; set => SolidworksObject.TextBox = value; }

        /// <summary>
        /// Gets or sets the type of connection at the target point for this callout.
        /// </summary>
        public swCalloutTargetStyle_e TargetStyle { get => (swCalloutTargetStyle_e)SolidworksObject.TargetStyle; set => SolidworksObject.TargetStyle = (int)value; }

        /// <summary>
        /// Gets and sets whether to add a colon<strong> : </strong> at the end of the callout label.
        /// </summary>
        public bool SkipColon { get => SolidworksObject.SkipColon; set => SolidworksObject.SkipColon = value; }

        /// <summary>
        /// Gets and sets the position of this callout.
        /// </summary>
        public Tuple<double, double, double> Position
        {
            get
            {
                var coords = (double[])SolidworksObject.Position.ArrayData;
                return Tuple.Create(coords[0],coords[1],coords[2]);
            }

            set
            {

                //get math util
                var mathUtil = Solidworks.GetMathUtility() as IMathUtility;

                //make a new math point from Point
                var mathpoint = mathUtil.CreatePoint(new double[] { value.Item1, value.Item2, value.Item3 }) as MathPoint ?? (MathPoint)mathUtil.CreatePoint(new double[] { 0, 0, 0 });

                //update the callout
                SolidworksObject.Position = mathpoint;
            }
        }

        /// <summary>
        /// Gets or sets the opaque (background) color for the labels for this callout.
        /// </summary>
        /// <remarks>You must use a <see cref="SysColor"/>; you cannot use any other RGB values. To see system colors, click <strong>Tools > Options > Colors.</strong>  in the SOLIDWORKS user interface</remarks>
        public SysColor OpaqueColor { get => (SysColor)SolidworksObject.OpaqueColor; set => SolidworksObject.OpaqueColor = (int)value; }

        /// <summary>
        /// Gets or sets the display of multiple leaders for this callout.  
        /// </summary>
        public bool MultipleLeaders { get => SolidworksObject.MultipleLeaders; set => SolidworksObject.MultipleLeaders = value; }

        /// <summary>
        /// Sets the leader properties of the callout. 
        /// </summary>
        /// <param name="visible">True to display the leader, false to not</param>
        /// <param name="multiple">True to display multiple leaders, false to not</param>
        /// <returns>True if the operation is successful, false if not</returns>
        /// <remarks>You can only use this method before the callout is shown or while the callout is hidden.<br/>
        ///If Visible is set to false, then ICallout::TargetStyle is automatically set to swCalloutTargetStyle_None.</remarks>
        public bool LeaderStatus(bool visible, bool multiple)
        {
            return SolidworksObject.SetLeader(visible, multiple);
        }
        #endregion
    }
}

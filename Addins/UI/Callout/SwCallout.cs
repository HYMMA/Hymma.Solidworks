using Hymma.Mathematics;
using Hymma.SolidTools.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Allows add-in applications to manipulate single and multi-row callouts
    /// </summary>
    public class SwCallout
    {
        private int rowID;
        #region constructors
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="rows">list of string in this callout. will be adde to the UI in the same order added to this list</param>
        public SwCallout(List<CalloutRow> rows)
        {
            this.Rows = new List<CalloutRow>();
            rows.ForEach(row => AddRow(row));
            //assign event handler that solidworks will use upon creation of callout
            this.Handler = new SolidworksCalloutHandler(this);
        }
        #endregion

        /// <summary>
        /// Gets or sets whether to ignore the callout value in the given row.
        /// </summary>
        /// Use this API to remove the white space that remains in the callout when ICallout::Value is set to an empty string.<br/>
        ///This property applies only to a callout that is independent of a selection or created with IModelDocExtension::CreateCallout
        public void IgnoreRow(CalloutRow calloutRow)
        {
            _ = Callout.IgnoreValue[calloutRow.RowId];
        }

        /// <summary>
        /// get the row id for the value provided
        /// </summary>
        /// <param name="value"></param>
        public List<int> GetRowIds(string value)
        {
            return Rows.Where(r => r.Value == value).Select(r => r.RowId).ToList();
        }

        
        #region properties

        /// <summary>
        /// each row in a callout has an id and a text value
        /// </summary>
        /// <value>list of rows in a callout </value>
        public List<CalloutRow> Rows { get; }

        /// <summary>
        /// adds a new row to this callout
        /// </summary>
        /// <param name="calloutRow"></param>
        public void AddRow(CalloutRow calloutRow)
        {
            //increase row id registration
            rowID++;

            //update proeprty of the row
            calloutRow.RowId = rowID;

            //add it to the collection
            Rows.Add(calloutRow);

            //map to solidwork callout
            Callout.Value[rowID] = calloutRow.Value;
            Callout.ValueInactive[rowID] = calloutRow.ValueInactive;
            Callout.Label2[rowID] = calloutRow.Label;
            Callout.TextColor[rowID] = calloutRow.TextColor;
        }

        /// <summary>
        /// actual solidworks <see cref="ICallout"/> object assigned to by addin
        /// </summary>
        public ICallout Callout { get; internal set; }

        /// <summary>
        /// solidworks event handler for a callout
        /// </summary>
        public SolidworksCalloutHandler Handler { get; }

        /// <summary>
        /// Gets or sets the font size for this callout.
        /// </summary>
        public int FontSize { get => Callout.FontSize; set => Callout.FontSize = value; }

        /// <summary>
        /// Gets or sets the text format for this callout.
        /// </summary>
        public TextFormat TextFormat { get => Callout.TextFormat; set => Callout.TextFormat = value; }
        
        /// <summary>
        /// Gets or sets whether to enable the pushpin for this callout. 
        /// </summary>
        public bool EnablePushPin { get => Callout.EnablePushPin; set => Callout.EnablePushPin = value; }

        /// <summary>
        /// Gets or sets whether the callout text is enclosed within a box
        /// </summary>
        public bool HasTextBox { get => Callout.TextBox; set => Callout.TextBox = value; }

        /// <summary>
        /// Gets or sets the type of connection at the target point for this callout.
        /// </summary>
        public swCalloutTargetStyle_e TargetStyle { get => (swCalloutTargetStyle_e)Callout.TargetStyle; set => Callout.TargetStyle = (int)value; }

        /// <summary>
        /// Gets and sets whether to add a colon at the end of the callout label.
        /// </summary>
        public bool SkipColon { get => Callout.SkipColon; set => Callout.SkipColon = value; }

        /// <summary>
        /// Gets and sets the position of this callout.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the opaque (background) color for the labels for this callout.
        /// </summary>
        /// <remarks>You must use a system color; you cannot use any other RGB values. To see system colors, click <strong>Tools > Options > Colors.</strong>  in the SOLIDWORKS user interface</remarks>
        public SysColor OpaqueColor { get; set; }

        /// <summary>
        /// Gets or sets the display of multiple leaders for this callout.  
        /// </summary>
        public bool MultipleLeaders { get; set; }
        
        /// <summary>
        /// int is the rowId and string is the value of that row
        /// </summary>
        /// <value>True to use updated text in RowID, false to use original text in RowID</value>
        public Func<int, string, bool> OnValueChanged { get; set; }
        #endregion

    }
}

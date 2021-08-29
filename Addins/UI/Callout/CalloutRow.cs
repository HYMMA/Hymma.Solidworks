using SolidWorks.Interop.sldworks;
using Hymma.Mathematics;
using Hymma.SolidTools.Core;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a row in a <see cref="CalloutModel"/>
    /// </summary>
    public class CalloutRow
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="value"> the value of the row</param>
        /// <param name="label"> assign a label for this row</param>
        public CalloutRow( string value, string label = "")
        {
            Value = value;
            Label = label;
            ValueInactive = false;
            Id = Counter.GetNextCalloutRowId();
        }

        /// <summary>
        /// <see cref="CalloutModel"/> that this row belongs to
        /// </summary>
        internal ICallout Callout { get; set; }

        /// <summary>
        /// Gets or sets the value in for the specified row in this callout.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the text for the label in the specified row of this callout1
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// id of this row in the callout
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the color of the text in the specified row in this callout.
        /// </summary>
        public SysColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets whether the user can edit the value in the specified row in this callout. 
        /// </summary>
        public bool ValueInactive { get; set; }

        /// <summary>
        /// sets or gets the target <see cref="Point"/> for this row
        /// </summary>
        public Point Target
        {
            get
            {
                //cannot run this if callout is not set
                if (Callout == null )
                    return new Point(0, 0, 0);

                //update the Callout
                Callout.GetTargetPoint(Id, out double x, out double y, out double z);
                return new Point(x, y, z);
            }
            set
            {
                //cannot set this if callout is not set yet
                if (Callout == null )
                    return;
                Callout.SetTargetPoint(Id, value.X, value.Y, value.Z);
            }
        }
    }
}

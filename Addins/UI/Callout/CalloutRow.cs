// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
//using Hymma.Solidworks.Extensions;
using System;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a row in a <see cref="CalloutModel"/>
    /// </summary>
    public class CalloutRow
    {
        private string _rowVal;
        private string _rowLbl;
        private SysColor _rolColor;
        private bool _rowIsInactive;
        private bool _ignored;
        private Tuple<double, double, double> _target;
        private readonly WeakEventSource<CalloutRowTargetChangedEventArgs> targetChangedSource = new WeakEventSource<CalloutRowTargetChangedEventArgs>();
        private readonly WeakEventSource<string> valueChangedSource = new WeakEventSource<string>();
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="value"> the value of the row</param>
        /// <param name="label"> assign a label for this row</param>
        public CalloutRow(string label, string value)
        {
            Value = value;
            Label = label;
            ValueInactive = false;
        }

        /// <summary>
        /// <see cref="CalloutModel"/> that this row belongs to
        /// </summary>
        internal ICallout Callout { get; set; }

        /// <summary>
        /// Gets or sets the value in for the specified row in this callout.
        /// </summary>
        public string Value
        {
            get => _rowVal;
            set => ValueChanged(value);
        }

        /// <summary>
        /// Gets or sets the text for the label in the specified row of this callout1
        /// </summary>
        public string Label
        {
            get
            {
                return _rowLbl;
            }
            set
            {
                _rowLbl = value;
                if (Callout != null)
                    Callout.Label2[Id] = value;
            }
        }

        /// <summary>
        /// id of this row in the callout
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Gets or sets the color of the text in the specified row in this callout.
        /// </summary>
        public SysColor TextColor
        {
            get
            {
                return _rolColor;
            }
            set
            {
                _rolColor = value;
                if (Callout != null)
                    Callout.TextColor[Id] = (int)value;

            }
        }

        /// <summary>
        /// Gets or sets whether the user can edit the value in the specified row in this callout. 
        /// </summary>
        public bool ValueInactive
        {
            get
            {
                return _rowIsInactive && Callout != null;
            }
            set
            {
                _rowIsInactive = value;
                if (Callout != null)
                    Callout.ValueInactive[Id] = value;
            }
        }

        /// <summary>
        /// sets or gets the target for this row
        /// </summary>
        /// <remarks>soldiworks internal unit is meter</remarks>
        public Tuple<double, double, double> Target
        {
            get => _target;
            set
            {
                _target = value;
                targetChangedSource?.Raise(this, new CalloutRowTargetChangedEventArgs(Id, _target));
            }
        }

        /// <summary>
        /// Gets or sets whether to ignore the callout value in the given row.
        /// </summary>
        /// Use this API to remove the white space that remains in the callout when <see cref="Value"/> is set to an empty string.<br/>
        ///This property applies only to a callout that is independent of a selection 
        public bool IgnoreValue
        {
            get
            {
                return _ignored && Callout != null;
            }
            set
            {
                _ignored = value;
                if (Callout != null)
                    Callout.IgnoreValue[Id] = value;
            }
        }

        //this will get called by solidowrks when user changes the value
        private void ValueChanged(string text)
        {
            //update the field that holds the text 
            _rowVal = text;

            //implemet event subscribers instruction otherwise
            valueChangedSource?.Raise(this, text);
        }

        /// <summary>
        /// fired when <see cref="Target"/> of this row changes
        /// </summary>
        public event EventHandler<CalloutRowTargetChangedEventArgs> OnTargetChanged
        {
            add { targetChangedSource.Subscribe(this, value); }
            remove { targetChangedSource.Unsubscribe(value); }
        }

        /// <summary>
        /// invoked by solidworks when user changes the value of this row
        /// </summary>
        /// <remarks>true to update the callout and false to not. this event is useful for input validation</remarks>
        public event EventHandler<string> OnValueChanged
        {
            add { valueChangedSource.Subscribe(this, value); }
            remove { valueChangedSource.Unsubscribe(value); }
        }
    }

    /// <summary>
    /// evnet handler for when a row's value is changed by the user
    /// </summary>
    /// <param name="sender">is the callout row</param>
    /// <param name="newValue">new value</param>
    /// <returns>true to update the callout and false to not</returns>
    public delegate void CalloutEventHandler(CalloutRow sender, string newValue);

    /// <summary>
    /// event args for when a row's target is changed
    /// </summary>
    public class CalloutRowTargetChangedEventArgs : EventArgs
    {
        /// <summary>
        /// id of the row that its target changed
        /// </summary>
        public int RowId { get; }
        /// <summary>
        /// new target
        /// </summary>
        public Tuple<double, double, double> NewTarget { get; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="rowId">id of the row that its target changed</param>
        /// <param name="newTarget">new target</param>
        public CalloutRowTargetChangedEventArgs(int rowId, Tuple<double, double, double> newTarget)
        {
            RowId = rowId;
            NewTarget = newTarget;
        }
    }

}

// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Linq;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a class to represent a text box inside a property manager page in solidworks
    /// </summary>
    public class PmpTextBox : PmpTextBase<PropertyManagerPageTextbox>
    {
        #region fields

        private TexTBoxStyles _style;
        private short _height;
        #endregion

        #region constructor

        /// <summary>
        /// make a text box for a property manager page in soldiworks
        /// </summary>
        /// <param name="initialValue">initial value for this text box once generated in a property manager page</param>
        /// <param name="tip">a tip for this text box</param>
        public PmpTextBox(string initialValue = "",string tip="") : base(swPropertyManagerPageControlType_e.swControlType_Textbox,tip:tip)
        {
            Value = initialValue;
        }
        #endregion

        #region properties

        /// <summary>
        /// value for this text box
        /// </summary>
        public string Value
        {
            get => SolidworksObject?.Text;
            set
            {
                //if add in is loaded
                if (SolidworksObject != null)
                {
                    SolidworksObject.Text = value;
                }
                else
                {
                    Registering += (s,e) => { SolidworksObject.Text = value; };
                }
            }
        }
        
        /// <summary>
        /// Styles as defined by bitmask <see cref="TexTBoxStyles"/>
        /// </summary>
        public TexTBoxStyles Style
        {
            get => _style;
            set
            {
                _style = value;

                //if add in is loaded
                if (SolidworksObject != null)
                    SolidworksObject.Style = (int)value;
                else
                    Registering += (s,e) => { SolidworksObject.Style = (int)value; };
            }
        }

        /// <summary>
        /// set the height of this control
        /// </summary>
        public short Height
        {
            get => _height;
            set
            {
                _height = value;

                //if addin is loaded
                if (SolidworksObject != null)
                    SolidworksObject.Height = value;
                else
                    Registering += (s,e) => { SolidworksObject.Height = value; };
            }
        }
        #endregion

        #region call backs

        internal void TypedIntoCallback(string e)
        {
            _editEventSource?.Raise(this, e);
        }

        #endregion

        #region events
        readonly WeakEventSource<string> _editEventSource = new WeakEventSource<string>();
        /// <summary>
        /// unsubscribe from events
        /// </summary>
        public override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _editEventSource.ClearHandlers();
            //TypedInto?.GetInvocationList()?.ToList()?.ForEach(d => TypedInto -= (EventHandler<string>)d);
        }
        /// <summary>
        /// fires when text box is changed
        /// </summary>
        ///<remarks>works only when defined before <see cref="PmpGroup.AddControl(IPmpControl)"/></remarks>
        public event EventHandler<string> TypedInto { 
            add { _editEventSource.Subscribe(this,value); } 
            remove { _editEventSource.Unsubscribe(value); }
        }
        #endregion
    }
}

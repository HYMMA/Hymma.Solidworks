// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Core;
using SolidWorks.Interop.swconst;
using System;
using System.Linq;
using System.Web.Compilation;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// represents a base class for all buttons in a property manager page 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PmpButtonBase<T> : PmpControl<T>
    {
        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="caption"></param>
        /// <param name="tip"></param>
        public PmpButtonBase(swPropertyManagerPageControlType_e type, string caption, string tip) : base(type, caption, tip)
        {
        }
        #endregion

        #region call backs
        //internal void ClickedCallback() => Clicked?.Raise(this, EventArgs.Empty);
        internal void ClickedCallback() => _clickedEvents.Raise(this, EventArgs.Empty);
        #endregion

        #region events
        private readonly WeakEventSource<EventArgs> _clickedEvents = new WeakEventSource<EventArgs>();

        /// <summary>
        /// invoked when this button is clicked
        /// </summary>
        public event EventHandler<EventArgs> Clicked
        {
            add { _clickedEvents.Subscribe(this,value); }
            remove { _clickedEvents.Unsubscribe(value); }
        }

        /// <summary>
        /// Unsubscribe from all events 
        /// </summary>
        public override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _clickedEvents.ClearHandlers();
            //_clickedEvents.ClearHandlers();
        }

        #endregion
    }
}

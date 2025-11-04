// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WeakEvent 
{
    internal sealed class WeakEventSource<TEventArgs>
    {
        private List<EventHandler<TEventArgs>> _handlers = new List<EventHandler<TEventArgs>>();

        // Keep the delegates alive with their handler. This prevent anonymous delegates from being garbage collected prematurely.

        //the key is the handler and the values are the targets (classes where the handler is calling their methods)
        //private readonly ConditionalWeakTable<object, List<object>> _delegateKeepAlive = new ConditionalWeakTable<object, List<object>>();

      
        public void Subscribe(object lifeTimeObject,EventHandler<TEventArgs> handler)
        {
            if (handler == null)
                return;

            //var weakReference = new EventHandler<TEventArgs>>(handler);
            if (!_handlers.Contains(handler))
                _handlers.Add(handler);

            //if (handler.Target != null)
            //{
            //    //_delegateKeepAlive.GetOrCreateValue(handler.Target).Add(handler);
            //}
        }

        public void ClearHandlers()
        {

            //foreach (var handler in _handlers)
            //{
            //    if (handler.Target != null)
            //    {
            //        _delegateKeepAlive.Remove(handler.Target);
            //    }
            //}
            if (_handlers != null)
            {
                _handlers.Clear();
                _handlers = null;
            }
        }
        public void Unsubscribe(EventHandler<TEventArgs> handler)
        {
            if (handler == null)
                return;

            // Remove the handler and all handlers that have been garbage collected
            //_handlers = _handlers.RemoveAll(wr => !wr.TryGetTarget(out var target) || handler.Equals(target));
            _handlers.Remove(handler);
            //if (handler.Target != null && _delegateKeepAlive.TryGetValue(handler.Target, out var weakReference))
            //{
            //    weakReference.Remove(handler);
            //}
        }

        public void Raise(object sender, TEventArgs args)
        {
            if (sender is null)
                return;

            foreach (var handler in _handlers)
            {
                if (handler != null)
                {
                    handler.Invoke(sender, args);
                }
                else
                {
                    // Remove the listener if the handler has been garbage collected
                    _handlers.Remove(handler);
                }
            }
        }

        internal bool HasHandlers()
        {
            return _handlers.Count > 0 ;
        }
    }
}

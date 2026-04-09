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
    /// A text box control for SolidWorks property manager pages.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Text boxes allow users to enter and edit text values. They can be styled as single-line
    /// or multi-line, and support various formatting options.
    /// </para>
    /// <para>
    /// Use the <see cref="TypedInto"/> event to respond to text changes in real-time.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>Creating a text box with initial value:</para>
    /// <code>
    /// var textBox = new PmpTextBox("Enter your text here", "Tooltip help text");
    /// textBox.TypedInto += (sender, newText) =>
    /// {
    ///     Console.WriteLine($"User typed: {newText}");
    /// };
    ///
    /// // Add to a group
    /// group.AddControls(new List&lt;IPmpControl&gt; { textBox });
    /// </code>
    /// </example>
    /// <example>
    /// <para>Creating a multi-line text box:</para>
    /// <code>
    /// var multiLineTextBox = new PmpTextBox("Default text")
    /// {
    ///     Style = TexTBoxStyles.Multiline,
    ///     Height = 100
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="PmpGroup"/>
    /// <seealso cref="TexTBoxStyles"/>
    public class PmpTextBox : PmpTextBase<PropertyManagerPageTextbox>
    {
        #region fields

        private TexTBoxStyles _style;
        private short _height;
        #endregion

        #region constructor

        /// <summary>
        /// Creates a new text box control for a property manager page.
        /// </summary>
        /// <param name="initialValue">The initial text displayed in the text box. Defaults to empty string.</param>
        /// <param name="tip">Tooltip text displayed when hovering over the control.</param>
        /// <example>
        /// <code>
        /// // Simple text box
        /// var nameBox = new PmpTextBox("John Doe", "Enter your name");
        ///
        /// // Text box with event handler
        /// var urlBox = new PmpTextBox("https://", "Enter URL");
        /// urlBox.TypedInto += (s, text) => ValidateUrl(text);
        /// </code>
        /// </example>
        public PmpTextBox(string initialValue = "", string tip = "") : base(swPropertyManagerPageControlType_e.swControlType_Textbox, tip: tip)
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
        readonly EventSource<string> _editEventSource = new EventSource<string>();
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
        /// Occurs when the user types into or modifies the text box content.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This event fires each time the text changes, allowing real-time validation
        /// or processing of user input.
        /// </para>
        /// <para>
        /// <b>Important:</b> Subscribe to this event before adding the control to a group
        /// via <see cref="PmpGroup.AddControls"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var textBox = new PmpTextBox("Enter value");
        /// textBox.TypedInto += (sender, newText) =>
        /// {
        ///     // Validate input
        ///     if (string.IsNullOrEmpty(newText))
        ///     {
        ///         ShowWarning("Value cannot be empty");
        ///     }
        /// };
        /// </code>
        /// </example>
        public event EventHandler<string> TypedInto
        {
            add { _editEventSource.Subscribe(this, value); }
            remove { _editEventSource.Unsubscribe(value); }
        }
        #endregion
    }
}

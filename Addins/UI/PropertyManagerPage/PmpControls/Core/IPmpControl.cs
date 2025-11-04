// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Core;
using Hymma.Solidworks.Addins.Utilities.DotNet;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page controllers
    /// </summary>
    public abstract class IPmpControl
    {
        private short _top;
        private bool _enabled = true;
        private bool _visible = true;
        private short _width;
        private short _left;
        private ControlResizeStyles _optionForResize;

        #region constructor

        internal IPmpControl(swPropertyManagerPageControlType_e type, string caption, string tip)
        {
            //Id = (short)AddinConstants.GetNextPmpId();
            Type = type;
            Caption = caption;
            Tip = tip;
        }
        #endregion

        #region properties
        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// this where the icon for this controller will be saved
        /// </summary>
        public DirectoryInfo SharedIconsDir { get; set; }

        /// <summary>
        /// toolTip (hint) for this controller
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// property manager page control as an object
        /// </summary>
        protected PropertyManagerPageControl Control { get; set; }

        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        public swPropertyManagerPageControlType_e Type { get; internal set; }

        /// <summary>
        /// id of this controller which is used by SOLIDWORKS to identify it
        /// </summary>
        public short Id { get; internal set; }

        /// <summary>
        /// Left alignment of this control as defined in <see cref="swPropertyManagerPageControlLeftAlign_e"/>
        /// </summary>
        /// <remarks>this property will be used when the page is displayed or while it is closed</remarks>
        private short LeftAlignment { get; set; } = (short)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value corresponds to a visible and enabled control
        /// </summary>
        private swAddControlOptions_e Options { get; set; } = swAddControlOptions_e.swControlOptions_Enabled | swAddControlOptions_e.swControlOptions_Visible;

        /// <summary>
        /// the solidworks document where the property manager page is displayed in. you can use this property before the property manager page is displayed
        /// </summary>
        public ModelDoc2 ActiveDoc { get; internal set; }

        /// <summary>
        /// Gets or sets the top edge of the control on a PropertyManager page
        /// </summary>
        /// <remarks>returns -1 on error, <strong>MUST USE NUMERICAL VALUE TO SEE THE EFFECT</strong></remarks>
        public short Top
        {
            get => _top;
            set
            {
                _top = value;
                if (Control != null)
                    Control.Top = value;
                else
                    Registering += (s, e) => { Control.Top = (short)(value ); };
            }
        }

        /// <summary>
        /// enables or disables this property control on
        /// </summary>
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                    Options |= swAddControlOptions_e.swControlOptions_Enabled;
                else
                    Options &= ~swAddControlOptions_e.swControlOptions_Enabled;
                if (Control != null)
                    Control.Enabled = value;
                else
                    Registering += (s, e) => { Control.Enabled = value; };
            }
        }

        /// <summary>
        /// gets or sets the visibility of this control
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (_visible)
                    Options |= swAddControlOptions_e.swControlOptions_Visible;
                else
                    Options &= ~swAddControlOptions_e.swControlOptions_Visible;

                if (Control != null)
                    Control.Visible = value;
                else
                    Registering += (s, e) => { Control.Visible = value; };
            }
        }


        /// <summary>
        /// Gets or sets how to override the SOLIDWORKS default behavior when changing the width of a PropertyManager page. <br/>
        /// Resize the PropertyManager page as defined in <see cref="ControlResizeStyles"/>
        /// you can use ths property only before the control is displayed or while it is closed
        /// </summary>
        public ControlResizeStyles ResizeStyles
        {
            get => _optionForResize;
            set
            {
                _optionForResize = value;
                if (Control != null)
                    Control.OptionsForResize = ((int)value);
                else
                    Registering += (s, e) => { Control.OptionsForResize = ((int)value); };
            }
        }

        /// <summary>
        /// Left edge of the control <br/>
        /// Use this property and <see cref="Top"/> to place controls side by side<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        /// <remarks>By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        public short Left
        {
            get => _left;
            set
            {
                _left = value;
                if (Control != null)
                    Control.Left = value;
                else
                    Registering += (s, e) => { Control.Left =(value) ; };
            }
        }

        /// <summary>
        /// By default, the width of the control is usually set so that it extends to the right edge of its group box (not for buttons). Using this API overrides that default.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The width of the group box is 100
        /// </summary>
        public short Width
        {
            get => _width;
            set
            {
                _width = value;
                if (Control != null)
                    Control.Width = value;
                else
                    Registering += (s, e) => { Control.Width = value; };
            }
        }
        #endregion

        #region methods

        /// <summary>
        /// Add this control to a group in a property manager page 
        /// </summary>
        /// <param name="group">the group that this control should be registered to</param>
        internal void Register(IPropertyManagerPageGroup group)
        {
            Control = group.AddControl2(Id, (short)Type, Caption, LeftAlignment, ((int)Options), Tip) as PropertyManagerPageControl;
            _top = Control.Top;
            _width = Control.Width;
            _left = Control.Left;
            _visible = Control.Visible;
            _enabled = Control.Enabled;
            //we raise this event here to give multiple controls set-up their initial state. some of the properties of a controller has to be set prior a property manager page is displayed or after it's closed
            RegisteringCallback();
            //Registering?.Raise();

            //the registering list of delegates happens once only ( when user loads the addin)
            // we don't need to keep these in memory
            _registeringEventSource?.ClearHandlers();
            //var list = Registering?.GetInvocationList();
            //if (list != null)
            //{
            //    for (int i = 0; i < list.Length; i++)
            //    {
            //        var action = list[i] as Action;
            //        Registering -= action;
            //    }
            //}

        }

        /// <summary>
        /// Sets the bitmap label for this control that appears next to it on the left hand side.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <remarks>
        /// You can only use this method on a PropertyManager page before the page is displayed, while it is displayed, or when it is closed. <br/>
        /// The image will be resized to 18 x 18
        /// </remarks>
        public void SetPictureLabel(Bitmap bitmap, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            if (Control != null)
                SetPictureLabelForControl(bitmap, fileName);
            else
                Registering += (s, e) => { SetPictureLabelForControl(bitmap, fileName); };

        }

        private void SetPictureLabelForControl(Bitmap bitmap, string fileName)
        {
            using (bitmap)
            {
                var fullFileName = Path.Combine(SharedIconsDir.CreateSubdirectory(Id.ToString()).FullName, fileName);
                MaskedBitmap.SaveAsPng(bitmap, new Size(18, 18), ref fullFileName);
                Control.SetPictureLabelByName(fullFileName, "");
            }
        }

        /// <summary>
        /// Displays a bubble ToolTip containing the specified title, message, and bitmap. A bubble ToolTip is useful for validating data typed or selected by users in controls on a PropertyManager page.
        /// </summary>
        /// <param name="title">Title to display in bubble ToolTip</param>
        /// <param name="message">Message to display in bubble ToolTip</param>
        /// <param name="bitmap">bitmap object to use as icon in the tooltip</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        public void ShowBubbleTooltip(string title, string message, Bitmap bitmap, string fileName)
        {
            if (Control != null)
                ShowBubbleTooltipForControl(title, message, bitmap, fileName);
            else
                Registering += (s, e) => { ShowBubbleTooltipForControl(title, message, bitmap, fileName); };
        }

        private void ShowBubbleTooltipForControl(string title, string message, Bitmap bitmap, string fileName)
        {
            if (bitmap == null)
            {
                Control.ShowBubbleTooltip(title, message, "");
                return;
            }
            var fullFileName = Path.Combine(SharedIconsDir.CreateSubdirectory(Id.ToString()).FullName, fileName);
            MaskedBitmap.SaveAsPng(bitmap, new Size(18, 18), ref fullFileName);
            Control.ShowBubbleTooltip(title, message, fullFileName);
        }
        #endregion

        #region call backs
        /// <summary>
        /// will be called just before this property manager page is displayed inside solidworks 
        /// </summary>
        ///<inheritdoc/>
        internal virtual void RegisteringCallback()
        {
            _registeringEventSource?.Raise(this, EventArgs.Empty);

            //we need it once
            _registeringEventSource.ClearHandlers();
        }
        internal virtual void DisplayingCallback()
        {
            _displayingEventSource?.Raise(this, new PmpControlDisplayingEventArgs(Control));
        }

        internal virtual void GainedFocusCallback()
        {
            //GainedFocus?.Raise(this, EventArgs.Empty);
            _gainedFocusEventSource?.Raise(this, EventArgs.Empty);
        }

        internal virtual void LostFocusCallback()
        {
            //LostFocus?.Raise(this, EventArgs.Empty);
            _lostFocusEventSource.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unsubscribes all event handlers from this control events
        /// </summary>
        public virtual void UnsubscribeFromEvents()
        {
            _displayingEventSource?.ClearHandlers();
            _gainedFocusEventSource?.ClearHandlers();
            _lostFocusEventSource?.ClearHandlers();
            _registeringEventSource?.ClearHandlers();
            //Displaying?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    Displaying -= (d as PmpControlDisplayingEventHandler);
            //});
            //LostFocus?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    LostFocus -= (d as EventHandler);
            //});
            //GainedFocus?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    GainedFocus -= (d as EventHandler);
            //});
            //Registering?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    Registering -= (d as Action);
            //});
        }


        #endregion

        #region events
        private readonly WeakEventSource<PmpControlDisplayingEventArgs> _displayingEventSource = new WeakEventSource<PmpControlDisplayingEventArgs>();

        private readonly WeakEventSource<EventArgs> _gainedFocusEventSource = new WeakEventSource<EventArgs>();
        private readonly WeakEventSource<EventArgs> _lostFocusEventSource = new WeakEventSource<EventArgs>();
        private readonly WeakEventSource<EventArgs> _registeringEventSource = new WeakEventSource<EventArgs>();

        /// <summary>
        /// fired when this controller is registered in a property manager page which is when the add-in is loaded. Either when solidworks starts or when user re-loads the addin
        /// </summary>
        internal event EventHandler<EventArgs> Registering
        {
            add { _registeringEventSource.Subscribe(this,value); }
            remove { _registeringEventSource.Unsubscribe(value); }
        }

        /// <summary>
        /// fired a moment before property manager page is displayed
        /// </summary>
        public event EventHandler<PmpControlDisplayingEventArgs> Displaying
        {
            add { _displayingEventSource.Subscribe(this,value); }
            remove { _displayingEventSource.Unsubscribe(value); }
        }

        /// <summary>
        /// fired when user starts interacting with this control, such as start of typing in a text box
        /// </summary>
        public event EventHandler<EventArgs> GainedFocus
        {
            add { _gainedFocusEventSource.Subscribe(this,value); }
            remove { _gainedFocusEventSource.Unsubscribe(value); }
        }

        /// <summary>
        /// fires when user browses away from this control
        /// </summary>
        public event EventHandler<EventArgs> LostFocus
        {
            add { _lostFocusEventSource.Subscribe(this,value); }
            remove { _lostFocusEventSource.Unsubscribe(value); }
        }
        #endregion
    }
}
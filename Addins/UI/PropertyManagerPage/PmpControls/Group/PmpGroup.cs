// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

//using Hymma.Solidworks.Extensions;
using Hymma.Solidworks.Addins.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page groups
    /// </summary>
    public class PmpGroup : IWrapSolidworksObject<IPropertyManagerPageGroup>
    {
        #region fields
        private SysColor _backgroundColor;
        private bool _visible;
        private string _caption;

        /// <summary>
        /// this is used at registering stage only
        /// </summary>
        protected swAddGroupBoxOptions_e _options;
        private bool _isExpanded;
        #endregion

        #region constructors
        /// <summary>
        /// construct a property manage page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="expanded">if set true  group box will appear expanded by default</param>
        /// <param name="visible">if set to false group box will be hidden by default</param>
        public PmpGroup(string caption = "Group", bool expanded = true, bool visible = true)
        {
            //assign properties
            //Id = AddinConstants.GetNextPmpId();
            _caption = caption;
            _backgroundColor = SysColor.PropertyManagerColor;
            IsExpanded = expanded;
            Visible = visible;
            Controls = new List<IPmpControl>();
            Expanded += PmpGroup_Expanded;
        }

        private void PmpGroup_Expanded(object sender, bool e)
        {
            if (_isExpanded != e)
                _isExpanded = e;
        }

        /// <summary>
        /// construct a property manager page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="Caption">text that appears next to a group box</param>
        /// <param name="Controls">list of controls to add to this group</param>
        /// <param name="expanded">if set true  group box will appear expanded by default</param>
        /// <param name="visible">if set to false group box will be hidden by default</param>
        public PmpGroup(string Caption, List<IPmpControl> Controls, bool expanded = true, bool visible = true) : this(Caption, expanded, visible)
        {
            this.Controls = Controls;
        }

        #endregion

        #region Properties
        /// <summary>
        /// identifier for this group box in Property manager page
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///  Gets or sets the group box visibility state. 
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (_visible)
                    _options |= swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;
                else
                    _options &= ~swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;
                if (SolidworksObject != null)
                    SolidworksObject.Visible = _visible;
                else
                    Registering += (s, e) => SolidworksObject.Visible = _visible;
            }
        }

        /// <summary>
        /// caption
        /// </summary>
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                if (SolidworksObject != null)
                    SolidworksObject.Caption = _caption;
                else
                    Registering += (s, e) => SolidworksObject.Caption = _caption;
            }
        }

        /// <summary>
        /// determines the expand state of this group box 
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                if (_isExpanded)
                    _options |= (swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded);
                else
                    _options &= ~swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded;
                if (SolidworksObject != null)
                {
                    SolidworksObject.Expanded = _isExpanded;
                }
                else
                {
                    Registering += (s, e) => SolidworksObject.Expanded = _isExpanded;
                }
            }
        }


        /// <summary>
        /// a list of solidworks controllers
        /// </summary>
        public List<IPmpControl> Controls { get; internal set; }

        ///<inheritdoc/>
        public IPropertyManagerPageGroup SolidworksObject { get; private set; }
        #endregion

        #region methods
        /// <summary>
        /// Registers a control to the <see cref="Controls"/> and solidworks UI
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(IPmpControl control)
        {
            Controls.Add(control);
        }

        /// <summary>
        /// adds a list of controls to the <see cref="Controls"/>
        /// </summary>
        /// <param name="controls"></param>
        public void AddControls(IEnumerable<IPmpControl> controls)
        {
            Controls.AddRange(controls);
        }

        /// <summary>
        ///  Gets or sets the background color of this PropertyManager group box. 
        /// </summary>
        public SysColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                if (SolidworksObject != null)
                    SolidworksObject.BackgroundColor = (int)value;
                else
                    Registering += (s, e) => SolidworksObject.BackgroundColor = ((int)value);
            }
        }


        private void RegisterControls()
        {
            Controls.ForEach(c => c.Register(SolidworksObject));

            //a special rule should apply for options (radio buttons) as they require a style value of 1 to indicate the beginning of a group of options
            //any following option without this value set are considered part of the group; the next option with this value set indicates the start of a new option group
            //we assume all the radio buttons in a PMPGroup are members of a group so we assign a value of 1 to the first one
            //get all radio buttons ...
            var groupOptions = Controls.Where(c => c.Type == swPropertyManagerPageControlType_e.swControlType_Option).Cast<PmpRadioButton>();

            if (groupOptions.Count() > 0)
            {
                groupOptions.ElementAt(0).SolidworksObject.Style = 1;

                //if the checked radio button should maintain its state 
                //across different sessions of the property manager page
                //the rest of the radio buttons should do the same which means the rest of them
                //stay un-checked in the next run
                if (groupOptions.Any(c => c.MaintainState))
                {
                    for (int j = 0; j < groupOptions.Count(); j++)
                    {
                        groupOptions.ElementAt(j).MaintainState = true;
                    }
                }
            }
        }

        private void DisposeRegisterEventHanders()
        {
            //the registering list of delegates happens once only ( when user loads the addin)
            // we don't need to keep these in memory
            _regesteringEvents.ClearHandlers();
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
        /// registers this group into a property manager page
        /// </summary>
        /// <param name="propertyManagerPage"></param>
        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            SolidworksObject = (IPropertyManagerPageGroup)propertyManagerPage.AddGroupBox(Id, Caption, ((int)_options));
            RegisterControls();
            _regesteringEvents?.Raise(this, EventArgs.Empty);
            DisposeRegisterEventHanders();
        }

        /// <summary>
        /// registers this group into a property manager page Tab
        /// </summary>
        /// <param name="propertyManagerPageTab"></param>
        internal void Register(IPropertyManagerPageTab propertyManagerPageTab)
        {
            SolidworksObject = (IPropertyManagerPageGroup)propertyManagerPageTab.AddGroupBox(Id, Caption, ((int)_options));
            RegisterControls();
            _regesteringEvents?.Raise(this, EventArgs.Empty);
            DisposeRegisterEventHanders();
        }

        #endregion

        #region events
        WeakEventSource<EventArgs> _regesteringEvents = new WeakEventSource<EventArgs>();
        WeakEventSource<EventArgs> _displayingEvents = new WeakEventSource<EventArgs>();
        WeakEventSource<bool> _expandedEvents = new WeakEventSource<bool>();
        /// <summary>
        /// invoked once this group is registers into solidworks
        /// </summary>
        public event EventHandler<EventArgs> Registering
        {
            add { _regesteringEvents.Subscribe(this, value); }
            remove { _regesteringEvents.Unsubscribe(value); }
        }

        /// <summary>
        /// an event that gets called right before this pmpGroup is displayed
        /// </summary>
        public event EventHandler<EventArgs> Displaying
        {
            add { _displayingEvents.Subscribe(this, value); }
            remove { _displayingEvents.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user expands a group <br/>
        /// this delegate requires a bool variable to indicate the IsExpanded status of the group
        /// </summary>
        public event EventHandler<bool> Expanded
        {
            add { _expandedEvents.Subscribe(this, value); }
            remove { _expandedEvents.Unsubscribe(value); }
        }


        #endregion

        #region call backs
        internal void GroupExpand(bool e)
        {
            _expandedEvents.Raise(this, e);
        }

        /// <summary>
        /// Unsusbcribe all event handlers from events
        /// </summary>
        public virtual void UnsubscribeFromEvents()
        {
            _displayingEvents.ClearHandlers();
            _expandedEvents.ClearHandlers();
            _regesteringEvents.ClearHandlers();
            if (Controls != null)
            {
                foreach (var item in Controls)
                {
                    item.UnsubscribeFromEvents();
                }
            }
            //Displaying?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    Displaying -= (d as EventHandler);
            //});
            //Expanded?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    Expanded -= (d as EventHandler<bool>);
            //});
            //Registering?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    Registering -= (d as Action);
            //});
        }
        internal void Display()
        {
            //invoke displaying for all controls inside this group
            Controls.ForEach(c => c.DisplayingCallback());


            //invoke displaying for this group
            _displayingEvents?.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// reease solidowrks object
        /// </summary>
        public void ReleaseSolidworksObject()
        {
            Marshal.ReleaseComObject(SolidworksObject);
            if (Controls != null)
            {
                foreach (var item in Controls)
                {
                    if (item is IReleaseSolidworksObject p)
                    {
                        p.ReleaseSolidworksObject();
                    }
                    if (item is IDisposable d)
                    {
                        d.Dispose();
                    }
                }
            }
            Controls = null;
        }
        #endregion
    }
}
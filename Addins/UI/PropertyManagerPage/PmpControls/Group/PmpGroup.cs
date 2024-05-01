﻿// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

//using Hymma.Solidworks.Extensions;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Id = Counter.GetNextPmpId();
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
                    OnRegister += () => SolidworksObject.Visible = _visible;
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
                    OnRegister += () => SolidworksObject.Caption = _caption;
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
                    OnRegister += () => SolidworksObject.Expanded = _isExpanded;
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
                    OnRegister += () => SolidworksObject.BackgroundColor = ((int)value);
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

        /// <summary>
        /// registers this group into a property manager page
        /// </summary>
        /// <param name="propertyManagerPage"></param>
        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            SolidworksObject = (IPropertyManagerPageGroup)propertyManagerPage.AddGroupBox(Id, Caption, ((int)_options));
            RegisterControls();
            OnRegister?.Invoke();
        }

        /// <summary>
        /// registers this group into a property manager page Tab
        /// </summary>
        /// <param name="propertyManagerPageTab"></param>
        internal void Register(IPropertyManagerPageTab propertyManagerPageTab)
        {
            SolidworksObject = (IPropertyManagerPageGroup)propertyManagerPageTab.AddGroupBox(Id, Caption, ((int)_options));
            RegisterControls();
            OnRegister?.Invoke();
        }

        #endregion

        #region events
        /// <summary>
        /// invoked once this group is registers into solidworks
        /// </summary>
        public Action OnRegister { get; set; }

        /// <summary>
        /// an event that gets called right before this pmpGroup is displayed
        /// </summary>
        public event EventHandler Displaying;

        /// <summary>
        /// method to invoke when user expands a group <br/>
        /// this delegate requires a bool variable to indicate the IsExpanded status of the group
        /// </summary>
        public event EventHandler<bool> Expanded;


        #endregion

        #region call backs
        internal void GroupExpand(bool e)
        {
            Expanded?.Invoke(this, e);
        }
        internal void Display()
        {
            Controls.ForEach(c => c.DisplayingCallback());
            Displaying?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
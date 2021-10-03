﻿using Hymma.SolidTools.Addins;
using System;
using System.Drawing;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// tab for property manager page
    /// </summary>
    public class PmpTabFluent : PmpTab, IPmpTabFluent
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="icon"></param>
        public PmpTabFluent(string caption, Bitmap icon = null) : base(caption, icon)
        {

        }

        internal IPmpUiModelFluent PmpUiModel { get; set; }

        /// <summary>
        /// add a group to this tab
        /// </summary>
        /// <param name="caption"></param>
        /// <remarks>each group can have its controls</remarks>
        public IPmpTabGroupFluent AddGroup(string caption)
        {
            var group = new PmpTabGroupFluent(caption, false)
            {
                Tab = this
            };
            Groups.Add(group);
            return group;
        }

        /// <summary>
        /// saves this tab to property manager page
        /// </summary>
        /// <returns></returns>
        public IPmpUiModelFluent SaveTab()
        {
            return PmpUiModel;
        }

        /// <summary>
        /// provide an <see cref="Action"/> that will be fired immidiately after this tab is displayed
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IPmpTabFluent WhenDisplayed(Action action)
        {
            OnDisplay += action;
            return this;
        }
    }
}

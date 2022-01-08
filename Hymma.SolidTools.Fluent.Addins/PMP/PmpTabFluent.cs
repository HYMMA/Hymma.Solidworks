using Hymma.Solidworks.Addins;
using System;
using System.Drawing;

namespace Hymma.Solidworks.Fluent.Addins
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
            TabGroups.Add(group);
            return group;
        }

        ///<inheritdoc/>
        public IPmpTabFluent OnClick(Action doThis)
        {
            OnPress += doThis;
            return this;
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
        public IPmpTabFluent OnDisplay(Action action)
        {
            base.OnDisplay += action;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable AddCheckableGroup(string caption)
        {
            var group = new PmpTabGroupFluentCheckable(caption, false)
            {
                Tab = this
            };
            TabGroups.Add(group);
            return group;
        }
    }
}

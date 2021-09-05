using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// a group in property manager page that host the <see cref="IPmpControl"/>
    /// </summary>
    public class PMP_Group : PMPGroup, IPmpGroup, IFluent
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption">caption of thie control inside the pmp</param>
        /// <param name="expanded">exapnsion state of the group upon load</param>
        public PMP_Group(string caption, bool expanded) : base(caption, expanded)
        {

        }

        /// <inheritdoc/>
        internal IPmpUi PropertyManagerPageUIBase { get; set; }

        /// <inheritdoc/>
        public IPmpGroup That()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroup And()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroup IsExpanded(bool isExpanded = true)
        {
            Expanded = isExpanded;
            return this;
        }

        ///<inheritdoc/>
        public IPmpGroup AndOnExpansionChange(Action<PMPGroup, bool> doThis)
        {
            OnGroupExpand += (sender, e) => { doThis?.Invoke((PMPGroup)sender, e); };
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroup HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker)
        {

            AddControls(controlMaker.Invoke());
            return this;
        }
        /// <inheritdoc/>
        public IPmpGroup HasTheseControls(IEnumerable<IPmpControl> controls)
        {

            AddControls(controls);
            return this;
        }

        /// <inheritdoc/>
        public IPmpUi SaveGroup()
        {
            return PropertyManagerPageUIBase as PmpUi;
        }
    }
}

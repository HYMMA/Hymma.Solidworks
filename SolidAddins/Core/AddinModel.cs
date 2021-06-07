using System.Collections.Generic;
using System.Drawing;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a wrapper class for a typical addin for solidworks
    /// </summary>
    public class AddinModel
    {
        /// <summary>
        /// unique identifier for this addin, gets assigned to by solidworks 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// a list of classes that inherites from <see cref="PmpBase"/>
        /// </summary>
        public IEnumerable<PmpBase> PropertyManagerPages { get; set; }

        /// <summary>
        /// list of command tabs that this addin will add to solidworks
        /// </summary>
        public IEnumerable<AddinCommandTab> CommandTabs { get; set; }
    }
}

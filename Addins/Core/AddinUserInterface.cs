using System;
using System.Collections.Generic;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper class for a typical addin for solidworks
    /// </summary>
    public class AddinUserInterface
    {
        /// <summary>
        /// unique identifier for this addin, gets assigned to by solidworks 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// a list of classes that inherit from <see cref="PmpFactoryBase"/>
        /// </summary>
        public List<PmpFactoryBase> PropertyManagerPages { get; set; } = new List<PmpFactoryBase>();

        /// <summary>
        /// list of command tabs that this addin will add to solidworks
        /// </summary>
        public List<AddinCommandTab> CommandTabs { get; set; } = new List<AddinCommandTab>();

    }
}

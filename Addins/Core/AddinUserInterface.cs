// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Collections.Generic;
using System.IO;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper class for a typical addin for solidworks
    /// </summary>
    public class AddinUserInterface
    {

        /// <summary>
        /// this is the main directory to save various addin icons
        /// </summary>
        ///<remarks></remarks>
        public DirectoryInfo IconsRootDir { get; set; } 

        /// <summary>
        /// unique identifier for this addin, gets assigned to by solidworks 
        /// </summary>
        public int Id { get;internal set; }
        /// <summary>
        /// a list of classes that inherit from <see cref="PropertyManagerPageBase"/>
        /// </summary>
        public List<PropertyManagerPageX64> PropertyManagerPages { get; set; } = new List<PropertyManagerPageX64>();

        /// <summary>
        /// list of command tabs that this addin will add to solidworks
        /// </summary>
        public List<AddinCommandTab> CommandTabs { get; set; } = new List<AddinCommandTab>();

    }
}

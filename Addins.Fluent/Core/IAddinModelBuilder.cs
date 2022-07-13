// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Drawing;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to create and register a property manager page
    /// </summary>
    public interface IAddinModelBuilder : IFluent
    {
        /// <summary>
        /// define a new property manger page. property manager pages open on the left hand side of the window (By default) when you run a command. 
        /// </summary>
        /// <returns></returns>
        IPmpUiModelFluent AddPropertyManagerPage(string title, ISldWorks solidworks);


        /// <summary>
        /// Add a command tab to the solidworks ui for example 'Features' and 'Sketch' are command tabs
        /// </summary>
        /// <returns></returns>
        IFluentCommandTab AddCommandTab();

        /// <summary>
        /// Creates a property manager page tab
        /// </summary>
        /// <returns>the <see cref="IPmpTabFluent"/> that was created</returns>
        IPmpTabFluent CreatePropertyManagerPageTab(string caption, Bitmap icon = null);
    }
}
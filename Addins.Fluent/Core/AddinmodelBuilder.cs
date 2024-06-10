// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Drawing;
using System.IO;

namespace Hymma.Solidworks.Addins.Fluent
{
    ///<inheritdoc/>
    public class AddinModelBuilder : AddinUserInterface, IAddinModelBuilder
    {
        private PmpUiModelFluent pmp;

        ///<inheritdoc/>
        public IFluentCommandTab AddCommandTab()
        {
            return new FluentCommandTab(this);
        }

        /// <summary>
        /// Add a tab to this property manager page
        /// </summary>
        /// <param name="caption">caption for the property manager page tab </param>
        /// <param name="icon">an icon that will be placed on the left side of the caption</param>
        /// <returns>an object of <see cref="PmpTabFluent"/> </returns>
        public IPmpTabFluent CreatePropertyManagerPageTab(string caption, Bitmap icon = null)
        {
            return new PmpTabFluent(caption, icon);
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent AddPropertyManagerPage(string title, ISldWorks solidworks)
        {
            pmp = new PmpUiModelFluent(solidworks,title)
            {
                Title = title,
                AddinModel = this
            };
            return pmp;
        }
        
        /// <summary>
        /// define the parent directory where various command and property manager page icons should be saved into
        /// </summary>
        /// <param name="iconsDir">absolute path to the folder </param>
        /// <returns></returns>
        public AddinModelBuilder WithIconsPath(DirectoryInfo iconsDir)
        {
            IconsRootDir = iconsDir;
            return this;
        }

        /// <summary>
        /// build the user interface object
        /// </summary>
        /// <returns></returns>
        ///<exception cref="DirectoryNotFoundException"></exception>
        public AddinUserInterface Build()
        {
            if (IconsRootDir is null)
            {
                throw new DirectoryNotFoundException("Icons directory is null");
            }
            return this;
        } 
    }
}
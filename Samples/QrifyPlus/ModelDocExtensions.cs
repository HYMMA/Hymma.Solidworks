// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;

namespace QrifyPlus
{
    public static class ModelDocExtensions
    {
        //this is copied from Hymma.solidworks.Extensions package.
        //we did not add that package to this sample addin to demonstrate that Hymma.Solidworks.Addins and Hymma.Solidworks.Addins.Fluent do not depend on the Hymma.solidworks.Extensions anymore
        public static string GetCustomProperty(this ModelDoc2 modelDoc, string property, string configuration = "", bool useCachedData = false)
        {
            ModelDocExtension extension = modelDoc.Extension;
            CustomPropertyManager customPropertyManager = ((IModelDocExtension)extension).get_CustomPropertyManager(configuration);
            int num = customPropertyManager.Get5(property, useCachedData, out string ValOut, out string ResolvedValOut, out bool WasResolved);
            if (num == 1)
            {
                return "";
            }

            return ResolvedValOut;
        }
    }
}

// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// a list of useful functions for an <see cref="AssemblyDoc"/> object
    /// </summary>
    public static class AssemblyDocExtensions
    {
        /// <summary>
        /// get the Qty of part in a parent assembly document
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="thisPart">the part </param>
        /// <param name="configuration">referenced configuration of the part</param>
        /// <returns>Quantity as integer</returns>
        public static int GetPartQty(this AssemblyDoc assembly, PartDoc thisPart, string configuration)
        {
            //get all parts and sub-assemblies and parts inside sub-assemblies
            var components = (object[])assembly.GetComponents(false);
            int counter = 0;
            //iterate through components
            foreach (Component2 component in components)
            {
                if (!(thisPart is ModelDoc2 thisModel)) return 0; //try to cast into modelDoc2
                if (component.GetPathName() == thisModel.GetPathName() // this also ensures that assembly files are filtered out
                    &&
                    !component.ExcludeFromBOM //if excluded from bom don't consider it
                    &&
                    !component.IsEnvelope()  //if is envelop don't consider it
                    &&
                    !component.IsSuppressed() //if suppressed don't consider it
                    &&
                    component.ReferencedConfiguration == configuration) //if configurations don't match don't consider it
                {
                    counter++;
                }
            }
            return counter;
        }

        
        /// <summary>
        /// get a list of parts in an assembly, does not take into account qty or suppression or envelope state
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="swDocumentTypes">documents of this type will be returned</param>
        /// <param name="topLevelOnly">if set to false will return all the components of this assembly and its sub assemblies</param>
        /// <returns></returns>
        public static IEnumerable<Component2> GetDistinctComponentsOfType(this AssemblyDoc assembly, swDocumentTypes_e swDocumentTypes, bool topLevelOnly = false)
        {
            var components = assembly.GetComponentsByType(swDocumentTypes,topLevelOnly);

            //filter out the ones that are similar
            return components.Distinct(new ComponentEqualityComparer()).ToList();
        }

        /// <summary>
        /// selects components of an assembly based on their document type
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="swDocumentTypes">documents of this type will be returned</param>
        /// <param name="topLevelOnly">if set to false will return all the components of this assembly and its sub assemblies</param>
        /// <returns></returns>
        public static IEnumerable<Component2> GetComponentsByType(this AssemblyDoc assembly, swDocumentTypes_e swDocumentTypes, bool topLevelOnly = false)
        {
            var comps = new List<Component2>();
            if (swDocumentTypes != swDocumentTypes_e.swDocASSEMBLY &&
                swDocumentTypes != swDocumentTypes_e.swDocIMPORTED_ASSEMBLY &&
                swDocumentTypes != swDocumentTypes_e.swDocPART &&
                swDocumentTypes != swDocumentTypes_e.swDocIMPORTED_PART)
            {
                throw new System.ArgumentException("document type is not supported", nameof(swDocumentTypes));
            }

            //get all parts and sub-assemblies and parts inside sub-assemblies
            object[] compArray = (object[])assembly.GetComponents(topLevelOnly);

            //if there is no component in the assembly
            if (compArray == null)
                return comps;

            //filter out components that are not required
            foreach (Component2 component in compArray)
            {
                if (component.GetModelDoc2() is ModelDoc2 model && model.GetType() == (int)swDocumentTypes)
                    comps.Add(component);
            }
            return comps;
        }
    }
}

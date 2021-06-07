using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Core
{
    /// <summary>
    /// a list of useful functions for an <see cref="AssemblyDoc"/> object
    /// </summary>
    public static class AssemblyDocExtensions
    {
        /// <summary>
        /// get the Qty of part in a parent assembly document
        /// </summary>
        /// <param name="assembly">the parten assembly where the calc should be done inside of</param>
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
                if (component.GetPathName()== thisModel.GetPathName() // this also ensures that assembly files are filtered out
                    &&
                    !component.ExcludeFromBOM //if excluded from bom dont consider it
                    &&
                    !component.IsEnvelope()  //if is envelop dont consider it
                    && 
                    !component.IsSuppressed() //if suppresed dont consider it
                    &&
                    component.ReferencedConfiguration==configuration) //if configs dont match dont consider it
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
        /// <returns></returns>
        public static IList<Component2> GetDistictParts(this AssemblyDoc assembly)
        {
            var parts = assembly.GetParts();
         
            //filter out the ones that are similar
            return parts.Distinct(new ComponentEqualityComparer()).ToList();
        }

        /// <summary>
        /// get parts in an assembly, if a part is multiplied in assembly will be returned multiple times here too
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IList<Component2> GetParts(this AssemblyDoc assembly)
        {
            //get all parts and sub-assemblies and parts inside sub-assemblies
            object[] components = (object[])assembly.GetComponents(false);
            var parts = new List<Component2>();

            //get Part components only
            foreach (Component2 component in components)
            {
                if (component.GetModelDoc2() is ModelDoc2 model && model.GetType() == (int)swDocumentTypes_e.swDocPART)
                    parts.Add(component);
            }
            return parts;
        }
    }
}

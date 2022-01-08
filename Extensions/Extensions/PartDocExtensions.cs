using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// extension sofr <see cref="PartDoc"/>
    /// </summary>
    public static class PartDocExtensions
    {
        /// <summary>
        /// determines if a part is sheetmetal or not
        /// </summary>
        /// <param name="part"></param>
        /// <param name="bendState">bend state of the part as defined by <see cref="swSMBendState_e"/></param>
        /// <returns></returns>
        public static bool IsSheetMetal(this PartDoc part, out swSMBendState_e bendState)
        {
            ModelDoc2 model = part as ModelDoc2;
            if (model == null)
            {
                bendState = swSMBendState_e.swSMBendStateNone;
                return false;
            }
            bendState = (swSMBendState_e)model.GetBendState();
            if (bendState == swSMBendState_e.swSMBendStateNone)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get flat patterns of the part document if there is any
        /// </summary>
        /// <param name="part">the sheet metal modelDoc2 object</param>
        /// <returns>an array of objects castable to <see cref="Feature"/> </returns>
        public static object[] GetFlatPatterns(this PartDoc part)
        {
            ModelDoc2 model = part as ModelDoc2;
            if (model == null)
                return null;
            var featureMgr = model.FeatureManager;
            var flatPatternFolder = (FlatPatternFolder)featureMgr.GetFlatPatternFolder();
            if (flatPatternFolder == null) return null;
            object[] flatPatterns = (object[])flatPatternFolder.GetFlatPatterns();
            return flatPatterns;
        }

        /// <summary>
        /// gets the sheetMetal Features of this part document fi there is any
        /// </summary>
        /// <param name="part">the sheet metal modelDoc2 or PartDoc object</param>
        /// <returns>an array of objects castable to <see cref="Feature"/></returns>
        public static object[] GetSheetMetals(this PartDoc part)
        {
            ModelDoc2 model = part as ModelDoc2;
            if (model == null)
                return null;
            var featureMgr = model.FeatureManager;
            SheetMetalFolder sheetMetalFolder = featureMgr.GetSheetMetalFolder() as SheetMetalFolder;
            if (sheetMetalFolder == null)
                return null;
            return (object[])sheetMetalFolder.GetSheetMetals();
        }

        /// <summary>
        /// gets the body of this partDoc by its name and null if doesnt exist
        /// </summary>
        /// <param name="part">the part document</param>
        /// <param name="name">the name of the body</param>
        /// <param name="visiblesOnly">toggle to return visible bodies or not</param>
        /// <returns></returns>
        public static Body2 GetBodyByName(this PartDoc part, string name, bool visiblesOnly = false)
        {
            object[] bodies = part.GetBodies2((int)swBodyType_e.swAllBodies, visiblesOnly) as object[];
            foreach (Body2 item in bodies)
            {
                if (item.Name == name)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// get a Ilist of features whose type is provided in the parameters
        /// </summary>
        /// <param name="part"></param>
        /// <param name="featureType">the feature type, refer to this link to for a list of types as string
        /// <a href="http://help.solidworks.com/2013/english/api/sldworksapi/solidworks.interop.sldworks~solidworks.interop.sldworks.ifeature~gettypename2.html"/>
        /// </param>
        /// <returns></returns>
        public static IList<Feature> GetFeaturesByTypeName(this PartDoc part, string featureType)
        {
            ModelDoc2 model = part as ModelDoc2;
            var allFeatures = model.FeatureManager.GetFeatures(false) as object[];
            var features = new List<Feature>();
            foreach (Feature feature in allFeatures)
            {
                if (feature.GetTypeName2() == featureType)
                {
                    features.Add(feature);
                }
            }
            return features;
        }

        /// <summary>
        /// get cut list folder of this part for a specific body.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="body">a sheetMetal or WeldMent body</param>
        /// <param name="solidworks"></param>
        /// <returns></returns>
        public static Feature GetCutListFolder(this PartDoc part, Body2 body, SldWorks solidworks)
        {
            return body.GetCutListFolder(part, solidworks);
        }
    }
}
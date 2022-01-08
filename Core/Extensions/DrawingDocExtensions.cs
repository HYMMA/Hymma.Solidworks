using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// this class provides extensions for <see cref="DrawingDoc"/> objects
    /// </summary>
    public static class DrawingDocExtensions
    {
        /// <summary>
        /// get a list of boms in this drawing
        /// </summary>
        /// <param name="drawingDoc"></param>
        /// <returns>List of <see cref="object"/> that can be cast into <see cref="BomFeature"/></returns>
        public static IList<object> GetAllBoms(this DrawingDoc drawingDoc)
        {
            ModelDoc2 model = drawingDoc as ModelDoc2;
            Feature feature = model.FirstFeature() as Feature;
            List<object> listOfBom = new List<object>();
            while (feature != null)
            {
                if (feature.GetTypeName2() == "BomFeat")
                    listOfBom.Add(feature.GetSpecificFeature2());
                feature = feature.GetNextFeature() as Feature;
            }
            return listOfBom;
        }

        /// <summary>
        /// get list of views in this drawing
        /// </summary>
        /// <returns>List<view></view></returns>
        public static IList<View> GetAllViews(this DrawingDoc drawingDoc)
        {
            object[] SheetNames = drawingDoc.GetSheetNames() as object[];
            var list = new List<View>();
            foreach (string name in SheetNames)
            {
                //get the sheet object
                Sheet sheet = drawingDoc.Sheet[name];
                //get views in this sheet
                object[] views = sheet.GetViews() as object[];
                if (views == null)
                    continue;
                foreach (View item in views)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// get list of absolute views in this drawing
        /// </summary>
        /// <returns>List<view></view></returns>
        public static IList<View> GetAbsoluteViews(this DrawingDoc drawingDoc)
        {
            object[] SheetNames = drawingDoc.GetSheetNames() as object[];
            var list = new List<View>();
            foreach (string name in SheetNames)
            {
                //get the sheet object
                Sheet sheet = drawingDoc.Sheet[name];
                //get views in this sheet
                object[] views = sheet.GetViews() as object[];
                //filter out parent views and put them in a list of views
                if (views != null)
                    FilterViews(drawingDoc, new string[] { "AbsoluteView" }, views, list);
            }
            return list;
        }

        /// <summary>
        /// gets a <see cref="List{View}"/> and adds to it views in the drawing
        /// whose .GetTypeName2() is in the filter array of strings
        /// </summary>
        /// <param name="drw">drawing to be processed where views are</param>
        /// <param name="filters"></param>
        /// <param name="views"></param>
        /// <param name="resultList"></param>
       private static void FilterViews(DrawingDoc drw, string[] filters, object[] views, List<View> resultList)
        {
            // Traverse the features in the view
            foreach (View view in views)
            {
                //this could be null for section views
                Feature subFeature = (Feature)drw.FeatureByName(view.Name);
                if (subFeature!=null && filters.Contains(subFeature.GetTypeName2()))
                {
                    if (resultList.Exists(v => v.GetReferencedModelName() == view.GetReferencedModelName()
                        && v.ReferencedConfiguration == view.ReferencedConfiguration))
                        continue;
                    resultList.Add(subFeature.GetSpecificFeature2() as View);
                }
            }
        }
    }
}


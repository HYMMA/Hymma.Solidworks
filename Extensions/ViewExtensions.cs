using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Extensions
{
    public static class ViewExtensions
    {
        public static Face2 GetFlatPatternFace(this View view)
        {
            if (!view.IsFlatPatternView())
                return null;
            object[] viewComps = view.GetVisibleComponents();
            if (viewComps != null)
            {
                object[] viewFaces = view.GetVisibleEntities2((Component2)viewComps[0], (int)swViewEntityType_e.swViewEntityType_Face);
                return viewFaces[0] as Face2;
            }
            return null;
        }

        /// <summary>
        /// get bodies of a multi body part in a view
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static object[] GetBodies(this View view)
        {
            //if it is not a part document
            if ( !view.GetReferencedModelName().EndsWith(".sldprt",System.StringComparison.OrdinalIgnoreCase))
                return null;

            //view is not flat pattern
            if (!view.IsFlatPatternView())
                return view.Bodies;
            //if view is flat pattern 
            object[] vs = new object[1];
            vs[0] = view.GetFlatPatternFace()?.GetBody();
            return vs;
        }
    }
}

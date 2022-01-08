using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// extensions for <see cref="View"/>
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// get the flat pattern face of this view
        /// </summary>
        /// <param name="view"></param>
        /// <returns><see cref="Face2"/> flat pattern or null if this view does not have one</returns>
        public static Face2 GetFlatPatternFace(this View view)
        {
            if (!view.IsFlatPatternView())
                return null;
            object[] viewComps = view.GetVisibleComponents() as object[];
            if (viewComps != null)
            {
                object[] viewFaces =(object[]) view.GetVisibleEntities2((Component2)viewComps[0], (int)swViewEntityType_e.swViewEntityType_Face);
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
                return view.Bodies as object[];
            //if view is flat pattern 
            object[] vs = new object[1];
            vs[0] = view.GetFlatPatternFace()?.GetBody();
            return vs;
        }
    }
}

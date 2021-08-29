using Hymma.Mathematics;
using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools
{
    public static class CalloutExtensions
    {
        /// <summary>
        /// set the position of the callout in the model
        /// </summary>
        /// <param name="callout"></param>
        /// <param name="solidworks"></param>
        /// <param name="point"></param>
        public static void SetPosition(this ICallout callout, SldWorks solidworks, Point point)
        {
            //get math util
            var mathUtil = solidworks.GetMathUtility() as IMathUtility;

            //make a new math point from Point
            var mathpoint = mathUtil.CreatePoint(new double[] { point.X, point.Y, point.Z }) as MathPoint ?? (MathPoint)mathUtil.CreatePoint(new double[] { 0, 0, 0 });

            //update the callout
            callout.Position = mathpoint;
        }
    }
}

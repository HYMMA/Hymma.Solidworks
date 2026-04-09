// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Face2"/> objects providing geometric operations
    /// and face analysis utilities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These extensions simplify working with faces in SolidWorks:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Get edge vertex points</description></item>
    ///   <item><description>Calculate face centroid</description></item>
    ///   <item><description>Check if face is planar</description></item>
    ///   <item><description>Find tangent faces (same normal direction)</description></item>
    ///   <item><description>Assign names to faces</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para>Common face operations:</para>
    /// <code>
    /// Face2 face = selectedFace as Face2;
    ///
    /// // Get centroid for annotation placement
    /// double[] center = face.GetCentroid();
    ///
    /// // Check surface type
    /// if (face.IsPlanar())
    /// {
    ///     // Find all connected tangent faces (coplanar faces)
    ///     var tangentFaces = face.GetTangentFaces();
    ///     double totalArea = face.GetArea();
    ///     foreach (Face2 tf in tangentFaces)
    ///     {
    ///         totalArea += tf.GetArea();
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class Face2Extensions
    {
        /// <summary>
        /// Gets all unique vertex points from the face's edges.
        /// </summary>
        /// <param name="face">The face to get points from.</param>
        /// <returns>
        /// A list of double arrays, each containing [X, Y, Z] coordinates in meters.
        /// Curved edges (circles, splines) that have no distinct vertices are skipped.
        /// </returns>
        /// <example>
        /// <code>
        /// List&lt;double[]&gt; points = face.GetPoints();
        /// foreach (var point in points)
        /// {
        ///     Console.WriteLine($"X: {point[0]}, Y: {point[1]}, Z: {point[2]}");
        /// }
        /// </code>
        /// </example>
        public static List<double[]> GetPoints(this Face2 face)
        {
            var edges = (object[])face.GetEdges();
            //get points of each edge and add them to a list of double array
            List<double[]> points = new List<double[]>();
            foreach (Edge edge in edges)
            {
                var startVertex = edge.GetStartVertex() as Vertex;
                //if face has curved edge (a hole for example) 
                //the start and end-vertex would be null. so we continue
                if (startVertex == null)
                    continue;
                var endVertex = edge.GetEndVertex() as Vertex;
                var startPoint = startVertex.GetPoint() as double[];
                var endPoint = endVertex.GetPoint() as double[];
                if (!points.Exists(p => p[0] == startPoint[0] && p[1] == startPoint[1] && p[2] == startPoint[2]))
                    points.Add(startPoint);
                if (!points.Exists(p => p[0] == endPoint[0] && p[1] == endPoint[1] && p[2] == endPoint[2]))
                    points.Add(endPoint);
            }
            return points;
        }

        /// <summary>
        /// get the center of a face
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public static double[] GetCentroid(this Face2 face)
        {
            var vUvBounds = face.GetUVBounds() as double[];
            double centerU = (vUvBounds[0] + vUvBounds[1]) / 2;
            double centerV = (vUvBounds[2] + vUvBounds[3]) / 2;
            var swSurf = face.GetSurface() as Surface;
            try
            {
                var vEvalRes = swSurf.Evaluate(centerU, centerV, 0, 0) as double[];
                double[] point = new double[3] { vEvalRes[0], vEvalRes[1], vEvalRes[2] };
                return point;
            }
            finally
            {
                if (swSurf != null) Marshal.ReleaseComObject(swSurf);
            }
        }

        /// <summary>
        /// determine if a face is planar or curved
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public static bool IsPlanar(this Face2 face)
        {
            Surface surface = (Surface)face.GetSurface();
            try
            {
                return surface.IsPlane();
            }
            finally
            {
                if (surface != null) Marshal.ReleaseComObject(surface);
            }
        }

        /// <summary>
        /// assigns a name to a face
        /// </summary>
        /// <param name="face">the face to assign name to</param>
        /// <param name="part">has to be part document only</param>
        /// <param name="name">string value to assign to face</param>
        public static void SetName(this Face2 face, PartDoc part, string name)
        {
            part.DeleteEntityName(face);
            part.SetEntityName(face, name);
        }

        /// <summary>
        /// get all faces that are tangent to this face 
        /// </summary>
        /// <param name="face">the face that you want to find the tangent faces to</param>
        /// <param name="digits">the value to be used to round the differences between the normals using <see cref="Mathematics.AlmostEqual(double[], double[], int)"/></param>
        /// <remarks>Because Double values can lose precision when arithmetic operations are performed on them,<br/>
        /// a comparison between two face instances that are logically tangent might fail. hence we use a tolerance</remarks>
        /// <returns></returns>
        public static IList<Face2> GetTangentFaces(this Face2 face, int digits = 5)
        {
            var faces = new List<Face2>();
            //For every loop on this face
            var faceloops = (object[])face.GetLoops();
            foreach (Loop2 loop in faceloops)
            {
                //for every coEdge in this loop
                var coEdges = (object[])loop.GetCoEdges();
                foreach (CoEdge coEdge in coEdges)
                {
                    CoEdge partner = coEdge.GetPartner() as CoEdge;
                    if (partner == null)
                        continue; // Open edge (boundary of surface body) — no partner face
                    var coEdgeNormal = GetFaceNormalAtMidCoEdge(coEdge);
                    var partnerNormal = GetFaceNormalAtMidCoEdge(partner);

                    //get faces whose normal at middle of co-edge are equal
                    if (Mathematics.AlmostEqual(coEdgeNormal, partnerNormal, digits))
                    {
                        Loop2 partnerLoop = (Loop2)partner.GetLoop();
                        Face2 partnerFace = (Face2)partnerLoop.GetFace();
                        faces.Add(partnerFace);
                        Marshal.ReleaseComObject(partnerLoop);
                    }
                    Marshal.ReleaseComObject(partner);
                }
            }
            return faces;
        }

        //This function returns the normal vector for the face at the provided co-edge
        private static double[] GetFaceNormalAtMidCoEdge(CoEdge coEdge)
        {
            //should be called so solidworks gets the curve (from solidworks API help)
            Edge edge = coEdge.GetEdge() as Edge;
            Curve curve = edge?.GetCurve() as Curve;

            double[] varParams = (double[])coEdge.GetCurveParams();
            double middleOfCoEdge = (varParams[6] + varParams[7]) / 2;
            double[] midCoEdgeCoord = (double[])coEdge.Evaluate2(middleOfCoEdge, 1);

            Loop2 loop = (Loop2)coEdge.GetLoop();
            Face2 face = (Face2)loop.GetFace();
            Surface surface = (Surface)face.GetSurface();
            bool bFaceSenseReversed = face.FaceInSurfaceSense();

            varParams = surface.EvaluateAtPoint(midCoEdgeCoord[0], midCoEdgeCoord[1], midCoEdgeCoord[2]) as double[];
            double[] dblNormal = new double[3];
            if (bFaceSenseReversed)
            {
                dblNormal[0] = -varParams[0];
                dblNormal[1] = -varParams[1];
                dblNormal[2] = -varParams[2];
            }
            else
            {
                dblNormal[0] = varParams[0];
                dblNormal[1] = varParams[1];
                dblNormal[2] = varParams[2];
            }

            // Release intermediate COM objects
            if (surface != null) Marshal.ReleaseComObject(surface);
            if (face != null) Marshal.ReleaseComObject(face);
            if (loop != null) Marshal.ReleaseComObject(loop);
            if (curve != null) Marshal.ReleaseComObject(curve);
            if (edge != null) Marshal.ReleaseComObject(edge);

            return dblNormal;
        }
    }
    
}

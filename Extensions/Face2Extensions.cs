using Hymma.Infrastructures.Math;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.SolidTools.Extensions
{
    public static class Face2Extensions
    {
        public static List<double[]> GetPoints(this Face2 face)
        {
            var edges = face.GetEdges();
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

        public static double[] GetCentroid(this Face2 face)
        {
            var vUvBounds = face.GetUVBounds() as double[];
            double centerU = (vUvBounds[0] + vUvBounds[1]) / 2;
            double centerV = (vUvBounds[2] + vUvBounds[3]) / 2;
            var swSurf = face.GetSurface() as Surface;

            var vEvalRes = swSurf.Evaluate(centerU, centerV, 0, 0) as double[];
            double[] point = new double[3] { vEvalRes[0], vEvalRes[1], vEvalRes[2] };
            return point;
        }
        public static bool IsPlanar(this Face2 face)
        {
            Surface surface = face.GetSurface();
            return surface.IsPlane();
        }

        /// <summary>
        /// assignes a name to a face
        /// </summary>
        /// <param name="face">the face to assign name to</param>
        /// <param name="part">has to be part document only</param>
        /// <param name="name">string value to assign to face</param>
        public static void SetName(this Face2 face, PartDoc part, string name)
        {
            part.DeleteEntityName(face);
            part.SetEntityName(face, name);
        }

        public static IList<Face2> GetTangentFaces(this Face2 face)
        {
            var faces = new List<Face2>();
            //For every loop on this face
            var faceloops = face.GetLoops();
            foreach (Loop2 loop in faceloops)
            {
                //for eavery coEdge in this loop
                var coEdges = loop.GetCoEdges();
                foreach (CoEdge coEdge in coEdges)
                {
                    CoEdge partner = coEdge.GetPartner();
                    var coEdgeNormal = GetFaceNormalAtMidCoEdge(coEdge);
                    var partnerNormal = GetFaceNormalAtMidCoEdge(partner);
                    //get faces whose normal at middle of coedge are equal
                    if (MathUtils.VectorsAreEqual(coEdgeNormal, partnerNormal))
                    {
                        Loop2 partnerLoop = partner.GetLoop();
                        Face2 partnerFace = partnerLoop.GetFace();
                        faces.Add(partnerFace);
                    }
                }
            }
            return faces;
        }

        //This function returns the normal vector for the face at the provided coedge
        private static double[] GetFaceNormalAtMidCoEdge(CoEdge coEdge)
        {
            //should be called so solidworks gets the curve (from solidworks api help)
            _ = coEdge.GetEdge().GetCurve();
            //The return value format is an array of 10 doubles:
            //retval[0]  X startpoint
            //retval[1]  Y startpoint
            //retval[2]  Z startpoint
            //retval[3]  X endpoint
            //retval[4]  Y endpoint
            //retval[5]  Z endpoint
            //retval[6]  startParam
            //retval[7]  endParam
            //retval[8]  sense(Not used)
            //retval[9]  curve type(Not used)
            double[] varParams = coEdge.GetCurveParams();
            double middleOfCoEdge = (varParams[6] + varParams[7]) / 2;
            // Get the location of the middle of the coedge
            double[] midCoEdgeCoord = coEdge.Evaluate2(middleOfCoEdge, 1);
            //obtain the surface that contains coEdge
            Loop2 loop = coEdge.GetLoop();
            Face2 face = loop.GetFace();
            Surface surface = face.GetSurface();
            //cureveParameters contains the normal vector at provided points (midpoint of edge)
            var surfaceNormal = surface.EvaluateAtPoint(midCoEdgeCoord[0], midCoEdgeCoord[1], midCoEdgeCoord[2]);
            //build the normal vector
            return new double[3] { surfaceNormal[0], surfaceNormal[1], surfaceNormal[2] };
        }
    }
}

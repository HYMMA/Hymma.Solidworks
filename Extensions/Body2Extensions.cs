using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Extensions
{
    public static class Body2Extensions
    {
        /// <summary>
        /// get sheetMetal feature of this body
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Feature GetSheetMetalFeature(this Body2 body)
        {
            if (!body.IsSheetMetal()) return null;
            var f = body.GetFeaturesByTypeName("SheetMetal");
            if (f.Count != 0)
            {
                //a body can have only one sheetmetal feature
                return f[0];
            }
            return null;
        }

        /// <summary>
        /// get feature by its name
        /// </summary>
        /// <param name="body"></param>
        /// <param name="name"></param>
        /// <param name="ExcludeSuppressed">disregard feature if is suppressed</param>
        /// <returns></returns>
        public static Feature GetFeatureByName(this Body2 body, string name, bool ExcludeSuppressed = true)
        {
            //feature names should be unique in solidworks 
            //so we always get one object in return
            foreach (Feature feature in body.GetFeatures())
            {
                if (feature.Name == name)
                    return feature;
            }
            return null;
        }

        /// <summary>
        /// get feature by its type name
        /// </summary>
        /// <param name="body"></param>
        /// <param name="type">type of feature in string <para>
        ///  <see cref="http://help.solidworks.com/2013/english/api/sldworksapi/solidworks.interop.sldworks~solidworks.interop.sldworks.ifeature~gettypename2.html"/>
        /// </para>
        /// </param>
        /// <param name="ExcludeSuppressed">disregard feature if is suppressed</param>
        /// <returns><see cref="Feature"/> and null if the feature is suppressed or could not find the feature</returns>
        public static IList<Feature> GetFeaturesByTypeName(this Body2 body, string type, bool ExcludeSuppressed = true)
        {
            var features = new List<Feature>();
            foreach (Feature feature in body.GetFeatures())
            {
                if (feature.GetTypeName2() == type)
                {
                    //if you found the feature but it is suppressed and we dont need suppressed ones
                    //continiue, otherwise add it to the list
                    if (feature.IsSuppressed() && ExcludeSuppressed)
                        continue;
                    features.Add(feature);
                }
            }
            return features;
        }

        /// <summary>
        /// get flat pattern in this body
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Feature GetFlatPattern(this Body2 body)
        {
            if (!body.IsSheetMetal()) return null;
            var f = body.GetFeaturesByTypeName("FlatPattern", false);
            if (f.Count != 0)
            {
                //a body can have only one flatpattern feature
                return f[0];
            }
            return null;
        }

        /// <summary>
        /// Use this method to get component mass properties 
        /// </summary>
        /// <returns>
        /// The return value is an array of doubles as follows:
        ///<list type="bullet">
        /// <item>
        /// <term>Solid body</term>
        /// <description>[ CenterOfMassX, CenterOfMassY, CenterOfMassZ, Volume, Area, Mass(Volume*density), MomXX, MomYY, MomZZ, MomXY, MomZX, MomYZ ]</description>
        /// </item>
        ///  <item>
        /// <term>Sheet body</term>
        /// <description>[ CenterOfMassX, CenterOfMassY, CenterOfMassZ, Area, Circumference, Mass(Area*density), MomXX, MomYY, MomZZ, MomXY, MomZX, MomYZ ]</description>
        /// </item>
        /// <item>
        /// <term>
        /// Wire body
        /// </term>
        /// <description>
        /// [ CenterOfMassX, CenterOfMassY, CenterOfMassZ, Length, 0, Mass(Length*density), MomXX, MomYY, MomZZ, MomXY, MomZX, MomYZ ]
        /// </description>
        /// </item>
        /// </list></returns>
        public static double[] GetMassProperties(this Body2 body, ModelDoc2 model)
        {
            var nDensity = model.GetUserPreferenceDoubleValue((int)swUserPreferenceDoubleValue_e.swMaterialPropertyDensity);
            var properties = body.GetMassProperties(nDensity) as double[];
            return properties;
        }

        /// <summary>
        /// if a face has name returns it otherwise returns null
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Face2 GetFaceByName(this Body2 body, ModelDoc2 model, string name)
        {
            object[] faces = body.GetFaces();
            foreach (Face2 face in faces)
            {
                if (model.GetEntityName(face) == name)
                {
                    return face;
                }
            }
            return null;
        }

        /// <summary>
        /// get the face with most area
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Face2 GetWidestFace(this Body2 body)
        {
            object[] faces = body.GetFaces();
            var areas = new List<double>(new double[] { 0 });
            Face2 biggest = default(Face2);
            foreach (Face2 face in faces)
            {
                if (face == null) continue;
                var area = face.GetArea();
                if (area > areas.Last())
                {
                    areas.Add(area);
                    biggest = face;
                }
            }
            return biggest;
        }

        /// <summary>
        /// determine if this body has weldment feature in it or not
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool IsWeldment(this Body2 body)
        {
            //if there is any weldmentFeature in the body then it is weldment
            return (body.GetFeaturesByTypeName("WeldmentFeature") != null);
        }

        /// <summary>
        /// gets the cut-list forlder of this body, it applies to weldment or sheetmetal components only
        /// </summary>
        /// <param name="body"></param>
        /// <param name="part"></param>
        /// <param name="solidwork"></param>
        /// <returns>GetCutListFolder as <see cref="Feature"/></returns>
        /// <exception cref="null"></exception>
        public static Feature GetCutListFolder(this Body2 body, PartDoc part, SldWorks solidwork)
        {
            //if this body is niether sheetMetal nor weldment return null
            if (!body.IsSheetMetal() || !body.IsWeldment()) return null;
            Feature feature = part.FirstFeature();
            while (feature != null)
            {
                if (feature.GetTypeName2() == "CutListFolder")
                {
                    BodyFolder bodyFolder = feature.GetSpecificFeature2();
                    var bodies = bodyFolder.GetBodies();
                    if (bodies == null) return null;
                    foreach (Body2 item in bodies)
                    {
                        if (solidwork.IsSame(item, body) == (int)swObjectEquality.swObjectSame)
                            return feature;
                    }
                }
                feature = feature.GetNextFeature();
            }
            return null;
        }

        /// <summary>
        /// get length, width and thickness of sheetmetal feature
        /// </summary>
        /// <param name="body"></param>
        /// <param name="part"></param>
        /// <param name="solidworks"></param>
        /// <returns>an array of string where first member is length in the unit of document, second member is width and thrid one is thickness</returns>
        /// <exception cref="null"></exception>
        public static string[] GetSheetMetalSizes(this Body2 body, PartDoc part, SldWorks solidworks)
        {
            if (!body.IsSheetMetal()) return null;
            var length = body.GetPropertyFromCutList("Bounding Box Length");
            var width = body.GetPropertyFromCutList("Bounding Box Width");
            var thickness = body.GetPropertyFromCutList("Sheet Metal Thickness");
            return new string[3] { length, width, thickness };
        }

        /// <summary>
        /// gets custom property value of a property in a weldment or sheetmetal componetn
        /// <br>required configuration must be activitated prior to calling this method</br>
        /// </summary>
        /// <param name="body"></param>
        /// <param name="customPropertyName"></param>
        /// <param name="useCachedData">set this to false to get up-to-date data - </param>
        /// <returns>value of the property or empty string if not applicable</returns>
        public static string GetPropertyFromCutList(this Body2 body, string customPropertyName, bool useCachedData = false)
        {
            Feature feature = body.GetFeatureByName("CutListFolder");
            if (feature == null) return "";
            CustomPropertyManager CustomPropMgr = feature.CustomPropertyManager;
            int result = CustomPropMgr.Get5(customPropertyName, useCachedData, out _, out string resolvedValue, out _);
            if (result == (int)swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent)
                return "";
            return resolvedValue;
        }

        /// <summary>
        /// will return a list of skethcPoints key value pairs where key is StartPoint Value is the end point of a bend line
        /// </summary>
        /// <param name="body">the body with flat pattern feature to get bend lines of</param>
        /// <returns>list of skethcPoints key value pairs</returns>
        public static IList<SketchPoint[]> GetBendLinePoints(this Body2 body)
        {
            if (!body.IsSheetMetal()) return null;
            var flatPattern = body.GetFlatPattern();
            if (flatPattern == null)
                return null;
            var points = new List<SketchPoint[]>();
            var swSubFeat = (Feature)flatPattern.GetFirstSubFeature();
            while ((swSubFeat != null))
            {
                switch (swSubFeat.GetTypeName())
                {
                    //If it is a sketch
                    case "ProfileFeature":
                        var flatPatternSketch = (Sketch)swSubFeat.GetSpecificFeature2();

                        if (flatPatternSketch == null)
                            break;

                        object[] sketchSegments = flatPatternSketch.GetSketchSegments();

                        //if there's no sketch segmets there is no bend line
                        if (sketchSegments == null) return null;

                        SketchSegment sketchSegment = (SketchSegment)sketchSegments[0];
                        //If the straight line is bending line
                        if (sketchSegment.IsBendLine())
                        {
                            for (int j = 0; j < sketchSegments.Length; j++)
                            {
                                sketchSegment = (SketchSegment)sketchSegments[j];
                                var sketchLine = (SketchLine)sketchSegment; //it's always a line in flat pattern sketches, get lines here
                                var startPoint = (SketchPoint)sketchLine.GetStartPoint2();
                                var endPoint = (SketchPoint)sketchLine.GetEndPoint2();
                                var lineEnds = new SketchPoint[2] { startPoint, endPoint };
                                if (!points.Contains(lineEnds))
                                    points.Add(lineEnds);
                            }
                        }
                        break;
                    default:
                        break;
                }
                swSubFeat = swSubFeat.GetNextSubFeature();
            }
            return points;
        }

        /// <summary>
        /// will return flatpattern bounding box coordinates in metric unit
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double[] GetFlatPatternBoxCoords(this Body2 body)
        {
            var flatPattern = body.GetFlatPattern();
            if (flatPattern == null)
                return null;
            var points = new List<SketchPoint>();
            var swSubFeat = (Feature)flatPattern.GetFirstSubFeature();
            while ((swSubFeat != null))
            {
                switch (swSubFeat.GetTypeName())
                {
                    //If it is a sketch
                    case "ProfileFeature":
                        var featureSketch = (Sketch)swSubFeat.GetSpecificFeature2();
                        if (featureSketch == null)
                            break;
                        object[] sketchSegments = featureSketch.GetSketchSegments();
                        SketchSegment sketchSegment = (SketchSegment)sketchSegments[0];
                        //If the straight line is not a bending line, it is a bounding box
                        if (sketchSegment.IsBendLine())
                            break;
                        for (int j = 0; j < sketchSegments.Length; j++)
                        {
                            sketchSegment = (SketchSegment)sketchSegments[j];
                            var sketchLine = (SketchLine)sketchSegment;
                            var startPoint = (SketchPoint)sketchLine.GetStartPoint2();
                            var endPoint = (SketchPoint)sketchLine.GetEndPoint2();
                            if (!points.Contains(startPoint))
                                points.Add(startPoint);
                            if (!points.Contains(endPoint))
                                points.Add(endPoint);
                        }
                        break;
                    default:
                        break;
                }
                swSubFeat = swSubFeat.GetNextSubFeature();
            }
            return new double[6] { points.Min(p => p.X), points.Min(p => p.Y), points.Min(p => p.Z), points.Max(p => p.X), points.Max(p => p.Y), points.Max(p => p.Z) };
        }
    }
}

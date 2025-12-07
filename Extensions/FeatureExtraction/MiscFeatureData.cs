// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from shell features
    /// </summary>
    public class ShellFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Wall thickness (meters)
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Whether shell is outward
        /// </summary>
        public bool ShellOutward { get; set; }

        /// <summary>
        /// Number of faces removed
        /// </summary>
        public int RemovedFaceCount { get; set; }

        /// <summary>
        /// Multi-thickness face settings
        /// </summary>
        public Dictionary<string, double> MultiThicknessFaces { get; set; } = new Dictionary<string, double>();
    }

    /// <summary>
    /// Data extracted from draft features
    /// </summary>
    public class DraftFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Draft angle (radians)
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Draft type
        /// </summary>
        public swDraftType_e DraftType { get; set; }

        /// <summary>
        /// Number of faces being drafted
        /// </summary>
        public int FaceCount { get; set; }

        /// <summary>
        /// Whether to propagate to tangent faces
        /// </summary>
        public bool PropagateToTangent { get; set; }

        /// <summary>
        /// Whether draft direction is reversed
        /// </summary>
        public bool ReverseDirection { get; set; }
    }

    /// <summary>
    /// Data extracted from rib features
    /// </summary>
    public class RibFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Rib thickness (meters)
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Extrusion direction (integer value)
        /// </summary>
        public int ExtrusionType { get; set; }

        /// <summary>
        /// Whether to flip material side
        /// </summary>
        public bool FlipMaterial { get; set; }

        /// <summary>
        /// Draft angle (radians)
        /// </summary>
        public double DraftAngle { get; set; }

        /// <summary>
        /// Whether draft is outward
        /// </summary>
        public bool DraftOutward { get; set; }

        /// <summary>
        /// Name of the sketch defining the rib
        /// </summary>
        public string SketchName { get; set; }
    }

    /// <summary>
    /// Data extracted from reference plane features
    /// </summary>
    public class ReferencePlaneFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of plane constraint
        /// </summary>
        public swRefPlaneReferenceConstraints_e ConstraintType { get; set; }

        /// <summary>
        /// Offset distance (meters) if offset plane
        /// </summary>
        public double OffsetDistance { get; set; }

        /// <summary>
        /// Angle (radians) if angle plane
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Whether the plane is reversed
        /// </summary>
        public bool Reversed { get; set; }
    }

    /// <summary>
    /// Data extracted from reference axis features
    /// </summary>
    public class ReferenceAxisFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of axis definition
        /// </summary>
        public swRefAxisType_e AxisType { get; set; }

        /// <summary>
        /// Reference entity names used to create the axis
        /// </summary>
        public List<string> ReferenceNames { get; set; } = new List<string>();
    }

    /// <summary>
    /// Data extracted from coordinate system features
    /// </summary>
    public class CoordinateSystemFeatureData : FeatureDataBase
    {
        /// <summary>
        /// X-axis direction vector
        /// </summary>
        public double[] XAxis { get; set; } = new double[3];

        /// <summary>
        /// Y-axis direction vector
        /// </summary>
        public double[] YAxis { get; set; } = new double[3];

        /// <summary>
        /// Z-axis direction vector
        /// </summary>
        public double[] ZAxis { get; set; } = new double[3];

        /// <summary>
        /// Origin point (meters)
        /// </summary>
        public double[] Origin { get; set; } = new double[3];
    }

    /// <summary>
    /// Data extracted from split features
    /// </summary>
    public class SplitFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Number of resulting bodies
        /// </summary>
        public int ResultingBodyCount { get; set; }

        /// <summary>
        /// Whether to consume cut bodies
        /// </summary>
        public bool ConsumeCutBodies { get; set; }

        /// <summary>
        /// Names of resulting bodies
        /// </summary>
        public List<string> ResultingBodyNames { get; set; } = new List<string>();
    }

    /// <summary>
    /// Data extracted from combine features
    /// </summary>
    public class CombineFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of combine operation (integer value)
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// Names of bodies being combined
        /// </summary>
        public List<string> CombinedBodyNames { get; set; } = new List<string>();

        /// <summary>
        /// Name of the main/tool body
        /// </summary>
        public string MainBodyName { get; set; }
    }

    /// <summary>
    /// Data extracted from move/copy body features
    /// </summary>
    public class MoveCopyBodyFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Whether this is a copy operation
        /// </summary>
        public bool IsCopy { get; set; }

        /// <summary>
        /// Translation distance X (meters)
        /// </summary>
        public double TranslateX { get; set; }

        /// <summary>
        /// Translation distance Y (meters)
        /// </summary>
        public double TranslateY { get; set; }

        /// <summary>
        /// Translation distance Z (meters)
        /// </summary>
        public double TranslateZ { get; set; }

        /// <summary>
        /// Rotation angle X (radians)
        /// </summary>
        public double RotateX { get; set; }

        /// <summary>
        /// Rotation angle Y (radians)
        /// </summary>
        public double RotateY { get; set; }

        /// <summary>
        /// Rotation angle Z (radians)
        /// </summary>
        public double RotateZ { get; set; }

        /// <summary>
        /// Number of bodies being moved/copied
        /// </summary>
        public int BodyCount { get; set; }

        /// <summary>
        /// Names of bodies being moved/copied
        /// </summary>
        public List<string> BodyNames { get; set; } = new List<string>();
    }

    /// <summary>
    /// Data extracted from sketch features
    /// </summary>
    public class SketchFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Whether the sketch is fully constrained
        /// </summary>
        public bool FullyConstrained { get; set; }

        /// <summary>
        /// Number of contours in the sketch
        /// </summary>
        public int ContourCount { get; set; }

        /// <summary>
        /// Number of constraints in the sketch
        /// </summary>
        public int ConstraintCount { get; set; }

        /// <summary>
        /// Number of dimensions in the sketch
        /// </summary>
        public int DimensionCount { get; set; }

        /// <summary>
        /// Whether the sketch is on a planar face
        /// </summary>
        public bool IsOnPlanarFace { get; set; }

        /// <summary>
        /// Reference plane name if on standard plane
        /// </summary>
        public string ReferencePlaneName { get; set; }
    }
}

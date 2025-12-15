// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from linear pattern features
    /// </summary>
    public class LinearPatternFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Number of instances in direction 1
        /// </summary>
        public int Direction1Count { get; set; }

        /// <summary>
        /// Spacing in direction 1 (meters)
        /// </summary>
        public double Direction1Spacing { get; set; }

        /// <summary>
        /// Whether direction 1 is reversed
        /// </summary>
        public bool Direction1Reverse { get; set; }

        /// <summary>
        /// Number of instances in direction 2
        /// </summary>
        public int Direction2Count { get; set; }

        /// <summary>
        /// Spacing in direction 2 (meters)
        /// </summary>
        public double Direction2Spacing { get; set; }

        /// <summary>
        /// Whether direction 2 is reversed
        /// </summary>
        public bool Direction2Reverse { get; set; }

        /// <summary>
        /// Whether pattern uses geometry pattern
        /// </summary>
        public bool GeometryPattern { get; set; }

        /// <summary>
        /// Whether varies sketch is enabled
        /// </summary>
        public bool VarySketch { get; set; }

        /// <summary>
        /// Names of features being patterned
        /// </summary>
        public List<string> PatternedFeatureNames { get; set; } = new List<string>();

        /// <summary>
        /// List of skipped instance indices
        /// </summary>
        public List<int> SkippedInstances { get; set; } = new List<int>();

        /// <summary>
        /// Total number of instances (excluding skipped)
        /// </summary>
        public int TotalInstances { get; set; }
    }

    /// <summary>
    /// Data extracted from circular pattern features
    /// </summary>
    public class CircularPatternFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Number of instances
        /// </summary>
        public int InstanceCount { get; set; }

        /// <summary>
        /// Total angle of pattern (radians)
        /// </summary>
        public double TotalAngle { get; set; }

        /// <summary>
        /// Whether instances are equally spaced
        /// </summary>
        public bool EqualSpacing { get; set; }

        /// <summary>
        /// Spacing angle between instances (radians) - if not equally spaced
        /// </summary>
        public double SpacingAngle { get; set; }

        /// <summary>
        /// Whether direction is reversed
        /// </summary>
        public bool ReverseDirection { get; set; }

        /// <summary>
        /// Whether pattern uses geometry pattern
        /// </summary>
        public bool GeometryPattern { get; set; }

        /// <summary>
        /// Whether varies sketch is enabled
        /// </summary>
        public bool VarySketch { get; set; }

        /// <summary>
        /// Names of features being patterned
        /// </summary>
        public List<string> PatternedFeatureNames { get; set; } = new List<string>();

        /// <summary>
        /// List of skipped instance indices
        /// </summary>
        public List<int> SkippedInstances { get; set; } = new List<int>();

        /// <summary>
        /// Whether the pattern axis is defined by a reference
        /// </summary>
        public bool AxisIsReference { get; set; }
    }

    /// <summary>
    /// Data extracted from mirror features
    /// </summary>
    public class MirrorFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Names of features being mirrored
        /// </summary>
        public List<string> MirroredFeatureNames { get; set; } = new List<string>();

        /// <summary>
        /// Names of faces being mirrored
        /// </summary>
        public List<string> MirroredFaceNames { get; set; } = new List<string>();

        /// <summary>
        /// Names of bodies being mirrored
        /// </summary>
        public List<string> MirroredBodyNames { get; set; } = new List<string>();

        /// <summary>
        /// Whether geometry pattern is used
        /// </summary>
        public bool GeometryPattern { get; set; }

        /// <summary>
        /// Whether to merge solids
        /// </summary>
        public bool MergeSolids { get; set; }

        /// <summary>
        /// Whether to knit surfaces
        /// </summary>
        public bool KnitSurfaces { get; set; }

        /// <summary>
        /// Mirror plane type
        /// </summary>
        public string MirrorPlaneType { get; set; }
    }

    /// <summary>
    /// Data extracted from sketch-driven pattern features
    /// </summary>
    public class SketchPatternFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Number of pattern instances
        /// </summary>
        public int InstanceCount { get; set; }

        /// <summary>
        /// Name of the driving sketch
        /// </summary>
        public string DrivingSketchName { get; set; }

        /// <summary>
        /// Whether geometry pattern is used
        /// </summary>
        public bool GeometryPattern { get; set; }

        /// <summary>
        /// Names of features being patterned
        /// </summary>
        public List<string> PatternedFeatureNames { get; set; } = new List<string>();

        /// <summary>
        /// Reference point type (integer value)
        /// </summary>
        public int ReferencePointType { get; set; }
    }

    /// <summary>
    /// Data extracted from table-driven pattern features
    /// </summary>
    public class TablePatternFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Number of pattern instances
        /// </summary>
        public int InstanceCount { get; set; }

        /// <summary>
        /// Path to the table file
        /// </summary>
        public string TableFilePath { get; set; }

        /// <summary>
        /// Names of features being patterned
        /// </summary>
        public List<string> PatternedFeatureNames { get; set; } = new List<string>();

        /// <summary>
        /// Whether geometry pattern is used
        /// </summary>
        public bool GeometryPattern { get; set; }

        /// <summary>
        /// Coordinate system name used for the pattern
        /// </summary>
        public string CoordinateSystemName { get; set; }
    }
}

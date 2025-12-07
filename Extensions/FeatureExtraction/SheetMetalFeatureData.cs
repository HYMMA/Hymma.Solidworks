// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from sheet metal base flange features
    /// </summary>
    public class BaseFlangeFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Sheet thickness (meters)
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Bend radius (meters)
        /// </summary>
        public double BendRadius { get; set; }

        /// <summary>
        /// K-Factor value
        /// </summary>
        public double KFactor { get; set; }

        /// <summary>
        /// Direction of extrusion (true = direction 1)
        /// </summary>
        public bool Direction { get; set; }

        /// <summary>
        /// Extrusion depth (meters)
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// Whether the base flange is reversed
        /// </summary>
        public bool ReverseDirection { get; set; }

        /// <summary>
        /// Name of the sketch used for base flange
        /// </summary>
        public string SketchName { get; set; }
    }

    /// <summary>
    /// Data extracted from edge flange features
    /// </summary>
    public class EdgeFlangeFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Flange angle (radians)
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Flange length (meters)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Position type of the flange (integer value)
        /// </summary>
        public int PositionType { get; set; }

        /// <summary>
        /// Gap distance (meters)
        /// </summary>
        public double GapDistance { get; set; }

        /// <summary>
        /// Offset distance (meters)
        /// </summary>
        public double OffsetDistance { get; set; }

        /// <summary>
        /// Bend radius (meters) - null if using default
        /// </summary>
        public double? CustomBendRadius { get; set; }

        /// <summary>
        /// Whether custom relief is used
        /// </summary>
        public bool UseCustomRelief { get; set; }

        /// <summary>
        /// Relief type (integer value)
        /// </summary>
        public int ReliefType { get; set; }

        /// <summary>
        /// Relief depth (meters)
        /// </summary>
        public double ReliefDepth { get; set; }

        /// <summary>
        /// Relief width (meters)
        /// </summary>
        public double ReliefWidth { get; set; }

        /// <summary>
        /// Number of edges this flange is applied to
        /// </summary>
        public int EdgeCount { get; set; }
    }

    /// <summary>
    /// Data extracted from miter flange features
    /// </summary>
    public class MiterFlangeFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Flange length (meters)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gap distance (meters)
        /// </summary>
        public double GapDistance { get; set; }

        /// <summary>
        /// Whether to use default bend radius
        /// </summary>
        public bool UseDefaultBendRadius { get; set; }

        /// <summary>
        /// Custom bend radius (meters)
        /// </summary>
        public double CustomBendRadius { get; set; }

        /// <summary>
        /// Start offset distance (meters)
        /// </summary>
        public double StartOffset { get; set; }

        /// <summary>
        /// End offset distance (meters)
        /// </summary>
        public double EndOffset { get; set; }

        /// <summary>
        /// Number of edges this flange is applied to
        /// </summary>
        public int EdgeCount { get; set; }
    }

    /// <summary>
    /// Data extracted from hem features
    /// </summary>
    public class HemFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of hem (integer value)
        /// </summary>
        public int HemType { get; set; }

        /// <summary>
        /// Hem length/depth (meters)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gap distance for open hems (meters)
        /// </summary>
        public double Gap { get; set; }

        /// <summary>
        /// Hem radius (meters)
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Hem angle (radians) - for teardrop type
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Number of edges the hem is applied to
        /// </summary>
        public int EdgeCount { get; set; }

        /// <summary>
        /// Whether material is added inside
        /// </summary>
        public bool MaterialInside { get; set; }

        /// <summary>
        /// Whether the hem is reversed
        /// </summary>
        public bool ReverseDirection { get; set; }
    }

    /// <summary>
    /// Data extracted from sketched bend features
    /// </summary>
    public class SketchedBendFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Bend radius (meters)
        /// </summary>
        public double BendRadius { get; set; }

        /// <summary>
        /// Bend angle (radians)
        /// </summary>
        public double BendAngle { get; set; }

        /// <summary>
        /// Whether to use default bend radius
        /// </summary>
        public bool UseDefaultRadius { get; set; }

        /// <summary>
        /// Name of the sketch defining the bend line
        /// </summary>
        public string SketchName { get; set; }

        /// <summary>
        /// Fixed face or edge reference
        /// </summary>
        public string FixedReference { get; set; }
    }

    /// <summary>
    /// Data extracted from jog features
    /// </summary>
    public class JogFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Jog offset distance (meters)
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// Whether to use default bend radius
        /// </summary>
        public bool UseDefaultBendRadius { get; set; }

        /// <summary>
        /// Custom bend radius (meters)
        /// </summary>
        public double BendRadius { get; set; }

        /// <summary>
        /// Jog position type (integer value)
        /// </summary>
        public int PositionType { get; set; }

        /// <summary>
        /// Fixed face option (integer value)
        /// </summary>
        public int FixedFaceType { get; set; }

        /// <summary>
        /// Name of the sketch defining the jog
        /// </summary>
        public string SketchName { get; set; }
    }

    /// <summary>
    /// Data extracted from tab features (sheet metal tabs)
    /// </summary>
    public class TabFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Tab height (meters)
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Offset distance (meters)
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// Whether to add bend
        /// </summary>
        public bool AddBend { get; set; }

        /// <summary>
        /// Name of the sketch defining the tab
        /// </summary>
        public string SketchName { get; set; }
    }
}

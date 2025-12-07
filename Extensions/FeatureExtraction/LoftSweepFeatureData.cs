// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from loft features
    /// </summary>
    public class LoftFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Number of profiles in the loft
        /// </summary>
        public int ProfileCount { get; set; }

        /// <summary>
        /// Number of guide curves
        /// </summary>
        public int GuideCurveCount { get; set; }

        /// <summary>
        /// Whether start tangency is applied
        /// </summary>
        public bool StartTangencyType { get; set; }

        /// <summary>
        /// Whether end tangency is applied
        /// </summary>
        public bool EndTangencyType { get; set; }

        /// <summary>
        /// Whether this is a thin feature
        /// </summary>
        public bool IsThinFeature { get; set; }

        /// <summary>
        /// Thin feature wall thickness (meters)
        /// </summary>
        public double ThinWallThickness { get; set; }

        /// <summary>
        /// Whether the feature merges result bodies
        /// </summary>
        public bool MergeResult { get; set; }

        /// <summary>
        /// Whether the loft is closed
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// Whether to maintain tangency
        /// </summary>
        public bool MaintainTangency { get; set; }
    }

    /// <summary>
    /// Data extracted from sweep features
    /// </summary>
    public class SweepFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Whether this is a thin feature
        /// </summary>
        public bool IsThinFeature { get; set; }

        /// <summary>
        /// Thin feature wall thickness (meters)
        /// </summary>
        public double ThinWallThickness { get; set; }

        /// <summary>
        /// Whether the feature merges result bodies
        /// </summary>
        public bool MergeResult { get; set; }

        /// <summary>
        /// Whether to maintain tangency
        /// </summary>
        public bool MaintainTangency { get; set; }

        /// <summary>
        /// Whether the sweep path is tangent to the profile
        /// </summary>
        public bool PathTangentType { get; set; }

        /// <summary>
        /// Twist angle (radians)
        /// </summary>
        public double TwistAngle { get; set; }

        /// <summary>
        /// Whether twist is defined
        /// </summary>
        public bool TwistControlType { get; set; }

        /// <summary>
        /// Number of guide curves
        /// </summary>
        public int GuideCurveCount { get; set; }

        /// <summary>
        /// Orientation/twist type
        /// </summary>
        public swTwistControlType_e OrientationType { get; set; }

        /// <summary>
        /// Name of the profile sketch
        /// </summary>
        public string ProfileSketchName { get; set; }

        /// <summary>
        /// Name of the path sketch
        /// </summary>
        public string PathSketchName { get; set; }
    }

    /// <summary>
    /// Data extracted from cut loft features
    /// </summary>
    public class CutLoftFeatureData : LoftFeatureData
    {
        /// <summary>
        /// Whether the cut affects all bodies
        /// </summary>
        public bool FeatureScope { get; set; }

        /// <summary>
        /// Whether the cut uses auto-select for bodies
        /// </summary>
        public bool AutoSelect { get; set; }
    }

    /// <summary>
    /// Data extracted from cut sweep features
    /// </summary>
    public class CutSweepFeatureData : SweepFeatureData
    {
        /// <summary>
        /// Whether the cut affects all bodies
        /// </summary>
        public bool FeatureScope { get; set; }

        /// <summary>
        /// Whether the cut uses auto-select for bodies
        /// </summary>
        public bool AutoSelect { get; set; }
    }
}

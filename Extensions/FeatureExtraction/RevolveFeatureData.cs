// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from revolve features
    /// </summary>
    public class RevolveFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Revolve angle in direction 1 (radians)
        /// </summary>
        public double Angle1 { get; set; }

        /// <summary>
        /// Revolve angle in direction 2 (radians)
        /// </summary>
        public double Angle2 { get; set; }

        /// <summary>
        /// End condition type for direction 1
        /// </summary>
        public swEndConditions_e EndCondition1 { get; set; }

        /// <summary>
        /// End condition type for direction 2
        /// </summary>
        public swEndConditions_e EndCondition2 { get; set; }

        /// <summary>
        /// Whether this is a thin feature
        /// </summary>
        public bool IsThinFeature { get; set; }

        /// <summary>
        /// Thin feature wall thickness (meters)
        /// </summary>
        public double ThinWallThickness { get; set; }

        /// <summary>
        /// Thin feature wall thickness in direction 2 (meters)
        /// </summary>
        public double ThinWallThickness2 { get; set; }

        /// <summary>
        /// Whether revolve is in both directions
        /// </summary>
        public bool IsBothDirections { get; set; }

        /// <summary>
        /// Whether the feature merges result bodies
        /// </summary>
        public bool MergeResult { get; set; }

        /// <summary>
        /// Name of the sketch used for revolve
        /// </summary>
        public string SketchName { get; set; }

        /// <summary>
        /// Revolve type
        /// </summary>
        public swRevolveType_e RevolveType { get; set; }
    }

    /// <summary>
    /// Data extracted from cut revolve features
    /// </summary>
    public class CutRevolveFeatureData : RevolveFeatureData
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

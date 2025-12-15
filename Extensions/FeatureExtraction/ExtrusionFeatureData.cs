// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from boss/base extrusion features
    /// </summary>
    public class ExtrusionFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Depth of extrusion in direction 1 (meters)
        /// </summary>
        public double Depth1 { get; set; }

        /// <summary>
        /// Depth of extrusion in direction 2 (meters)
        /// </summary>
        public double Depth2 { get; set; }

        /// <summary>
        /// End condition for direction 1
        /// </summary>
        public swEndConditions_e EndCondition1 { get; set; }

        /// <summary>
        /// End condition for direction 2
        /// </summary>
        public swEndConditions_e EndCondition2 { get; set; }

        /// <summary>
        /// Draft angle in direction 1 (radians)
        /// </summary>
        public double DraftAngle1 { get; set; }

        /// <summary>
        /// Draft angle in direction 2 (radians)
        /// </summary>
        public double DraftAngle2 { get; set; }

        /// <summary>
        /// Whether draft is outward in direction 1
        /// </summary>
        public bool DraftOutward1 { get; set; }

        /// <summary>
        /// Whether draft is outward in direction 2
        /// </summary>
        public bool DraftOutward2 { get; set; }

        /// <summary>
        /// Whether this is a thin feature
        /// </summary>
        public bool IsThinFeature { get; set; }

        /// <summary>
        /// Thin feature wall thickness (meters)
        /// </summary>
        public double ThinWallThickness { get; set; }

        /// <summary>
        /// Whether extrusion is in both directions
        /// </summary>
        public bool IsBothDirections { get; set; }

        /// <summary>
        /// Whether the feature merges result bodies
        /// </summary>
        public bool MergeResult { get; set; }

        /// <summary>
        /// Name of the sketch used for extrusion
        /// </summary>
        public string SketchName { get; set; }
    }

    /// <summary>
    /// Data extracted from cut extrusion features
    /// </summary>
    public class CutExtrusionFeatureData : ExtrusionFeatureData
    {
        /// <summary>
        /// Whether the cut goes through all bodies
        /// </summary>
        public bool FeatureScope { get; set; }

        /// <summary>
        /// Whether the cut uses auto-select for bodies
        /// </summary>
        public bool AutoSelect { get; set; }
    }
}

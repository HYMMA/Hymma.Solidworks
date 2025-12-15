// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from hole wizard features
    /// </summary>
    public class HoleWizardFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of hole
        /// </summary>
        public swWzdHoleTypes_e HoleType { get; set; }

        /// <summary>
        /// Standard used (ANSI, ISO, etc.) - integer value
        /// </summary>
        public int Standard { get; set; }

        /// <summary>
        /// Fastener type (integer value)
        /// </summary>
        public int FastenerType { get; set; }

        /// <summary>
        /// Hole diameter (meters)
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// Hole depth (meters)
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// End condition type
        /// </summary>
        public swEndConditions_e EndCondition { get; set; }

        /// <summary>
        /// Counterbore diameter (meters) - if applicable
        /// </summary>
        public double CounterboreDiameter { get; set; }

        /// <summary>
        /// Counterbore depth (meters) - if applicable
        /// </summary>
        public double CounterboreDepth { get; set; }

        /// <summary>
        /// Countersink diameter (meters) - if applicable
        /// </summary>
        public double CountersinkDiameter { get; set; }

        /// <summary>
        /// Countersink angle (radians) - if applicable
        /// </summary>
        public double CountersinkAngle { get; set; }

        /// <summary>
        /// Thread size string (e.g., "M10x1.5")
        /// </summary>
        public string ThreadSize { get; set; }

        /// <summary>
        /// Thread depth (meters) - for tapped holes
        /// </summary>
        public double ThreadDepth { get; set; }

        /// <summary>
        /// Whether thread is cosmetic only
        /// </summary>
        public bool CosmeticThread { get; set; }

        /// <summary>
        /// Near side countersink (if applicable)
        /// </summary>
        public bool NearSideCountersink { get; set; }

        /// <summary>
        /// Far side countersink (if applicable)
        /// </summary>
        public bool FarSideCountersink { get; set; }

        /// <summary>
        /// Number of hole instances
        /// </summary>
        public int InstanceCount { get; set; }
    }

    /// <summary>
    /// Data extracted from simple hole features
    /// </summary>
    public class SimpleHoleFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Hole diameter (meters)
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// Hole depth (meters)
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// End condition type
        /// </summary>
        public swEndConditions_e EndCondition { get; set; }

        /// <summary>
        /// Draft angle (radians)
        /// </summary>
        public double DraftAngle { get; set; }

        /// <summary>
        /// Whether draft is outward
        /// </summary>
        public bool DraftOutward { get; set; }

        /// <summary>
        /// X position on the sketch plane (meters)
        /// </summary>
        public double PositionX { get; set; }

        /// <summary>
        /// Y position on the sketch plane (meters)
        /// </summary>
        public double PositionY { get; set; }
    }
}

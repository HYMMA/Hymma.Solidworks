// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from structural member features
    /// </summary>
    public class StructuralMemberFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Profile standard (e.g., "ansi inch", "iso")
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// Profile type (e.g., "c channel", "angle iron")
        /// </summary>
        public string ProfileType { get; set; }

        /// <summary>
        /// Profile size (e.g., "3 x 4 x 1/4")
        /// </summary>
        public string ProfileSize { get; set; }

        /// <summary>
        /// Number of groups in this structural member feature
        /// </summary>
        public int GroupCount { get; set; }

        /// <summary>
        /// Details for each group
        /// </summary>
        public List<StructuralMemberGroup> Groups { get; set; } = new List<StructuralMemberGroup>();

        /// <summary>
        /// Rotation angle (radians)
        /// </summary>
        public double RotationAngle { get; set; }

        /// <summary>
        /// Whether to mirror profile
        /// </summary>
        public bool MirrorProfile { get; set; }

        /// <summary>
        /// Whether to merge arc segments
        /// </summary>
        public bool MergeArcSegments { get; set; }

        /// <summary>
        /// Path to the profile library file
        /// </summary>
        public string ProfilePath { get; set; }
    }

    /// <summary>
    /// Represents a group within a structural member feature
    /// </summary>
    public class StructuralMemberGroup
    {
        /// <summary>
        /// Number of segments in this group
        /// </summary>
        public int SegmentCount { get; set; }

        /// <summary>
        /// Corner treatment type (integer value)
        /// </summary>
        public int CornerTreatment { get; set; }

        /// <summary>
        /// Whether to apply corner treatment
        /// </summary>
        public bool ApplyCornerTreatment { get; set; }

        /// <summary>
        /// Gap distance for corner treatment (meters)
        /// </summary>
        public double GapDistance { get; set; }
    }

    /// <summary>
    /// Data extracted from end cap features
    /// </summary>
    public class EndCapFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Thickness of the end cap (meters)
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Offset distance (meters)
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// Whether the offset is inward
        /// </summary>
        public bool OffsetInward { get; set; }

        /// <summary>
        /// Corner chamfer size (meters)
        /// </summary>
        public double ChamferSize { get; set; }

        /// <summary>
        /// Whether to add chamfer to corners
        /// </summary>
        public bool ChamferCorners { get; set; }

        /// <summary>
        /// Number of faces the cap is applied to
        /// </summary>
        public int FaceCount { get; set; }
    }

    /// <summary>
    /// Data extracted from gusset features
    /// </summary>
    public class GussetFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Gusset thickness (meters)
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Profile dimension d1 (meters)
        /// </summary>
        public double D1 { get; set; }

        /// <summary>
        /// Profile dimension d2 (meters)
        /// </summary>
        public double D2 { get; set; }

        /// <summary>
        /// Indent depth (meters)
        /// </summary>
        public double IndentDepth { get; set; }

        /// <summary>
        /// Whether profile is flipped
        /// </summary>
        public bool FlipProfile { get; set; }

        /// <summary>
        /// Whether to use indent
        /// </summary>
        public bool UseIndent { get; set; }

        /// <summary>
        /// Gusset profile type (integer value)
        /// </summary>
        public int ProfileType { get; set; }
    }

    /// <summary>
    /// Data extracted from weld bead features
    /// </summary>
    public class WeldBeadFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Weld bead size (meters)
        /// </summary>
        public double BeadSize { get; set; }

        /// <summary>
        /// Weld bead length (meters) - for intermittent welds
        /// </summary>
        public double BeadLength { get; set; }

        /// <summary>
        /// Pitch/spacing (meters) - for intermittent welds
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        /// Whether this is an intermittent weld
        /// </summary>
        public bool IsIntermittent { get; set; }

        /// <summary>
        /// Number of faces selected for weld
        /// </summary>
        public int FaceCount { get; set; }

        /// <summary>
        /// Weld type
        /// </summary>
        public swWeldBeadType_e WeldType { get; set; }
    }

    /// <summary>
    /// Data extracted from trim/extend features
    /// </summary>
    public class TrimExtendFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of operation (trim or extend) - integer value
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// End condition type (integer value)
        /// </summary>
        public int EndCondition { get; set; }

        /// <summary>
        /// Number of bodies being trimmed/extended
        /// </summary>
        public int BodyCount { get; set; }

        /// <summary>
        /// Gap distance (meters)
        /// </summary>
        public double GapDistance { get; set; }

        /// <summary>
        /// Whether to allow extension
        /// </summary>
        public bool AllowExtension { get; set; }
    }
}

// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Data extracted from fillet features
    /// </summary>
    public class FilletFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of fillet
        /// </summary>
        public swFeatureFilletType_e FilletType { get; set; }

        /// <summary>
        /// Default fillet radius (meters)
        /// </summary>
        public double DefaultRadius { get; set; }

        /// <summary>
        /// Whether tangent propagation is enabled
        /// </summary>
        public bool PropagateToTangentFaces { get; set; }

        /// <summary>
        /// Whether the fillet is asymmetric
        /// </summary>
        public bool IsAsymmetric { get; set; }

        /// <summary>
        /// Radius for asymmetric fillet distance 2 (meters)
        /// </summary>
        public double AsymmetricRadius2 { get; set; }

        /// <summary>
        /// Whether curvature continuous is enabled
        /// </summary>
        public bool CurvatureContinuous { get; set; }

        /// <summary>
        /// Number of edges being filleted
        /// </summary>
        public int EdgeCount { get; set; }

        /// <summary>
        /// Number of faces being filleted
        /// </summary>
        public int FaceCount { get; set; }

        /// <summary>
        /// Whether full preview is enabled
        /// </summary>
        public bool FullPreview { get; set; }

        /// <summary>
        /// Whether partial preview is enabled
        /// </summary>
        public bool PartialPreview { get; set; }

        /// <summary>
        /// Fillet profile type for variable radius fillets (integer value)
        /// </summary>
        public int ProfileType { get; set; }
    }

    /// <summary>
    /// Data extracted from chamfer features
    /// </summary>
    public class ChamferFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Type of chamfer
        /// </summary>
        public swChamferType_e ChamferType { get; set; }

        /// <summary>
        /// First distance/width (meters)
        /// </summary>
        public double Distance1 { get; set; }

        /// <summary>
        /// Second distance (meters) - used for Distance-Distance type
        /// </summary>
        public double Distance2 { get; set; }

        /// <summary>
        /// Chamfer angle (radians) - used for Angle-Distance type
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Whether tangent propagation is enabled
        /// </summary>
        public bool PropagateToTangentFaces { get; set; }

        /// <summary>
        /// Number of edges being chamfered
        /// </summary>
        public int EdgeCount { get; set; }

        /// <summary>
        /// Whether to keep features
        /// </summary>
        public bool KeepFeatures { get; set; }
    }
}

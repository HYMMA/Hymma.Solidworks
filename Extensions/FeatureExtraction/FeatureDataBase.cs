// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Base class for all extracted feature data
    /// </summary>
    public abstract class FeatureDataBase
    {
        /// <summary>
        /// The name of the feature as it appears in the feature tree
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The SolidWorks feature type name (from GetTypeName2)
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Whether the feature is suppressed
        /// </summary>
        public bool IsSuppressed { get; set; }

        /// <summary>
        /// The feature ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Any errors during extraction
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Generic feature data for features that don't have specific extractors
    /// </summary>
    public class GenericFeatureData : FeatureDataBase
    {
        /// <summary>
        /// Raw parameter names and values when specific extraction is not available
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}

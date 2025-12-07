// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Interface for extracting data from specific SolidWorks feature types
    /// </summary>
    public interface IFeatureExtractor
    {
        /// <summary>
        /// The SolidWorks feature type name this extractor handles (from GetTypeName2)
        /// </summary>
        string FeatureTypeName { get; }

        /// <summary>
        /// Extracts data from the given feature
        /// </summary>
        /// <param name="feature">The SolidWorks feature to extract data from</param>
        /// <returns>A FeatureDataBase derived object containing the extracted data</returns>
        FeatureDataBase Extract(IFeature feature);

        /// <summary>
        /// Checks if this extractor can handle the given feature type
        /// </summary>
        /// <param name="typeName">The feature type name from GetTypeName2</param>
        /// <returns>True if this extractor can handle the feature type</returns>
        bool CanExtract(string typeName);
    }
}

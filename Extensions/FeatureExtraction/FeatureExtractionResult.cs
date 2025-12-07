// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Contains the result of feature extraction from a SolidWorks document
    /// </summary>
    public class FeatureExtractionResult
    {
        /// <summary>
        /// The name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// The full path of the document
        /// </summary>
        public string DocumentPath { get; set; }

        /// <summary>
        /// The configuration name used for extraction
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// The document type (Part, Assembly, Drawing)
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// All extracted features
        /// </summary>
        public List<FeatureDataBase> Features { get; set; } = new List<FeatureDataBase>();

        /// <summary>
        /// Gets features of a specific type
        /// </summary>
        /// <typeparam name="T">The feature data type to filter by</typeparam>
        /// <returns>Enumerable of matching features</returns>
        public IEnumerable<T> GetFeatures<T>() where T : FeatureDataBase
        {
            return Features.OfType<T>();
        }

        /// <summary>
        /// Gets features by their SolidWorks type name
        /// </summary>
        /// <param name="typeName">The SolidWorks feature type name</param>
        /// <returns>Enumerable of matching features</returns>
        public IEnumerable<FeatureDataBase> GetFeaturesByTypeName(string typeName)
        {
            return Features.Where(f => f.TypeName == typeName);
        }

        /// <summary>
        /// Gets the total count of features
        /// </summary>
        public int TotalFeatureCount => Features.Count;

        /// <summary>
        /// Gets the count of features with extraction errors
        /// </summary>
        public int FeaturesWithErrors => Features.Count(f => f.Errors.Count > 0);

        /// <summary>
        /// Gets a summary of feature types and their counts
        /// </summary>
        /// <returns>Dictionary of type name to count</returns>
        public Dictionary<string, int> GetFeatureTypeSummary()
        {
            return Features
                .GroupBy(f => f.TypeName)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Gets all extraction errors from all features
        /// </summary>
        /// <returns>Enumerable of feature name and error pairs</returns>
        public IEnumerable<(string FeatureName, string Error)> GetAllErrors()
        {
            foreach (var feature in Features)
            {
                foreach (var error in feature.Errors)
                {
                    yield return (feature.Name, error);
                }
            }
        }
    }

    /// <summary>
    /// Contains the result of feature extraction from an assembly, including component features
    /// </summary>
    public class AssemblyFeatureExtractionResult : FeatureExtractionResult
    {
        /// <summary>
        /// Feature extraction results for each component in the assembly
        /// </summary>
        public List<ComponentFeatureExtractionResult> ComponentResults { get; set; } = new List<ComponentFeatureExtractionResult>();

        /// <summary>
        /// Gets the total feature count including all components
        /// </summary>
        public int TotalFeatureCountIncludingComponents
        {
            get
            {
                int total = Features.Count;
                foreach (var component in ComponentResults)
                {
                    total += component.TotalFeatureCount;
                }
                return total;
            }
        }

        /// <summary>
        /// Gets features from a specific component by name
        /// </summary>
        /// <param name="componentName">The component name</param>
        /// <returns>The component's extraction result or null</returns>
        public ComponentFeatureExtractionResult GetComponentResult(string componentName)
        {
            return ComponentResults.FirstOrDefault(c => c.ComponentName == componentName);
        }
    }

    /// <summary>
    /// Contains the result of feature extraction from a component
    /// </summary>
    public class ComponentFeatureExtractionResult : FeatureExtractionResult
    {
        /// <summary>
        /// The name of the component instance
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// The referenced configuration of the component
        /// </summary>
        public string ReferencedConfiguration { get; set; }

        /// <summary>
        /// Whether the component is suppressed
        /// </summary>
        public bool IsSuppressed { get; set; }

        /// <summary>
        /// Whether the component is an envelope
        /// </summary>
        public bool IsEnvelope { get; set; }

        /// <summary>
        /// Whether the component is a virtual component
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// The component's transform matrix (16 doubles)
        /// </summary>
        public double[] Transform { get; set; }
    }
}

// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Base class for feature extractors providing common functionality
    /// </summary>
    public abstract class FeatureExtractorBase : IFeatureExtractor
    {
        /// <summary>
        /// List of feature type names this extractor can handle
        /// </summary>
        protected readonly HashSet<string> SupportedTypeNames;

        /// <summary>
        /// Creates a new feature extractor with the given supported type names
        /// </summary>
        /// <param name="typeNames">One or more feature type names this extractor supports</param>
        protected FeatureExtractorBase(params string[] typeNames)
        {
            SupportedTypeNames = new HashSet<string>(typeNames, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The primary feature type name this extractor handles
        /// </summary>
        public abstract string FeatureTypeName { get; }

        /// <summary>
        /// Checks if this extractor can handle the given feature type
        /// </summary>
        public bool CanExtract(string typeName)
        {
            return SupportedTypeNames.Contains(typeName);
        }

        /// <summary>
        /// Extracts data from the given feature
        /// </summary>
        public abstract FeatureDataBase Extract(IFeature feature);

        /// <summary>
        /// Populates common properties from the feature
        /// </summary>
        /// <param name="data">The feature data object to populate</param>
        /// <param name="feature">The source feature</param>
        protected void PopulateBaseData(FeatureDataBase data, IFeature feature)
        {
            data.Name = feature.Name;
            data.TypeName = feature.GetTypeName2();
            data.Id = feature.GetID();

            try
            {
                var suppressed = feature.IsSuppressed2(
                    (int)SolidWorks.Interop.swconst.swInConfigurationOpts_e.swThisConfiguration,
                    null) as bool[];
                data.IsSuppressed = suppressed != null && suppressed.Length > 0 && suppressed[0];
            }
            catch
            {
                data.IsSuppressed = false;
            }
        }

        /// <summary>
        /// Safely executes an extraction action and captures any errors
        /// </summary>
        /// <param name="data">The feature data to add errors to</param>
        /// <param name="action">The action to execute</param>
        /// <param name="propertyName">Name of the property being extracted for error reporting</param>
        protected void SafeExtract(FeatureDataBase data, Action action, string propertyName)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                data.Errors.Add($"Error extracting {propertyName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Safely gets a value with error handling
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="data">The feature data to add errors to</param>
        /// <param name="func">The function to execute</param>
        /// <param name="propertyName">Name of the property being extracted for error reporting</param>
        /// <param name="defaultValue">Default value if extraction fails</param>
        /// <returns>The extracted value or default</returns>
        protected T SafeGet<T>(FeatureDataBase data, Func<T> func, string propertyName, T defaultValue = default)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                data.Errors.Add($"Error extracting {propertyName}: {ex.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the sketch name from a feature if it has an associated sketch
        /// </summary>
        /// <param name="feature">The feature to get the sketch from</param>
        /// <returns>The sketch name or null if no sketch</returns>
        protected string GetSketchName(IFeature feature)
        {
            try
            {
                var sketch = feature.GetFirstSubFeature() as IFeature;
                while (sketch != null)
                {
                    var typeName = sketch.GetTypeName2();
                    if (typeName == "ProfileFeature" ||
                        typeName == "3DProfileFeature" ||
                        typeName.Contains("Sketch"))
                    {
                        return sketch.Name;
                    }
                    sketch = sketch.GetNextSubFeature() as IFeature;
                }
            }
            catch
            {
                // Ignore errors - sketch name is optional
            }
            return null;
        }
    }
}

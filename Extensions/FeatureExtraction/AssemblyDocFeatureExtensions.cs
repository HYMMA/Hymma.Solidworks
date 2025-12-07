// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Extension methods for extracting features from AssemblyDoc objects
    /// </summary>
    public static class AssemblyDocFeatureExtensions
    {
        /// <summary>
        /// Extracts all features from an assembly document (assembly-level features only)
        /// </summary>
        /// <param name="assembly">The assembly document</param>
        /// <param name="includeSystemFeatures">Whether to include system features</param>
        /// <returns>Feature extraction result containing assembly-level features</returns>
        public static FeatureExtractionResult ExtractAssemblyFeatures(this AssemblyDoc assembly, bool includeSystemFeatures = false)
        {
            var model = assembly as IModelDoc2;
            if (model == null)
                return new FeatureExtractionResult();

            var result = new FeatureExtractionResult
            {
                DocumentName = model.GetTitle(),
                DocumentPath = model.GetPathName(),
                Configuration = model.IGetActiveConfiguration()?.Name,
                DocumentType = "Assembly"
            };

            result.Features = FeatureExtractionManager.Instance.ExtractAllFeatures(model, includeSystemFeatures);

            return result;
        }

        /// <summary>
        /// Extracts all features from an assembly including all component features
        /// </summary>
        /// <param name="assembly">The assembly document</param>
        /// <param name="includeSystemFeatures">Whether to include system features</param>
        /// <param name="includeSubAssemblyComponents">Whether to traverse into sub-assemblies</param>
        /// <param name="includeSuppressedComponents">Whether to include suppressed components</param>
        /// <returns>Assembly feature extraction result with component features</returns>
        public static AssemblyFeatureExtractionResult ExtractAllFeatures(
            this AssemblyDoc assembly,
            bool includeSystemFeatures = false,
            bool includeSubAssemblyComponents = true,
            bool includeSuppressedComponents = false)
        {
            var model = assembly as IModelDoc2;
            if (model == null)
                return new AssemblyFeatureExtractionResult();

            var result = new AssemblyFeatureExtractionResult
            {
                DocumentName = model.GetTitle(),
                DocumentPath = model.GetPathName(),
                Configuration = model.IGetActiveConfiguration()?.Name,
                DocumentType = "Assembly"
            };

            // Extract assembly-level features
            result.Features = FeatureExtractionManager.Instance.ExtractAllFeatures(model, includeSystemFeatures);

            // Extract component features
            var components = assembly.GetComponents(!includeSubAssemblyComponents) as object[];
            if (components != null)
            {
                foreach (IComponent2 component in components)
                {
                    // Skip suppressed components if not requested
                    if (!includeSuppressedComponents && component.IsSuppressed())
                        continue;

                    var componentResult = ExtractComponentFeatures(component, includeSystemFeatures);
                    if (componentResult != null)
                    {
                        result.ComponentResults.Add(componentResult);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts features from a specific component
        /// </summary>
        /// <param name="component">The component to extract features from</param>
        /// <param name="includeSystemFeatures">Whether to include system features</param>
        /// <returns>Component feature extraction result</returns>
        public static ComponentFeatureExtractionResult ExtractComponentFeatures(
            IComponent2 component,
            bool includeSystemFeatures = false)
        {
            if (component == null)
                return null;

            var componentModel = component.GetModelDoc2() as IModelDoc2;
            if (componentModel == null)
                return null;

            var result = new ComponentFeatureExtractionResult
            {
                ComponentName = component.Name2,
                DocumentName = componentModel.GetTitle(),
                DocumentPath = component.GetPathName(),
                ReferencedConfiguration = component.ReferencedConfiguration,
                IsSuppressed = component.IsSuppressed(),
                IsEnvelope = component.IsEnvelope(),
                IsVirtual = component.IsVirtual,
                DocumentType = componentModel.GetType() == (int)swDocumentTypes_e.swDocPART ? "Part" : "Assembly"
            };

            // Get transform
            var transform = component.Transform2 as IMathTransform;
            if (transform != null)
            {
                result.Transform = transform.ArrayData as double[];
            }

            // Extract features from the component's model
            result.Features = FeatureExtractionManager.Instance.ExtractAllFeatures(componentModel, includeSystemFeatures);

            return result;
        }

        /// <summary>
        /// Extracts features from components of a specific type (parts only or assemblies only)
        /// </summary>
        /// <param name="assembly">The assembly document</param>
        /// <param name="documentType">The document type to filter by</param>
        /// <param name="includeSystemFeatures">Whether to include system features</param>
        /// <returns>List of component feature extraction results</returns>
        public static IEnumerable<ComponentFeatureExtractionResult> ExtractComponentFeaturesByType(
            this AssemblyDoc assembly,
            swDocumentTypes_e documentType,
            bool includeSystemFeatures = false)
        {
            var components = assembly.GetComponents(false) as object[];
            if (components == null)
                yield break;

            foreach (IComponent2 component in components)
            {
                if (component.IsSuppressed())
                    continue;

                var componentModel = component.GetModelDoc2() as IModelDoc2;
                if (componentModel == null)
                    continue;

                if (componentModel.GetType() != (int)documentType)
                    continue;

                var result = ExtractComponentFeatures(component, includeSystemFeatures);
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Extracts features from all part components in an assembly
        /// </summary>
        /// <param name="assembly">The assembly document</param>
        /// <param name="includeSystemFeatures">Whether to include system features</param>
        /// <returns>List of component feature extraction results for parts only</returns>
        public static IEnumerable<ComponentFeatureExtractionResult> ExtractPartComponentFeatures(
            this AssemblyDoc assembly,
            bool includeSystemFeatures = false)
        {
            return assembly.ExtractComponentFeaturesByType(swDocumentTypes_e.swDocPART, includeSystemFeatures);
        }

        /// <summary>
        /// Extracts a specific feature type from all components
        /// </summary>
        /// <typeparam name="T">The feature data type to extract</typeparam>
        /// <param name="assembly">The assembly document</param>
        /// <returns>Dictionary of component name to list of features</returns>
        public static Dictionary<string, List<T>> ExtractFeatureTypeFromAllComponents<T>(
            this AssemblyDoc assembly) where T : FeatureDataBase
        {
            var result = new Dictionary<string, List<T>>();

            var fullResult = assembly.ExtractAllFeatures(false, true, false);

            foreach (var componentResult in fullResult.ComponentResults)
            {
                var features = componentResult.GetFeatures<T>().ToList();
                if (features.Any())
                {
                    result[componentResult.ComponentName] = features;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a summary of all feature types across all components
        /// </summary>
        /// <param name="assembly">The assembly document</param>
        /// <returns>Dictionary of feature type names to their total counts across all components</returns>
        public static Dictionary<string, int> GetFeatureTypeSummaryAcrossAllComponents(this AssemblyDoc assembly)
        {
            var result = new Dictionary<string, int>();

            var fullResult = assembly.ExtractAllFeatures(false, true, false);

            // Add assembly-level features
            foreach (var feature in fullResult.Features)
            {
                if (!result.ContainsKey(feature.TypeName))
                    result[feature.TypeName] = 0;
                result[feature.TypeName]++;
            }

            // Add component features
            foreach (var componentResult in fullResult.ComponentResults)
            {
                foreach (var feature in componentResult.Features)
                {
                    if (!result.ContainsKey(feature.TypeName))
                        result[feature.TypeName] = 0;
                    result[feature.TypeName]++;
                }
            }

            return result;
        }

        /// <summary>
        /// Finds all components that contain a specific feature type
        /// </summary>
        /// <typeparam name="T">The feature data type to search for</typeparam>
        /// <param name="assembly">The assembly document</param>
        /// <returns>List of component names that contain the specified feature type</returns>
        public static IEnumerable<string> FindComponentsWithFeature<T>(this AssemblyDoc assembly) where T : FeatureDataBase
        {
            var fullResult = assembly.ExtractAllFeatures(false, true, false);

            return fullResult.ComponentResults
                .Where(c => c.GetFeatures<T>().Any())
                .Select(c => c.ComponentName);
        }

        /// <summary>
        /// Counts total features of a specific type across all components
        /// </summary>
        /// <typeparam name="T">The feature data type to count</typeparam>
        /// <param name="assembly">The assembly document</param>
        /// <returns>Total count of features of the specified type</returns>
        public static int CountFeaturesAcrossAllComponents<T>(this AssemblyDoc assembly) where T : FeatureDataBase
        {
            var fullResult = assembly.ExtractAllFeatures(false, true, false);
            return fullResult.ComponentResults.Sum(c => c.GetFeatures<T>().Count());
        }

        /// <summary>
        /// Extracts unique parts (by path) and their features
        /// </summary>
        /// <param name="assembly">The assembly document</param>
        /// <param name="includeSystemFeatures">Whether to include system features</param>
        /// <returns>Dictionary of part path to extraction result (eliminates duplicate parts)</returns>
        public static Dictionary<string, ComponentFeatureExtractionResult> ExtractUniquePartFeatures(
            this AssemblyDoc assembly,
            bool includeSystemFeatures = false)
        {
            var result = new Dictionary<string, ComponentFeatureExtractionResult>();

            var components = assembly.GetComponents(false) as object[];
            if (components == null)
                return result;

            foreach (IComponent2 component in components)
            {
                if (component.IsSuppressed())
                    continue;

                var componentModel = component.GetModelDoc2() as IModelDoc2;
                if (componentModel == null)
                    continue;

                // Only process parts
                if (componentModel.GetType() != (int)swDocumentTypes_e.swDocPART)
                    continue;

                string path = component.GetPathName();

                // Skip if we've already processed this part
                if (result.ContainsKey(path))
                    continue;

                var componentResult = ExtractComponentFeatures(component, includeSystemFeatures);
                if (componentResult != null)
                {
                    result[path] = componentResult;
                }
            }

            return result;
        }
    }
}

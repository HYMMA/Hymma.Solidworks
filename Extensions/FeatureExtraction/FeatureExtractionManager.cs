// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Extensions.FeatureExtraction.Extractors;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Manages feature extraction from SolidWorks documents
    /// </summary>
    public class FeatureExtractionManager
    {
        private readonly Dictionary<string, IFeatureExtractor> _extractors;
        private static FeatureExtractionManager _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the singleton instance of the FeatureExtractionManager
        /// </summary>
        public static FeatureExtractionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FeatureExtractionManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Creates a new FeatureExtractionManager with default extractors
        /// </summary>
        private FeatureExtractionManager()
        {
            _extractors = new Dictionary<string, IFeatureExtractor>(StringComparer.OrdinalIgnoreCase);
            RegisterDefaultExtractors();
        }

        /// <summary>
        /// Registers all default extractors
        /// </summary>
        private void RegisterDefaultExtractors()
        {
            // Extrusion extractors
            RegisterExtractor(new ExtrusionExtractor());
            RegisterExtractor(new CutExtrusionExtractor());

            // Revolve extractors
            RegisterExtractor(new RevolveExtractor());
            RegisterExtractor(new CutRevolveExtractor());

            // Loft and Sweep extractors
            RegisterExtractor(new LoftExtractor());
            RegisterExtractor(new SweepExtractor());
            RegisterExtractor(new CutLoftExtractor());
            RegisterExtractor(new CutSweepExtractor());

            // Fillet and Chamfer extractors
            RegisterExtractor(new FilletExtractor());
            RegisterExtractor(new ChamferExtractor());

            // Hole extractors
            RegisterExtractor(new HoleWizardExtractor());
            RegisterExtractor(new SimpleHoleExtractor());

            // Pattern extractors
            RegisterExtractor(new LinearPatternExtractor());
            RegisterExtractor(new CircularPatternExtractor());
            RegisterExtractor(new MirrorExtractor());
            RegisterExtractor(new SketchPatternExtractor());

            // Sheet metal extractors
            RegisterExtractor(new BaseFlangeExtractor());
            RegisterExtractor(new EdgeFlangeExtractor());
            RegisterExtractor(new HemExtractor());
            RegisterExtractor(new SketchedBendExtractor());
            RegisterExtractor(new JogExtractor());
            RegisterExtractor(new MiterFlangeExtractor());

            // Misc extractors
            RegisterExtractor(new ShellExtractor());
            RegisterExtractor(new DraftExtractor());
            RegisterExtractor(new RibExtractor());
            RegisterExtractor(new ReferencePlaneExtractor());
            RegisterExtractor(new SketchExtractor());
            RegisterExtractor(new CombineExtractor());
        }

        /// <summary>
        /// Registers a feature extractor
        /// </summary>
        /// <param name="extractor">The extractor to register</param>
        public void RegisterExtractor(IFeatureExtractor extractor)
        {
            // Register for the primary type name
            _extractors[extractor.FeatureTypeName] = extractor;
        }

        /// <summary>
        /// Gets an extractor for the specified feature type
        /// </summary>
        /// <param name="featureTypeName">The feature type name from GetTypeName2</param>
        /// <returns>The extractor or null if not found</returns>
        public IFeatureExtractor GetExtractor(string featureTypeName)
        {
            // First try direct lookup
            if (_extractors.TryGetValue(featureTypeName, out var extractor))
            {
                return extractor;
            }

            // Then try to find one that can extract this type
            foreach (var ext in _extractors.Values)
            {
                if (ext.CanExtract(featureTypeName))
                {
                    return ext;
                }
            }

            return null;
        }

        /// <summary>
        /// Extracts data from a single feature
        /// </summary>
        /// <param name="feature">The feature to extract data from</param>
        /// <returns>The extracted feature data, or a generic data object if no specific extractor exists</returns>
        public FeatureDataBase ExtractFeature(IFeature feature)
        {
            if (feature == null)
                return null;

            string typeName = feature.GetTypeName2();
            var extractor = GetExtractor(typeName);

            if (extractor != null)
            {
                try
                {
                    return extractor.Extract(feature);
                }
                catch (Exception ex)
                {
                    var data = new GenericFeatureData
                    {
                        Name = feature.Name,
                        TypeName = typeName,
                        Id = feature.GetID()
                    };
                    data.Errors.Add($"Extraction failed: {ex.Message}");
                    return data;
                }
            }

            // Return generic data for unsupported feature types
            return CreateGenericData(feature);
        }

        /// <summary>
        /// Creates generic feature data for unsupported feature types
        /// </summary>
        private GenericFeatureData CreateGenericData(IFeature feature)
        {
            var data = new GenericFeatureData
            {
                Name = feature.Name,
                TypeName = feature.GetTypeName2(),
                Id = feature.GetID()
            };

            try
            {
                var suppressed = feature.IsSuppressed2(
                    (int)SolidWorks.Interop.swconst.swInConfigurationOpts_e.swThisConfiguration,
                    null) as bool[];
                data.IsSuppressed = suppressed != null && suppressed.Length > 0 && suppressed[0];
            }
            catch
            {
                // Ignore suppression check errors
            }

            return data;
        }

        /// <summary>
        /// Extracts all features from a model document
        /// </summary>
        /// <param name="model">The model document</param>
        /// <param name="includeSystemFeatures">Whether to include system features (origins, planes, etc.)</param>
        /// <returns>List of extracted feature data</returns>
        public List<FeatureDataBase> ExtractAllFeatures(IModelDoc2 model, bool includeSystemFeatures = false)
        {
            var result = new List<FeatureDataBase>();
            if (model == null)
                return result;

            var feature = model.IFirstFeature();
            while (feature != null)
            {
                string typeName = feature.GetTypeName2();

                // Skip system features if requested
                if (!includeSystemFeatures && IsSystemFeature(typeName))
                {
                    feature = feature.IGetNextFeature();
                    continue;
                }

                var data = ExtractFeature(feature);
                if (data != null)
                {
                    result.Add(data);
                }

                feature = feature.IGetNextFeature();
            }

            return result;
        }

        /// <summary>
        /// Checks if a feature type is a system feature
        /// </summary>
        private bool IsSystemFeature(string typeName)
        {
            switch (typeName)
            {
                case "OriginProfileFeature":
                case "RefPlane":
                case "RefAxis":
                case "RefPoint":
                case "HistoryFolder":
                case "SensorFolder":
                case "DetailCabinet":
                case "MaterialFolder":
                case "FlatPatternFolder":
                case "CutListFolder":
                case "SolidBodyFolder":
                case "SurfaceBodyFolder":
                case "BodyFolder":
                case "CommentsFolder":
                case "FtrFolder":
                case "Attribute":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a list of all registered feature type names
        /// </summary>
        /// <returns>List of supported feature type names</returns>
        public IEnumerable<string> GetSupportedFeatureTypes()
        {
            return _extractors.Keys;
        }
    }
}

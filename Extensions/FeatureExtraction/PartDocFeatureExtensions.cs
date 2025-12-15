// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Extensions.FeatureExtraction
{
    /// <summary>
    /// Extension methods for extracting features from PartDoc objects
    /// </summary>
    public static class PartDocFeatureExtensions
    {
        /// <summary>
        /// Extracts all features from a part document
        /// </summary>
        /// <param name="part">The part document</param>
        /// <param name="includeSystemFeatures">Whether to include system features (origins, planes, etc.)</param>
        /// <returns>Feature extraction result containing all extracted features</returns>
        public static FeatureExtractionResult ExtractAllFeatures(this PartDoc part, bool includeSystemFeatures = false)
        {
            var model = part as IModelDoc2;
            if (model == null)
                return new FeatureExtractionResult();

            var result = new FeatureExtractionResult
            {
                DocumentName = model.GetTitle(),
                DocumentPath = model.GetPathName(),
                Configuration = model.IGetActiveConfiguration()?.Name,
                DocumentType = "Part"
            };

            result.Features = FeatureExtractionManager.Instance.ExtractAllFeatures(model, includeSystemFeatures);

            return result;
        }

        /// <summary>
        /// Extracts features of a specific type from a part document
        /// </summary>
        /// <typeparam name="T">The feature data type to extract</typeparam>
        /// <param name="part">The part document</param>
        /// <returns>Enumerable of extracted features of the specified type</returns>
        public static IEnumerable<T> ExtractFeatures<T>(this PartDoc part) where T : FeatureDataBase
        {
            var result = part.ExtractAllFeatures(false);
            return result.GetFeatures<T>();
        }

        /// <summary>
        /// Extracts features by their SolidWorks type name
        /// </summary>
        /// <param name="part">The part document</param>
        /// <param name="featureTypeName">The SolidWorks feature type name (e.g., "Extrusion", "Fillet")</param>
        /// <returns>Enumerable of extracted features matching the type name</returns>
        public static IEnumerable<FeatureDataBase> ExtractFeaturesByType(this PartDoc part, string featureTypeName)
        {
            var result = part.ExtractAllFeatures(false);
            return result.GetFeaturesByTypeName(featureTypeName);
        }

        /// <summary>
        /// Extracts all extrusion features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of extrusion feature data</returns>
        public static IEnumerable<ExtrusionFeatureData> ExtractExtrusions(this PartDoc part)
        {
            return part.ExtractFeatures<ExtrusionFeatureData>();
        }

        /// <summary>
        /// Extracts all cut extrusion features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of cut extrusion feature data</returns>
        public static IEnumerable<CutExtrusionFeatureData> ExtractCutExtrusions(this PartDoc part)
        {
            return part.ExtractFeatures<CutExtrusionFeatureData>();
        }

        /// <summary>
        /// Extracts all fillet features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of fillet feature data</returns>
        public static IEnumerable<FilletFeatureData> ExtractFillets(this PartDoc part)
        {
            return part.ExtractFeatures<FilletFeatureData>();
        }

        /// <summary>
        /// Extracts all chamfer features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of chamfer feature data</returns>
        public static IEnumerable<ChamferFeatureData> ExtractChamfers(this PartDoc part)
        {
            return part.ExtractFeatures<ChamferFeatureData>();
        }

        /// <summary>
        /// Extracts all hole wizard features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of hole wizard feature data</returns>
        public static IEnumerable<HoleWizardFeatureData> ExtractHoleWizardFeatures(this PartDoc part)
        {
            return part.ExtractFeatures<HoleWizardFeatureData>();
        }

        /// <summary>
        /// Extracts all pattern features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of pattern feature data (linear, circular, mirror, sketch-driven)</returns>
        public static IEnumerable<FeatureDataBase> ExtractPatterns(this PartDoc part)
        {
            var result = part.ExtractAllFeatures(false);
            return result.Features.Where(f =>
                f is LinearPatternFeatureData ||
                f is CircularPatternFeatureData ||
                f is MirrorFeatureData ||
                f is SketchPatternFeatureData);
        }

        /// <summary>
        /// Extracts all sheet metal features from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>List of sheet metal feature data</returns>
        public static IEnumerable<FeatureDataBase> ExtractSheetMetalFeatures(this PartDoc part)
        {
            var result = part.ExtractAllFeatures(false);
            return result.Features.Where(f =>
                f is BaseFlangeFeatureData ||
                f is EdgeFlangeFeatureData ||
                f is MiterFlangeFeatureData ||
                f is HemFeatureData ||
                f is SketchedBendFeatureData ||
                f is JogFeatureData ||
                f is TabFeatureData);
        }

        /// <summary>
        /// Extracts a specific feature by name from a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <param name="featureName">The name of the feature to extract</param>
        /// <returns>The extracted feature data or null if not found</returns>
        public static FeatureDataBase ExtractFeatureByName(this PartDoc part, string featureName)
        {
            var model = part as IModelDoc2;
            if (model == null)
                return null;

            // Iterate through features to find by name
            var feature = model.IFirstFeature();
            while (feature != null)
            {
                if (feature.Name == featureName)
                {
                    return FeatureExtractionManager.Instance.ExtractFeature(feature);
                }
                feature = feature.IGetNextFeature();
            }

            return null;
        }

        /// <summary>
        /// Gets a summary of all feature types in a part
        /// </summary>
        /// <param name="part">The part document</param>
        /// <returns>Dictionary of feature type names to their counts</returns>
        public static Dictionary<string, int> GetFeatureTypeSummary(this PartDoc part)
        {
            var result = part.ExtractAllFeatures(false);
            return result.GetFeatureTypeSummary();
        }

        /// <summary>
        /// Checks if a part contains features of a specific type
        /// </summary>
        /// <typeparam name="T">The feature data type to check for</typeparam>
        /// <param name="part">The part document</param>
        /// <returns>True if the part contains at least one feature of the specified type</returns>
        public static bool HasFeatures<T>(this PartDoc part) where T : FeatureDataBase
        {
            return part.ExtractFeatures<T>().Any();
        }

        /// <summary>
        /// Counts features of a specific type in a part
        /// </summary>
        /// <typeparam name="T">The feature data type to count</typeparam>
        /// <param name="part">The part document</param>
        /// <returns>The number of features of the specified type</returns>
        public static int CountFeatures<T>(this PartDoc part) where T : FeatureDataBase
        {
            return part.ExtractFeatures<T>().Count();
        }
    }
}

// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from linear pattern features
    /// </summary>
    public class LinearPatternExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new linear pattern extractor
        /// </summary>
        public LinearPatternExtractor() : base("LPattern", "LinearPattern")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "LPattern";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new LinearPatternFeatureData();
            PopulateBaseData(data, feature);

            var patternData = feature.GetDefinition() as ILinearPatternFeatureData;
            if (patternData == null)
            {
                data.Errors.Add("Failed to get linear pattern feature definition");
                return data;
            }

            SafeExtract(data, () => data.Direction1Count = patternData.D1TotalInstances, "Direction1Count");
            SafeExtract(data, () => data.Direction1Spacing = patternData.D1Spacing, "Direction1Spacing");
            SafeExtract(data, () => data.Direction1Reverse = patternData.D1ReverseDirection, "Direction1Reverse");
            SafeExtract(data, () => data.Direction2Count = patternData.D2TotalInstances, "Direction2Count");
            SafeExtract(data, () => data.Direction2Spacing = patternData.D2Spacing, "Direction2Spacing");
            SafeExtract(data, () => data.Direction2Reverse = patternData.D2ReverseDirection, "Direction2Reverse");
            SafeExtract(data, () => data.GeometryPattern = patternData.GeometryPattern, "GeometryPattern");
            SafeExtract(data, () => data.VarySketch = patternData.VarySketch, "VarySketch");

            // Get patterned feature names
            SafeExtract(data, () =>
            {
                var features = patternData.PatternFeatureArray as object[];
                if (features != null)
                {
                    foreach (IFeature feat in features)
                    {
                        data.PatternedFeatureNames.Add(feat.Name);
                    }
                }
            }, "PatternedFeatureNames");

            // Calculate total instances
            data.TotalInstances = data.Direction1Count * (data.Direction2Count > 0 ? data.Direction2Count : 1);

            // Get skipped instances
            SafeExtract(data, () =>
            {
                var skipped = patternData.SkippedItemArray as int[];
                if (skipped != null)
                {
                    data.SkippedInstances = new List<int>(skipped);
                    data.TotalInstances -= skipped.Length;
                }
            }, "SkippedInstances");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from circular pattern features
    /// </summary>
    public class CircularPatternExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new circular pattern extractor
        /// </summary>
        public CircularPatternExtractor() : base("CirPattern", "CircularPattern")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "CirPattern";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new CircularPatternFeatureData();
            PopulateBaseData(data, feature);

            var patternData = feature.GetDefinition() as ICircularPatternFeatureData;
            if (patternData == null)
            {
                data.Errors.Add("Failed to get circular pattern feature definition");
                return data;
            }

            SafeExtract(data, () => data.InstanceCount = patternData.TotalInstances, "InstanceCount");
            SafeExtract(data, () => data.EqualSpacing = patternData.EqualSpacing, "EqualSpacing");
            SafeExtract(data, () => data.SpacingAngle = patternData.Spacing, "SpacingAngle");
            SafeExtract(data, () => data.ReverseDirection = patternData.ReverseDirection, "ReverseDirection");
            SafeExtract(data, () => data.GeometryPattern = patternData.GeometryPattern, "GeometryPattern");
            SafeExtract(data, () => data.VarySketch = patternData.VarySketch, "VarySketch");

            // Get patterned feature names
            SafeExtract(data, () =>
            {
                var features = patternData.PatternFeatureArray as object[];
                if (features != null)
                {
                    foreach (IFeature feat in features)
                    {
                        data.PatternedFeatureNames.Add(feat.Name);
                    }
                }
            }, "PatternedFeatureNames");

            // Get skipped instances
            SafeExtract(data, () =>
            {
                var skipped = patternData.SkippedItemArray as int[];
                if (skipped != null)
                {
                    data.SkippedInstances = new List<int>(skipped);
                }
            }, "SkippedInstances");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from mirror features
    /// </summary>
    public class MirrorExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new mirror extractor
        /// </summary>
        public MirrorExtractor() : base("MirrorPattern", "Mirror", "MirrorSolid")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "MirrorPattern";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new MirrorFeatureData();
            PopulateBaseData(data, feature);

            var mirrorData = feature.GetDefinition() as IMirrorPatternFeatureData;
            if (mirrorData == null)
            {
                data.Errors.Add("Failed to get mirror feature definition");
                return data;
            }

            SafeExtract(data, () => data.GeometryPattern = mirrorData.GeometryPattern, "GeometryPattern");

            // Get mirrored feature names
            SafeExtract(data, () =>
            {
                var features = mirrorData.PatternFeatureArray as object[];
                if (features != null)
                {
                    foreach (IFeature feat in features)
                    {
                        data.MirroredFeatureNames.Add(feat.Name);
                    }
                }
            }, "MirroredFeatureNames");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from sketch-driven pattern features
    /// </summary>
    public class SketchPatternExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new sketch pattern extractor
        /// </summary>
        public SketchPatternExtractor() : base("SketchPattern", "SketchDrivenPattern")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "SketchPattern";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new SketchPatternFeatureData();
            PopulateBaseData(data, feature);

            var patternData = feature.GetDefinition() as ISketchPatternFeatureData;
            if (patternData == null)
            {
                data.Errors.Add("Failed to get sketch pattern feature definition");
                return data;
            }

            SafeExtract(data, () => data.GeometryPattern = patternData.GeometryPattern, "GeometryPattern");

            // Get patterned feature names
            SafeExtract(data, () =>
            {
                var features = patternData.PatternFeatureArray as object[];
                if (features != null)
                {
                    data.InstanceCount = features.Length;
                    foreach (IFeature feat in features)
                    {
                        data.PatternedFeatureNames.Add(feat.Name);
                    }
                }
            }, "PatternedFeatureNames");

            // Get driving sketch name from sub-feature
            var subFeat = feature.GetFirstSubFeature() as IFeature;
            while (subFeat != null)
            {
                var typeName = subFeat.GetTypeName2();
                if (typeName == "ProfileFeature" || typeName.Contains("Sketch"))
                {
                    data.DrivingSketchName = subFeat.Name;
                    break;
                }
                subFeat = subFeat.GetNextSubFeature() as IFeature;
            }

            return data;
        }
    }
}

// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from base flange features
    /// </summary>
    public class BaseFlangeExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new base flange extractor
        /// </summary>
        public BaseFlangeExtractor() : base("SMBaseFlange", "SheetMetal", "BaseFlange")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "SMBaseFlange";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new BaseFlangeFeatureData();
            PopulateBaseData(data, feature);

            var baseFlangeData = feature.GetDefinition() as IBaseFlangeFeatureData;
            if (baseFlangeData == null)
            {
                data.Errors.Add("Failed to get base flange feature definition");
                return data;
            }

            SafeExtract(data, () => data.Thickness = baseFlangeData.Thickness, "Thickness");
            SafeExtract(data, () => data.BendRadius = baseFlangeData.BendRadius, "BendRadius");
            SafeExtract(data, () => data.ReverseDirection = baseFlangeData.ReverseDirection, "ReverseDirection");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }

    /// <summary>
    /// Extracts data from edge flange features
    /// </summary>
    public class EdgeFlangeExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new edge flange extractor
        /// </summary>
        public EdgeFlangeExtractor() : base("EdgeFlange", "SM Edge Flange")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "EdgeFlange";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new EdgeFlangeFeatureData();
            PopulateBaseData(data, feature);

            var edgeFlangeData = feature.GetDefinition() as IEdgeFlangeFeatureData;
            if (edgeFlangeData == null)
            {
                data.Errors.Add("Failed to get edge flange feature definition");
                return data;
            }

            SafeExtract(data, () => data.GapDistance = edgeFlangeData.GapDistance, "GapDistance");
            SafeExtract(data, () => data.OffsetDistance = edgeFlangeData.OffsetDistance, "OffsetDistance");
            SafeExtract(data, () => data.UseCustomRelief = edgeFlangeData.UseReliefRatio, "UseCustomRelief");

            // Custom bend radius
            SafeExtract(data, () =>
            {
                if (!edgeFlangeData.UseDefaultBendRadius)
                {
                    data.CustomBendRadius = edgeFlangeData.BendRadius;
                }
            }, "CustomBendRadius");

            SafeExtract(data, () =>
            {
                var edges = edgeFlangeData.Edges;
                data.EdgeCount = edges != null ? ((object[])edges).Length : 0;
            }, "EdgeCount");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from hem features
    /// </summary>
    public class HemExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new hem extractor
        /// </summary>
        public HemExtractor() : base("Hem", "SMHem")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Hem";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new HemFeatureData();
            PopulateBaseData(data, feature);

            var hemData = feature.GetDefinition() as IHemFeatureData;
            if (hemData == null)
            {
                data.Errors.Add("Failed to get hem feature definition");
                return data;
            }

            SafeExtract(data, () => data.HemType = hemData.Type, "HemType");
            SafeExtract(data, () => data.Length = hemData.Length, "Length");
            SafeExtract(data, () => data.Radius = hemData.Radius, "Radius");
            SafeExtract(data, () => data.Angle = hemData.Angle, "Angle");
            SafeExtract(data, () => data.ReverseDirection = hemData.ReverseDirection, "ReverseDirection");

            SafeExtract(data, () =>
            {
                var edges = hemData.Edges;
                data.EdgeCount = edges != null ? ((object[])edges).Length : 0;
            }, "EdgeCount");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from sketched bend features
    /// </summary>
    public class SketchedBendExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new sketched bend extractor
        /// </summary>
        public SketchedBendExtractor() : base("SketchBend", "SketchedBend", "SM Sketched Bend")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "SketchBend";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new SketchedBendFeatureData();
            PopulateBaseData(data, feature);

            var sketchBendData = feature.GetDefinition() as ISketchedBendFeatureData;
            if (sketchBendData == null)
            {
                data.Errors.Add("Failed to get sketched bend feature definition");
                return data;
            }

            SafeExtract(data, () => data.BendRadius = sketchBendData.BendRadius, "BendRadius");
            SafeExtract(data, () => data.UseDefaultRadius = sketchBendData.UseDefaultBendRadius, "UseDefaultRadius");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }

    /// <summary>
    /// Extracts data from jog features
    /// </summary>
    public class JogExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new jog extractor
        /// </summary>
        public JogExtractor() : base("Jog", "SMJog", "SM Jog")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Jog";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new JogFeatureData();
            PopulateBaseData(data, feature);

            var jogData = feature.GetDefinition() as IJogFeatureData;
            if (jogData == null)
            {
                data.Errors.Add("Failed to get jog feature definition");
                return data;
            }

            SafeExtract(data, () => data.UseDefaultBendRadius = jogData.UseDefaultBendRadius, "UseDefaultBendRadius");
            SafeExtract(data, () => data.BendRadius = jogData.BendRadius, "BendRadius");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }

    /// <summary>
    /// Extracts data from miter flange features
    /// </summary>
    public class MiterFlangeExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new miter flange extractor
        /// </summary>
        public MiterFlangeExtractor() : base("MiterFlange", "SMMiterFlange", "SM Miter Flange")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "MiterFlange";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new MiterFlangeFeatureData();
            PopulateBaseData(data, feature);

            var miterData = feature.GetDefinition() as IMiterFlangeFeatureData;
            if (miterData == null)
            {
                data.Errors.Add("Failed to get miter flange feature definition");
                return data;
            }

            SafeExtract(data, () => data.GapDistance = miterData.GapDistance, "GapDistance");
            SafeExtract(data, () => data.UseDefaultBendRadius = miterData.UseDefaultBendRadius, "UseDefaultBendRadius");
            SafeExtract(data, () => data.CustomBendRadius = miterData.BendRadius, "CustomBendRadius");
            SafeExtract(data, () => data.StartOffset = miterData.StartOffset, "StartOffset");
            SafeExtract(data, () => data.EndOffset = miterData.EndOffset, "EndOffset");

            SafeExtract(data, () =>
            {
                var edges = miterData.Edges;
                data.EdgeCount = edges != null ? ((object[])edges).Length : 0;
            }, "EdgeCount");

            return data;
        }
    }
}

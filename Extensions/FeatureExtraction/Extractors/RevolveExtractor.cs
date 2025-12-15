// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from revolve features
    /// </summary>
    public class RevolveExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new revolve extractor
        /// </summary>
        public RevolveExtractor() : base("Revolution", "Revolve", "Boss-Revolve")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Revolution";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new RevolveFeatureData();
            PopulateBaseData(data, feature);

            var revolveData = feature.GetDefinition() as RevolveFeatureData2;
            if (revolveData == null)
            {
                data.Errors.Add("Failed to get revolve feature definition");
                return data;
            }

            SafeExtract(data, () => data.Angle1 = revolveData.GetRevolutionAngle(true), "Angle1");
            SafeExtract(data, () => data.Angle2 = revolveData.GetRevolutionAngle(false), "Angle2");
            SafeExtract(data, () => data.MergeResult = revolveData.Merge, "MergeResult");
            SafeExtract(data, () => data.RevolveType = (swRevolveType_e)revolveData.Type, "RevolveType");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }

    /// <summary>
    /// Extracts data from cut revolve features
    /// </summary>
    public class CutRevolveExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new cut revolve extractor
        /// </summary>
        public CutRevolveExtractor() : base("RevCut", "Cut-Revolve")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "RevCut";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new CutRevolveFeatureData();
            PopulateBaseData(data, feature);

            var revolveData = feature.GetDefinition() as RevolveFeatureData2;
            if (revolveData == null)
            {
                data.Errors.Add("Failed to get cut revolve feature definition");
                return data;
            }

            SafeExtract(data, () => data.Angle1 = revolveData.GetRevolutionAngle(true), "Angle1");
            SafeExtract(data, () => data.Angle2 = revolveData.GetRevolutionAngle(false), "Angle2");
            SafeExtract(data, () => data.FeatureScope = revolveData.FeatureScope, "FeatureScope");
            SafeExtract(data, () => data.AutoSelect = revolveData.AutoSelect, "AutoSelect");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }
}

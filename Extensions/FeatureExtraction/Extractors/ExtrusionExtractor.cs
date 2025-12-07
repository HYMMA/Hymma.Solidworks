// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from boss/base extrusion features
    /// </summary>
    public class ExtrusionExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new extrusion extractor
        /// </summary>
        public ExtrusionExtractor() : base("Extrusion", "Boss-Extrude", "Extrude")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Extrusion";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new ExtrusionFeatureData();
            PopulateBaseData(data, feature);

            var extrudeData = feature.GetDefinition() as ExtrudeFeatureData2;
            if (extrudeData == null)
            {
                data.Errors.Add("Failed to get extrusion feature definition");
                return data;
            }

            SafeExtract(data, () => data.Depth1 = extrudeData.GetDepth(true), "Depth1");
            SafeExtract(data, () => data.Depth2 = extrudeData.GetDepth(false), "Depth2");
            SafeExtract(data, () => data.EndCondition1 = (swEndConditions_e)extrudeData.GetEndCondition(true), "EndCondition1");
            SafeExtract(data, () => data.EndCondition2 = (swEndConditions_e)extrudeData.GetEndCondition(false), "EndCondition2");
            SafeExtract(data, () => data.DraftAngle1 = extrudeData.GetDraftAngle(true), "DraftAngle1");
            SafeExtract(data, () => data.DraftAngle2 = extrudeData.GetDraftAngle(false), "DraftAngle2");
            SafeExtract(data, () => data.DraftOutward1 = extrudeData.GetDraftOutward(true), "DraftOutward1");
            SafeExtract(data, () => data.DraftOutward2 = extrudeData.GetDraftOutward(false), "DraftOutward2");
            SafeExtract(data, () => data.MergeResult = extrudeData.Merge, "MergeResult");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }

    /// <summary>
    /// Extracts data from cut extrusion features
    /// </summary>
    public class CutExtrusionExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new cut extrusion extractor
        /// </summary>
        public CutExtrusionExtractor() : base("Cut-Extrude", "Cut", "ICE")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Cut-Extrude";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new CutExtrusionFeatureData();
            PopulateBaseData(data, feature);

            var extrudeData = feature.GetDefinition() as ExtrudeFeatureData2;
            if (extrudeData == null)
            {
                data.Errors.Add("Failed to get cut extrusion feature definition");
                return data;
            }

            SafeExtract(data, () => data.Depth1 = extrudeData.GetDepth(true), "Depth1");
            SafeExtract(data, () => data.Depth2 = extrudeData.GetDepth(false), "Depth2");
            SafeExtract(data, () => data.EndCondition1 = (swEndConditions_e)extrudeData.GetEndCondition(true), "EndCondition1");
            SafeExtract(data, () => data.EndCondition2 = (swEndConditions_e)extrudeData.GetEndCondition(false), "EndCondition2");
            SafeExtract(data, () => data.DraftAngle1 = extrudeData.GetDraftAngle(true), "DraftAngle1");
            SafeExtract(data, () => data.DraftAngle2 = extrudeData.GetDraftAngle(false), "DraftAngle2");
            SafeExtract(data, () => data.DraftOutward1 = extrudeData.GetDraftOutward(true), "DraftOutward1");
            SafeExtract(data, () => data.DraftOutward2 = extrudeData.GetDraftOutward(false), "DraftOutward2");
            SafeExtract(data, () => data.FeatureScope = extrudeData.FeatureScope, "FeatureScope");
            SafeExtract(data, () => data.AutoSelect = extrudeData.AutoSelect, "AutoSelect");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }
}

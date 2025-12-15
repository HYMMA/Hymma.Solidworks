// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from hole wizard features
    /// </summary>
    public class HoleWizardExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new hole wizard extractor
        /// </summary>
        public HoleWizardExtractor() : base("HoleWzd", "HoleWizard", "Hole Wizard")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "HoleWzd";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new HoleWizardFeatureData();
            PopulateBaseData(data, feature);

            var holeData = feature.GetDefinition() as IWizardHoleFeatureData2;
            if (holeData == null)
            {
                data.Errors.Add("Failed to get hole wizard feature definition");
                return data;
            }

            SafeExtract(data, () => data.HoleType = (swWzdHoleTypes_e)holeData.Type, "HoleType");
            SafeExtract(data, () => data.Diameter = holeData.HoleDiameter, "Diameter");
            SafeExtract(data, () => data.Depth = holeData.HoleDepth, "Depth");
            SafeExtract(data, () => data.EndCondition = (swEndConditions_e)holeData.EndCondition, "EndCondition");

            // Thread settings
            SafeExtract(data, () => data.ThreadDepth = holeData.ThreadDepth, "ThreadDepth");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from simple hole features
    /// </summary>
    public class SimpleHoleExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new simple hole extractor
        /// </summary>
        public SimpleHoleExtractor() : base("Hole", "SimpleHole")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Hole";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new SimpleHoleFeatureData();
            PopulateBaseData(data, feature);

            var holeData = feature.GetDefinition() as ISimpleHoleFeatureData2;
            if (holeData == null)
            {
                data.Errors.Add("Failed to get simple hole feature definition");
                return data;
            }

            SafeExtract(data, () => data.Diameter = holeData.Diameter, "Diameter");
            SafeExtract(data, () => data.Depth = holeData.Depth, "Depth");
            SafeExtract(data, () => data.DraftAngle = holeData.DraftAngle, "DraftAngle");
            SafeExtract(data, () => data.DraftOutward = holeData.DraftOutward, "DraftOutward");

            return data;
        }
    }
}

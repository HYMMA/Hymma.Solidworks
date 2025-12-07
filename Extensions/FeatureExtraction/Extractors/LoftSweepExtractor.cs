// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from loft features
    /// </summary>
    public class LoftExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new loft extractor
        /// </summary>
        public LoftExtractor() : base("Loft", "Boss-Loft", "LoftBase")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Loft";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new LoftFeatureData();
            PopulateBaseData(data, feature);

            var loftData = feature.GetDefinition() as ILoftFeatureData;
            if (loftData == null)
            {
                data.Errors.Add("Failed to get loft feature definition");
                return data;
            }

            SafeExtract(data, () =>
            {
                var profiles = loftData.Profiles;
                data.ProfileCount = profiles != null ? ((object[])profiles).Length : 0;
            }, "ProfileCount");

            SafeExtract(data, () =>
            {
                var guides = loftData.GuideCurves;
                data.GuideCurveCount = guides != null ? ((object[])guides).Length : 0;
            }, "GuideCurveCount");

            SafeExtract(data, () => data.MergeResult = loftData.Merge, "MergeResult");
            SafeExtract(data, () => data.Closed = loftData.Close, "Closed");
            SafeExtract(data, () => data.MaintainTangency = loftData.MaintainTangency, "MaintainTangency");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from sweep features
    /// </summary>
    public class SweepExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new sweep extractor
        /// </summary>
        public SweepExtractor() : base("Sweep", "Boss-Sweep", "SweepBase")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Sweep";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new SweepFeatureData();
            PopulateBaseData(data, feature);

            var sweepData = feature.GetDefinition() as ISweepFeatureData;
            if (sweepData == null)
            {
                data.Errors.Add("Failed to get sweep feature definition");
                return data;
            }

            SafeExtract(data, () => data.MergeResult = sweepData.Merge, "MergeResult");
            SafeExtract(data, () => data.MaintainTangency = sweepData.MaintainTangency, "MaintainTangency");
            SafeExtract(data, () => data.OrientationType = (swTwistControlType_e)sweepData.TwistControlType, "OrientationType");

            SafeExtract(data, () =>
            {
                var guides = sweepData.GuideCurves;
                data.GuideCurveCount = guides != null ? ((object[])guides).Length : 0;
            }, "GuideCurveCount");

            // Get profile and path sketch names from sub-features
            var subFeat = feature.GetFirstSubFeature() as IFeature;
            while (subFeat != null)
            {
                var typeName = subFeat.GetTypeName2();
                if (typeName == "ProfileFeature" || typeName.Contains("Sketch"))
                {
                    if (string.IsNullOrEmpty(data.ProfileSketchName))
                        data.ProfileSketchName = subFeat.Name;
                    else if (string.IsNullOrEmpty(data.PathSketchName))
                        data.PathSketchName = subFeat.Name;
                }
                subFeat = subFeat.GetNextSubFeature() as IFeature;
            }

            return data;
        }
    }

    /// <summary>
    /// Extracts data from cut loft features
    /// </summary>
    public class CutLoftExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new cut loft extractor
        /// </summary>
        public CutLoftExtractor() : base("LoftCut", "Cut-Loft")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "LoftCut";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new CutLoftFeatureData();
            PopulateBaseData(data, feature);

            var loftData = feature.GetDefinition() as ILoftFeatureData;
            if (loftData == null)
            {
                data.Errors.Add("Failed to get cut loft feature definition");
                return data;
            }

            SafeExtract(data, () =>
            {
                var profiles = loftData.Profiles;
                data.ProfileCount = profiles != null ? ((object[])profiles).Length : 0;
            }, "ProfileCount");

            SafeExtract(data, () =>
            {
                var guides = loftData.GuideCurves;
                data.GuideCurveCount = guides != null ? ((object[])guides).Length : 0;
            }, "GuideCurveCount");

            SafeExtract(data, () => data.Closed = loftData.Close, "Closed");
            SafeExtract(data, () => data.FeatureScope = loftData.FeatureScope, "FeatureScope");
            SafeExtract(data, () => data.AutoSelect = loftData.AutoSelect, "AutoSelect");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from cut sweep features
    /// </summary>
    public class CutSweepExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new cut sweep extractor
        /// </summary>
        public CutSweepExtractor() : base("SweepCut", "Cut-Sweep")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "SweepCut";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new CutSweepFeatureData();
            PopulateBaseData(data, feature);

            var sweepData = feature.GetDefinition() as ISweepFeatureData;
            if (sweepData == null)
            {
                data.Errors.Add("Failed to get cut sweep feature definition");
                return data;
            }

            SafeExtract(data, () => data.MaintainTangency = sweepData.MaintainTangency, "MaintainTangency");
            SafeExtract(data, () => data.OrientationType = (swTwistControlType_e)sweepData.TwistControlType, "OrientationType");
            SafeExtract(data, () => data.FeatureScope = sweepData.FeatureScope, "FeatureScope");
            SafeExtract(data, () => data.AutoSelect = sweepData.AutoSelect, "AutoSelect");

            return data;
        }
    }
}

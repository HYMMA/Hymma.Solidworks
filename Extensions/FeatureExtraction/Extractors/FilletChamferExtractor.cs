// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from fillet features
    /// </summary>
    public class FilletExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new fillet extractor
        /// </summary>
        public FilletExtractor() : base("Fillet", "ConstRadiusFillet", "VarRadiusFillet", "FullRoundFillet", "SimpleFillet")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Fillet";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new FilletFeatureData();
            PopulateBaseData(data, feature);

            var filletData = feature.GetDefinition() as ISimpleFilletFeatureData2;
            if (filletData == null)
            {
                data.Errors.Add("Failed to get fillet feature definition");
                return data;
            }

            SafeExtract(data, () => data.FilletType = (swFeatureFilletType_e)filletData.Type, "FilletType");
            SafeExtract(data, () => data.DefaultRadius = filletData.DefaultRadius, "DefaultRadius");
            SafeExtract(data, () => data.PropagateToTangentFaces = filletData.PropagateToTangentFaces, "PropagateToTangentFaces");
            SafeExtract(data, () => data.IsAsymmetric = filletData.AsymmetricFillet, "IsAsymmetric");
            SafeExtract(data, () => data.CurvatureContinuous = filletData.CurvatureContinuous, "CurvatureContinuous");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from chamfer features
    /// </summary>
    public class ChamferExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new chamfer extractor
        /// </summary>
        public ChamferExtractor() : base("Chamfer")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Chamfer";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new ChamferFeatureData();
            PopulateBaseData(data, feature);

            var chamferData = feature.GetDefinition() as IChamferFeatureData2;
            if (chamferData == null)
            {
                data.Errors.Add("Failed to get chamfer feature definition");
                return data;
            }

            SafeExtract(data, () => data.ChamferType = (swChamferType_e)chamferData.Type, "ChamferType");
            // Note: Distance and Angle are not directly available in SW2018 IChamferFeatureData2
            // The values need to be accessed through feature dimensions

            return data;
        }
    }
}

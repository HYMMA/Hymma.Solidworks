// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions.FeatureExtraction.Extractors
{
    /// <summary>
    /// Extracts data from shell features
    /// </summary>
    public class ShellExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new shell extractor
        /// </summary>
        public ShellExtractor() : base("Shell", "Shelling")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Shell";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new ShellFeatureData();
            PopulateBaseData(data, feature);

            var shellData = feature.GetDefinition() as IShellFeatureData;
            if (shellData == null)
            {
                data.Errors.Add("Failed to get shell feature definition");
                return data;
            }

            SafeExtract(data, () => data.Thickness = shellData.Thickness, "Thickness");

            SafeExtract(data, () =>
            {
                var faces = shellData.FacesRemoved;
                data.RemovedFaceCount = faces != null ? ((object[])faces).Length : 0;
            }, "RemovedFaceCount");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from draft features
    /// </summary>
    public class DraftExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new draft extractor
        /// </summary>
        public DraftExtractor() : base("Draft")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Draft";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new DraftFeatureData();
            PopulateBaseData(data, feature);

            var draftData = feature.GetDefinition() as IDraftFeatureData2;
            if (draftData == null)
            {
                data.Errors.Add("Failed to get draft feature definition");
                return data;
            }

            SafeExtract(data, () => data.Angle = draftData.Angle, "Angle");
            SafeExtract(data, () => data.DraftType = (swDraftType_e)draftData.Type, "DraftType");
            SafeExtract(data, () => data.ReverseDirection = draftData.ReverseDirection, "ReverseDirection");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from rib features
    /// </summary>
    public class RibExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new rib extractor
        /// </summary>
        public RibExtractor() : base("Rib")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Rib";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new RibFeatureData();
            PopulateBaseData(data, feature);

            var ribData = feature.GetDefinition() as IRibFeatureData;
            if (ribData == null)
            {
                data.Errors.Add("Failed to get rib feature definition");
                return data;
            }

            SafeExtract(data, () => data.Thickness = ribData.Thickness, "Thickness");
            SafeExtract(data, () => data.FlipMaterial = ribData.FlipSide, "FlipMaterial");
            SafeExtract(data, () => data.DraftAngle = ribData.DraftAngle, "DraftAngle");
            SafeExtract(data, () => data.DraftOutward = ribData.DraftOutward, "DraftOutward");

            data.SketchName = GetSketchName(feature);

            return data;
        }
    }

    /// <summary>
    /// Extracts data from reference plane features
    /// </summary>
    public class ReferencePlaneExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new reference plane extractor
        /// </summary>
        public ReferencePlaneExtractor() : base("RefPlane", "Plane")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "RefPlane";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new ReferencePlaneFeatureData();
            PopulateBaseData(data, feature);

            var planeData = feature.GetDefinition() as IRefPlaneFeatureData;
            if (planeData == null)
            {
                data.Errors.Add("Failed to get reference plane feature definition");
                return data;
            }

            SafeExtract(data, () => data.ConstraintType = (swRefPlaneReferenceConstraints_e)planeData.Type, "ConstraintType");
            SafeExtract(data, () => data.OffsetDistance = planeData.Distance, "OffsetDistance");
            SafeExtract(data, () => data.Angle = planeData.Angle, "Angle");
            SafeExtract(data, () => data.Reversed = planeData.ReverseDirection, "Reversed");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from sketch features
    /// </summary>
    public class SketchExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new sketch extractor
        /// </summary>
        public SketchExtractor() : base("ProfileFeature", "3DProfileFeature", "Sketch")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "ProfileFeature";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new SketchFeatureData();
            PopulateBaseData(data, feature);

            var sketch = feature.GetSpecificFeature2() as ISketch;
            if (sketch == null)
            {
                data.Errors.Add("Failed to get sketch from feature");
                return data;
            }

            SafeExtract(data, () =>
            {
                var constraintStatus = sketch.GetConstrainedStatus();
                // 0 = fully constrained, 1 = under-constrained, 2 = over-constrained
                data.FullyConstrained = constraintStatus == 0;
            }, "FullyConstrained");

            SafeExtract(data, () =>
            {
                var contours = sketch.GetSketchContours() as object[];
                data.ContourCount = contours?.Length ?? 0;
            }, "ContourCount");

            return data;
        }
    }

    /// <summary>
    /// Extracts data from combine features
    /// </summary>
    public class CombineExtractor : FeatureExtractorBase
    {
        /// <summary>
        /// Creates a new combine extractor
        /// </summary>
        public CombineExtractor() : base("Combine", "CombineBodies")
        {
        }

        /// <inheritdoc/>
        public override string FeatureTypeName => "Combine";

        /// <inheritdoc/>
        public override FeatureDataBase Extract(IFeature feature)
        {
            var data = new CombineFeatureData();
            PopulateBaseData(data, feature);

            var combineData = feature.GetDefinition() as ICombineBodiesFeatureData;
            if (combineData == null)
            {
                data.Errors.Add("Failed to get combine feature definition");
                return data;
            }

            SafeExtract(data, () => data.OperationType = combineData.OperationType, "OperationType");

            SafeExtract(data, () =>
            {
                var mainBody = combineData.MainBody as IBody2;
                if (mainBody != null)
                {
                    data.MainBodyName = mainBody.Name;
                }
            }, "MainBodyName");

            SafeExtract(data, () =>
            {
                var bodies = combineData.BodiesToCombine as object[];
                if (bodies != null)
                {
                    foreach (IBody2 body in bodies)
                    {
                        data.CombinedBodyNames.Add(body.Name);
                    }
                }
            }, "CombinedBodyNames");

            return data;
        }
    }
}

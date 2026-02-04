// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing;
using System.IO;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ModelDoc2"/> objects providing common operations
    /// for parts, assemblies, and drawings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These extensions simplify common SolidWorks API operations such as:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Configuration management</description></item>
    ///   <item><description>Graphics performance optimization (freeze/unfreeze)</description></item>
    ///   <item><description>Custom property access</description></item>
    ///   <item><description>Mass properties retrieval</description></item>
    ///   <item><description>Unit conversions</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para>Common usage patterns:</para>
    /// <code>
    /// ModelDoc2 model = solidworks.ActiveDoc as ModelDoc2;
    ///
    /// // Switch configuration
    /// model.ActivateConfiguration("Config2");
    ///
    /// // Get custom property
    /// string partNumber = model.GetCustomProperty("PartNumber");
    ///
    /// // Optimize performance for batch operations
    /// model.Freeze();
    /// try
    /// {
    ///     // Perform many operations...
    /// }
    /// finally
    /// {
    ///     model.UnFreeze();
    /// }
    /// </code>
    /// </example>
    public static class ModelDoc2Extensions
    {
        /// <summary>
        /// Activates (switches to) a specified configuration in the model.
        /// </summary>
        /// <param name="model">The model document.</param>
        /// <param name="configurationName">The name of the configuration to activate.</param>
        /// <returns>
        /// <c>true</c> if the configuration was successfully activated or is already active;
        /// <c>false</c> if the configuration could not be activated.
        /// </returns>
        /// <example>
        /// <code>
        /// if (model.ActivateConfiguration("Machined"))
        /// {
        ///     // Configuration is now active, proceed with operations
        /// }
        /// </code>
        /// </example>
        public static bool ActivateConfiguration(this ModelDoc2 model, string configurationName)
        {
            //activate the configuration
            string activeConfiguration = model.ConfigurationManager.ActiveConfiguration.Name;
            if (activeConfiguration != configurationName && !model.ShowConfiguration2(configurationName))
                return false;
            return true;
        }

        /// <summary>
        /// Renders the active view to a bitmap using SOLIDWORKS image export.
        /// </summary>
        /// <param name="model">The active model document.</param>
        /// <returns>A bitmap containing the rendered view.</returns>
        /// <remarks>
        /// This uses the current active view and therefore reflects real model colors/materials.
        /// The model must be open for accurate rendering.
        /// </remarks>
        public static Bitmap RenderActiveViewToBitmap(this ModelDoc2 model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var docPath = model.GetPathName();
            var name = string.IsNullOrWhiteSpace(docPath) ? "model" : Path.GetFileNameWithoutExtension(docPath);
            var tempPath = Path.Combine(Path.GetTempPath(), $"{name}_{Guid.NewGuid():N}.png");

            int errors = 0;
            int warnings = 0;
            object data = null;
            try
            {
                // Normalize view before capture
                model.ShowNamedView2("*Isometric", (int)swStandardViews_e.swIsometricView);
                model.ViewZoomtofit2();

                var saved = model.Extension.SaveAs(
                    tempPath,
                    (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    (int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                    data,
                    ref errors,
                    ref warnings);

                if (!saved || !File.Exists(tempPath))
                    throw new InvalidOperationException($"Could not render view for {name}. SaveAs errors={errors}, warnings={warnings}.");

                using (var image = Image.FromFile(tempPath))
                {
                    return new Bitmap(image);
                }
            }
            finally
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }

        /// <summary>
        /// Freezes graphics updates and feature tree to improve performance during batch operations.
        /// </summary>
        /// <param name="model">The model document to freeze.</param>
        /// <remarks>
        /// <para>
        /// Freezing disables graphics updates and feature tree refreshes, which can significantly
        /// improve performance when performing many operations in sequence.
        /// </para>
        /// <para>
        /// <b>Important:</b> Always call <see cref="UnFreeze"/> in a finally block to ensure
        /// the UI is restored even if an exception occurs.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// model.Freeze();
        /// try
        /// {
        ///     // Perform batch operations (add features, modify dimensions, etc.)
        ///     for (int i = 0; i &lt; 100; i++)
        ///     {
        ///         // Operations here run much faster with graphics frozen
        ///     }
        /// }
        /// finally
        /// {
        ///     model.UnFreeze();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="UnFreeze"/>
        public static void Freeze(this ModelDoc2 model)
        {
            var modelView = model.ActiveView as ModelView;
            model.FeatureManager.EnableFeatureTree = false;
            model.FeatureManager.EnableFeatureTreeWindow = false;
            modelView.EnableGraphicsUpdate = false;
        }

        /// <summary>
        /// Restores graphics updates and feature tree after a <see cref="Freeze"/> operation.
        /// </summary>
        /// <param name="model">The model document to unfreeze.</param>
        /// <remarks>
        /// Call this method after completing batch operations to restore normal UI behavior.
        /// The graphics view will be refreshed automatically.
        /// </remarks>
        /// <seealso cref="Freeze"/>
        public static void UnFreeze(this ModelDoc2 model)
        {
            var modelView = model.ActiveView as ModelView;
            model.FeatureManager.EnableFeatureTree = true;
            model.FeatureManager.EnableFeatureTreeWindow = true;
            modelView.EnableGraphicsUpdate = true;
        }
        
        /// <summary>
        /// Gets the custom properties for this document or configuration
        /// <br/>File custom information is stored in the document file. It can be:
        /// <list type="bullet">
        /// <item><description>General to the file, in which case there is a single value whatever the model's configuration</description></item>
        /// <item><description>Configuration-specific, in which case a different value may be set for each configuration in the model</description></item>
        /// </list>
        /// <para>To access a general custom
        /// information value, set the configuration argument 
        /// to an empty string. To get a document-level property, 
        /// pass an empty string ("") to the configuration argument.
        /// </para>
        /// </summary>
        /// <param name="modelDoc"></param>
        /// <param name="property"></param>
        /// <param name="configuration"></param>
        /// <param name="useCachedData"></param>
        /// <returns></returns>
        public static string GetCustomProperty(this ModelDoc2 modelDoc, string property, string configuration = "", bool useCachedData = false)
        {
            ModelDocExtension modelDocExtension = modelDoc.Extension;
            CustomPropertyManager swCustProp = modelDocExtension.CustomPropertyManager[configuration];
            int result = swCustProp.Get5(property, useCachedData, out string valOut, out string resolvedVal, out bool wasResolved);
            if (result == (int)swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent)
                return "";
            return resolvedVal;
        }

        /// <summary>
        /// Use this method to get component mass properties 
        /// <para>use index (base zero) to retrieve a specific value</para>
        /// </summary>
        /// <returns>
        /// The return value is an array of doubles as follows:
        ///<list type="bullet">
        /// <item>
        /// <term>Solid body</term>
        /// <description>[ CenterOfMassX, CenterOfMassY, CenterOfMassZ, Volume, Area, Mass(Volume*density), MomXX, MomYY, MomZZ, MomXY, MomZX, MomYZ ]</description>
        /// </item>
        ///  <item>
        /// <term>Sheet body</term>
        /// <description>[ CenterOfMassX, CenterOfMassY, CenterOfMassZ, Area, Circumference, Mass(Area*density), MomXX, MomYY, MomZZ, MomXY, MomZX, MomYZ ]</description>
        /// </item>
        /// <item>
        /// <term>
        /// Wire body
        /// </term>
        /// <description>
        /// [ CenterOfMassX, CenterOfMassY, CenterOfMassZ, Length, 0, Mass(Length*density), MomXX, MomYY, MomZZ, MomXY, MomZX, MomYZ ]
        /// </description>
        /// </item>
        /// </list></returns>
        public static double GetMassProperties(this ModelDoc2 model, Body2 body, int index)
        {
            var nDensity = model.GetUserPreferenceDoubleValue((int)swUserPreferenceDoubleValue_e.swMaterialPropertyDensity);
            var properties = body.GetMassProperties(nDensity) as double[];
            return properties[index];
        }

        /// <summary>
        /// returns units of length in string format
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetLengthUnitName(this ModelDoc2 model)
        {
            switch (model.LengthUnit)
            {
                case (int)swLengthUnit_e.swMM:
                    return "mm";
                case (int)swLengthUnit_e.swCM:
                    return "cm";
                case (int)swLengthUnit_e.swMETER:
                    return "m";
                case (int)swLengthUnit_e.swINCHES:
                    return "in";
                case (int)swLengthUnit_e.swFEET:
                    return "ft";
                case (int)swLengthUnit_e.swFEETINCHES:
                    return "ft-in";
                case (int)swLengthUnit_e.swANGSTROM:
                    return "angstorm";
                case (int)swLengthUnit_e.swNANOMETER:
                    return "Nano-Meter";
                case (int)swLengthUnit_e.swMICRON:
                    return "Micro-Meter";
                case (int)swLengthUnit_e.swMIL:
                    return "mil";
                case (int)swLengthUnit_e.swUIN:
                    return "uin";
                default:
                    return "mm";
            }
        }
    }
}

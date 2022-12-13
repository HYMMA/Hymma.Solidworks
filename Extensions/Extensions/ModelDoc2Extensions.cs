// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// extensions for a <see cref="ModelDoc2"/> object
    /// </summary>
    public static class ModelDoc2Extensions
    {
        /// <summary>
        /// switch to a new a model configuration
        /// </summary>
        /// <param name="model"></param>
        /// <param name="configurationName">new configuration</param>
        /// <returns></returns> 
        public static bool ActivateConfiguration(this ModelDoc2 model, string configurationName)
        {
            //activate the configuration
            string activeConfiguration = model.ConfigurationManager.ActiveConfiguration.Name;
            if (activeConfiguration != configurationName && !model.ShowConfiguration2(configurationName))
                return false;
            return true;
        }

        /// <summary>
        /// freeze graphics in a model, to increase processing power
        /// </summary>
        /// <param name="model"></param>
        public static void Freez(this ModelDoc2 model)
        {
            var modelView = model.ActiveView as ModelView;
            model.FeatureManager.EnableFeatureTree = false;
            model.FeatureManager.EnableFeatureTreeWindow = false;
            modelView.EnableGraphicsUpdate = false;
        }

        /// <summary>
        /// UN-freeze graphics in a model
        /// </summary>
        /// <param name="model"></param>
        public static void UnFreez(this ModelDoc2 model)
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

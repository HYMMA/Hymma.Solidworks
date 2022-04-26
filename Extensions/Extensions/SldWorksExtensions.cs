using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// extensions for <see cref="SldWorks"/>
    /// </summary>
    public static class SldWorksExtensions
    {
        /// <summary>
        /// get the default map file in solidworks, does not check if map file actually exists on HDD
        /// </summary>
        /// <returns>address of map file as set in solidworks settings</returns>
        public static string GetDefaultMapFilePath(this SldWorks solidworks)
        {
            //get list of mapping files from solidworks
            string mappingFiles =
                solidworks.GetUserPreferenceStringListValue((int)swUserPreferenceStringListValue_e.swDxfMappingFiles);

            //get index of mapping file used
            int index =
                solidworks.GetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swDxfMappingFileIndex);

            //if map file exists but is not enabled index is -1
            if (mappingFiles == "" || index == -1)
                return "";

            //split list and get the address of each mapping file
            string[] mappingFilesList = mappingFiles.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            //get the file from list 
            return mappingFilesList[index];
        }

        /// <summary>
        /// opens and activates a specific solidworks document
        /// </summary>
        /// <returns></returns>
        public static ModelDoc2 GetModel(this SldWorks solidworks, string modelPathName, string configuration)
        {
            //Determine the type of SOLIDWORKS file based on
            // its filename extension
            var doctype = solidworks.GetModelType(modelPathName);
            if (string.IsNullOrEmpty(configuration) && doctype != swDocumentTypes_e.swDocDRAWING)
                throw new Exception("A configuration was empty so we could not open the document");

            int error = 0;
            int warning = 0;
            return solidworks.OpenDoc6(modelPathName
                , ((int)doctype)
                , (int)swOpenDocOptions_e.swOpenDocOptions_Silent
                , configuration
                , ref error, ref warning);
        }

        /// <summary>
        /// get the type of a solidworks document its extension name
        /// </summary>
        /// <param name="solidworks"></param>
        /// <param name="modelPathName">full file name of the document including the extensions</param>
        /// <returns></returns>
        public static swDocumentTypes_e GetModelType(this SldWorks solidworks, string modelPathName)
        {
            if (String.IsNullOrEmpty(modelPathName))
                throw new Exception("A file name was empty so we could not open it");
            var extension = Path.GetExtension(modelPathName);
            swDocumentTypes_e doctype;
            switch (extension.ToLower())
            {
                case ".sldprt":
                    doctype = swDocumentTypes_e.swDocPART;
                    break;
                case ".sldasm":
                    doctype = swDocumentTypes_e.swDocASSEMBLY;
                    break;
                case ".slddrw":
                    doctype = swDocumentTypes_e.swDocDRAWING;
                    break;

                default:
                    doctype = swDocumentTypes_e.swDocNONE;
                    break;
            }
            return doctype;
        }

        /// <summary>
        /// sets visibility of solidworks to false <br/>
        /// useful when you want to activate a document in silent mode<br/>
        /// </summary>
        /// <param name="solidworks"></param>
        /// <returns>true if successful and false if not
        /// <br/> make sure you set solidworks visibility to true at the end of operation <see cref="UnFreezGraphics(SldWorks)"/>
        /// </returns>
        public static bool FreezGraphics(this SldWorks solidworks)
        {
            try
            {
                // Allow SOLIDWORKS to run in the background
                // and be invisible
                solidworks.UserControlBackground = false;


                // If the following property is true, then the
                // SOLIDWORKS frame will be visible on a call to
                // ISldWorks::ActivateDoc2; so, set it to false
                solidworks.Visible = false;


                // Keep SOLIDWORKS frame invisible when
                // ISldWorks::ActivateDoc2 is called
                var frame = (Frame)solidworks.Frame();
                frame.KeepInvisible = true;
                return (frame.KeepInvisible && !solidworks.Visible && !solidworks.UserControlBackground);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// unfreezes the graphics of the solidworks application
        /// </summary>
        /// <param name="solidworks"></param>
        public static void UnFreezGraphics(this SldWorks solidworks)
        {
            var frame = solidworks.Frame() as Frame;
            if (frame == null)
                return;
            frame.KeepInvisible = false;
            solidworks.Visible = true;
        }

        /// <summary>
        /// freezes graphics during an <see cref="Action"/> and unfreeze it afterwards
        /// </summary>
        /// <param name="solidworks"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void FreezGraphics(this SldWorks solidworks, Action action)
        {
            solidworks.FreezGraphics();
            action.Invoke();
            solidworks.UnFreezGraphics();
        }

        ///// <summary>
        ///// create, start, update, and stop a progress indicator on the system task bar
        ///// </summary>
        ///// <param name = "solidworks" ></ param >
        ///// < param name="actionsTitles">a list of key value pairs where key is the<see cref="Action"/> <br/>
        ///// and value is the title of the progress bar for that specific step</param>
        //public static void ShowProgressBar(this SldWorks solidworks, Dictionary<Action,string> actionsTitles)
        //{

        //    solidworks.GetUserProgressBar(out UserProgressBar userProgressBar);
        //    userProgressBar.Start(0, actionsTitles.Count, actionsTitles.Values.ElementAt(0));
        //    for (int i = 0; i < actionsTitles.Count; i++)
        //    {
        //        userProgressBar.UpdateTitle($"Preforming... { actionsTitles.Keys.ElementAt(i).Method.Name}");
        //        userProgressBar.UpdateProgress(i++);
        //        actionsTitles.Keys.ElementAt(i).Invoke();
        //    }
        //    userProgressBar.End();
        //}
    }
}

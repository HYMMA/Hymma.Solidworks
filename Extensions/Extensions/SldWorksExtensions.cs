// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using EnvDTE;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// extensions for <see cref="SldWorks"/>
    /// </summary>
    public static class SldWorksExtensions
    {
        /// <summary>
        /// get the preview thumbnail of the path provided. regardless the file is open or not.
        /// </summary>
        /// <param name="solidworks"></param>
        /// <param name="path">path to the solidworks document.</param>
        /// <param name="configuration">name of the configuration of the document</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        ///<exception cref="System.Runtime.InteropServices.COMException"></exception>
        ///<remarks>Currently only in-process applications(that is add-ins) can use this method; out-of-process applications(that is, executables) will get an <see cref="System.Runtime.InteropServices.COMException"/> error because the IPictureDisp interface cannot be marshalled across process boundaries.This is a Microsoft behavior by design.See the Microsoft Knowledge Base for details. 
        ///<para>This method is not supported in macros or out-of-process applications in SOLIDWORKS x64.</para></remarks>
        public static Bitmap GetPreviewImage(this ISldWorks solidworks, string path, string configuration)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            var img = solidworks.GetPreviewBitmap(path, configuration);

            if (!(img is stdole.StdPicture pic))
                throw new Exception($"Could not get preview of {path}.");
            return Bitmap.FromHbitmap((IntPtr)pic.Handle);
        }

        /// <summary>
        /// returns the path to SLDWORKS.exe on this computer.
        /// </summary>
        /// <param name="sldWorks">current running instance of SolidWorks</param>
        /// <returns>path to SOLIDWORKS.exe on this computer</returns>
        public static string GetExecutableFullFileName(this ISldWorks sldWorks)
        {
            return Path.Combine(sldWorks.GetExecutablePath(), "SLDWORKS.exe");
        }

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
        /// <br/> make sure you set solidworks visibility to true at the end of operation <see cref="UnFreezeGraphics(SldWorks)"/>
        /// </returns>
        ///<remarks>you should call this before any documents are open. In other words, if a document is open and you call this method, it will have not effect.</remarks>
        public static bool FreezeGraphics(this SldWorks solidworks)
        {
            if (solidworks.GetDocumentCount() != 0)
                return false;

            try
            {
                // Allow SOLIDWORKS to run in the background
                // and be invisible
                solidworks.UserControl = false;

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
                return (frame.KeepInvisible && !solidworks.UserControl && !solidworks.Visible && !solidworks.UserControlBackground);
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
        public static void UnFreezeGraphics(this SldWorks solidworks)
        {
            if (!(solidworks.Frame() is Frame frame))
                return;
            frame.KeepInvisible = false;
            solidworks.Visible = true;
            solidworks.UserControl = true;
        }

        /// <summary>
        /// freezes graphics during an <see cref="Action"/> and unfreeze it afterwards
        /// </summary>
        /// <param name="solidworks"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void FreezeGraphics(this SldWorks solidworks, Action action)
        {
            solidworks.FreezeGraphics();
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                solidworks.UnFreezeGraphics();
            }
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

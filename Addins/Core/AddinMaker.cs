using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// registers an <see cref="Addins.AddinModel"/> into solidworks
    /// </summary>
    [ComVisible(true)]
    public abstract class AddinMaker : ISwAddin
    {

        #region private fields & variables
        
        /// <summary>
        /// identifier for this addin assigned by SOLIDWORKS    
        /// </summary>
        protected int addinCookie;

        /// <summary>
        /// command manager for this addin assigned by SOLIDWORKS
        /// </summary>
        protected CommandManager _commandManager;

        /// <summary>
        /// a collection of documents and their associated events
        /// </summary>
        protected Hashtable documentsEventsRepo;
        #endregion

        #region constructor

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="addin">type of class that inherits from <see cref="ISwAddin"/></param>
        public AddinMaker(Type addin)
        {
            //typeof(ISwAddin).IsAssignableFrom(addin)
            //    &&
            if (addin.TryGetAttribute<AddinAttribute>(false) is AddinAttribute addinAttr)
            {
                Logger.Source = addinAttr.Title;
            }
        }
        #endregion
        
        #region Public Properties
        /// <summary>
        /// solidowrks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }

        /// <summary>
        /// construct the data model for this addin here
        /// </summary>
        /// <returns></returns>
        public abstract AddinModel AddinModel { get; }
        #endregion

        #region com register/un-register
        /// <summary>
        /// registers <see cref="Type"/> provided to COM so solidworks can find it
        /// </summary>
        /// <param name="t">type of class that inherrits from  <see cref="AddinMaker"/></param>
        [ComRegisterFunctionAttribute]
        public static void BaseRegisterFunction(Type t)
        {
            var addinAttribute = t.TryGetAttribute<AddinAttribute>(false) as AddinAttribute;
            if (addinAttribute == null)
                return;
            Log("Getting attributes from child class");
            try
            {
                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                RegistryKey addinKey = Registry.LocalMachine.CreateSubKey(keyname);
                addinKey.SetValue(null, 0);

                addinKey.SetValue("Description", addinAttribute.Description);
                addinKey.SetValue("Title", addinAttribute.Title);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                RegistryKey addinStartUpKey = Registry.CurrentUser.CreateSubKey(keyname);
                addinStartUpKey.SetValue(null, Convert.ToInt32(addinAttribute.LoadAtStartup), RegistryValueKind.DWord);

                //save addin icon in the current assembly folder
                var rm = new ResourceManager($"{t.Name}.Properties.Resources", t.Assembly);
                var addinIcon = rm.GetObject(addinAttribute.AddinIcon) as Bitmap;
                var iconPath = IconGenerator.GetAddinIcon(addinIcon, t.Name);
                #region Extract icon during registration

                addinKey.SetValue("Icon Path", iconPath);

                RegisterLogger(addinAttribute.Title);
                //Log($"registering icon location in registry {iconPath}");
                #endregion
            }
            catch (System.NullReferenceException nl)
            {
                Log($"Error! There was a problem registering this dll: addinModel is null. \n\"" + nl.Message + "\"");
            }

            catch (System.Exception e)
            {
                Log(e);
            }
        }

        /// <summary>
        /// Un-registers <see cref="Type"/> from COM
        /// </summary>
        /// <param name="t">type of class that inherrits from  <see cref="AddinMaker"/></param>
        [ComUnregisterFunctionAttribute]
        public static void BaseUnregisterFunction(Type t)
        {
            var swAttr = t.TryGetAttribute<AddinAttribute>(false) as AddinAttribute;
            if (swAttr == null)
                Log("not attrubute found for addin");
            try
            {
                Log("trying to unregister");
                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                Registry.LocalMachine.DeleteSubKey(keyname);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                Registry.CurrentUser.DeleteSubKey(keyname);

                UnRegisterLogger(swAttr.Title);
            }
            catch (System.NullReferenceException nl)
            {
                Log(nl.Message);

                //TODO:log this
                Console.WriteLine("There was a problem unregistering this dll: " + nl.Message);
            }
            catch (System.Exception e)
            {
                //TODO:log this
                Log(e.Message);
                Console.WriteLine("There was a problem unregistering this dll: " + e.Message);
            }
        }
        #endregion

        #region solidworks integration

        /// <summary>
        /// set <see cref="PmpBase"/> object to null here
        /// </summary>
        private void RemovePMPs(List<PmpBase> propertyManagerPages)
        {
            for (int i = 0; i < propertyManagerPages.Count(); i++)
            {
                propertyManagerPages[i] = null;
                Log($"PMP {i} set to null");
            }
        }

        private void DetachEventsFromAllDocuments()
        {
            throw new NotImplementedException();
        }

        private void DetachSwEvents()
        {
            throw new NotImplementedException();
        }

        private void RemoveCmdTabs(IEnumerable<AddinCommandTab> commandTabs)

        {
            foreach (var tab in commandTabs)
            {
                _ = _commandManager.RemoveCommandGroup(tab.CommandGroup.UserId);
                Log($"removed command group with id {tab.CommandGroup.UserId}");
            }
        }
        #endregion

        #region UI
        /// <summary>
        /// Adds commands to the addin
        /// </summary>
        /// <returns></returns>
        public void AddCommands(IEnumerable<AddinCommandTab> commandTabs)
        {
            try
            {
                foreach (var tab in commandTabs)
                {
                    //make command groups
                    Log("Adding command group...");
                    tab.CommandGroup.AddCommandGroup(_commandManager);
                    Log("finished setting up commadn group");

                    //make command tabs
                    Log("Adding commadn tab...");
                    tab.Register(_commandManager);
                    Log("finished adding command tab");
                }
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Events
/*
        public bool AttachEventsToAllDocuments()
        {
            throw new NotImplementedException();
        }

        public bool AttachSwEvents()
        {
            throw new NotImplementedException();
        }*/

        #endregion

        /// <summary>
        /// SOLIDWORKS calls these command once addin is unloaded.
        /// </summary>
        /// <returns></returns>
        public virtual bool DisconnectFromSW()
        {
            RemoveCmdTabs(AddinModel.CommandTabs);
            RemovePMPs(AddinModel.PropertyManagerPages);
            //DetachSwEvents();
            //DetachEventsFromAllDocuments();

            Marshal.ReleaseComObject(_commandManager);
            _commandManager = null;

            Marshal.ReleaseComObject(Solidworks);
            Solidworks = null;

            //The addin _must_ call GC.Collect() here in order to retrieve all managed code pointers 
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        /// <summary>
        /// SOLIDWORKS calls this method and assigns its params once addin is loaded
        /// </summary>
        /// <param name="ThisSW"></param>
        /// <param name="Cookie"></param>
        /// <returns></returns>
        public virtual bool ConnectToSW(object ThisSW, int Cookie)
        {
            Log("connecting to solidworks from Addin maker base class");
            Solidworks = (ISldWorks)ThisSW;
            addinCookie = Cookie;

            Log($"addin cookie is  {addinCookie} ");
            //Setup callbacks
            Solidworks.SetAddinCallbackInfo2(0, this, addinCookie);

            Log("setting up Addin Model");

            #region Setup the Command Manager
            _commandManager = Solidworks.GetCommandManager(Cookie);

            Log("addin commands . . .");
            AddCommands(AddinModel.CommandTabs);
            Log($"finished addin commands");

            #endregion

            #region Setup the Event Handlers
            //addin = (SldWorks)Solidworks;
            //documentsEventsRepo = new Hashtable();

            //this will be called only the first time the addin is loaded
            //this method will attached events to all documents that open after the addin is loaded.

            //AttachSwEvents();

            //Listen for events on all currently open docs
            //we need to call this method here because sometimes user fires the addin while he has some documents open already
            //there are events that will attach event handlers to all documents but until those events are fired this call to the method will suffice
            //AttachEventsToAllDocuments();
            #endregion

            return true;
        }
    }
}

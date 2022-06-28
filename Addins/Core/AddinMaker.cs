using Hymma.Solidworks.Addins.Helpers;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Environment = System.Environment;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// registers an <see cref="Addins.AddinUserInterface"/> into solidworks
    /// </summary>
    [ComVisible(true)]
    public abstract class AddinMaker : ISwAddin
    {
        #region private fields & variables

        /// <summary>
        /// title of this addin
        /// </summary>
        private static string _addinTitle;

        /// <summary>
        /// command manager for this addin assigned by SOLIDWORKS
        /// </summary>
        protected CommandManager _commandManager;

        /// <summary>
        /// a collection of documents and their associated events
        /// </summary>
        protected Hashtable documentsEventsRepo;

        /// <summary>
        /// construct the data model for this addin here
        /// </summary>
        private AddinUserInterface _addinUi;
        #endregion


        #region Public Properties

        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }

        #endregion

        #region com register/unregister
        /// <summary>
        /// registers <see cref="Type"/> provided to RegisteryHelper so solidworks can find it
        /// </summary>
        /// <param name="t">type of class that inherits from  <see cref="AddinMaker"/></param>
        [ComRegisterFunction]
        public static void Register(Type t)
        {
            RegisteryHelper.RegisterSolidworksAddin(t);
        }

        /// <summary>
        /// unregisters the addin once removed or when the project is cleaned
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            RegisteryHelper.UnregisterSolidworksAddin(t);
        }
        #endregion

        #region solidworks integration

        /// <summary>
        /// set <see cref="PmpFactoryBase"/> object to null here
        /// </summary>
        private void RemovePMPs(List<PmpFactoryBase> propertyManagerPages)
        {
            for (int i = 0; i < propertyManagerPages.Count(); i++)
            {
                propertyManagerPages[i].Close(false);
                propertyManagerPages[i] = null;
                Log($"PMP {i} set to null");
            }
            propertyManagerPages = null;
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
                _ = _commandManager.RemoveCommandGroup(tab.CommandGroup.UserId);
            commandTabs = null;
        }

        /// <summary>
        /// SOLIDWORKS calls these command once addin is unloaded.
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            RemoveCmdTabs(_addinUi.CommandTabs);
            RemovePMPs(_addinUi.PropertyManagerPages);
            //DetachSwEvents();
            //DetachEventsFromAllDocuments();

            Marshal.ReleaseComObject(_commandManager);
            _commandManager = null;

            Marshal.ReleaseComObject(Solidworks);
            Solidworks = null;

            //fire event
            OnExit?.Invoke(this, new OnConnectToSwEventArgs { solidworks = Solidworks, cookie = _addinUi.Id });

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
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            Log("connecting to solidworks from Addin maker base class");
            Solidworks = (ISldWorks)ThisSW;
            _addinUi = GetUserInterFace();
            _addinUi.Id = Cookie;

            Log($"addin cookie is  {_addinUi.Id} ");
            //Setup callbacks
            Solidworks.SetAddinCallbackInfo2(0, this, _addinUi.Id);

            Log("setting up Addin Model");

            #region Setup the Command Manager and add commands
            _commandManager = Solidworks.GetCommandManager(Cookie);

            Log("addin commands . . .");
            AddCommands(_addinUi.CommandTabs);
            Log($"finished addin commands");

            #endregion

            //fire event
            OnStart?.Invoke(this, new OnConnectToSwEventArgs { solidworks = (ISldWorks)ThisSW, cookie = Cookie });
            return true;
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

                    tab.CommandGroup.Register(_commandManager);
                    //make command tabs
                    tab.Register(_commandManager);
                }
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Events

        /// <summary>
        /// Events that fires when your add-in connects to solidworks
        /// </summary>
        public event EventHandler<OnConnectToSwEventArgs> OnStart;

        /// <summary>
        /// event that fires when user unloads the addin (example when user unchecked the addin from the list of addins)
        /// </summary>
        public event EventHandler<OnConnectToSwEventArgs> OnExit;
        #endregion

        /// <summary>
        /// define a the user interface,ex: property manager page, command tabs, etc
        /// </summary>
        /// <returns></returns>
        public abstract AddinUserInterface GetUserInterFace();
    }

    /// <summary>
    /// event arguments for when solidworks connects or disconnects from your add-in
    /// </summary>
    public class OnConnectToSwEventArgs : EventArgs
    {
        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks solidworks { get; set; }

        /// <summary>
        /// the identifier for this addin
        /// </summary>
        public int cookie { get; set; }
    }
}

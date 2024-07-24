// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Core;
using Hymma.Solidworks.Addins.Helpers;
using Hymma.Solidworks.Addins.Utilities.DotNet;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// registers an <see cref="Addins.AddinUserInterface"/> into SolidWORKS 
    /// </summary>
    [ComVisible(true)]
    public abstract class AddinMaker : ISwAddin
    {
        #region private fields

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

        /// <summary>
        /// initial values will be saved to memory here.
        /// </summary>
        protected AddinMaker()
        {
            GraphicsHelper.SaveDpiScaleInMemory();
        }
        #region Public Properties

        /// <summary>
        /// SolidWORKS object
        /// </summary>
        internal ISldWorks Solidworks { get; set; }

        #endregion

        #region com register/unregister
        /// <summary>
        /// registers <see cref="Type"/> provided to Register Helper so SolidWORKS can find it
        /// </summary>
        /// <param name="t">type of class that inherits from  <see cref="AddinMaker"/></param>
        [ComRegisterFunction]
        public static void Register(Type t)
        {
            RegisterHelper.TryRegisterSolidworksAddin(t);
        }

        /// <summary>
        /// unregisters the addin once removed or when the project is cleaned
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            RegisterHelper.TryUnregisterSolidworksAddin(t);
        }
        #endregion

        #region SolidWORKS integration

        /// <summary>
        /// set <see cref="PropertyManagerPageX64"/> object to null here
        /// </summary>
        private void RemovePMPs(List<PropertyManagerPageX64> propertyManagerPages)
        {
            for (int i = 0; i < propertyManagerPages.Count(); i++)
            {
                propertyManagerPages[i].Close(false);
                propertyManagerPages[i] = null;
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
                _ = _commandManager.RemoveCommandGroup(tab.CommandGroup.UserId);
        }

        /// <summary>
        /// SOLIDWORKS calls these command once addin is unloaded.
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            //fire event
            OnExit?.Invoke(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = _addinUi.Id });
            RemoveCmdTabs(_addinUi.CommandTabs);
            RemovePMPs(_addinUi.PropertyManagerPages);
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
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            //fire event
            OnStart?.Invoke(this, new OnConnectToSwEventArgs { Solidworks = (ISldWorks)ThisSW, Cookie = Cookie });

            Solidworks = (ISldWorks)ThisSW;
            _addinUi = GetUserInterFace();
            _addinUi.Id = Cookie;

            //Setup callbacks
            Solidworks.SetAddinCallbackInfo2(0, this, _addinUi.Id);

            #region Setup the Command Manager and add commands
            _commandManager = Solidworks.GetCommandManager(Cookie);

            AddinIcons.CreateSubDirForUiItems(_addinUi);
            AddCommands(_addinUi.CommandTabs);
            AddPropertyManagerPages(_addinUi.PropertyManagerPages);

            #endregion

            return true;
        }

        private void AddPropertyManagerPages(List<PropertyManagerPageX64> propertyManagerPages)
        {
            foreach (var pmp in propertyManagerPages)
            {
                pmp.CreatePropertyManagerPage();
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
                    _commandManager.Register(tab.CommandGroup);
                    _commandManager.Register(tab);
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
}

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// registers an <see cref="AddinModel"/> into solidworks
    /// </summary>
    [ComVisible(true)]
    public abstract class AddinBase : ISwAddin
    {
        #region Default constructor
        public AddinBase()
        {
            UpdateAddinModel(AppModel);
        }
        #endregion

        #region private fields & variables
        protected int addinCookie;
        protected CommandManager commandManager;
        protected SldWorks addin;
        protected Hashtable documentsEventsRepo;
        #endregion

        #region Public Properties

        /// <summary>
        /// apps setting where description and commands are defined
        /// </summary>
        public AddinModel AppModel { get; set; } = new AddinModel();

        /// <summary>
        /// solidowrks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }
        #endregion

        #region solidworks integration
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            Solidworks = (ISldWorks)ThisSW;
            addinCookie = Cookie;

            //Setup callbacks
            Solidworks.SetAddinCallbackInfo2(0, this, addinCookie);

            #region Setup the Command Manager

            commandManager = Solidworks.GetCommandManager(Cookie);
            AddCommands();

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

            #region Setup Property Manager
            //AddPMP();
            #endregion
            return OnConnect();
        }

        /// <summary>
        /// This gets called when solidworks initiated
        /// </summary>
        /// <returns></returns>
        public abstract bool OnConnect();


        /// <summary>
        /// this get called when soliworks exists
        /// </summary>
        /// <returns></returns>
        public abstract void OnDisconnect();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool DisconnectFromSW()
        {
            RemoveCommandMgr();
            //RemovePMP();
            //DetachSwEvents();
            //DetachEventsFromAllDocuments();

            Marshal.ReleaseComObject(commandManager);
            commandManager = null;

            Marshal.ReleaseComObject(Solidworks);
            Solidworks = null;

            OnDisconnect();

            //The addin _must_ call GC.Collect() here in order to retrieve all managed code pointers 
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        private void DetachEventsFromAllDocuments()
        {
            throw new NotImplementedException();
        }

        private void DetachSwEvents()
        {
            throw new NotImplementedException();
        }

        private void RemovePMP()
        {
            throw new NotImplementedException();
        }

        private void RemoveCommandMgr()
        {
            AppModel.CommandGroups.ToList().ForEach(group => commandManager.RemoveCommandGroup(group.GroupID));
        }
        #endregion

        #region Com Registration
        /// <summary>
        /// registers <see cref="Type"/> provided to COM so solidworks can find it
        /// </summary>
        /// <param name="t">type of class that inherrits from  <see cref="AddinBase"/></param>
        /// <param name="Description">the description of this addin</param>
        /// <param name="Title">title of this addin</param>
        /// <param name="LoadAtStartup">determines if this addin gets loaded when solidworks starts up</param>
        protected static void RegisterAddinToCom(Type t, string Description, string Title, bool LoadAtStartup)
        {
            //var swAttr = t.TryGetAttribute<SwAddinAttribute>(false) as SwAddinAttribute;
            //if (swAttr == null)
            //TODO:log this
            //Console.WriteLine("couldnet get the proper attribute from the child class");
            try
            {
                Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                Microsoft.Win32.RegistryKey addinkey = hklm.CreateSubKey(keyname);
                addinkey.SetValue(null, 0);

                addinkey.SetValue("Description", Description);
                addinkey.SetValue("Title", Title);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                addinkey = hkcu.CreateSubKey(keyname);
                addinkey.SetValue(null, Convert.ToInt32(LoadAtStartup), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (System.NullReferenceException nl)
            {
                //TODO:log this
                Console.WriteLine("There was a problem registering this dll: addinModel is null. \n\"" + nl.Message + "\"");
            }

            catch (System.Exception e)
            {
                //TODO:log this
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Un-registers <see cref="Type"/> from COM
        /// </summary>
        /// <param name="t">type of class that inherrits from  <see cref="AddinBase"/></param>
        protected static void UnregisterAdidnFromCOM(Type t)
        {
            try
            {
                Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                hklm.DeleteSubKey(keyname);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                hkcu.DeleteSubKey(keyname);
            }
            catch (System.NullReferenceException nl)
            {
                //TODO:log this
                Console.WriteLine("There was a problem unregistering this dll: " + nl.Message);
            }
            catch (System.Exception e)
            {
                //TODO:log this
                Console.WriteLine("There was a problem unregistering this dll: " + e.Message);
            }
        }
        #endregion

        #region UI

        public bool AddPMP()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Adds commands to the addin
        /// </summary>
        /// <returns></returns>
        public bool AddCommands()
        {
            try
            {
                AppModel.CommandGroups.ToList().ForEach(cmg => commandManager.AddCommandGroup(cmg));
                AppModel.CommandTabs.ToList().ForEach(cmt => commandManager.AddCommandTab(cmt));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Events

        public bool AttachEventsToAllDocuments()
        {
            throw new NotImplementedException();
        }

        public bool AttachSwEvents()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region abstract methods/funcitons

        /// <summary>
        /// this methode should update the addin model
        /// </summary>
        public abstract void UpdateAddinModel(AddinModel addinModel);
        #endregion
    }
}

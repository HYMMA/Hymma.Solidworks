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
using static Hymma.Solidworks.Addins.Logger;
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

        #region constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public AddinMaker(Type t)
        {
            if (!typeof(AddinMaker).IsAssignableFrom(t))
                throw new ApplicationException("the type should implement the AddinMaker");
            if (!(t.TryGetAttribute<AddinAttribute>(false) is AddinAttribute addinAttribute))
                throw new ApplicationException("the type should implement the AddinAttribute");
            _addinTitle = addinAttribute.Title;
            Logger.Source = "Solidworks Addin";
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }

        #endregion

        #region com register/unregister
        /// <summary>
        /// registers <see cref="Type"/> provided to COM so solidworks can find it
        /// </summary>
        /// <param name="t">type of class that inherits from  <see cref="AddinMaker"/></param>
        [ComRegisterFunction]
        public static void BaseRegisterFunction(Type t)
        {
            var addinAttribute = t.TryGetAttribute<AddinAttribute>(false) as AddinAttribute;
            if (addinAttribute == null)
                return;

            try
            {
                Logger.Source = "Solidworks Addin";
                Log("trying to register");
                Console.WriteLine("trying to register");

                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                RegistryKey addinKey = Registry.LocalMachine.CreateSubKey(keyname);
                addinKey.SetValue(null, 0);

                addinKey.SetValue("Description", addinAttribute.Description);
                addinKey.SetValue("Title", addinAttribute.Title);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                RegistryKey addinStartUpKey = Registry.CurrentUser.CreateSubKey(keyname);
                addinStartUpKey.SetValue(null, Convert.ToInt32(addinAttribute.LoadAtStartup), RegistryValueKind.DWord);

                #region Extract icon during registration

                var iconPath = SaveAddinIcon(t);

                addinKey.SetValue("Icon Path", iconPath);

                Log("Registration was successful!");
                #endregion
            }
            catch (MissingManifestResourceException e)
            {
                Log($"Error! it seems the resource {addinAttribute.AddinIcon} was not found." +
                    $" this happens when the string provided for the resource is not correct or the resource not in the Properties Folder of the project. \n {e.Message}");
            }
            catch (System.NullReferenceException e)
            {
                Log($"Error! There was a problem registering this library: addinModel is null. \n\"" + e.Message + "\"");
            }

            catch (System.Exception e)
            {
                Log(e);
            }
        }

        /// <summary>
        /// unregisters the addin once removed or when the project is cleaned
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction]
        public static void BaseUnregisterFunction(Type t)
        {
            Logger.Source = "Solidworks Addin";
            var swAttr = t.TryGetAttribute<AddinAttribute>(false) as AddinAttribute;
            if (swAttr == null)
                try
                {
                    Log("trying to unregister");
                    string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                    Registry.LocalMachine.DeleteSubKey(keyname);

                    keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                    Registry.CurrentUser.DeleteSubKey(keyname);

                    Log("Unregister successful");
                    //UnRegisterLogger(swAttr.Title);

                }
                catch (System.NullReferenceException nl)
                {
                    Log(nl.Message);

                    //TODO:log this
                    Console.WriteLine("Error! There was a problem unregistering this library: " + nl.Message);
                }
                catch (System.Exception e)
                {
                    //TODO:log this
                    Log(e.Message);
                    Console.WriteLine("Error! There was a problem unregistering this library: " + e.Message);
                }
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
        /*
                public bool AttachEventsToAllDocuments()
                {
                    throw new NotImplementedException();
                }

                public bool AttachSwEvents()
                {
                    throw new NotImplementedException();
                }*/

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

        #region static members
        /// <summary>
        /// generates an addin icon (.png) format and saves it on assembly folder
        /// </summary>
        /// <returns></returns>
        private static string SaveAddinIcon(Type t)
        {
            var icon = GetBitmap(t);
            if (icon == null)
                return "addin icon was null";
            if (!(t.TryGetAttribute<AddinAttribute>(false) is AddinAttribute addinAttribute))
                throw new ApplicationException("the type should implement the AddinAttribute");
            _addinTitle = addinAttribute.Title;
            string addinIconAddress = Path.Combine(GetIconsDir().FullName, t.Name + ".png");
            using (var addinIcon = new Bitmap(icon, 16, 16))
            {
                addinIcon.Save(addinIconAddress);
            }
            return addinIconAddress;
        }

        /// <summary>
        /// this is a folder where the icons will get saved to
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo GetIconsDir()
        {
            //directory should be a folder where user has access to at all times
            //because we make icons for commands every time solidworks starts
            string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            try
            {
                return Directory.CreateDirectory(localApp).CreateSubdirectory(_addinTitle);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Bitmap GetBitmap(Type t)
        {
            if (!(t.GetCustomAttribute(typeof(AddinAttribute)) is AddinAttribute attribute)) return null;
            var asm = t.Assembly;
            string[] resNames = asm.GetManifestResourceNames();
            foreach (var resName in resNames)
            {
                var rm = new ResourceManager(resName, asm);

                // Get the fully qualified resource type name
                // Resources are suffixed with .resource
                var resName2 = resName.Substring(0, resName.IndexOf(".resource"));
                var type = asm.GetType(resName2, true);

                var resources = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (PropertyInfo res in resources)
                {
                    // collect string type resources
                    if (res.PropertyType == typeof(Bitmap) && res.Name == attribute.AddinIcon)
                        return res.GetValue(null, null) as Bitmap;
                }
            }
            return null;
        }
        #endregion
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

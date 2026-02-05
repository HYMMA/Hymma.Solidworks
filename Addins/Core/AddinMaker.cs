// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.ContextMenus;
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
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Base class for creating SolidWorks add-ins. Inherit from this class and override
    /// <see cref="GetUserInterFace"/> to define your add-in's user interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class handles COM registration, connection to SolidWorks, and lifecycle management
    /// of your add-in. You must decorate your derived class with the following attributes:
    /// </para>
    /// <list type="bullet">
    ///   <item><description><see cref="AddinAttribute"/> - Defines add-in metadata (title, description, icon)</description></item>
    ///   <item><description><see cref="GuidAttribute"/> - Unique identifier for COM registration</description></item>
    ///   <item><description><see cref="ComVisibleAttribute"/> - Must be set to <c>true</c></description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para>Basic add-in implementation:</para>
    /// <code>
    /// [Addin(title: "My Add-in",
    ///        AddinIcon = "icon.png",
    ///        Description = "My SolidWorks add-in",
    ///        LoadAtStartup = true)]
    /// [ComVisible(true)]
    /// [Guid("YOUR-GUID-HERE")]
    /// public class MyAddin : AddinMaker
    /// {
    ///     private ISldWorks _solidworks;
    ///
    ///     public MyAddin()
    ///     {
    ///         // Subscribe to lifecycle events
    ///         OnStart += (sender, e) => _solidworks = e.Solidworks;
    ///         OnExit += (sender, e) => { /* cleanup */ };
    ///     }
    ///
    ///     public override AddinUserInterface GetUserInterFace()
    ///     {
    ///         return new AddinUserInterface
    ///         {
    ///             CommandTabs = new List&lt;AddinCommandTab&gt; { /* your tabs */ },
    ///             IconsRootDir = new DirectoryInfo(@"C:\MyAddin\Icons")
    ///         };
    ///     }
    ///
    ///     // Callback methods must be public and defined in this class
    ///     public void MyButtonCallback() { /* handle click */ }
    ///     public int MyEnableMethod() => _solidworks?.ActiveDoc != null ? 1 : 0;
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="AddinUserInterface"/>
    /// <seealso cref="AddinAttribute"/>
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

        private readonly ContextMenuRouter _contextMenuRouter = new ContextMenuRouter();

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

        internal CommandManager CommandManager => _commandManager;

        internal ContextMenuRouter ContextMenuRouter => _contextMenuRouter;

        internal int AddinId => _addinUi?.Id ?? 0;

        internal System.IO.DirectoryInfo IconsRootDir => _addinUi?.IconsRootDir;

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
                propertyManagerPages[i].Release();
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
            _onExitEvents?.Raise(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = _addinUi.Id });
            _onExitEvents.ClearHandlers();
            RemoveCmdTabs(_addinUi.CommandTabs);
            RemovePMPs(_addinUi.PropertyManagerPages);
            _contextMenuRouter.Clear();
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
            Solidworks = (ISldWorks)ThisSW;
            //fire event
            _onStartEvents?.Raise(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = Cookie });
            _onStartEvents.ClearHandlers();
            _addinUi = GetUserInterFace();
            _addinUi.Id = Cookie;

            //Setup callbacks
            Solidworks.SetAddinCallbackInfo2(0, this, _addinUi.Id);

            #region Setup the Command Manager and add commands
            _commandManager = Solidworks.GetCommandManager(Cookie);

            //fire event (command manager is now available)
            _onStartEvents?.Raise(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = Cookie });
            _onStartEvents.ClearHandlers();

            AddinIcons.CreateSubDirForUiItems(_addinUi);
            AddCommands(_addinUi.CommandTabs);
            AddPropertyManagerPages(_addinUi.PropertyManagerPages);

            #endregion
            _onUiReadyEvents?.Raise(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = Cookie });
            _onUiReadyEvents.ClearHandlers();

            //first collect all the bitmaps we created during registering the addin
            //the framework has already called Dispose() on them but GC might not collect them
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();
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
                    ////validate commands
                    //foreach (var command in tab.CommandGroup.Commands)
                    //{
                    //    //if it is a spacer, skip
                    //    if (string.IsNullOrEmpty(command.EnableMethod) || string.IsNullOrEmpty(command.CallBackFunction))
                    //        continue;

                    //    var enableMethod = this.GetType().GetMethod(command.EnableMethod);
                    //    var callBackFunction = this.GetType().GetMethod(command.CallBackFunction);
                    //    if (enableMethod.ReturnType != typeof(int))
                    //    {
                    //        throw new Exception($"Enable method '{command.EnableMethod}' must return int but it is returning {enableMethod.ReturnType}");
                    //    }
                    //    if (!enableMethod.IsPublic)
                    //    {
                    //        throw new Exception($"Enable method '{command.EnableMethod}' must be public");
                    //    }
                    //    if(callBackFunction.ReturnType != typeof(void))
                    //    {
                    //        throw new Exception($"Callback function '{command.CallBackFunction}' must be void");
                    //    }
                    //    if (!callBackFunction.IsPublic)
                    //    {
                    //        throw new Exception($"Callback function '{command.CallBackFunction}' must be public");
                    //    }
                    //}
                    _commandManager.Register(tab.CommandGroup);
                    _commandManager.Register(tab);
                }
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Events

        readonly WeakEventSource<OnConnectToSwEventArgs> _onStartEvents = new WeakEventSource<OnConnectToSwEventArgs>();
        readonly WeakEventSource<OnConnectToSwEventArgs> _onExitEvents = new WeakEventSource<OnConnectToSwEventArgs>();
        readonly WeakEventSource<OnConnectToSwEventArgs> _onUiReadyEvents = new WeakEventSource<OnConnectToSwEventArgs>();
        /// <summary>
        /// Occurs when the add-in successfully connects to SolidWorks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the recommended place to obtain the <see cref="ISldWorks"/> reference
        /// and perform any initialization that requires SolidWorks to be available.
        /// </para>
        /// <para>
        /// The event uses weak references to prevent memory leaks. Handlers are automatically
        /// cleared after the event fires.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// public class MyAddin : AddinMaker
        /// {
        ///     private ISldWorks _solidworks;
        ///
        ///     public MyAddin()
        ///     {
        ///         OnStart += (sender, e) =>
        ///         {
        ///             _solidworks = e.Solidworks;
        ///             // Initialize resources, load settings, etc.
        ///         };
        ///     }
        /// }
        /// </code>
        /// </example>
        public event EventHandler<OnConnectToSwEventArgs> OnStart
        {
            add =>
                _onStartEvents.Subscribe(this, value);
            remove =>
                _onStartEvents.Unsubscribe(value);
        }

        /// <summary>
        /// Occurs when the add-in is being unloaded from SolidWorks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This event fires when the user disables the add-in (unchecks it in Tools → Add-Ins)
        /// or when SolidWorks is closing. Use this event to clean up resources, save settings,
        /// and release any COM objects.
        /// </para>
        /// <para>
        /// The event uses weak references to prevent memory leaks. Handlers are automatically
        /// cleared after the event fires.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// public MyAddin()
        /// {
        ///     OnExit += (sender, e) =>
        ///     {
        ///         // Save user settings
        ///         Settings.Save();
        ///
        ///         // Clean up resources
        ///         _myResource?.Dispose();
        ///     };
        /// }
        /// </code>
        /// </example>
        public event EventHandler<OnConnectToSwEventArgs> OnExit
        {
            add =>
                _onExitEvents.Subscribe(this, value);
            remove =>
                _onExitEvents.Unsubscribe(value);
        }

        /// <summary>
        /// Occurs after command groups and property manager pages are registered.
        /// </summary>
        public event EventHandler<OnConnectToSwEventArgs> OnUiReady
        {
            add =>
                _onUiReadyEvents.Subscribe(this, value);
            remove =>
                _onUiReadyEvents.Unsubscribe(value);
        }
        #endregion

        /// <summary>
        /// Override this method to define your add-in's user interface including command tabs,
        /// command groups, and property manager pages.
        /// </summary>
        /// <returns>
        /// An <see cref="AddinUserInterface"/> instance containing all UI elements for your add-in.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is called by SolidWorks during add-in initialization. The returned
        /// <see cref="AddinUserInterface"/> should contain:
        /// </para>
        /// <list type="bullet">
        ///   <item><description><see cref="AddinUserInterface.CommandTabs"/> - Toolbar tabs with command buttons</description></item>
        ///   <item><description><see cref="AddinUserInterface.PropertyManagerPages"/> - Property manager page definitions</description></item>
        ///   <item><description><see cref="AddinUserInterface.IconsRootDir"/> - Directory where icons will be stored</description></item>
        /// </list>
        /// <para>
        /// <b>Important:</b> Callback method names (e.g., <c>EnableMethod</c>, <c>CallBackFunction</c>)
        /// must reference public methods defined in your add-in class.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Simple implementation:</para>
        /// <code>
        /// public override AddinUserInterface GetUserInterFace()
        /// {
        ///     return new AddinUserInterface
        ///     {
        ///         CommandTabs = new List&lt;AddinCommandTab&gt;
        ///         {
        ///             new MyCommandTab()
        ///         },
        ///         PropertyManagerPages = new List&lt;PropertyManagerPageX64&gt;
        ///         {
        ///             new MyPropertyManagerPage(_solidworks)
        ///         },
        ///         IconsRootDir = new DirectoryInfo(
        ///             Path.Combine(Environment.GetFolderPath(
        ///                 Environment.SpecialFolder.LocalApplicationData), "MyAddinIcons"))
        ///     };
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AddinUserInterface"/>
        /// <seealso cref="AddinCommandTab"/>
        /// <seealso cref="PropertyManagerPageX64"/>
        public abstract AddinUserInterface GetUserInterFace();

        /// <summary>
        /// Routes context menu callbacks to registered handlers.
        /// </summary>
        public void ContextMenuCommand(string token)
        {
            _contextMenuRouter.Execute(token, Solidworks);
        }

        /// <summary>
        /// Routes context menu enable checks to registered predicates.
        /// </summary>
        public int ContextMenuEnable(string token)
        {
            return _contextMenuRouter.Enable(token, Solidworks);
        }
    }
}

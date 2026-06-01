// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.ContextMenus;
using Hymma.Solidworks.Addins.Core;
using Hymma.Solidworks.Addins.Helpers;
using Hymma.Solidworks.Addins.Utilities.DotNet;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
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
        /// construct the data model for this addin here
        /// </summary>
        private AddinUserInterface _addinUi;

        private readonly ContextMenuRouter _contextMenuRouter = new ContextMenuRouter();

        /// <summary>
        /// Name of the <see cref="ConnectToSW"/> phase currently executing, so the connect error
        /// boundary can report exactly which stage failed.
        /// </summary>
        private string _connectPhase;

        #endregion

        /// <summary>
        /// initial values will be saved to memory here.
        /// </summary>
        protected AddinMaker()
        {
            GraphicsHelper.SaveDpiScaleInMemory();
        }

        /// <summary>
        /// Routes framework connect-failure logs to a Windows Event Log <paramref name="source"/>
        /// (in <paramref name="logName"/>, default "Application") in addition to the connect log file,
        /// so a silent <c>ConnectToSW</c> failure reaches an Event-Log-based telemetry pipeline. The
        /// source must already be registered (e.g. by your installer); the framework never creates it.
        /// Call this from your add-in constructor — it must run before <c>ConnectToSW</c>. Passing a
        /// null/empty source disables Event Log routing. Never throws.
        /// </summary>
        public static void ConfigureConnectLogEventSource(string source, string logName = "Application")
            => BootLog.ConfigureEventLog(source, logName);
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
            for (int i = 0; i < propertyManagerPages.Count; i++)
            {
                propertyManagerPages[i].Release();
            }
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
            BootLog.Init();
            BootLog.Info($"[{GetType().Name}] ConnectToSW starting (cookie={Cookie})");
            try
            {
                Solidworks = (ISldWorks)ThisSW;

                //Fire OnStart BEFORE building the UI. Consumers use OnStart to capture the
                //Solidworks object and initialize runtime state (license, services) that their
                //GetUserInterFace() implementation depends on to build license-gated UI.
                //GetUserInterFace() must never run before OnStart, otherwise that state is null
                //and throws. Enforced by ConnectToSW_RaisesOnStart_BeforeGetUserInterFace test.
                RunPhase("OnStart", () =>
                {
                    _onStartEvents?.Raise(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = Cookie });
                    _onStartEvents.ClearHandlers();
                });

                RunPhase("GetUserInterFace", () =>
                {
                    _addinUi = GetUserInterFace();
                    _addinUi.Id = Cookie;
                });

                RunPhase("SetupCallbacks", () =>
                {
                    Solidworks.SetAddinCallbackInfo2(0, this, _addinUi.Id);
                    _commandManager = Solidworks.GetCommandManager(Cookie);
                });

                RunPhase("AddCommands", () =>
                {
                    AddinIcons.CreateSubDirForUiItems(_addinUi);
                    AddCommands(_addinUi.CommandTabs);
                    AddPropertyManagerPages(_addinUi.PropertyManagerPages);
                });

                RunPhase("OnUiReady", () =>
                {
                    _onUiReadyEvents?.Raise(this, new OnConnectToSwEventArgs { Solidworks = Solidworks, Cookie = Cookie });
                    _onUiReadyEvents.ClearHandlers();
                });

                //first collect all the bitmaps we created during registering the addin
                //the framework has already called Dispose() on them but GC might not collect them
                GC.Collect();
                GC.WaitForPendingFinalizers();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                BootLog.Info($"[{GetType().Name}] ConnectToSW completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                //Error boundary. Without this, a consumer exception here crosses the COM boundary
                //into native SOLIDWORKS, which swallows it and silently unchecks the add-in with no
                //dialog and no log. Instead we record it, surface it to the developer, and fail clean.
                BootLog.Error($"[{GetType().Name}] ConnectToSW FAILED in phase '{_connectPhase}'", ex);
                ShowConnectError(ex);
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
#endif
                return false;
            }
        }

        /// <summary>
        /// Runs one named phase of <see cref="ConnectToSW"/>, recording its start and finish to
        /// <see cref="BootLog"/> and tracking <see cref="_connectPhase"/> so the connect error
        /// boundary can report exactly which stage failed.
        /// </summary>
        private void RunPhase(string phase, Action action)
        {
            _connectPhase = phase;
            BootLog.Info($"[{GetType().Name}] phase '{phase}' started");
            action();
            BootLog.Info($"[{GetType().Name}] phase '{phase}' completed");
        }

        /// <summary>
        /// Surfaces a connect failure to the developer through SOLIDWORKS. Never throws &#8212; the
        /// error reporter must not be able to mask the original failure.
        /// </summary>
        private void ShowConnectError(Exception ex)
        {
            try
            {
                Solidworks?.SendMsgToUser2(
                    $"The '{GetType().Name}' add-in failed to load during phase '{_connectPhase}'.\n\n" +
                    $"{ex.GetType().Name}: {ex.Message}\n\nSee {BootLog.LogPath} for the full stack trace.",
                    (int)swMessageBoxIcon_e.swMbWarning,
                    (int)swMessageBoxBtn_e.swMbOk);
            }
            catch
            {
                // Never let the error reporter throw.
            }
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
            foreach (var tab in commandTabs)
            {
                _commandManager.Register(tab.CommandGroup);
                _commandManager.Register(tab);
            }
        }
        #endregion

        #region Events

        readonly EventSource<OnConnectToSwEventArgs> _onStartEvents = new EventSource<OnConnectToSwEventArgs>();
        readonly EventSource<OnConnectToSwEventArgs> _onExitEvents = new EventSource<OnConnectToSwEventArgs>();
        readonly EventSource<OnConnectToSwEventArgs> _onUiReadyEvents = new EventSource<OnConnectToSwEventArgs>();
        /// <summary>
        /// Occurs when the add-in successfully connects to SolidWorks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the recommended place to obtain the <see cref="ISldWorks"/> reference
        /// and perform any initialization that requires SolidWorks to be available.
        /// </para>
        /// <para>
        /// Handlers are automatically cleared after the event fires.
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
        /// Handlers are automatically cleared after the event fires.
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

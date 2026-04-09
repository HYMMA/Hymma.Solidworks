// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Drawing;
using System.IO;

namespace Hymma.Solidworks.Addins.ContextMenus
{
    /// <summary>
    /// Registers and manages context menus for SolidWorks selection types.
    /// Uses IFrame.AddMenuPopupIcon2 which works for both graphics area and feature tree selections.
    /// </summary>
    public sealed class ContextMenuRegistrar : IDisposable
    {
        private const string CallbackMethodName = "ContextMenuCommand";
        private const string EnableMethodName = "ContextMenuEnable";

        private readonly ISldWorks _solidworks;
        private readonly ICommandManager _commandManager;
        private readonly IFrame _frame;
        private readonly int _addinId;
        private readonly ContextMenuRouter _router;
        private readonly Dictionary<ContextMenuKey, ContextMenuRegistration> _registrations;
        private int _tokenCounter = 1;
        private int _menuIdCounter = 1;
        private bool _disposed;
        private readonly DirectoryInfo _iconsRootDir;
#if DEBUG
        private static readonly HashSet<swSelectType_e> DebugSelectionTypeFilter = new HashSet<swSelectType_e>
        {
            swSelectType_e.swSelFACES,
            swSelectType_e.swSelBODYFEATURES,
            swSelectType_e.swSelSKETCHSEGS
        };
#endif

        private static readonly swDocumentTypes_e[] DefaultDocTypes = new[]
        {
            swDocumentTypes_e.swDocPART,
            swDocumentTypes_e.swDocASSEMBLY
        };

        private ContextMenuRegistrar(ISldWorks solidworks, ICommandManager commandManager, ContextMenuRouter router, int addinId, DirectoryInfo iconsRootDir)
        {
            _solidworks = solidworks ?? throw new ArgumentNullException(nameof(solidworks));
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _frame = solidworks.Frame() as IFrame ?? throw new InvalidOperationException("SolidWorks frame is not available.");
            _router = router ?? throw new ArgumentNullException(nameof(router));
            _addinId = addinId;
            _registrations = new Dictionary<ContextMenuKey, ContextMenuRegistration>();
            _iconsRootDir = iconsRootDir;
        }

        /// <summary>
        /// Creates a registrar tied to an <see cref="AddinMaker"/> instance after the command manager is ready.
        /// </summary>
        public static ContextMenuRegistrar Create(AddinMaker addin)
        {
            if (addin == null)
                throw new ArgumentNullException(nameof(addin));
            if (addin.CommandManager == null)
                throw new InvalidOperationException("Command manager is not initialized yet. Use Create(addin, cookie) during OnStart.");

            return new ContextMenuRegistrar(addin.Solidworks, addin.CommandManager, addin.ContextMenuRouter, addin.AddinId, addin.IconsRootDir);
        }

        /// <summary>
        /// Creates a registrar tied to an <see cref="AddinMaker"/> instance during OnStart.
        /// </summary>
        public static ContextMenuRegistrar Create(AddinMaker addin, int cookie)
        {
            if (addin == null)
                throw new ArgumentNullException(nameof(addin));
            if (addin.Solidworks == null)
                throw new InvalidOperationException("SolidWorks object is not initialized yet.");

            var commandManager = addin.Solidworks.GetCommandManager(cookie);
            return new ContextMenuRegistrar(addin.Solidworks, commandManager, addin.ContextMenuRouter, cookie, addin.IconsRootDir);
        }

        /// <summary>
        /// Registers the given context menu definition for its selection targets.
        /// </summary>
        public void Register(ContextMenuDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            foreach (var target in definition.Targets)
            {
                foreach (var selectionType in target.SelectionTypes)
                {
                    if (!IsSelectionTypeEnabled(selectionType))
                        continue;
                    var key = new ContextMenuKey(selectionType, definition.Title);
                    if (_registrations.ContainsKey(key))
                        continue;

                    var registration = RegisterForSelectionType(definition, selectionType);
                    _registrations[key] = registration;
                }
            }
        }

        /// <summary>
        /// Registers a context menu using the existing add-in command group model.
        /// </summary>
        public void Register(IAddinCommandGroup group, IEnumerable<ContextMenuTarget> targets)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            if (targets == null)
                throw new ArgumentNullException(nameof(targets));

#if DEBUG
            Debug.WriteLine("ContextMenu: Register(IAddinCommandGroup) start");
#endif
            var title = group.Title ?? string.Empty;

            foreach (var target in targets)
            {
                foreach (var selectionType in target.SelectionTypes)
                {
                    var enabled = IsSelectionTypeEnabled(selectionType);
#if DEBUG
                    Debug.WriteLine($"ContextMenu: selectionType {selectionType} enabled={enabled}");
#endif
                    if (!enabled)
                        continue;
                    var key = new ContextMenuKey(selectionType, title);
                    if (_registrations.ContainsKey(key))
                        continue;

                    var registration = RegisterForSelectionType(group, selectionType);
                    _registrations[key] = registration;
                }
            }
        }

        /// <summary>
        /// Registers a context menu using the existing add-in command group model.
        /// </summary>
        public void Register(IAddinCommandGroup group, params ContextMenuTarget[] targets)
        {
            Register(group, (IEnumerable<ContextMenuTarget>)targets);
        }

        private ContextMenuRegistration RegisterForSelectionType(ContextMenuDefinition definition, swSelectType_e selectionType)
        {
            var tokens = new List<string>();
            var entries = new List<PopupEntry>();

            foreach (var item in definition.Items)
            {
                var token = NextToken();
                tokens.Add(token);
                _router.Register(token, item);

                var callback = BuildCallbackName(CallbackMethodName, token);
                var enable = BuildCallbackName(EnableMethodName, token);

                foreach (var docType in DefaultDocTypes)
                {
                    var bitmapPath = BuildSingleIconPath(item.Icon, $"{definition.Title}_{token}", _addinId);
#if DEBUG
                    Debug.WriteLine($"ContextMenu: AddMenuPopupIcon2 call item '{item.Title}' sel={selectionType} doc={docType} icon={bitmapPath ?? "<none>"}");
#endif
                    var created = _frame.AddMenuPopupIcon2(
                        (int)docType,
                        (int)selectionType,
                        item.Title,
                        _addinId,
                        callback,
                        enable,
                        string.Empty,
                        bitmapPath ?? string.Empty);

                    if (created)
                    {
                        entries.Add(new PopupEntry(docType, selectionType, _addinId));
#if DEBUG
                        Debug.WriteLine($"ContextMenu: AddMenuPopupIcon2 success item '{item.Title}' sel={selectionType} doc={docType}");
#endif
                    }
#if DEBUG
                    else
                    {
                        Debug.WriteLine($"ContextMenu: AddMenuPopupIcon2 failed for {selectionType} ({docType})");
                    }
#endif
                }
            }

            return new ContextMenuRegistration(selectionType, definition.Title, tokens, entries);
        }

        private ContextMenuRegistration RegisterForSelectionType(IAddinCommandGroup group, swSelectType_e selectionType)
        {
            var title = group.Title ?? string.Empty;
            var commands = (group.Commands ?? Enumerable.Empty<AddinCommand>()).ToList();
            var entries = new List<PopupEntry>();

            foreach (var command in commands)
            {
                if (command.UserId == -1)
                    continue;

                var callback = command.CallBackFunction ?? string.Empty;
                var enable = command.EnableMethod ?? string.Empty;
                var userId = command.UserId > 0 ? command.UserId : NextMenuId();

                foreach (var docType in DefaultDocTypes)
                {
                    var bitmapPath = BuildSingleIconPath(command.IconBitmap, $"{title}_{userId}", userId);
#if DEBUG
                    Debug.WriteLine($"ContextMenu: AddMenuPopupIcon2 call cmd '{command.Name}' sel={selectionType} doc={docType} icon={bitmapPath ?? "<none>"}");
#endif
                    var created = _frame.AddMenuPopupIcon2(
                        (int)docType,
                        (int)selectionType,
                        command.Name,
                        _addinId,
                        callback,
                        enable,
                        string.Empty,
                        bitmapPath ?? string.Empty);

                    if (created)
                    {
                        entries.Add(new PopupEntry(docType, selectionType, _addinId));
#if DEBUG
                        Debug.WriteLine($"ContextMenu: AddMenuPopupIcon2 success cmd '{command.Name}' sel={selectionType} doc={docType}");
#endif
                    }
#if DEBUG
                    else
                    {
                        Debug.WriteLine($"ContextMenu: AddMenuPopupIcon2 failed for {selectionType} ({docType})");
                    }
#endif
                }
            }

            return new ContextMenuRegistration(selectionType, title, Array.Empty<string>(), entries);
        }

        private string NextToken()
        {
            return _tokenCounter++.ToString(CultureInfo.InvariantCulture);
        }

        private int NextMenuId()
        {
            return _menuIdCounter++;
        }

        private static string BuildCallbackName(string methodName, string token)
        {
            return $"{methodName}({token})";
        }

        private string BuildSingleIconPath(Bitmap bitmap, string baseName, int identifier)
        {
            if (bitmap == null || _iconsRootDir == null)
                return null;

            var safeName = MakeSafeFileName(baseName);
            var dir = Path.Combine(_iconsRootDir.FullName, "context-menus", safeName);
            Directory.CreateDirectory(dir);

            var size = 16;
            var fileName = $"{safeName}_{identifier}_{size}.png";
            var fullPath = Path.Combine(dir, fileName);

            using (var clone = new Bitmap(bitmap))
            using (var resized = new Bitmap(clone, new Size(size, size)))
            {
                resized.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
            }

#if DEBUG
            Debug.WriteLine($"ContextMenu: icon[{size}] -> {fullPath} exists={File.Exists(fullPath)}");
#endif

            return fullPath;
        }

        private static string MakeSafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "context";

            var invalid = Path.GetInvalidFileNameChars();
            var chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Array.IndexOf(invalid, chars[i]) >= 0)
                    chars[i] = '_';
            }

            return new string(chars);
        }

        private static bool IsSelectionTypeEnabled(swSelectType_e selectionType)
        {
#if DEBUG
            return DebugSelectionTypeFilter.Contains(selectionType);
#else
            return true;
#endif
        }

        /// <summary>
        /// Disposes the registrar and removes all registered context menu items.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            foreach (var registration in _registrations.Values.ToList())
            {
                RemoveRegistration(registration);
            }

            _registrations.Clear();
            _disposed = true;
        }

        private void RemoveRegistration(ContextMenuRegistration registration)
        {
            foreach (var token in registration.Tokens)
            {
                _router.Unregister(token);
            }

            foreach (var entry in registration.Entries)
            {
                try
                {
                    _frame.RemoveMenuPopupIcon(entry.Identifier, (int)entry.DocumentType, (int)entry.SelectionType);
                }
                catch
                {
                    // ignore
                }
            }
        }

        private readonly struct ContextMenuKey : IEquatable<ContextMenuKey>
        {
            private readonly swSelectType_e _selectionType;
            private readonly string _title;

            public ContextMenuKey(swSelectType_e selectionType, string title)
            {
                _selectionType = selectionType;
                _title = title ?? string.Empty;
            }

            public bool Equals(ContextMenuKey other)
                => _selectionType == other._selectionType && string.Equals(_title, other._title, StringComparison.OrdinalIgnoreCase);

            public override bool Equals(object obj)
                => obj is ContextMenuKey other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = 17;
                    hash = (hash * 31) + _selectionType.GetHashCode();
                    hash = (hash * 31) + StringComparer.OrdinalIgnoreCase.GetHashCode(_title);
                    return hash;
                }
            }
        }

        private sealed class ContextMenuRegistration
        {
            public swSelectType_e SelectionType { get; }
            public string Title { get; }
            public IReadOnlyList<string> Tokens { get; }
            public IReadOnlyList<PopupEntry> Entries { get; }

            public ContextMenuRegistration(swSelectType_e selectionType, string title, IReadOnlyList<string> tokens, IReadOnlyList<PopupEntry> entries)
            {
                SelectionType = selectionType;
                Title = title;
                Tokens = tokens;
                Entries = entries ?? Array.Empty<PopupEntry>();
            }
        }

        private readonly struct PopupEntry
        {
            public swDocumentTypes_e DocumentType { get; }
            public swSelectType_e SelectionType { get; }
            public int Identifier { get; }

            public PopupEntry(swDocumentTypes_e documentType, swSelectType_e selectionType, int identifier)
            {
                DocumentType = documentType;
                SelectionType = selectionType;
                Identifier = identifier;
            }
        }
    }
}

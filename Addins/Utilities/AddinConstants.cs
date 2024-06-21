// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// generate a new id
    /// </summary>
    public sealed class AddinConstants
    {
        /// <summary>
        /// provides names of all menus  in SOLIDWORKS
        /// </summary>
        public static class SolidworksMenu
        {
            /// <summary>
            /// a list of all menus 
            /// </summary>
            public static string[] AsArray = new[] { File,  View, Insert, Tools,  Help };

            /// <summary>
            /// the file menu
            /// </summary>
            public const string File = "File";

            /// <summary>
            /// the view menu
            /// </summary>
            public const string View = "View";

            /// <summary>
            /// the insert menu
            /// </summary>
            public const string Insert= "Insert";
            /// <summary>
            /// the tools menu
            /// </summary>
            public const string Tools = "Tools";

            /// <summary>
            /// the help menu
            /// </summary>
            public const string Help = "Help";
        }

        internal static string GetBtnSize(BtnSize btnSize)
        {
            switch (btnSize)
            {
                case BtnSize.sixteen:
                    return "16";
                case BtnSize.thirtyTwo:
                    return "32";
                case BtnSize.forty:
                    return "40";
                case BtnSize.sixtyFour:
                    return "64";
                case BtnSize.ninetySix:
                    return "96";
                case BtnSize.hundredTwentyEight:
                    return "128";
                default:
                    return "16";
            }
        }
    }
}

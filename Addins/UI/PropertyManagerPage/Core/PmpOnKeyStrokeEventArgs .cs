﻿// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{

    /// <summary>
    /// event arguments for <see cref="PmpUiModel.KeyStroke"/>
    /// </summary>
    public class PmpKeyStrokeEventArgs : EventArgs
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="wParam"></param>
        /// <param name="message"></param>
        /// <param name="lParam"></param>
        public PmpKeyStrokeEventArgs(int wParam, int message, int lParam)
        {
            this.Wparam = wParam;
            this.Message = message;
            this.Lparam = lParam;
        }

        ///<summary>
        /// wparam argument from Windows processing; indicates the keystroke that occurred
        ///</summary>
        public int Wparam { get; }

        ///<summary>
        /// Message being processed by Windows; one of these values:
        ///</summary>
        public int Message { get; }

        ///<summary>
        ///lparam argument from Windows processing; bitmask containing various pieces of information; dependent on specific message
        ///</summary>
        public int Lparam { get; }
    }
}

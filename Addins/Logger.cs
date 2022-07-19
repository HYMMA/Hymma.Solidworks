// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins
{
    public static class Logger
    {
        private static string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"AddinLogs.txt");
        public static void log(string text, [CallerMemberName] string memberName = "")
        {
            File.AppendAllLines(logPath, new[] { $"from {memberName}: {text}" });
        }
    }
}

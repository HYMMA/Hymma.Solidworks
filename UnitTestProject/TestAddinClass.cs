// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using QRify;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = System.Environment;

namespace UnitTestProject
{
    internal class TestAddinUi : AddinUserInterface
    {
        public TestAddinUi()
        {
            var solidworks = new DummySolidworks();
            IconsRootDir = GetIconsRootDir();
            var pmp1 = new QrPropertyManagerPage(solidworks);
            var pmp2 = new QrPropertyManagerPage(solidworks);
            var tab= new QrTab();
            var tab2 = new QrTab() { Title = "Title2" };
            PropertyManagerPages = new List<PropertyManagerPageX64>()
            {
                pmp1, pmp2
            };
            CommandTabs = new List<AddinCommandTab>()
            {
               tab, tab2
            };
        }

        static DirectoryInfo GetIconsRootDir() => new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
    }
}

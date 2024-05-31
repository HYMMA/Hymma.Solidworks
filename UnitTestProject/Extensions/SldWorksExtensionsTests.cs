// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnitTestProject.Extensions
{
    [TestClass]
    [TestCategory("Extensions")]
    public class SldWorksExtensionsTests
    {
        List<int> solidworksProcessIds = new List<int>();
        private ISldWorks swApp;

        [TestInitialize]
        [TestMethod]
        public void WhenSolidworksIsInstalled_AndIsNotRunning_ShouldStartANewInstance()
        {
            //when solidworks is installed
            //var type = Type.GetTypeFromCLSID(new Guid("83A33D22-27C5-11CE-BFD4-00400513BB57"));
            //Assert.IsNotNull(type);
            //Assert.IsTrue(type.IsCOMObject);

            //and is not running
            var solidworksProcess = Process.GetProcessesByName("SLDWORKS.exe", ".");
            Assert.IsTrue(solidworksProcess.Count() == 0);

            //should start new instance
            swApp = SolidworksManager.StartNewSolidworksApp();
            solidworksProcessIds.Add(swApp.GetProcessID());
            Assert.IsNotNull(swApp.RevisionNumber());
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            foreach (var item in solidworksProcessIds)
            {
                Process.GetProcessById(item).Kill();
            }
        }
    }
}

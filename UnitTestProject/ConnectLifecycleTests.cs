// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class ConnectLifecycleTests
    {
        /// <summary>
        /// Minimal <see cref="AddinMaker"/> that records the order in which the framework invokes
        /// its lifecycle callbacks during <c>ConnectToSW</c>.
        /// </summary>
        private sealed class OrderRecordingAddin : AddinMaker
        {
            public readonly List<string> Order = new List<string>();

            public OrderRecordingAddin()
            {
                OnStart += (s, e) => Order.Add("OnStart");
            }

            public override AddinUserInterface GetUserInterFace()
            {
                Order.Add("GetUserInterFace");
                return new AddinUserInterface
                {
                    IconsRootDir = new DirectoryInfo(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                };
            }
        }

        /// <summary>
        /// Guards the connect lifecycle contract: <c>OnStart</c> MUST be raised before
        /// <c>GetUserInterFace()</c>. Consumers build license-gated UI in <c>GetUserInterFace()</c>
        /// using state initialised in their <c>OnStart</c> handler; if this order is ever flipped
        /// again the add-in silently fails to load in SOLIDWORKS (a NullReferenceException is
        /// swallowed across the COM boundary, with no dialog and no log).
        /// </summary>
        [TestMethod]
        public void ConnectToSW_RaisesOnStart_BeforeGetUserInterFace()
        {
            var addin = new OrderRecordingAddin();
            var sw = new DummySolidworks();

            // ConnectToSW has an internal error boundary, so the dummy throwing in a later phase
            // (SetupCallbacks) returns false instead of throwing. OnStart and GetUserInterFace have
            // already run, in order, by then.
            addin.ConnectToSW(sw, 0);

            CollectionAssert.Contains(addin.Order, "OnStart",
                "OnStart was never raised during ConnectToSW.");
            CollectionAssert.Contains(addin.Order, "GetUserInterFace",
                "GetUserInterFace was never called during ConnectToSW.");
            Assert.IsTrue(
                addin.Order.IndexOf("OnStart") < addin.Order.IndexOf("GetUserInterFace"),
                "OnStart must be raised BEFORE GetUserInterFace(). Actual order: "
                + string.Join(", ", addin.Order));
        }
    }
}

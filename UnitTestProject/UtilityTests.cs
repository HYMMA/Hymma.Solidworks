// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRify;
using QrifyPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace UnitTestProject
{
    [TestClass]
    [TestCategory("Framework")]
    public class UtilityTests
    {
        private DirectoryInfo _testImageDir;

        [TestInitialize]
        public void Initialize()
        {
            _testImageDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"TestImages"));
        }
        [DynamicData(nameof(GetImages), DynamicDataSourceType.Method)]
        [DataTestMethod]
        public void ShouldSaveAddinIconAsStandard(Bitmap image)
        {
            var fullFileName = AddinIcons.SaveAsStandardSize(image, _testImageDir.FullName, Guid.NewGuid().ToString());

            Assert.IsTrue(File.Exists(fullFileName));

            using (var bmp = new Bitmap(fullFileName, false))
            {
                Assert.IsTrue(bmp.RawFormat.Equals(ImageFormat.Png));
                Assert.IsTrue(bmp.Size == new Size(16, 16));
            }
        }

        [TestMethod]
        public void WhenAddinModule_HasAddinIconAsEmbeddedResource_ShouldExtractItAsBitmap()
        {
            //Arrange
            var type = typeof(Qrify);

            //Act
            var icon = AddinIcons.GetAddinIcon(type);

            //Assert
            Assert.IsNotNull(icon);
        }

        [TestMethod]
        public void WhenAddinModule_HasAddinIconInResxFiles_ShouldExtractItAsBitmap()
        {
            //Arrange
            var type = typeof(QrifyPlus.QrifyPlus);

            //Act
            var icon = AddinIcons.GetAddinIcon(type);

            //Assert
            Assert.IsNotNull(icon);
        }

        [ClassCleanup]
        public static void CleanClass()
        {
            //var localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var _testImageDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"TestImages"));
            Directory.Delete(_testImageDir.FullName, true);
        }

        public static IEnumerable<object[]> GetImages()
        {
            yield return new object[] { Properties.Resources.simpson };
            yield return new object[] { Properties.Resources.qrify };
            yield return new object[] { Properties.Resources.box };
            yield return new object[] { Properties.Resources.knight };
            yield return new object[] { Properties.Resources.ExportDxf };
        }
    }
}

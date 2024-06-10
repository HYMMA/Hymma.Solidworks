// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.Utilities.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRify;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnitTestProject
{
    [TestClass]
    [TestCategory("Framework")]
    public class UtilityTests
    {
        private DirectoryInfo _testImageDir;
        private AddinUserInterface _ui;

        [TestInitialize]
        public void Initialize()
        {
            _testImageDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"TestImages"));
            _ui = new AddinUserInterface() { IconsRootDir = _testImageDir };

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
            var _testDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"TestImages"));
            Directory.Delete(_testDir.FullName, true);
        }

        [TestMethod]
        public void WhenAddinHasMultiplePMP_WithUniqueTitles_ShouldCreateSubFolderForEach()
        {
            //Arrange
            var addinUi = new TestAddinUi
            {
                IconsRootDir = _testImageDir
            };

            //ACT
            int i = 0;
            foreach (var item in addinUi.PropertyManagerPages)
            {
                item.UiModel.Title = "Title for this property manager page" + i;
                i++;
            }
            AddinIcons.CreateSubDirForUiItems(addinUi);

            //Assert
            var foldersCreated = addinUi.PropertyManagerPages.GroupBy(p => p.UiModel.IconDir.FullName)
                                                             .Count();

            var foldersRequired = addinUi.PropertyManagerPages.Count;
            Assert.IsTrue(foldersCreated == foldersRequired);
        }

        [TestMethod]
        public void WhenAddinHasMultipleTabs_WithUniqueTabTitles_ShouldCreateSubFolderForEach()
        {
            //Arrange
            var addinUi = new TestAddinUi
            {
                IconsRootDir = _testImageDir
            };

            //ACT
            for (int i = 0; i < addinUi.CommandTabs.Count; i++)
            {
                var tab = addinUi.CommandTabs[i];
                tab.CommandGroup.UserId = i;
                tab.Title = "some" + i;
            }
            AddinIcons.CreateSubDirForUiItems(addinUi);

            //Assert
            var foldersCreated = addinUi.CommandTabs.GroupBy(p => p.CommandGroup.IconsDir.FullName)
                                                             .Count();

            var foldersRequired = addinUi.CommandTabs.Count;

            Assert.IsTrue(foldersCreated == foldersRequired);
        }

        [TestMethod]
        public void WhenAddinHasMultipleTabs_IfTabUserIdsAreNotUnique_ShouldCreateUniqueFolderNames()
        {
            //Arrange
            var addinUi = new TestAddinUi
            {
                IconsRootDir = _testImageDir
            };
            var expectedDirNames = new List<string>();
            for (int i = 0; i < addinUi.CommandTabs.Count; i++)
            {
                var tab  = addinUi.CommandTabs [i];
                tab.Title = "same";
                expectedDirNames.Add(Path.Combine(_testImageDir.FullName, "cmdGrp"+tab.Title + i));
            }

            //ACT
            AddinIcons.CreateTabIconsDir(addinUi);

            //Assert
            Assert.IsTrue(expectedDirNames.All(e => Directory.Exists(e)));
        }

        [TestMethod]
        public void WhenAddinHasMultiplePMP_IfPmpTitlesAreNotUnique_ShouldCreateUniqueSubfolders()
        {
            //Arrange
            var addinUi = new TestAddinUi
            {
                IconsRootDir = _testImageDir
            };
            var expectedDirNames = new List<string>();
            for (int i = 0; i < addinUi.PropertyManagerPages.Count; i++)
            {
                var pmp  = addinUi.PropertyManagerPages [i];
                pmp.UiModel.Title= "same";
                expectedDirNames.Add(Path.Combine(_testImageDir.FullName, "pmp"+pmp.UiModel.Title + i));
            }

            AddinIcons.CreatePropertyManagerPageIconsDir(addinUi);

            //ACT
            //Assert
            Assert.IsTrue(expectedDirNames.All(e => Directory.Exists(e)));
        }

        [TestMethod]
        public void ShouldFindTask_InAssembly()
        {
            var path = @"..\..\..\Interop\build\Hymma.BuildTasks.dll";
            var assy =Assembly.LoadFrom(path);
            Assert.IsNotNull(assy);
        }

        [DynamicData(nameof(GetInvalidFileNames), DynamicDataSourceType.Method)]
        [TestMethod]
        public void PathHelper_ShouldRemoveInvalidCharFromString(string input)
        {
            var output = PathHelpers.RemoveInvalidFileNameChars(input);
            var dir = _testImageDir.CreateSubdirectory(output);
            Assert.IsTrue(dir.Exists);
        }

        public static IEnumerable<object[]> GetInvalidFileNames()
        {
            var chars = Path.GetInvalidFileNameChars();
            foreach (var item in chars)
            {
                yield return new object[] { "a" + item };
            }
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

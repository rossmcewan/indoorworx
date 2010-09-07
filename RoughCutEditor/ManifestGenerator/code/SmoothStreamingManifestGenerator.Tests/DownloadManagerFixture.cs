// <copyright file="DownloadManagerFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: DownloadManagerFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace SmoothStreamingManifestGenerator.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DownloadManagerFixture
    {
        private readonly Uri manifestUri = new Uri("http://mediadl.microsoft.com/mediadl/iisnet/smoothmedia/Experience/BigBuckBunny_720p.ism/Manifest");

        [TestMethod]
        public void ShouldDownloadManifestStream()
        {
            DownloaderManager downloaderManager = new DownloaderManager();

            var stream = downloaderManager.DownloadManifest(this.manifestUri, true);

            Assert.IsNotNull(stream);
        }
    }
}

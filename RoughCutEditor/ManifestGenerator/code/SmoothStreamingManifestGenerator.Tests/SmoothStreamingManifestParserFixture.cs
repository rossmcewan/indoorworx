// <copyright file="SmoothStreamingManifestParserFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SmoothStreamingManifestParser.cs                     
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
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SmoothStreamingManifestParserFixture
    {
        [TestMethod]
        [DeploymentItem(@".\Content\SampleManifest_version1.ismc", @".\Content")]
        public void ShouldExposeManifestInformationViaManifestInfoClassOfaSmoothStreamingManifestVersion1()
        {
            var manifestStream = new FileStream(@".\Content\SampleManifest_version1.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            Assert.AreEqual(1, manifestParser.ManifestInfo.MajorVersion);
            Assert.AreEqual(0, manifestParser.ManifestInfo.MinorVersion);
            Assert.AreEqual(5964583334u, manifestParser.ManifestInfo.ManifestDuration);
            Assert.AreEqual(299, manifestParser.ManifestInfo.Streams[0].Chunks.Count);
            Assert.AreEqual(299, manifestParser.ManifestInfo.Streams[1].Chunks .Count);
        }

        [TestMethod]
        [DeploymentItem(@".\Content\SampleManifest_version2.ismc", @".\Content")]
        public void ShouldExposeManifestInformationViaManifestInfoClassOfaSmoothStreamingManifestVersion2()
        {
            var manifestStream = new FileStream(@".\Content\SampleManifest_version2.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            Assert.AreEqual(2, manifestParser.ManifestInfo.MajorVersion);
            Assert.AreEqual(0, manifestParser.ManifestInfo.MinorVersion);
            Assert.AreEqual(13088400000u, manifestParser.ManifestInfo.ManifestDuration);
            Assert.AreEqual(803, manifestParser.ManifestInfo.Streams[0].Chunks.Count);
            Assert.AreEqual(655, manifestParser.ManifestInfo.Streams[1].Chunks.Count);
        }
    }
}

// <copyright file="ManifestParserAndManifestWriterFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ReflectionParserAndManifestWriterFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace SmoothStreamingManifestGenerator.Tests.IntegrationTests
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;

    [TestClass]
    public class ManifestParserAndManifestWriterFixture
    {
        private readonly Uri manifestUri = new Uri("http://mediadl.microsoft.com/mediadl/iisnet/smoothmedia/Experience/BigBuckBunny_720p.ism/Manifest");

        [TestMethod]
        [DeploymentItem(@".\Content\SampleManifest_version1.ismc", @".\Content")]
        public void ShouldGenerateSmoothStreamingManifestVersion1()
        {
            var manifestStream = new FileStream(@".\Content\SampleManifest_version1.ismc", FileMode.Open, FileAccess.Read);

            StreamReader reader = new StreamReader(manifestStream);
            string input = reader.ReadToEnd();
            
            manifestStream.Seek(0, SeekOrigin.Begin);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            reader.Close();

            var manifestWriter = new SmoothStreamingManifestWriter();

            string output = manifestWriter.GenerateClientManifest(manifestParser.ManifestInfo);
            
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        [DeploymentItem(@".\Content\PackersBears.ismc", @".\Content")]
        public void ShouldGenerateSmoothStreamingManifestVersion1WithCustomAttributes()
        {
            var manifestStream = new FileStream(@".\Content\PackersBears.ismc", FileMode.Open, FileAccess.Read);

            StreamReader reader = new StreamReader(manifestStream);
            string input = reader.ReadToEnd();

            manifestStream.Seek(0, SeekOrigin.Begin);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            reader.Close();

            var manifestWriter = new SmoothStreamingManifestWriter();

            string output = manifestWriter.GenerateClientManifest(manifestParser.ManifestInfo);

            Assert.AreEqual(input, output);
        }

        [TestMethod]
        [DeploymentItem(@".\Content\SampleManifest_version2.ismc", @".\Content")]
        public void ShouldGenerateSmoothStreamingManifestVersion2()
        {
            var manifestStream = new FileStream(@".\Content\SampleManifest_version2.ismc", FileMode.Open, FileAccess.Read);

            StreamReader reader = new StreamReader(manifestStream);
            string input = reader.ReadToEnd();

            manifestStream.Seek(0, SeekOrigin.Begin);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            reader.Close();

            var manifestWriter = new SmoothStreamingManifestWriter();

            string output = manifestWriter.GenerateClientManifest(manifestParser.ManifestInfo);

            Assert.AreEqual(input, output);
        }

        [TestMethod]
        [DeploymentItem(@".\Content\SampleManifest_version1.ismc", @".\Content")]
        [DeploymentItem(@".\Content\VideoAudioCompositeSampleManifest_version1.ismc", @".\Content")]
        public void ShouldGenerateCompositeSmoothStreamingManifestVersion1()
        {
            string expectedManifest;
            
            using (var expectedManifestStream = new FileStream(@".\Content\VideoAudioCompositeSampleManifest_version1.ismc", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\SampleManifest_version1.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 400000000, 900000000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\Manifest_version2.ismc", @".\Content")]
        [DeploymentItem(@".\Content\VideoAudioCompositeSampleManifest_version2.ismc", @".\Content")]
        public void ShouldGenerateCompositeSmoothStreamingManifestVersion2()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\VideoAudioCompositeSampleManifest_version2.ismc", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\Manifest_version2.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 0, 250000000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\Bumper.ismc", @".\Content")]
        [DeploymentItem(@".\Content\Bumper.csm", @".\Content")]
        public void ShouldGenerateBumperCompositeSmoothStreamingManifestVersion2()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\Bumper.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\Bumper.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 0, 30000000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\Bumper.ismc", @".\Content")]
        [DeploymentItem(@".\Content\Bumper1.csm", @".\Content")]
        public void ShouldGenerateBumperCompositeSmoothStreamingManifestVersion2EvenIfClipEndIsOutOfRange()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\Bumper1.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\Bumper.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 0, 600000000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\FirstAudioChunkLarger.ismc", @".\Content")]
        [DeploymentItem(@".\Content\CompositeManifestAudioChunkLarger.csm", @".\Content")]
        public void ShouldGenerateCompositeSmoothStreamingManifestVersion2WithASourceManifestWithLargerFirstAudioChunk()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\CompositeManifestAudioChunkLarger.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\FirstAudioChunkLarger.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 72020000000, 78270000000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\FirstAudioChunkLarger.ismc", @".\Content")]
        [DeploymentItem(@".\Content\CompositeManifestAudioChunkLarger1.csm", @".\Content")]
        public void ShouldGenerateCompositeSmoothStreamingManifestVersion2WithASourceManifestWithLargerFirstAudioChunk1()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\CompositeManifestAudioChunkLarger1.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\LargerAudio1.ismc", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 348681667, 1840000000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\frame1.xml", @".\Content")]
        [DeploymentItem(@".\Content\frame1.csm", @".\Content")]
        public void ShouldMaintainFrameAccuracy1WhenGeneratingCompositeSmoothStreamingManifestVersion2()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\frame1.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\frame1.xml", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 355962580000, 356805200000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\frame2.xml", @".\Content")]
        [DeploymentItem(@".\Content\frame2.csm", @".\Content")]
        public void ShouldMaintainFrameAccuracy2WhenGeneratingCompositeSmoothStreamingManifestVersion2()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\frame2.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\frame2.xml", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 394311093750, 395006410000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\interstitial1.xml", @".\Content")]
        [DeploymentItem(@".\Content\interstitial1.csm", @".\Content")]
        public void ShouldNotOverflowOnSourceManifestWithDuration1()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\interstitial1.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\interstitial1.xml", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 11550000, 46280000);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\interstitial2.xml", @".\Content")]
        [DeploymentItem(@".\Content\interstitial2.csm", @".\Content")]
        public void ShouldNotOverflowOnSourceManifestWithDuration2()
        {
            string expectedManifest;

            using (var expectedManifestStream = new FileStream(@".\Content\interstitial2.csm", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(expectedManifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            var manifestStream = new FileStream(@".\Content\interstitial2.xml", FileMode.Open, FileAccess.Read);

            var manifestParser = new SmoothStreamingManifestParser(manifestStream);

            var manifestWriter = new SmoothStreamingManifestWriter();

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(manifestParser.ManifestInfo.MajorVersion, manifestParser.ManifestInfo.MinorVersion);

            Clip clip = new Clip(this.manifestUri, 611540, 23391570);

            foreach (StreamInfo streamInfo in manifestParser.ManifestInfo.Streams)
            {
                clip.Streams.Add(streamInfo);
            }

            manifestInfo.Clips.Add(clip);

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }
    }
}

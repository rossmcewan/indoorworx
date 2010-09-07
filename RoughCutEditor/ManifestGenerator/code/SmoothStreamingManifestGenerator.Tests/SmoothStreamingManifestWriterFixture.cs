// <copyright file="SmoothStreamingManifestWriterFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SmoothStreamingManifestWriterFixture.cs                     
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
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;

    [TestClass]
    public class SmoothStreamingManifestWriterFixture
    {
        [TestMethod]
        public void ShouldGenerateServerManifest()
        {
            const string ExpectedServerManifest = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<smil xmlns=\"http://www.w3.org/2001/SMIL20/Language\">\r\n  <head>\r\n    <meta\r\n      name=\"clientManifestRelativePath\"\r\n      content=\"test.ismc\" />\r\n  </head>\r\n  <body>\r\n    <switch>\r\n      <video\r\n        src=\"test_4000.ismv\"\r\n        systemBitrate=\"4000000\">\r\n        <param\r\n          name=\"trackID\"\r\n          value=\"2\"\r\n          valuetype=\"data\" />\r\n      </video>\r\n      <video\r\n        src=\"test_1700.ismv\"\r\n        systemBitrate=\"1700000\">\r\n        <param\r\n          name=\"trackID\"\r\n          value=\"2\"\r\n          valuetype=\"data\" />\r\n      </video>\r\n      <video\r\n        src=\"test_900.ismv\"\r\n        systemBitrate=\"900000\">\r\n        <param\r\n          name=\"trackID\"\r\n          value=\"2\"\r\n          valuetype=\"data\" />\r\n      </video>\r\n      <audio\r\n        src=\"test_4000.ismv\"\r\n        systemBitrate=\"64000\">\r\n        <param\r\n          name=\"trackID\"\r\n          value=\"1\"\r\n          valuetype=\"data\" />\r\n      </audio>\r\n    </switch>\r\n  </body>\r\n</smil>";

            var manifestWriter = new SmoothStreamingManifestWriter();

            var switchs = new List<SwitchInfo>
                              {
                                  new SwitchInfo("test_4000.ismv", 4000, FileType.Video),
                                  new SwitchInfo("test_1700.ismv", 1700, FileType.Video),
                                  new SwitchInfo("test_900.ismv", 900, FileType.Video),
                                  new SwitchInfo("test_4000.ismv", 64, FileType.Audio),
                              };

            var output = manifestWriter.GenerateServerManifest("test.ismc", switchs);

            Assert.AreEqual(ExpectedServerManifest, output);
        }
        
        [TestMethod]
        [DeploymentItem(@".\Content\SimpleCompositeSampleManifest_version1.csm", @".\Content")]
        public void ShouldGenerateSimpleCompositeManifestVersion1()
        {
            string expectedManifest;

            using (var manifestStream = new FileStream(@".\Content\SimpleCompositeSampleManifest_version1.csm", FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(manifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(1, 0);
            Clip clip = new Clip(new Uri("http://server/stream1.isml/Manifest"), 6400000000, 6510000000);

            StreamInfo streamInfo = new StreamInfo("video");

            streamInfo.AddAttribute("Chunks", "0");
            streamInfo.AddAttribute("Type", "video");
            streamInfo.AddAttribute("SubType", "WVC1");
            streamInfo.AddAttribute("Url", "QualityLevels({bitrate})/Fragments(video={start time})");

            ulong[] bitrates = { 350000, 1050000, 600000, 1450000 };

            int[][] sizes = { new[] { 320, 176 }, new[] { 592, 336 }, new[] { 424, 240 }, new[] { 848, 476 } };

            for (int i = 0; i < bitrates.Length; i ++)
            {
                QualityLevel qualityLevel = new QualityLevel();
                qualityLevel.AddAttribute("Bitrate", bitrates[i].ToString());
                qualityLevel.AddAttribute("FourCC", "WVC1");
                qualityLevel.AddAttribute("Width", sizes[i][0].ToString());
                qualityLevel.AddAttribute("Height", sizes[i][1].ToString());
                qualityLevel.AddAttribute("CodecPrivateData", "250000010FCBA01270A58A12782968045080A00AE020C00000010E5A47F840");

                streamInfo.QualityLevels.Add(qualityLevel);
            }

            for (int i = 0; i < 325; i++)
            {
                ulong time = ((ulong)i * 20000000) + 20000000;

                ulong? duration = null;

                if (i == 324)
                {
                    duration = 17500001;
                }

                Chunk chunk = new Chunk(null, time, duration);

                streamInfo.Chunks.Add(chunk);
            }
            
            clip.Streams.Add(streamInfo);
            manifestInfo.Clips.Add(clip);

            var manifestWriter = new SmoothStreamingManifestWriter();

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, true);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\Content\SimpleCompositeSampleManifest_version1.csm", @".\Content")]
        public void ShouldGenerateSimpleCompositeManifestVersion1WithAdOpportunities()
        {
            string expectedManifest;

            using (var manifestStream = new FileStream(@".\Content\SimpleCompositeSampleManifest_version1_ads.csm", FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(manifestStream))
                {
                    expectedManifest = reader.ReadToEnd();
                }
            }

            CompositeManifestInfo manifestInfo = new CompositeManifestInfo(1, 0);
            Clip clip = new Clip(new Uri("http://server/stream1.isml/Manifest"), 6400000000, 6510000000);

            StreamInfo streamInfo = new StreamInfo("video");

            streamInfo.AddAttribute("Chunks", "0");
            streamInfo.AddAttribute("Type", "video");
            streamInfo.AddAttribute("SubType", "WVC1");
            streamInfo.AddAttribute("Url", "QualityLevels({bitrate})/Fragments(video={start time})");

            ulong[] bitrates = { 350000, 1050000, 600000, 1450000 };

            int[][] sizes = { new[] { 320, 176 }, new[] { 592, 336 }, new[] { 424, 240 }, new[] { 848, 476 } };

            for (int i = 0; i < bitrates.Length; i++)
            {
                QualityLevel qualityLevel = new QualityLevel();
                qualityLevel.AddAttribute("Bitrate", bitrates[i].ToString());
                qualityLevel.AddAttribute("FourCC", "WVC1");
                qualityLevel.AddAttribute("Width", sizes[i][0].ToString());
                qualityLevel.AddAttribute("Height", sizes[i][1].ToString());
                qualityLevel.AddAttribute("CodecPrivateData", "250000010FCBA01270A58A12782968045080A00AE020C00000010E5A47F840");

                streamInfo.QualityLevels.Add(qualityLevel);
            }

            for (int i = 0; i < 325; i++)
            {
                ulong time = ((ulong)i * 20000000) + 20000000;

                ulong? duration = null;

                if (i == 324)
                {
                    duration = 17500001;
                }

                Chunk chunk = new Chunk(null, time, duration);

                streamInfo.Chunks.Add(chunk);
            }

            clip.Streams.Add(streamInfo);
            manifestInfo.Clips.Add(clip);
            manifestInfo.AdsDataStreamName = "NBC-AD";
            manifestInfo.AddAdOpportunity(new Guid("36eb977c-d018-41c9-b565-e2ad2c4fa04f"), "A", TimeSpan.FromSeconds(38745.816).Ticks);

            var manifestWriter = new SmoothStreamingManifestWriter();

            string output = manifestWriter.GenerateCompositeManifest(manifestInfo, false);

            XDocument expectedDocument = XDocument.Parse(expectedManifest);
            XDocument outputDocument = XDocument.Parse(output);

            Assert.AreEqual(expectedDocument.ToString(), outputDocument.ToString());
        }

        // [TestMethod]
        // public void ShouldGenerateClientManifestVersion1()
        // {
        //    const string ExpectedClientManifest = "";
        //    StreamInfo streamInfo = new StreamInfo();
        //    ManifestInfo manifestInfo = new ManifestInfo(1, 0, 5964583334, null, null);
        //    var manifestWriter = new SmoothStreamingManifestWriter();
        //    var output = manifestWriter.GenerateClientManifest(manifestInfo);
        //    Assert.AreEqual(ExpectedClientManifest, output);
        // }
    }
}

// <copyright file="SmoothStreamingManifestParser.cs" company="Microsoft Corporation">
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

namespace IndoorWorx.SmoothStreaming
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Models;

    /// <summary>
    /// Parses a Smooth Streaming Manifest.
    /// </summary>
    public class SmoothStreamingManifestParser
    {
        /// <summary>
        /// Defines the Manifest SmoothStreamingMedia element.
        /// </summary>
        private const string ManifestSmoothStreamingMediaElement = "SmoothStreamingMedia";

        /// <summary>
        /// Defines the Manifest MajorVersion attribute.
        /// </summary>
        private const string ManifestMajorVersionAttribute = "MajorVersion";

        /// <summary>
        /// Defines the Manifest MinorVersion attribute.
        /// </summary>
        private const string ManifestMinorVersionAttribute = "MinorVersion";

        /// <summary>
        /// Defines the Manifest Duration attribute.
        /// </summary>
        private const string ManifestDurationAttribute = "Duration";

        /// <summary>
        /// Defines the Manifest StreamIndex element.
        /// </summary>
        private const string ManifestStreamIndexElement = "StreamIndex";

        /// <summary>
        /// Defines the Manifest StreamIndex Type attribute.
        /// </summary>
        private const string ManifestStreamIndexTypeAttribute = "Type";

        /// <summary>
        /// Defines the Manifest StreamIndex SubType attribute.
        /// </summary>
        private const string ManifestStreamIndexSubTypeAttribute = "SubType";

        /// <summary>
        /// Defines the Manifest StreamIndex Url attribute.
        /// </summary>
        private const string ManifestStreamIndexUrlAttribute = "Url";

        /// <summary>
        /// Initializes a new instance of the <see cref="SmoothStreamingManifestParser"/> class.
        /// </summary>
        /// <param name="manifestStream">The stream of the manifest being parsed.</param>
        public SmoothStreamingManifestParser(Stream manifestStream)
        {
            this.ParseManifest(manifestStream);
        }

        /// <summary>
        /// Gets the <see cref="ManifestInfo"/> of the parsed stream.
        /// </summary>
        /// <value>The manifest information containing all the need it information.</value>
        public ManifestInfo ManifestInfo { get; private set; }

        /// <summary>
        /// Adds attributes to the stream info.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        /// <param name="streamInfo">The stream info.</param>
        private static void AddAttributes(XmlReader reader, StreamInfo streamInfo)
        {
            if (reader.HasAttributes && reader.MoveToFirstAttribute())
            {
                do
                {
                    streamInfo.AddAttribute(reader.Name, reader.Value);
                }
                while (reader.MoveToNextAttribute());
                reader.MoveToFirstAttribute();
            }
        }

        /// <summary>
        /// Adds attributes to the quality level.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        /// <param name="qualityLevel">The quality level.</param>
        private static void AddAttributes(XmlReader reader, QualityLevel qualityLevel)
        {
            if (reader.HasAttributes && reader.MoveToFirstAttribute())
            {
                do
                {
                    qualityLevel.AddAttribute(reader.Name, reader.Value);
                }
                while (reader.MoveToNextAttribute());
                reader.MoveToElement();
            }
        }

        /// <summary>
        /// Adds custom attributes to the quality level.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        /// <param name="qualityLevel">The quality level.</param>
        private static void AddCustomAttributes(XmlReader reader, QualityLevel qualityLevel)
        {
            if (!reader.IsEmptyElement)
            {
                while (reader.Read())
                {
                    if (reader.Name == "CustomAttributes" && reader.NodeType == XmlNodeType.Element)
                    {
                        while (reader.Read())
                        {
                            if ((reader.Name == "Attribute") && (reader.NodeType == XmlNodeType.Element))
                            {
                                string attribute = reader.GetAttribute("Name");

                                if (!string.IsNullOrEmpty(attribute))
                                {
                                    qualityLevel.AddCustomAttribute(attribute, reader.GetAttribute("Value"));
                                }
                            }

                            if ((reader.Name == "CustomAttributes") && (reader.NodeType == XmlNodeType.EndElement))
                            {
                                return;
                            }
                        }
                    }

                    if (reader.Name == "QualityLevel" && reader.NodeType == XmlNodeType.EndElement)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Parses the manifest stream.
        /// </summary>
        /// <param name="manifestStream">The manifest stream being parsed.</param>
        private void ParseManifest(Stream manifestStream)
        {
            using (XmlReader reader = XmlReader.Create(manifestStream))
            {
                if (reader.Read() && reader.IsStartElement(ManifestSmoothStreamingMediaElement))
                {
                    int majorVersion = reader.GetValueAsInt(ManifestMajorVersionAttribute).GetValueOrDefault();
                    int minorVersion = reader.GetValueAsInt(ManifestMinorVersionAttribute).GetValueOrDefault();
                    ulong manifestDuration = reader.GetValueAsULong(ManifestDurationAttribute).GetValueOrDefault();

                    List<StreamInfo> streams = new List<StreamInfo>();

                    while (reader.Read())
                    {
                        if (reader.Name == ManifestStreamIndexElement && reader.NodeType == XmlNodeType.Element)
                        {
                            string type = reader.GetValue(ManifestStreamIndexTypeAttribute);

                            StreamInfo streamInfo = new StreamInfo(type);

                            AddAttributes(reader, streamInfo);

                            while (reader.Read())
                            {
                                if (reader.Name == ManifestStreamIndexElement && reader.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }

                                if ((reader.Name == "QualityLevel") && (reader.NodeType == XmlNodeType.Element))
                                {
                                    QualityLevel qualityLevel = new QualityLevel();

                                    AddAttributes(reader, qualityLevel);

                                    AddCustomAttributes(reader, qualityLevel);

                                    streamInfo.QualityLevels.Add(qualityLevel);
                                }

                                if ((reader.Name == "c") && (reader.NodeType == XmlNodeType.Element))
                                {
                                    int? chunkId = reader.GetValueAsInt("n");
                                    ulong? time = reader.GetValueAsULong("t");
                                    ulong? duration = reader.GetValueAsULong("d");

                                    Chunk chunk = new Chunk(chunkId, time, duration);

                                    if (((!reader.IsEmptyElement && reader.Read()) && (reader.IsStartElement("f") && reader.Read())) && (reader.NodeType == XmlNodeType.Text))
                                    {
                                        chunk.Value = reader.Value;
                                    }

                                    streamInfo.Chunks.Add(chunk);
                                }
                            }

                            streams.Add(streamInfo);
                        }
                    }

                    streams.ToArray();

                    ManifestInfo manifestInfo = new ManifestInfo(majorVersion, minorVersion, manifestDuration, streams);

                    this.ManifestInfo = manifestInfo;
                }
            }
        }
    }
}

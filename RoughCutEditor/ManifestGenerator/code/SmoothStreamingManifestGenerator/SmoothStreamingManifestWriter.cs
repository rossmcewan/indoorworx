// <copyright file="SmoothStreamingManifestWriter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SmoothStreamingManifestWriter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace SmoothStreamingManifestGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using Models;

    /// <summary>
    /// Generates SmoothStreaming client and server manifests.
    /// </summary>
    public class SmoothStreamingManifestWriter
    {
        /// <summary>
        /// Generates a client manifest based on the given <see cref="ManifestInfo"/>.
        /// </summary>
        /// <param name="manifestInfo">The manifest info that contains all the information about the manifest to generate.</param>
        /// <returns>The generated Smooth Streaming client manifest.</returns>
        public string GenerateClientManifest(ManifestInfo manifestInfo)
        {
            StringBuilder output = new StringBuilder();

            XmlWriter writer = CreateWriter(output, false, false);

            if (writer != null)
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("SmoothStreamingMedia");
                WriteHeader(writer, manifestInfo);

                foreach (StreamInfo streamInfo in manifestInfo.Streams)
                {
                    WriteStreamIndex(writer, streamInfo, manifestInfo.MajorVersion, true);
                }

                // TODO: Add support for text streams.
                writer.WriteEndElement();
                writer.Close();
            }

            return output.ToString();
        }

        /// <summary>
        /// Generates a composite manifest based on the given <see cref="CompositeManifestInfo"/>.
        /// </summary>
        /// <param name="manifestInfo">The composite manifest info that contains all the information about the manifest go generate.</param>
        /// <returns>The generated Smooth Streaming composite manifest.</returns>
        public string GenerateCompositeManifest(CompositeManifestInfo manifestInfo, bool writeSourceDataStreams)
        {
            StringBuilder output = new StringBuilder();

            XmlWriter writer = CreateWriter(output, false, true);

            if (writer != null)
            {
                writer.WriteProcessingInstruction("xml", "version=\"1.0\"");
                writer.WriteStartElement("SmoothStreamingMedia");

                WriteHeader(writer, manifestInfo);

                foreach (Clip clip in manifestInfo.Clips)
                {
                    WriteClip(writer, clip, manifestInfo.MajorVersion);
                }

                if (writeSourceDataStreams)
                {
                    foreach (StreamInfo streamInfo in manifestInfo.Streams)
                    {
                        WriteClipSparseStreamIndex(writer, streamInfo, manifestInfo.MajorVersion);
                    }
                }

                WriteAdOpportunities(writer, manifestInfo);

                WritePlayByPlay(writer, manifestInfo);

                writer.WriteEndElement();
                writer.Close();
            }

            return output.ToString();
        }

        /// <summary>
        /// Generates a Smooth Streaming server manifest.
        /// </summary>
        /// <param name="clientManifestName">The client manifest name.</param>
        /// <param name="streams">The available streams.</param>
        /// <returns>The generated Smooth Streaming server manifest.</returns>
        public string GenerateServerManifest(string clientManifestName, IEnumerable<SwitchInfo> streams)
        {
            const string Ns = "http://www.w3.org/2001/SMIL20/Language";

            StringBuilder output = new StringBuilder();

            XmlWriter writer = CreateWriter(output, false, true);

            if (writer != null)
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("smil", Ns);
                writer.WriteAttributeString("xmlns", Ns);
                writer.WriteStartElement("head");
                writer.WriteStartElement("meta");
                writer.WriteAttributeString("name", "clientManifestRelativePath");
                writer.WriteAttributeString("content", clientManifestName);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("body");
                writer.WriteStartElement("switch");

                foreach (SwitchInfo switchInfo in streams)
                {
                    string type = switchInfo.FileType == FileType.Video ? "video" : "audio";

                    WriteTrack(writer, type, switchInfo.Source, switchInfo.Bitrate * 1000, (int)switchInfo.FileType);
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Close();
            }

            return output.ToString();
        }

        private static void WriteHeader(XmlWriter writer, ManifestInfo manifestInfo)
        {
            writer.WriteAttributeString("MajorVersion", manifestInfo.MajorVersion.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("MinorVersion", manifestInfo.MinorVersion.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Duration", manifestInfo.ManifestDuration.ToString(CultureInfo.InvariantCulture));
        }

        private static void WriteStreamIndex(XmlWriter writer, StreamInfo streamInfo, int majorVersion, bool writeChunksAndEndElement)
        {
            writer.WriteStartElement("StreamIndex");

            foreach (string attribute in streamInfo.Attributes.Keys)
            {
                writer.WriteAttributeString(attribute, streamInfo.Attributes[attribute].ToString(CultureInfo.InvariantCulture));
            }

            foreach (QualityLevel trackInfo in streamInfo.QualityLevels)
            {
                WriteQualityLevel(writer, trackInfo);
            }

            if (writeChunksAndEndElement)
            {
                foreach (Chunk chunk in streamInfo.Chunks)
                {
                    WriteChunk(writer, chunk);
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes a Track of the Smooth Streaming server manifest.
        /// </summary>
        /// <param name="writer">The writer where the track is being added.</param>
        /// <param name="type">The type of the track.</param>
        /// <param name="file">The file name.</param>
        /// <param name="bitrate">The bitrate.</param>
        /// <param name="trackId">The track identifier.</param>
        private static void WriteTrack(XmlWriter writer, string type, string file, IConvertible bitrate, IConvertible trackId)
        {
            writer.WriteStartElement(type);
            writer.WriteAttributeString("src", file);
            writer.WriteAttributeString("systemBitrate", bitrate.ToString(CultureInfo.InvariantCulture));
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "trackID");
            writer.WriteAttributeString("value", trackId.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("valuetype", "data");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes a QualityLevel to the given <seealso cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The writer where the element is being written.</param>
        /// <param name="qualityLevel">The QualityLevel to write.</param>
        private static void WriteQualityLevel(XmlWriter writer, QualityLevel qualityLevel)
        {
            writer.WriteStartElement("QualityLevel");

            foreach (string attribute in qualityLevel.Attributes.Keys)
            {
                writer.WriteAttributeString(attribute, qualityLevel.Attributes[attribute].ToString(CultureInfo.InvariantCulture));
            }

            if (qualityLevel.CustomAttributes.Count > 0)
            {
                writer.WriteStartElement("CustomAttributes");

                foreach (string attribute in qualityLevel.CustomAttributes.Keys)
                {
                    writer.WriteStartElement("Attribute");

                    writer.WriteAttributeString("Name", attribute);
                    writer.WriteAttributeString("Value", qualityLevel.CustomAttributes[attribute]);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes a chunk to the given <seealso cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The writer where the chunk is being written.</param>
        /// <param name="chunk">The chunk to write.</param>
        private static void WriteChunk(XmlWriter writer, Chunk chunk)
        {
            writer.WriteStartElement("c");

            if (chunk.Id.HasValue)
            {
                writer.WriteAttributeString("n", chunk.Id.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (chunk.Time.HasValue)
            {
                writer.WriteAttributeString("t", chunk.Time.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (chunk.Duration.HasValue)
            {
                writer.WriteAttributeString("d", chunk.Duration.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (!String.IsNullOrEmpty(chunk.Value))
            {
                writer.WriteStartElement("f");
                writer.WriteString(chunk.Value);
                writer.WriteEndElement();
            }

            // if (chunkSizes != null)
            // {
            //    for (int j = 0; j < chunkSizes.Length; j++)
            //    {
            //        writer.WriteStartElement("f");
            //        writer.WriteAttributeString("i", j.ToString(CultureInfo.InvariantCulture));
            //        writer.WriteAttributeString("s", (chunkSizes[j][i] / 1024).ToString(CultureInfo.InvariantCulture));
            //        writer.WriteAttributeString("q", chunkQuality[j][i].ToString(CultureInfo.InvariantCulture));
            //        writer.WriteEndElement();
            //    }
            // }
            writer.WriteEndElement();
        }

        private static ulong GetCandidateClipBegin(Clip clip)
        {
            ulong candidateClipBegin = clip.ClipBegin;
            ulong? chunkBeginPositionTime = null;

            foreach (StreamInfo streamInfo in clip.Streams)
            {
                int? chunkBeginPositionIndex = null;

                ulong comparisonTime = 0;

                for (int i = 0; i < streamInfo.Chunks.Count; i++)
                {
                    ulong tempComparisonTime = 0;

                    Chunk chunk = streamInfo.Chunks[i];

                    if (chunk.Time.HasValue)
                    {
                        tempComparisonTime = chunk.Time.Value;
                    }
                    else
                    {
                        ulong prevChunkDuration = (i > 0) ? streamInfo.Chunks[i - 1].Duration.GetValueOrDefault() : 0;
                        tempComparisonTime = comparisonTime + prevChunkDuration;
                        comparisonTime = tempComparisonTime;
                    }

                    if (tempComparisonTime >= clip.ClipBegin && tempComparisonTime <= clip.ClipEnd)
                    {
                        comparisonTime = tempComparisonTime;

                        if (chunkBeginPositionIndex == null)
                        {
                            if (i > 0 || (i == 0 && tempComparisonTime > clip.ClipBegin))
                            {
                                chunkBeginPositionIndex = (i > 0) ? i - 1 : 0;

                                if (chunk.Time.HasValue && chunk.Time.Value > clip.ClipBegin)
                                {
                                    Chunk previousChunk = streamInfo.Chunks[chunkBeginPositionIndex.Value];

                                    ulong newTime = previousChunk.Time ?? comparisonTime + previousChunk.Duration.GetValueOrDefault();
                                   
                                    if (!chunkBeginPositionTime.HasValue || chunkBeginPositionTime.Value < newTime)
                                    {
                                        chunkBeginPositionTime = newTime;
                                    }
                                }
                                else if (chunk.Duration.HasValue && comparisonTime > clip.ClipBegin)
                                {
                                    ulong newTime = comparisonTime - chunk.Duration.Value;

                                    if (!chunkBeginPositionTime.HasValue || chunkBeginPositionTime.Value < newTime)
                                    {
                                        chunkBeginPositionTime = newTime;
                                    }
                                }
                            }
                            else
                            {
                                chunkBeginPositionIndex = i;
                                chunkBeginPositionTime = comparisonTime;
                            }
                        }
                    }

                    if (!chunkBeginPositionTime.HasValue)
                    {
                        chunkBeginPositionTime = comparisonTime;
                    }

                    if (chunkBeginPositionTime.Value > candidateClipBegin)
                    {
                        candidateClipBegin = chunkBeginPositionTime.Value;
                    }
                }
            }

            return candidateClipBegin;
        }

        private static ulong GetCandidateClipEnd(Clip clip)
        {
            ulong candidateClipEnd = clip.ClipEnd;
            ulong? chunkEndPositionTime = null;

            foreach (StreamInfo streamInfo in clip.Streams)
            {
                ulong comparisonTime = 0;
            
                 for (int i = 0; i < streamInfo.Chunks.Count; i++)
                 {
                    ulong tempComparisonTime = 0;

                    Chunk chunk = streamInfo.Chunks[i];

                    if (chunk.Time.HasValue)
                    {
                        tempComparisonTime = chunk.Time.Value;
                    }
                    else
                    {
                        ulong prevChunkDuration = (i > 0) ? streamInfo.Chunks[i - 1].Duration.GetValueOrDefault() : 0;
                        tempComparisonTime = comparisonTime + prevChunkDuration;
                        comparisonTime = tempComparisonTime;
                    }

                    if (tempComparisonTime >= clip.ClipBegin && tempComparisonTime <= clip.ClipEnd)
                    {
                        int chunkEndPositionIndex = i;
                        ulong? chunkDuration = null;

                        if (chunkEndPositionIndex == streamInfo.Chunks.Count - 1)
                        {
                            chunkDuration = chunk.Duration;
                            Chunk lastChunk = streamInfo.Chunks[chunkEndPositionIndex];

                            if (lastChunk.Time.HasValue)
                            {
                                chunkEndPositionTime = lastChunk.Time;
                            }
                            else if (lastChunk.Duration.HasValue)
                            {
                                chunkEndPositionTime = comparisonTime + lastChunk.Duration;
                            }
                        }

                        if (!chunkDuration.HasValue)
                        {
                            Chunk lastChunk = streamInfo.Chunks[chunkEndPositionIndex];

                            if (lastChunk.Time.HasValue)
                            {
                                ulong? duration = streamInfo.Chunks[chunkEndPositionIndex + 1].Time - lastChunk.Time;

                                if (lastChunk.Time + duration >= clip.ClipEnd)
                                {
                                    tempComparisonTime = lastChunk.Time.GetValueOrDefault() + duration.GetValueOrDefault();
                                }
                            }
                            else if (lastChunk.Duration.HasValue)
                            {
                                if (comparisonTime + lastChunk.Duration >= clip.ClipEnd)
                                {
                                    tempComparisonTime = comparisonTime + lastChunk.Duration.GetValueOrDefault();
                                }
                            }
                        }
                    }

                    if (tempComparisonTime >= clip.ClipEnd)
                    {
                        if (!chunkEndPositionTime.HasValue)
                        {
                            chunkEndPositionTime = clip.ClipEnd;
                        }

                        break;
                    }

                    if (chunkEndPositionTime.HasValue && tempComparisonTime < clip.ClipEnd && (i == streamInfo.Chunks.Count - 1))
                    {
                        ulong newClipEnd = clip.ClipEnd < chunkEndPositionTime.Value ? clip.ClipEnd : chunkEndPositionTime.Value;

                        if (newClipEnd < candidateClipEnd)
                        {
                            candidateClipEnd = newClipEnd;
                        }
                    }
                 }
            }

            return candidateClipEnd;
        }

        private static void WriteClip(XmlWriter writer, Clip clip, int majorVersion)
        {
            ulong? chunkBeginPositionTime = null;
            ulong? chunkEndPositionTime = null;

            StringBuilder output = new StringBuilder();
            XmlWriter tempWriter = CreateWriter(output, true, true);

            // clip.ClipBegin = GetCandidateClipBegin(clip);
            clip.ClipEnd = GetCandidateClipEnd(clip);

            foreach (StreamInfo streamInfo in clip.Streams)
            {
                int numberOfChunks = 0;
                StringBuilder chunkOutput = new StringBuilder();
                XmlWriter chunkWritter = CreateWriter(chunkOutput, true, true);

                if (chunkBeginPositionTime.HasValue)
                {
                    clip.ClipBegin = chunkBeginPositionTime.Value;
                }

                // if (chunkEndPositionTime.HasValue)
                // {
                //    clip.ClipEnd = chunkEndPositionTime.Value;
                // }
                ulong comparisonTime = 0;

                int? chunkBeginPositionIndex = null;

                for (int i = 0; i < streamInfo.Chunks.Count; i++)
                {
                    ulong tempComparisonTime = 0;

                    Chunk chunk = streamInfo.Chunks[i];

                    if (chunk.Time.HasValue)
                    {
                        tempComparisonTime = chunk.Time.Value;
                    }
                    else
                    {
                        ulong prevChunkDuration = (i > 0) ? streamInfo.Chunks[i - 1].Duration.GetValueOrDefault() : 0;
                        tempComparisonTime = comparisonTime + prevChunkDuration;
                        comparisonTime = tempComparisonTime;
                    }

                    if (tempComparisonTime >= clip.ClipBegin && tempComparisonTime <= clip.ClipEnd)
                    {
                        comparisonTime = tempComparisonTime;

                        bool chunkWritten = false;

                        if (chunkBeginPositionIndex == null)
                        {
                            if (i > 0 || (i == 0 && tempComparisonTime > clip.ClipBegin))
                            {
                                chunkBeginPositionIndex = (i > 0) ? i - 1 : 0;

                                if (chunk.Time.HasValue && chunk.Time.Value > clip.ClipBegin)
                                {
                                    Chunk previousChunk = streamInfo.Chunks[chunkBeginPositionIndex.Value];

                                    ulong newTime = previousChunk.Time ?? comparisonTime + previousChunk.Duration.GetValueOrDefault();
                                    WriteChunk(chunkWritter, new Chunk(null, newTime, null));
                                    numberOfChunks++;

                                    if (!chunkBeginPositionTime.HasValue || chunkBeginPositionTime.Value < newTime)
                                    {
                                        if (newTime < clip.ClipBegin && clip.ClipBegin < chunk.Time.Value)
                                        {
                                            chunkBeginPositionTime = clip.ClipBegin;
                                        }
                                        else
                                        {
                                            chunkBeginPositionTime = newTime;
                                        }
                                    }

                                    chunkWritten = true;
                                }
                                else if (chunk.Duration.HasValue && comparisonTime > clip.ClipBegin)
                                {
                                    // ulong newTime = comparisonTime - chunk.Duration.Value;
                                    ulong prevChunkDuration = (i > 0) ? streamInfo.Chunks[i - 1].Duration.GetValueOrDefault() : 0;
                                    ulong newTime = comparisonTime - prevChunkDuration;

                                   WriteChunk(chunkWritter, new Chunk(null, newTime, null));
                                    numberOfChunks++;

                                    if (!chunkBeginPositionTime.HasValue || chunkBeginPositionTime.Value < newTime)
                                    {
                                        chunkBeginPositionTime = newTime;
                                    }

                                    chunkWritten = true;
                                }
                            }
                            else
                            {
                                chunkBeginPositionIndex = i;
                                chunkBeginPositionTime = comparisonTime;
                            }
                        }

                        int chunkEndPositionIndex = i;

                        if (chunkBeginPositionIndex == 0 && chunkWritten)
                        {
                            continue;
                        }

                        ulong? chunkDuration = null;

                        if (chunkEndPositionIndex == streamInfo.Chunks.Count - 1)
                        {
                            chunkDuration = chunk.Duration;
                            Chunk lastChunk = streamInfo.Chunks[chunkEndPositionIndex];

                            if (lastChunk.Time.HasValue)
                            {
                                chunkEndPositionTime = lastChunk.Time;
                            }
                            else if (lastChunk.Duration.HasValue)
                            {
                                chunkEndPositionTime = comparisonTime + lastChunk.Duration;
                            }
                        }

                        if (!chunkDuration.HasValue)
                        {
                            Chunk lastChunk = streamInfo.Chunks[chunkEndPositionIndex];

                            if (lastChunk.Time.HasValue)
                            {
                                ulong? duration = streamInfo.Chunks[chunkEndPositionIndex + 1].Time - lastChunk.Time;

                                if (lastChunk.Time + duration >= clip.ClipEnd)
                                {
                                    chunkDuration = duration;
                                    tempComparisonTime = lastChunk.Time.GetValueOrDefault() + duration.GetValueOrDefault();
                                }
                            }
                            else if (lastChunk.Duration.HasValue)
                            {
                                if (comparisonTime + lastChunk.Duration >= clip.ClipEnd)
                                {
                                    chunkDuration = lastChunk.Duration.Value;
                                    tempComparisonTime = comparisonTime + lastChunk.Duration.GetValueOrDefault();
                                }
                            }
                        }

                        if (!chunkBeginPositionTime.HasValue)
                        {
                            chunkBeginPositionTime = comparisonTime;
                        }

                        WriteChunk(chunkWritter, new Chunk(null, comparisonTime, chunkDuration));
                        numberOfChunks++;
                    }

                    if (tempComparisonTime >= clip.ClipEnd)
                    {
                        if (!chunkEndPositionTime.HasValue)
                        {
                            chunkEndPositionTime = clip.ClipEnd;
                        }

                        break;
                    }

                    if (chunkEndPositionTime.HasValue && tempComparisonTime < clip.ClipEnd && (i == streamInfo.Chunks.Count - 1))
                    {
                        clip.ClipEnd = clip.ClipEnd < chunkEndPositionTime.Value ? clip.ClipEnd : chunkEndPositionTime.Value;
                    }
                }

                chunkWritter.Close();

                streamInfo.Attributes["Chunks"] = numberOfChunks.ToString(CultureInfo.InvariantCulture);

                WriteStreamIndex(tempWriter, streamInfo, majorVersion, false);
                tempWriter.WriteRaw(Environment.NewLine);
                tempWriter.WriteRaw(chunkOutput.ToString());
                tempWriter.WriteRaw(Environment.NewLine);
                tempWriter.WriteEndElement();
            }

            tempWriter.Close();

            if (chunkBeginPositionTime.HasValue && chunkBeginPositionTime.Value > clip.ClipBegin)
            {
                clip.ClipBegin = chunkBeginPositionTime.Value;
            }

            writer.WriteStartElement("Clip");
            writer.WriteAttributeString("Url", clip.Url.ToString());
            writer.WriteAttributeString("ClipBegin", clip.ClipBegin.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("ClipEnd", clip.ClipEnd.ToString(CultureInfo.InvariantCulture));
            writer.WriteRaw(Environment.NewLine);
            writer.WriteRaw(output.ToString());
            writer.WriteRaw(Environment.NewLine);
            writer.WriteEndElement();
        }

        private static void WriteClipSparseStreamIndex(XmlWriter writer, StreamInfo streamInfo, int majorVersion)
        {
            WriteStreamIndex(writer, streamInfo, majorVersion, false);

            ulong comparisonTime = 0;

            for (int i = 0; i < streamInfo.Chunks.Count; i++)
            {
                ulong tempComparisonTime = 0;

                Chunk chunk = streamInfo.Chunks[i];

                if (chunk.Time.HasValue)
                {
                    tempComparisonTime = chunk.Time.Value;
                }
                else
                {
                    tempComparisonTime = comparisonTime + chunk.Duration.GetValueOrDefault();
                }

                if (tempComparisonTime >= streamInfo.ParentClip.ClipBegin && tempComparisonTime <= streamInfo.ParentClip.ClipEnd)
                {
                    comparisonTime = tempComparisonTime;

                    WriteChunk(writer, new Chunk(null, comparisonTime - streamInfo.ParentClip.ClipBegin, chunk.Duration, chunk.Value));
                }

                if (tempComparisonTime >= streamInfo.ParentClip.ClipEnd)
                {
                    break;
                }
            }

            writer.WriteEndElement();
        }

        private static void WriteAdOpportunities(XmlWriter writer, CompositeManifestInfo manifestInfo)
        {
            if (manifestInfo.AdOpportunities.Count > 0)
            {
                StreamInfo streamInfo = WriteCompositeManifestStandardStreamIndex(manifestInfo.AdsDataStreamName, manifestInfo.AdOpportunities.Count);

                for (int i = 0; i < manifestInfo.AdOpportunities.Count; i++)
                {
                    AdOpportunity adOpportunity = manifestInfo.AdOpportunities[i];

                    StringBuilder output = new StringBuilder();

                    XmlWriter tempWriter = CreateWriter(output, true, true);

                    if (tempWriter != null)
                    {
                        tempWriter.WriteStartElement("InStreamEnvelope");
                        tempWriter.WriteAttributeString("Id", adOpportunity.ID.ToString());
                        tempWriter.WriteAttributeString("Action", "Add");
                        tempWriter.WriteAttributeString("TargetID", string.Empty);
                        tempWriter.WriteAttributeString("Priority", "1");
                        tempWriter.WriteStartElement("AdOpportunity");
                        tempWriter.WriteAttributeString("ID", adOpportunity.ID.ToString());
                        tempWriter.WriteAttributeString("Time", adOpportunity.Time.ToString(CultureInfo.InvariantCulture));
                        tempWriter.WriteAttributeString("TemplateType", adOpportunity.TemplateType);
                        tempWriter.WriteEndElement();
                        tempWriter.Close();

                        byte[] envelopeBytes = Encoding.UTF8.GetBytes(output.ToString());

                        string encodedEnvelope = Convert.ToBase64String(envelopeBytes);

                        ulong? duration = null;

                        if (i == manifestInfo.AdOpportunities.Count - 1)
                        {
                            duration = 10000000;
                        }

                        Chunk chunk = new Chunk(null, (ulong)adOpportunity.Time, duration, encodedEnvelope);

                        streamInfo.Chunks.Add(chunk);
                    }
                }

                WriteStreamIndex(writer, streamInfo, manifestInfo.MajorVersion, true);
            }
        }

        private static void WritePlayByPlay(XmlWriter writer, CompositeManifestInfo manifestInfo)
        {
            if (manifestInfo.PlayByPlayEvents.Count > 0)
            {
                StreamInfo streamInfo = WriteCompositeManifestStandardStreamIndex(manifestInfo.PlayByPlayDataStreamName, manifestInfo.PlayByPlayEvents.Count);

                for (int i = 0; i < manifestInfo.PlayByPlayEvents.Count; i++)
                {
                    PlayByPlay pbp = manifestInfo.PlayByPlayEvents[i];

                    StringBuilder output = new StringBuilder();

                    XmlWriter tempWriter = CreateWriter(output, true, true);

                    if (tempWriter != null)
                    {
                        tempWriter.WriteStartElement("InStreamEnvelope");
                        tempWriter.WriteAttributeString("Id", pbp.ID.ToString());
                        tempWriter.WriteAttributeString("Action", "Add");
                        tempWriter.WriteAttributeString("TargetID", string.Empty);
                        tempWriter.WriteAttributeString("Priority", "1");
                        tempWriter.WriteStartElement("PlayByPlay");
                        tempWriter.WriteAttributeString("ID", pbp.ID.ToString());
                        tempWriter.WriteAttributeString("Time", pbp.Time.ToString(CultureInfo.InvariantCulture));
                        tempWriter.WriteAttributeString("IsTimelineMarker", "True");
                        tempWriter.WriteAttributeString("IsNavigable", "True");
                        tempWriter.WriteAttributeString("EditorialType", string.Empty);
                        tempWriter.WriteAttributeString("Type", string.Empty);
                        tempWriter.WriteCData(pbp.Text);
                        tempWriter.WriteEndElement();
                        tempWriter.Close();

                        byte[] envelopeBytes = Encoding.UTF8.GetBytes(output.ToString());

                        string encodedEnvelope = Convert.ToBase64String(envelopeBytes);

                        ulong? duration = null;

                        if (i == manifestInfo.PlayByPlayEvents.Count - 1)
                        {
                            duration = 10000000;
                        }

                        Chunk chunk = new Chunk(null, (ulong)pbp.Time, duration, encodedEnvelope);

                        streamInfo.Chunks.Add(chunk);
                    }
                }

                WriteStreamIndex(writer, streamInfo, manifestInfo.MajorVersion, true);
            }
        }

        private static StreamInfo WriteCompositeManifestStandardStreamIndex(string streamName, int numberOfChunks)
        {
            StreamInfo streamInfo = new StreamInfo("text");
            streamInfo.AddAttribute("Name", streamName);
            streamInfo.AddAttribute("Type", "text");
            streamInfo.AddAttribute("SubType", "ADVT");
            streamInfo.AddAttribute("Chunks", numberOfChunks.ToString(CultureInfo.InvariantCulture));
            streamInfo.AddAttribute("TimeScale", "10000000");
            streamInfo.AddAttribute("ParentStreamIndex", "video");
            streamInfo.AddAttribute("ManifestOutput", "TRUE");
            streamInfo.AddAttribute("Url", String.Format(CultureInfo.InvariantCulture, @"QualityLevels({{bitrate}})/Fragments({0}={{start time}})", streamName));

            QualityLevel qualityLevel = new QualityLevel();
            qualityLevel.AddAttribute("Index", "0");
            qualityLevel.AddAttribute("Bitrate", "1000");
            qualityLevel.AddAttribute("CodecPrivateData", string.Empty);
            qualityLevel.AddAttribute("FourCC", string.Empty);

            streamInfo.QualityLevels.Add(qualityLevel);

            return streamInfo;
        }

        private static XmlWriter CreateWriter(StringBuilder output, bool omitXmlDeclaration, bool newLineOnAttributes)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.Unicode,
                Indent = true,
                NewLineOnAttributes = newLineOnAttributes,
                CheckCharacters = false,
                OmitXmlDeclaration = omitXmlDeclaration,
                ConformanceLevel = ConformanceLevel.Auto
            };

            return XmlWriter.Create(output, settings);
        }
    }
}
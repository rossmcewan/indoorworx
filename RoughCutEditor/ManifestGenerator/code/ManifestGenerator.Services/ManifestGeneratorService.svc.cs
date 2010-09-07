// <copyright file="ManifestGeneratorService.svc.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ManifestGeneratorService.svc.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace ManifestGenerator.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    using RCE.Services.Contracts;

    using SmoothStreamingManifestGenerator;
    using SmoothStreamingManifestGenerator.Models;

    public class ManifestGeneratorService : IManifestGeneratorService
    {
        public string GetSubClipManifest(Uri manifestUri, double markIn, double markOut)
        {
            string manifest = null;

            DownloaderManager downloaderManager = new DownloaderManager();
            Stream manifestStream = downloaderManager.DownloadManifest(manifestUri, true, null);

            if (manifestStream != null)
            {
                SmoothStreamingManifestParser parser = new SmoothStreamingManifestParser(manifestStream);

                SmoothStreamingManifestWriter writer = new SmoothStreamingManifestWriter();

                CompositeManifestInfo compositeManifestInfo = new CompositeManifestInfo(parser.ManifestInfo.MajorVersion, parser.ManifestInfo.MinorVersion);

                compositeManifestInfo.AddClip(manifestUri, (ulong)markIn, (ulong)markOut, parser.ManifestInfo);

                manifest = writer.GenerateCompositeManifest(compositeManifestInfo, false);
            }

            return manifest;
        }

        public string GetManifest(string projectXml, string pbpDataStreamName, string adsDataStreamName)
        {
            CompositeManifestInfo compositeManifestInfo = new CompositeManifestInfo(2, 0);

            compositeManifestInfo.PlayByPlayDataStreamName = pbpDataStreamName;
            compositeManifestInfo.AdsDataStreamName = adsDataStreamName;

            DownloaderManager manager = new DownloaderManager();

            const ulong Timescale = 10000000;

            Project project;

            try
            {
                project = Deserialize<Project>(projectXml);
            }
            catch
            {
                return Resources.Resources.InvalidRCEProjectXml;
            }

            if (project.Timeline != null)
            {
                Track track = project.Timeline.SingleOrDefault(x => x.TrackType.ToUpperInvariant() == "VISUAL");

                if (track != null && track.Shots != null)
                {
                    foreach (Shot shot in track.Shots)
                    {
                        if (shot.Source != null && shot.Source is VideoItem && shot.Source.Resources.Count > 0 && shot.SourceAnchor != null)
                        {
                            Resource resource = shot.Source.Resources.SingleOrDefault(x => !String.IsNullOrEmpty(x.Ref));

                            Uri assetUri;

                            if (resource != null && Uri.TryCreate(resource.Ref, UriKind.Absolute, out assetUri))
                            {
                                Stream manifestStream = manager.DownloadManifest(assetUri, true, null);

                                if (manifestStream != null)
                                {
                                    double startPosition = (shot.Source is SmoothStreamingVideoItem) ? ((SmoothStreamingVideoItem)shot.Source).StartPosition : 0;

                                    SmoothStreamingManifestParser parser = new SmoothStreamingManifestParser(manifestStream);

                                    ulong clipBegin = (ulong)((shot.SourceAnchor.MarkIn.GetValueOrDefault() * Timescale) + (startPosition * Timescale));
                                    ulong clipEnd = (ulong)((shot.SourceAnchor.MarkOut.GetValueOrDefault() * Timescale) + (startPosition * Timescale));

                                    compositeManifestInfo.AddClip(assetUri, clipBegin, clipEnd, parser.ManifestInfo);
                                }
                            }


                        }

                        //if (shot.Source != null && shot.Source is ImageItem && shot.Source.Resources.Count > 0 && shot.SourceAnchor != null)
                        //{
                        //    Resource resource = shot.Source.Resources.SingleOrDefault(x => !String.IsNullOrEmpty(x.Ref));

                        //    Uri assetUri;

                        //    if (resource != null && Uri.TryCreate(resource.Ref, UriKind.Absolute, out assetUri))
                        //    {
                        //        Stream imageStream = manager.DownloadManifest(assetUri, true, null);

                        //        if (imageStream != null)
                        //        {
                        //            double startPosition = shot.SourceAnchor.MarkIn.GetValueOrDefault();
                        //            //double startPosition = (shot.Source is SmoothStreamingVideoItem) ? ((SmoothStreamingVideoItem)shot.Source).StartPosition : 0;

                        //            SmoothStreamingManifestParser parser = new SmoothStreamingManifestParser(imageStream);

                        //            ulong clipBegin = (ulong)((shot.SourceAnchor.MarkIn.GetValueOrDefault() * Timescale) + (startPosition * Timescale));
                        //            ulong clipEnd = (ulong)((shot.SourceAnchor.MarkOut.GetValueOrDefault() * Timescale) + (startPosition * Timescale));

                        //            compositeManifestInfo.AddClip(assetUri, clipBegin, clipEnd, parser.ManifestInfo);
                        //        }
                        //    }


                        }
                    }
                }
            }

            if (project.AdOpportunities != null)
            {
                foreach (RCE.Services.Contracts.AdOpportunity adOpportunity in project.AdOpportunities)
                {
                    compositeManifestInfo.AddAdOpportunity(adOpportunity.ID, adOpportunity.TemplateType, adOpportunity.Time);
                }
            }

            if (project.Markers != null)
            {
                foreach (Marker marker in project.Markers)
                {
                    compositeManifestInfo.AddPlayByPlay(marker.ID, marker.Text, marker.Time);
                }
            }

            SmoothStreamingManifestWriter writer = new SmoothStreamingManifestWriter();

            string manifest = writer.GenerateCompositeManifest(compositeManifestInfo, false);

            return manifest;
        }

        /// <summary>
        /// Deserializes the result into a known type.
        /// </summary>
        /// <typeparam name="T">The known type.</typeparam>
        /// <param name="result">The result being deserialized.</param>
        /// <returns>A known type instance.</returns>
        private static T Deserialize<T>(string result)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(result);
            T graph;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                graph = (T) serializer.ReadObject(ms);
            }

            return graph;
        }
    }
}
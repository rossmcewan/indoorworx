// <copyright file="CompositeManifestInfo.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: CompositeManifestInfo.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace SmoothStreamingManifestGenerator.Models
{
    using System;
    using System.Collections.Generic;

    public class CompositeManifestInfo : ManifestInfo
    {
        public CompositeManifestInfo(int majorVersion, int minorVersion)
            : base(majorVersion, minorVersion)
        {
            this.Clips = new List<Clip>();
            this.AdOpportunities = new List<AdOpportunity>();
            this.PlayByPlayEvents = new List<PlayByPlay>();
        }

        [CLSCompliant(false)]
        public override ulong ManifestDuration
        {
            get
            {
                ulong duration = 0;

                foreach (Clip clip in this.Clips)
                {
                    duration += clip.ClipEnd - clip.ClipBegin;
                }

                return duration;
            } 
        }

        public IList<Clip> Clips { get; private set; }

        public string PlayByPlayDataStreamName { get; set; }

        public string AdsDataStreamName { get; set; }

        public List<AdOpportunity> AdOpportunities { get; private set; }

        public List<PlayByPlay> PlayByPlayEvents { get; private set; }

        [CLSCompliant(false)]
        public void AddClip(Uri manifestUri, ulong clipBegin, ulong clipEnd, ManifestInfo manifestInfo)
        {
            Clip clip = new Clip(manifestUri, clipBegin, clipEnd);

            foreach (StreamInfo streamInfo in manifestInfo.Streams)
            {
                if (streamInfo.IsSparseStream)
                {
                    streamInfo.ParentClip = clip;
                    this.Streams.Add(streamInfo);
                }
                else
                {
                    clip.Streams.Add(streamInfo);
                }
            }

            this.Clips.Add(clip);
        }

        public void AddClips(List<Clip> clips)
        {
            foreach (Clip clip in clips)
            {
               this.Clips.Add(clip);
            }
        }

        public void AddAdOpportunity(Guid id, string templateType, long time)
        {
            AdOpportunity adOpportunity = new AdOpportunity(id, templateType, time);
            this.AdOpportunities.Add(adOpportunity);
            this.AdOpportunities.Sort((ad1, ad2) => ad1.Time.CompareTo(ad2.Time));
        }

        public void AddPlayByPlay(Guid id, string text, long time)
        {
            PlayByPlay playByPlay = new PlayByPlay(id, text, time);
            this.PlayByPlayEvents.Add(playByPlay);
            this.PlayByPlayEvents.Sort((pbp1, pbp2) => pbp1.Time.CompareTo(pbp2.Time));
        }
    }
}
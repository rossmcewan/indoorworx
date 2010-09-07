// <copyright file="Clip.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Clip.cs                     
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

    public class Clip
    {
        [CLSCompliant(false)]
        public Clip(Uri url, ulong clipBegin, ulong clipEnd)
            : this(url, clipBegin, clipEnd, new List<StreamInfo>())
        {
        }

        [CLSCompliant(false)]
        public Clip(Uri url, ulong clipBegin, ulong clipEnd, IList<StreamInfo> streams)
        {
            this.Url = url;
            this.ClipBegin = clipBegin;
            this.ClipEnd = clipEnd;
            this.Streams = streams;
        }

        public Uri Url { get; private set; }

        [CLSCompliant(false)]
        public ulong ClipBegin { get; set; }

        [CLSCompliant(false)]
        public ulong ClipEnd { get; set; }

        public IList<StreamInfo> Streams { get; private set; }
    }
}

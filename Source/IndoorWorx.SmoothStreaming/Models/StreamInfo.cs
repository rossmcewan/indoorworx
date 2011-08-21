// <copyright file="StreamInfo.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: StreamInfo.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace IndoorWorx.SmoothStreaming.Models
{
    using System.Collections.Generic;
    using System.Globalization;

    public class StreamInfo
    {
        public StreamInfo(string type)
        {
            this.Attributes = new Dictionary<string, string>();
            this.QualityLevels = new List<QualityLevel>();
            this.Chunks = new List<Chunk>();
            this.StreamType = type;
        }

        public string StreamType { get; private set; }

        public IList<QualityLevel> QualityLevels { get; private set; }

        public IList<Chunk> Chunks { get; private set; }

        public IDictionary<string, string> Attributes { get; private set; }

        public bool IsSparseStream
        {
            get { return this.StreamType.ToUpper(CultureInfo.InvariantCulture) != "VIDEO" && this.StreamType.ToUpper(CultureInfo.InvariantCulture) != "AUDIO"; }
        }

        public Clip ParentClip { get; set; }

        public void AddAttribute(string key, string value)
        {
            this.Attributes.Add(key, value);
        }
    }
}
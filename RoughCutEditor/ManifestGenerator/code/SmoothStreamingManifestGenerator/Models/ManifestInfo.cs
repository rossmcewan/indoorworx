// <copyright file="ManifestInfo.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ManifestInfo.cs                     
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

    public class ManifestInfo
    {
        [CLSCompliant(false)]
        public ManifestInfo(int majorVersion, int minorVersion, ulong manifestDuration, IList<StreamInfo> streams)
        {
            this.MajorVersion = majorVersion;
            this.MinorVersion = minorVersion;
            this.ManifestDuration = manifestDuration;
            this.Streams = streams;
        }

        protected ManifestInfo(int majorVersion, int minorVersion)
        {
            this.MajorVersion = majorVersion;
            this.MinorVersion = minorVersion;
            this.Streams = new List<StreamInfo>();
        }

        public int MajorVersion { get; protected set; }

        public int MinorVersion { get; protected set; }

        /// <summary>
        /// Gets the duration of the manifest.
        /// </summary>
        /// <value>The duration of the manifest.</value>
        [CLSCompliant(false)]
        public virtual ulong ManifestDuration { get; private set; }

        public IList<StreamInfo> Streams { get; private set; }
    }
}
// <copyright file="Chunk.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Chunk.cs                     
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
    using System;

    public class Chunk
    {
        [CLSCompliant(false)]
        public Chunk(int? id, ulong? time, ulong? duration)
        {
            this.Id = id;
            this.Time = time;
            this.Duration = duration;
            this.Repeat = 1;
        }

        [CLSCompliant(false)]
        public Chunk(int? id, ulong? time, ulong? duration, string value)
            : this(id, time, duration)
        {
            this.Value = value;
        }

        public int? Id { get; private set; }

        [CLSCompliant(false)]
        public ulong? Time { get; private set; }

        [CLSCompliant(false)]
        public ulong? Duration { get; private set; }

        public string Value { get; set; }

        [CLSCompliant(false)]
        public ulong Repeat { get; set; }
    }
}
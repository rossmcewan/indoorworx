// <copyright file="SwitchInfo.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SwitchInfo.cs                     
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
    public class SwitchInfo
    {
        public SwitchInfo(string source, int bitrate, FileType fileType)
        {
            this.Source = source;
            this.Bitrate = bitrate;
            this.FileType = fileType;
        }

        public string Source { get; private set; }
        
        public int Bitrate { get; private set; }

        public FileType FileType { get; private set; }
    }
}
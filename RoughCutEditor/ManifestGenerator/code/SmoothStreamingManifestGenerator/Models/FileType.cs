// <copyright file="FileType.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: FileType.cs                     
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
    /// <summary>
    /// Defines the availables file types.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Not defined FileType.
        /// </summary>
        None = 0,

        /// <summary>
        /// The file type is an audio.
        /// </summary>
        Audio = 1,

        /// <summary>
        /// The file type is a video.
        /// </summary>
        Video = 2
    }
}
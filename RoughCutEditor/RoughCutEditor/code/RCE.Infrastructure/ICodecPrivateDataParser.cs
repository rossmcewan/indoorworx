// <copyright file="ICodecPrivateDataParser.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ICodecPrivateDataParser.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Infrastructure
{
    using System;
    using Services.Contracts;
    using SMPTETimecode;

    /// <summary>
    /// Defines the interface for the codec private data parsers.
    /// </summary>
    public interface ICodecPrivateDataParser
    {
        /// <summary>
        /// Occurs when the GetFrameRate operation is completed.
        /// </summary>
        event EventHandler<DataEventArgs<SmpteFrameRate>> GetFrameRateCompleted;
     
        /// <summary>
        /// Parses a codec private data to get the frame rate.
        /// </summary>
        /// <param name="codecPrivateData">The codec private data to parse.</param>
        /// <returns>The frame rate.</returns>
        SmpteFrameRate GetFrameRate(string codecPrivateData);

        /// <summary>
        /// Parses a manifest to get the frame rate associated with it.
        /// </summary>
        /// <param name="manifestUri">The manifest uri.</param>
        void GetFrameRateAsync(Uri manifestUri);
    }
}

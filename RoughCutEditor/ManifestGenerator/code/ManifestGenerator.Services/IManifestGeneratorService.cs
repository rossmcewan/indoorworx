// <copyright file="IManifestGeneratorService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: IManifestGeneratorService.cs                     
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
    using System.ServiceModel;

    [ServiceContract]
    public interface IManifestGeneratorService
    {
        [OperationContract]
        string GetSubClipManifest(Uri manifestUri, double markIn, double markOut);

        [OperationContract]
        string GetManifest(string projectXml, string pbpDataStreamName, string adsDataStreamName);
    }
}
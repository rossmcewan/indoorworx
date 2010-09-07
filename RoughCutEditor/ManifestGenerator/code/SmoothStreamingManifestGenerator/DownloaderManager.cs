// <copyright file="DownloaderManager.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: DownloaderManager.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace SmoothStreamingManifestGenerator
{
    using System;
    using System.IO;
    using System.Net;

    public class DownloaderManager
    {
        public virtual Stream DownloadManifest(Uri manifestUri, bool forceNewDownload, CookieContainer cookies)
        {
            if (forceNewDownload)
            {
                string newUriString = String.Concat(manifestUri.AbsoluteUri, "?ignore=", Guid.NewGuid());
                manifestUri = new Uri(newUriString);
            }

            HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(manifestUri);
            client.Timeout = (1000 * 60) * 5;

            if (cookies != null)
            {
                client.CookieContainer = cookies;
            }

            HttpWebResponse response = (HttpWebResponse)client.GetResponse();

            return response.GetResponseStream();
        }

        public virtual Stream DownloadManifest(Uri manifestUri, bool forceNewDownload)
        {
            return this.DownloadManifest(manifestUri, forceNewDownload, null);
        }
    }
}

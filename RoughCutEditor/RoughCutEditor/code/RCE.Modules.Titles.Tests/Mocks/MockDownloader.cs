// <copyright file="MockDownloader.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockDownloader.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles.Tests.Mocks
{
    using Infrastructure;
    using Infrastructure.Models;

    public class MockDownloader : Downloader
    {
        public MockDownloader()
            : base(new MockLoggerFacade())
        {
        }

        public object UserState { get; set; }

        public string Result { get; set; }

        public override void DownloadStringAsync(System.Uri address, object userState)
        {
            base.DownloadStringAsync(address, userState);

            DownloadStringCompletedEventArgs args = new DownloadStringCompletedEventArgs(this.Result, null, false, this.UserState);

            this.OnDownloadStringCompleted(args);
        }
    }
}

// <copyright file="Default.aspx.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Default.aspx.cs                     
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

    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void GenerateSubClipManifest(object sender, EventArgs e)
        {
            string subClipManifest = string.Empty;
            string uriString = this.ManifestUriTextBox.Text;
            string clipBeginString = this.ClipBeginTextBox.Text;
            string clipEndString = this.ClipEndTextBox.Text;

            Uri manifestUri;

            if (Uri.TryCreate(uriString, UriKind.Absolute, out manifestUri))
            {
                ulong clipBegin;
                ulong clipEnd;

                if (ulong.TryParse(clipBeginString, out clipBegin) && ulong.TryParse(clipEndString, out clipEnd))
                {
                    ManifestGeneratorService client = new ManifestGeneratorService();

                    subClipManifest = client.GetSubClipManifest(manifestUri, clipBegin, clipEnd);
                }
            }
            else
            {
                subClipManifest = Resources.Resources.InvalidManifestUri;
            }

            this.SubClipManifestTextBox.Text = subClipManifest;
            this.SubClipManifestTextBox.Visible = true;
        }

        protected void GenerateManifest(object sender, EventArgs e)
        {
            ManifestGeneratorService client = new ManifestGeneratorService();

            string projectXml = this.ProjectTextBox.Text;
            string pbpDataStreamName = this.PBPDataStreamName.Text;
            string adsDataStreamName = this.AdsDataStreamName.Text;

            string manifest = client.GetManifest(projectXml, pbpDataStreamName, adsDataStreamName);

            this.ManifestTextBox.Text = manifest;
            this.ManifestTextBox.Visible = true;
            this.ProjectTextBox.Height = 100;
        }
    }
}
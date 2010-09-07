// <copyright file="QualityLevel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: QualityLevel.cs                     
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

    public class QualityLevel
    {
        public QualityLevel()
        {
            this.Attributes = new Dictionary<string, string>();
            this.CustomAttributes = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Attributes { get; private set; }

        public IDictionary<string, string> CustomAttributes { get; private set; }

        public void AddAttribute(string attribute, string value)
        {
            this.Attributes.Add(attribute, value);
        }

        public void AddCustomAttribute(string attribute, string value)
        {
            this.CustomAttributes.Add(attribute, value);
        }
    }
}
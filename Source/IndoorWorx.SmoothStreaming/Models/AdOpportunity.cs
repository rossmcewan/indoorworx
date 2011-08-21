// <copyright file="AdOpportunity.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Insertion.cs                     
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

    public class AdOpportunity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdOpportunity"/> class.
        /// </summary>
        public AdOpportunity(Guid id, string templateType, long time)
        {
            this.ID = id;
            this.TemplateType = templateType;
            this.Time = time;
        }

        /// <summary>
        /// Gets the AdOpportunity id.
        /// </summary>
        /// <value>The unique identifier for the AdOpportunity.</value>
        public Guid ID { get; private set; }

        /// <summary>
        /// Gets the TemplateType.
        /// </summary>
        /// <value>The AdOpportunity TemplateType.</value>
        public string TemplateType { get; private set; }

        /// <summary>
        /// Gets the position of the ad opportunity.
        /// </summary>
        /// <value>The absolute time in ticks.</value>
        public long Time { get; private set; }
    }
}
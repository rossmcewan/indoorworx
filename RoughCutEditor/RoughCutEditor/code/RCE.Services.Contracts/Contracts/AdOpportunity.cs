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

namespace RCE.Services.Contracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the AdOpportunity class.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [DataContract(Namespace = "http://schemas.microsoft.com/rce/")]
    public class AdOpportunity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdOpportunity"/> class.
        /// </summary>
        public AdOpportunity()
        {
            this.ID = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the AdOpportunity id.
        /// </summary>
        /// <value>The unique identifier for the AdOpportunity.</value>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the TemplateType.
        /// </summary>
        /// <value>The AdOpportunity TemplateType.</value>
        [DataMember]
        public string TemplateType { get; set; }

        /// <summary>
        /// Gets or sets the position of the ad opportunity.
        /// </summary>
        /// <value>The absolute time in ticks.</value>
        [DataMember]
        public long Time { get; set; }
    }
}
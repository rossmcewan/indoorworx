// <copyright file="Marker.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Marker.cs                     
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

    /// <summary>
    /// Defines the Marker class.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [DataContract(Namespace = "http://schemas.microsoft.com/rce/")]
    public class Marker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Marker"/> class.
        /// </summary>
        public Marker()
        {
            this.ID = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the Marker id.
        /// </summary>
        /// <value>The unique identifier for the Marker.</value>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The Marker text.</value>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the position of the ad opportunity.
        /// </summary>
        /// <value>The absolute time in ticks.</value>
        [DataMember]
        public long Time { get; set; }
    }
}

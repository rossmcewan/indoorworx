// <copyright file="NoOpEventDataParser.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: NoOpEventDataParser.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Services
{
    using System.Xml.Linq;
    using Infrastructure;
    using Infrastructure.Models;

    /// <summary>
    /// A no op event data parser being registered as default.
    /// </summary>
    public class NoOpEventDataParser : IEventDataParser<EventData>
    {
        /// <summary>
        /// Parses the element and returns an EventData instance.
        /// </summary>
        /// <param name="element">The element being parsed.</param>
        /// <returns>An instance of EventData.</returns>
        public EventData ParseEventData(XElement element)
        {
            return null;
        }
    }
}

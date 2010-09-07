// <copyright file="XmlReaderExtensions.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: XmlReaderExtensions.cs                     
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
    using System.Xml;

    public static class XmlReaderExtensions
    {
        public static string GetValue(this XmlReader reader, string name)
        {
            return reader.GetAttribute(name);
        }

        public static int? GetValueAsInt(this XmlReader reader, string name)
        {
            int result;

            if (int.TryParse(GetValue(reader, name), out result))
            {
                return result;
            }

            return null;
        }

        [CLSCompliant(false)]
        public static ulong? GetValueAsULong(this XmlReader reader, string name)
        {
            ulong result;

            if (ulong.TryParse(GetValue(reader, name), out result))
            {
                return result;
            }

            return null;
        }
    }
}

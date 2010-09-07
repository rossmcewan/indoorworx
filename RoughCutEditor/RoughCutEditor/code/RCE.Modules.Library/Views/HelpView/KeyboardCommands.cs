// <copyright file="KeyboardCommands.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: KeyboardCommands.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Library
{
    /// <summary>
    /// Class to have the keyboar shortcut and it's description.
    /// </summary>
    public class KeyboardCommands
    {
        /// <summary>
        /// Gets or sets the shortcut of the key.
        /// </summary>
        /// <value>The shortcut key.</value>
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets the description for the shortcut key.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}

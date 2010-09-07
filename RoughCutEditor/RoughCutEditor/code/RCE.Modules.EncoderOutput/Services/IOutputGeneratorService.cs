// <copyright file="IOutputGeneratorService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: IOutputGeneratorService.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.EncoderOutput.Services
{
    using System;
    using Infrastructure;
    using Infrastructure.Models;

    public interface IOutputGeneratorService
    {
        event EventHandler<DataEventArgs<bool>> GenerateOuputCompleted;
        
        void GenerateOutputAsync(Project project);
    }
}

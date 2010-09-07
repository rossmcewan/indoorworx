// <copyright file="MockOutputGeneratorService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockOutputGeneratorService.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.EncoderOutput.Tests.Mocks
{
    using System;
    using Infrastructure;
    using Infrastructure.Models;
    using Services;

    public class MockOutputGeneratorService : IOutputGeneratorService
    {
        public event EventHandler<DataEventArgs<bool>> GenerateOuputCompleted;

        public bool GenerateOutputCalled { get; set; }

        public Project GenerateOutputArgument { get; set; }

        public void GenerateOutputAsync(Project project)
        {
            this.GenerateOutputCalled = true;
            this.GenerateOutputArgument = project;
        }

        public void InvokeGenerateOuputCompleted(bool result)
        {
            EventHandler<DataEventArgs<bool>> handler = this.GenerateOuputCompleted;
            if (handler != null)
            {
                handler(this, new DataEventArgs<bool>(result));
            }
        }
    }
}
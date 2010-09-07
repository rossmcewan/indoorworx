﻿// <copyright file="MockConfigurationService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockConfigurationService.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Metadata.Tests.Mocks
{
    using System;
    using Infrastructure;

   public class MockConfigurationService : IConfigurationService
    {
        public MockConfigurationService()
        {
            this.GetParameterValueReturnFunction = (parameter) => null;
        }

        public Func<string, string> GetParameterValueReturnFunction { get; set; }

        public string GetParameterValueArgument { get; set; }

        public bool GetParameterValueCalled { get; set; }

        public string GetParameterValue(string parameter)
        {
            this.GetParameterValueCalled = true;
            this.GetParameterValueArgument = parameter;
            return this.GetParameterValueReturnFunction.Invoke(parameter);
        }
    }
}
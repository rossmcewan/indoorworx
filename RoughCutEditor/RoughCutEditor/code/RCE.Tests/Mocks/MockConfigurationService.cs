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

namespace RCE.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;

    public class MockConfigurationService : IConfigurationService
    {
        public string GetUserNameReturnValue { get; set; }

        public bool GetProjectIdCalled { get; set; }

        public Uri GetProjectIdReturnValue { private get; set; }

        public string GetUserName()
        {
            return this.GetUserNameReturnValue;
        }

        public Uri GetProjectId()
        {
            this.GetProjectIdCalled = true;
            return this.GetProjectIdReturnValue;
        }

        public IList<string> GetMetadataFields()
        {
            throw new System.NotImplementedException();
        }

        public Uri GetMediaServicesUri()
        {
            throw new System.NotImplementedException();
        }

        public int GetMaxNumberOfItems()
        {
            throw new System.NotImplementedException();
        }

        public IList<string> GetCommentTypes()
        {
            throw new System.NotImplementedException();
        }

        public int GetUndoLevel()
        {
            throw new System.NotImplementedException();
        }

        public Uri GetTitleTemplate(string titleTemplate)
        {
            throw new System.NotImplementedException();
        }

        public string GetParameterValue(string parameter)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetParameterValueAsDateTime(string parameter)
        {
            throw new NotImplementedException();
        }

        public int? GetParameterValueAsInt(string parameter)
        {
            throw new NotImplementedException();
        }

        public double? GetParameterValueAsDouble(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}
﻿// <copyright file="MockProjectService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockProjectService.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Comment.Tests.Mocks
{
    using System;
    using Infrastructure;
    using Infrastructure.Models;

    public class MockProjectService : IProjectService
    {
        private readonly Project project = new Project();

        public MockProjectService()
        {
            this.State = ProjectState.Retrieved;
        }

        public event EventHandler ProjectRetrieved;

        public event EventHandler ProjectError;

        public event EventHandler ProjectSaving;
        
        public event EventHandler<DataEventArgs<bool>> ProjectSaved;

        public ProjectState State { get; set; }

        public bool GetCurrentProjectCalled { get; set; }

        public Project GetCurrentProject()
        {
            this.GetCurrentProjectCalled = true;

            return this.project;
        }

        public void SaveProject()
        {
            throw new System.NotImplementedException();
        }

        public void InvokeProjectRetrieved()
        {
            EventHandler retrieved = this.ProjectRetrieved;
            if (retrieved != null)
            {
                retrieved(this, EventArgs.Empty);
            }
        }
    }
}
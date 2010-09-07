// <copyright file="MockDataServiceFacade.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockDataServiceFacade.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Infrastructure;
    using Infrastructure.Models;

    public class MockDataServiceFacade : IDataServiceFacade
    {
        public MockDataServiceFacade()
        {
            this.TitleTemplates = new List<TitleTemplate>();
        }

        public event EventHandler<DataEventArgs<MediaBin>> LoadMediaBinAssetCompleted;

        public event EventHandler<DataEventArgs<Project>>LoadProjectCompleted;

        public event EventHandler<DataEventArgs<bool>> SaveProjectCompleted;

        public event EventHandler<DataEventArgs<List<TitleTemplate>>> LoadTitleTemplatesCompleted;

        public event EventHandler<DataEventArgs<List<Project>>> GetProjectsByUserCompleted;

        /// <summary>
        /// The handler that invokes when DeleteProject method completed.
        /// </summary>
        public event EventHandler<DataEventArgs<bool>> DeleteProjectCompleted;

        public List<TitleTemplate> TitleTemplates { get; set; }

        public bool LoadTitleTemplatesAsyncCalled { get; set; }

        public void GetProjectsByUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public void LoadMediaBinAssetsAsync(Uri containerUri)
        {
            throw new System.NotImplementedException();
        }

        public void LoadProjectAsync(Uri projectUri)
        {
            throw new System.NotImplementedException();
        }

        public void SaveProjectAsync(Project project)
        {
            throw new System.NotImplementedException();
        }

        public void LoadTitleTemplatesAsync()
        {
            this.LoadTitleTemplatesAsyncCalled = true;
            this.LoadTitleTemplatesCompleted(this, new DataEventArgs<List<TitleTemplate>>(this.TitleTemplates));
        }
        
        public void DeleteProject(Uri site)
        {
        }
    }
}
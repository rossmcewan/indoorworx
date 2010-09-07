// <copyright file="ProjectViewPresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ProjectViewPresenter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using RCE.Infrastructure;
    using RCE.Infrastructure.Models;

    /// <summary>
    /// Presenter class for the Project View control.
    /// </summary>
    public class ProjectViewPresenter : BaseModel, IProjectViewPresenter
    {
        /// <summary>
        /// The <seealso cref="IDataServiceFacade"/> instance used to used to get the list of projects.
        /// </summary>
        private readonly IDataServiceFacade serviceFacade;

        /// <summary>
        /// The <seealso cref="IConfigurationService"/> instance used to get the username.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// The <see cref="IRegionManager"/>.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Command used to delete a Project.
        /// </summary>
        private readonly DelegateCommand<object> deleteCommand;

        /// <summary>
        /// List of projects for the current user.
        /// </summary>
        private ObservableCollection<Project> projects;

        /// <summary>
        /// Uri of the project to be deleted.
        /// </summary>
        private Uri deletedProjectUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectViewPresenter"/> class.
        /// </summary>
        /// <param name="view">The <see cref="IProjectView"/> view instance.</param>
        /// <param name="configurationService">The <seealso cref="IConfigurationService"/> service used get the list of the projects.</param>
        /// <param name="serviceFacade">The service facade.</param>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        public ProjectViewPresenter(IProjectView view, IConfigurationService configurationService, IDataServiceFacade serviceFacade, IRegionManager regionManager)
        {
            this.serviceFacade = serviceFacade;
            this.configurationService = configurationService;
            this.regionManager = regionManager;
            this.deleteCommand = new DelegateCommand<object>(this.Delete);

            this.View = view;
            this.View.Model = this;
            
            this.serviceFacade.GetProjectsByUserCompleted += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            if (this.configurationService.GetProjectId() != null)
                            {
                                Project project = e.Data.Where(x => x.ProviderUri == this.configurationService.GetProjectId()).SingleOrDefault();
                                if (project != null && e.Data.Contains(project))
                                {
                                    e.Data.Remove(project);
                                }
                            }

                            this.Projects = new ObservableCollection<Project>();
                            e.Data.ForEach(x => this.Projects.Add(x)); 
                        }
                    };

            this.serviceFacade.DeleteProjectCompleted += (sender, e) =>
                     {
                         if (e.Data && this.deletedProjectUri != null)
                         {
                             Project project = this.projects.SingleOrDefault(x => x.ProviderUri == this.deletedProjectUri);
                             
                             if (project != null)
                             {
                                 this.Projects.Remove(project);
                             }
                         }

                         this.deletedProjectUri = null;
                     };

            this.serviceFacade.GetProjectsByUserAsync(this.configurationService.GetUserName());
        }

        /// <summary>
        /// Gets or sets the <see cref="IProjectView"/> of the presenter.
        /// </summary>
        /// <value>A <seealso cref="IProjectView"/> that represents the current view of the presenter.</value>
        public IProjectView View { get; set; }

        /// <summary>
        /// Gets or sets the Projects of the presenter.
        /// </summary>
        /// <value>A List of projects for the given user.</value>
        public ObservableCollection<Project> Projects
        {
            get
            {
                return this.projects;
            }

            set
            {
                this.projects = value;
                this.OnPropertyChanged("Projects");
            }
        }

        /// <summary>
        /// Gets the header icon (off status).
        /// </summary>
        /// <value>An <seealso cref="string" /> that represents the header icon off resource.</value>
        public string HeaderIconOff
        {
            get { return Resources.Resources.HeaderIconOff; }
        }

        /// <summary>
        /// Gets the header info.
        /// </summary>
        /// <value>The header info.</value>
        public string HeaderInfo
        {
            get { return Resources.Resources.HeaderInfo; }
        }

        /// <summary>
        /// Gets the Header Icon.
        /// </summary>
        /// <value>The header icon name.</value>
        public string HeaderIconOn
        {
            get { return Resources.Resources.HeaderIconOn; }
        }

        /// <summary>
        /// Gets the command that deletes the selected Project.
        /// </summary>
        /// <value>The <see cref="DelegateCommand{T}"/>.</value>
        public DelegateCommand<object> DeleteCommand
        {
            get
            {
                return this.deleteCommand;
            }
        }

        /// <summary>
        /// Activates this Project view.
        /// </summary>
        public void Activate()
        {
            this.regionManager.Regions[RegionNames.ToolsRegion].Activate(this.View);
        }

        /// <summary>
        /// Deletes the Project having the given Uri.
        /// </summary>
        /// <param name="projectUri">The Uri of the project.</param>
        private void Delete(object projectUri)
        {
            Uri uri = projectUri as Uri;

            if (uri != null)
            {
                this.deletedProjectUri = uri;
                this.serviceFacade.DeleteProject(uri);
            }
        }
    }
}

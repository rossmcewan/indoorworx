﻿// <copyright file="ShellPresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ShellPresenter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE
{
    using System.Globalization;
    using System.Reflection;
    using System.Windows;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;

    /// <summary>
    /// Interacts with the <see cref="IShell"/> view.
    /// </summary>
    public class ShellPresenter : BaseModel
    {
        /// <summary>
        /// The <seealso cref="IEventAggregator"/> instance used to publish and subscribe to events.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The command to save the project.
        /// </summary>
        private readonly DelegateCommand<object> saveCommand;

        /// <summary>
        /// The status message.
        /// </summary>
        private string status;

        /// <summary>
        /// The current project name.
        /// </summary>
        private string projectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellPresenter"/> class.
        /// </summary>
        /// <param name="shell">The <see cref="IShell"/> view instance.</param>
        /// <param name="eventAggregator">The <seealso cref="IEventAggregator"/> service used to publish and subscribe to events.</param>
        public ShellPresenter(IShell shell, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.Shell = shell;
            
            this.eventAggregator.GetEvent<FullScreenEvent>().Subscribe(this.FullScreenChanged, true);
            this.eventAggregator.GetEvent<StatusEvent>().Subscribe(this.ShowStatus, true);
            this.saveCommand = new DelegateCommand<object>(this.Save);
            
            this.Shell.KeyMappingActionEvent += (sender, e) => this.eventAggregator.GetEvent<KeyMappingEvent>().Publish(e.KayMappingAction);
            this.Shell.SaveProject += (sender, e) => this.SaveProject();

            this.Version = GetVersion();

            this.Shell.Model = this;
        }

        /// <summary>
        /// Gets or sets the <see cref="IShell"/> view.
        /// </summary>
        /// <value>A <seealso cref="IShell"/> that represents the current view.</value>
        public IShell Shell { get; set; }

        /// <summary>
        /// Gets the command used to save the project.
        /// </summary>
        /// <value>The command to save the project.</value>
        public DelegateCommand<object> SaveCommand
        {
            get { return this.saveCommand; }
        }

        /// <summary>
        /// Gets or sets the name of the current project.
        /// </summary>
        /// <value>The project name.</value>
        public string ProjectName
        {
            get
            {
                return this.projectName;
            }

            set
            {
                this.projectName = value;
                this.OnPropertyChanged("ProjectName");
            }
        }

        /// <summary>
        /// Gets or sets the name of the running version number.
        /// </summary>
        /// <value>The running version number.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        /// <value>The current status.</value>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Gets the current version of the application.
        /// </summary>
        /// <returns>The build number and the revision number.</returns>
        private static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            if (assembly != null && !string.IsNullOrEmpty(assembly.FullName))
            {
                string[] fullNameParts = assembly.FullName.Split(',');
                string plainVersion = fullNameParts[1].Split('=')[1];
                string[] version = plainVersion.Split('.');
                string build = version[2];
                string revision = version[3];

                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", build, revision);
            }

            return "Undefined";
        }

        /// <summary>
        /// Saves the current project.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void Save(object parameter)
        {
            this.SaveProject();
        }

        /// <summary>
        /// Toggle the fullscreen of the application or the player depending on the <see cref="FullScreenMode"/> mode.
        /// </summary>
        /// <param name="mode">The mode to determine if the toggling of the fullscreen should be done in the application or in the player.</param>
        private void FullScreenChanged(FullScreenMode mode)
        {
            this.Shell.ToggleFullScreen(mode);
        }

        /// <summary>
        /// Save the current project.
        /// </summary>
        private void SaveProject()
        {
            this.eventAggregator.GetEvent<SaveProjectEvent>().Publish(null);
        }

        /// <summary>
        /// Shows the status message.
        /// </summary>
        /// <param name="statusMessage">The status being showed.</param>
        private void ShowStatus(string statusMessage)
        {
            if (Deployment.Current != null && Deployment.Current.Dispatcher != null)
            {
                if (Deployment.Current.Dispatcher.CheckAccess())
                {
                    this.Status = statusMessage;
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => this.ShowStatus(statusMessage));
                }
            }
        }
    }
}

// <copyright file="EncoderSettingsPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: EncoderSettingsPresentationModel.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.EncoderOutput.Views
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using RCE.Services.Contracts.Output;
    using Services;

    public class EncoderSettingsPresentationModel : IEncoderSettingsPresentationModel
    {
        private readonly IProjectService projectService;

        private readonly IOutputGeneratorService outputService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncoderSettingsPresentationModel"/> class.
        /// </summary>
        public EncoderSettingsPresentationModel(IEncoderSettingsView view, IProjectService projectService, IOutputGeneratorService outputService)
        {
            this.View = view;
            this.projectService = projectService;
            this.outputService = outputService;
            this.outputService.GenerateOuputCompleted += this.OnGenerateOutputCompleted;

            this.Metadata = new ExpressionEncoderMetadata
                                {
                                    Settings = new ExpressionEncoderSettings(),
                                    WindowsMediaHeaderProperties = new WindowsMediaHeaderProperties()
                                };

            this.GenerateOutputCommand = new DelegateCommand<object>(this.GenerateOutput);

            this.ResizeModeOptions = new List<string> { "Stretch", "Letterbox" };

            this.AspectRatioOptions = new List<string> { "Custom", "16:9", "4:3" };

            this.FrameRateOptions = new List<double> { 23.976, 24, 25, 29.97, 30 };

            this.View.Model = this;
        }

        public IEncoderSettingsView View { get; private set; }

        public ExpressionEncoderMetadata Metadata { get; private set; }

        public List<string> ResizeModeOptions { get; private set; }

        public List<string> AspectRatioOptions { get; private set; }

        public List<double> FrameRateOptions { get; private set; }

        public DelegateCommand<object> GenerateOutputCommand { get; private set; }

        public string HeaderInfo
        {
            get { return Resources.Resources.HeaderInfo; }
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
        /// Gets the Header Icon.
        /// </summary>
        /// <value>The header icon name.</value>
        public string HeaderIconOn
        {
            get { return Resources.Resources.HeaderIconOn; }
        }

        private void GenerateOutput(object obj)
        {
            Project project = this.projectService.GetCurrentProject();

            project.Metadata = this.Metadata;

            // TODO: Makes this async.
            this.View.ShowProgressBar();
            this.outputService.GenerateOutputAsync(project);
        }

        private void OnGenerateOutputCompleted(object sender, DataEventArgs<bool> e)
        {
            this.View.HideProgressBar();
        }
    }
}
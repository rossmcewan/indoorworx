// <copyright file="OutputGeneratorService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: OutputGeneratorService.cs                     
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
    using System.ServiceModel;
    using ExpressionEncoderService;
    using Infrastructure;
    using Infrastructure.Models;
    using Infrastructure.Translators;

    public class OutputGeneratorService : IOutputGeneratorService
    {
        private readonly ILogger logger;
        private readonly string serviceAddress;

        public OutputGeneratorService(IConfigurationService configurationService, ILogger logger)
        {
            this.logger = logger;
            this.serviceAddress = configurationService.GetParameterValue("ExpressionEncoderServiceUrl");
        }

        public event EventHandler<DataEventArgs<bool>> GenerateOuputCompleted;

        public void GenerateOutputAsync(Project project)
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                Name = "ExpressionEncoderServiceBinding",
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647,
            };

            EndpointAddress endpointAddress = new EndpointAddress(this.serviceAddress);

            ExpressionEncoderServiceClient client = new ExpressionEncoderServiceClient(binding, endpointAddress);
            client.EnqueueJobCompleted += this.Client_EnqueueJobCompleted;

            RCE.Services.Contracts.Project dataProject = DataServiceTranslator.ConvertToDataServiceProject(project);

            client.EnqueueJobAsync(dataProject);
        }

        private void Client_EnqueueJobCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            bool generationCompleted = true;

            if (e.Error != null)
            {
                generationCompleted = false;

                this.logger.Log(this.GetType().Name, e.Error);
            }

            this.OnGenerateOutputCompleted(generationCompleted);
        }

        private void OnGenerateOutputCompleted(bool value)
        {
            EventHandler<DataEventArgs<bool>> completed = this.GenerateOuputCompleted;
            if (completed != null)
            {
                completed(this, new DataEventArgs<bool>(value));
            }
        }
    }
}
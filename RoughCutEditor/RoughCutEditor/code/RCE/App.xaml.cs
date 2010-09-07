﻿// <copyright file="App.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: App.xaml.cs                     
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
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Browser;
    using Infrastructure;
    using LAgger;
    using Services;

    /// <summary>
    /// The application entry point.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The application logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += Application_Exit;
            this.UnhandledException += Application_UnhandledException;

            InitializeComponent();
        }

        /// <summary>
        /// Reports errors to the Document Object Model.
        /// </summary>
        /// <param name="e">The <seealso cref="ApplicationUnhandledExceptionEventArgs"/> args that contains the exception ocurred.</param>
        private static void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in the Rough Cut Editor: " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ocurrs when the application finish.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="EventArgs"/>args.</param>
        private static void Application_Exit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when an exception is not handled in the application.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="ApplicationUnhandledExceptionEventArgs"/> args that containes the exception ocurred.</param>
        private static void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(() => ReportErrorToDOM(e));
            }

            Deployment.Current.Dispatcher.BeginInvoke(() => ReportErrorToDOM(e));
        }

        /// <summary>
        /// Ocurrs when the application start. Retrieves the configuration values.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="EventArgs"/> args.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.logger = new LoggerFacade();

            string settingsUriString = e.InitParams["settings"];

            if (Uri.IsWellFormedUriString(settingsUriString, UriKind.Relative))
            {
                Uri source = Application.Current.Host.Source;

                string location = source.AbsoluteUri.Substring(0, source.AbsoluteUri.IndexOf("ClientBin", StringComparison.OrdinalIgnoreCase));

                settingsUriString = String.Concat(location, settingsUriString);
            }
            
            Uri settingsUri = new Uri(settingsUriString, UriKind.Absolute);

            SettingsClient settingsService = new SettingsClient(settingsUri, this.logger);
            settingsService.GetSettingsCompleted += this.SettingsService_GetSettingsCompleted;
            settingsService.GetSettingsAsync();            
        }

        /// <summary>
        /// Handles the GetSettignsCompleted event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event args containing event data.</param>
       private void SettingsService_GetSettingsCompleted(object sender, DataEventArgs<IDictionary<string, string>> args)
        {
            if (args.Error != null)
            {
                throw args.Error;
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                IDictionary<string, string> settings = args.Data;
                IDictionary<string, string> queryString = HtmlPage.Document.QueryString;

                if (queryString != null)
                {
                    foreach (string key in queryString.Keys)
                    {
                        if (!settings.ContainsKey(key))
                        {
                            settings.Add(key, queryString[key]);
                        }
                    }
                }

                this.Run(settings);
            });
        }

        /// <summary>
        /// Runs the bootstrapper.
        /// </summary>
        /// <param name="settings">The application settings.</param>
        private void Run(IDictionary<string, string> settings)
        {
            if (!LoggerManager.Started && settings.ContainsKey("LoggerServiceUri"))
            {
                LoggerManager.Start(new Uri(settings["LoggerServiceUri"], UriKind.Absolute));
            }

            Bootstrapper bootstrapper = new Bootstrapper(settings, this.logger);
            bootstrapper.Run();
        }
    }
}

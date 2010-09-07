using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RCE.Modules.CompositeOutput.Services;
using Microsoft.Practices.Composite.Presentation.Commands;
using RCE.Infrastructure;

namespace RCE.Modules.CompositeOutput.Views
{
    public class CompositeOutputSettingsPresentationModel : ICompositeOutputSettingsPresentationModel
    {
        private readonly ICompositeOutputSettingsView view;
        private readonly ICompositeStreamManifestGeneratorService generator;
        private readonly IProjectService projects;

        public CompositeOutputSettingsPresentationModel(ICompositeOutputSettingsView view, ICompositeStreamManifestGeneratorService generator, IProjectService projects)
        {
            this.view = view;
            this.generator = generator;
            this.projects = projects;
            this.generateCompositeStreamManifestCommand = new DelegateCommand<object>(GenerateCompositeStreamManifest);
            this.view.Model = this;
        }

        private readonly ICommand generateCompositeStreamManifestCommand;
        public ICommand GenerateCompositeStreamManifestCommand
        {
            get { return this.generateCompositeStreamManifestCommand; }
        }

        public ICompositeOutputSettingsView View
        {
            get { return this.view; }
        }

        protected virtual void GenerateCompositeStreamManifest(object arg)
        {
            var project = projects.GetCurrentProject();
            View.ShowProgressBar();
            generator.GenerateCompositeStreamManifest(
                project,
                (result) => { View.HideProgressBar(); },
                (error) => { View.HideProgressBar(); MessageBox.Show(error.Message); });
        }

        #region IHeaderInfoProvider<string> Members

        public string HeaderInfo
        {
            get { return Resources.Resources.HeaderInfo; }
        }

        #endregion

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
    }
}

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
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;

namespace IndoorWorx.Library.Services
{
    /// <summary>
    /// Provides an implementation for the <see cref="IConfigurationService"/> service.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        /// <summary>
        /// The parameters defined in the configuration.
        /// </summary>
        private readonly IDictionary<string, string> settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        /// <param name="settings">The settings definied in the configuration.</param>
        public ConfigurationService(IDictionary<string, string> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
        }

        /// <summary>
        /// Retrieves the parameter value based on the given <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter to look for.</param>
        /// <returns>The parameter value if the parameter exists;othwerwise null.</returns>
        public string GetParameterValue(string parameter)
        {
            if (this.settings.ContainsKey(parameter))
            {
                return this.settings[parameter];
            }

            return null;
        }
    }
}

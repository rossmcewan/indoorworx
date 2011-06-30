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

namespace IndoorWorx.Infrastructure.Services
{
    /// <summary>
    /// Defines an interface to be used to retrieve the values specified in the configuration.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Retrieves the parameter value based on the given <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter to look for.</param>
        /// <returns>The parameter value if the parameter exists;othwerwise null.</returns>
        string GetParameterValue(string parameter);
    }
}

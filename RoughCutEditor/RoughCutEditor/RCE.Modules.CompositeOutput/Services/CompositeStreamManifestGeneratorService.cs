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
using System.ServiceModel;
using RCE.Infrastructure;
using RCE.Infrastructure.Models;
using RCE.Infrastructure.Translators;

namespace RCE.Modules.CompositeOutput.Services
{
    public class CompositeStreamManifestGeneratorService : ICompositeStreamManifestGeneratorService
    {
        private readonly string serviceAddress;
        public CompositeStreamManifestGeneratorService(IConfigurationService config)
        {
            this.serviceAddress = config.GetParameterValue("CompositeStreamManifestUri");
        }

        #region ICompositeStreamManifestGeneratorService Members

        public void GenerateCompositeStreamManifest(Project project, Action<bool> success, Action<Exception> error)
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                Name = "ExpressionEncoderServiceBinding",
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647,
            };
            EndpointAddress endpointAddress = new EndpointAddress(this.serviceAddress);
            var client = new CompositeStreamManifestService.CompositeStreamManifestServiceClient(binding, endpointAddress);
            client.CreateCompositeStreamCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                        error(e.Error);
                    else
                        success(!e.Cancelled);
                };
            var dsProject = DataServiceTranslator.ConvertToDataServiceProject(project);
            client.CreateCompositeStreamAsync(dsProject);
        }

        #endregion
    }
}

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
using RCE.Infrastructure.Models;

namespace RCE.Modules.CompositeOutput.Services
{
    public interface ICompositeStreamManifestGeneratorService
    {
        void GenerateCompositeStreamManifest(Project project, Action<bool> success, Action<Exception> error);
    }
}

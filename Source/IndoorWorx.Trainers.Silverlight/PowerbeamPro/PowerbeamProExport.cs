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
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Trainers.Resources;

namespace IndoorWorx.Trainers.PowerbeamPro
{
    public class PowerbeamProExport : ITrainerExport
    {
        public string CreateExport(ICollection<Telemetry> telemetry)
        {
            throw new NotImplementedException();
        }

        public string Title
        {
            get { return TrainersResources.PowerbeamProTitle; }
        }

        public string Description
        {
            get { return TrainersResources.PowerbeamProDescription; }
        }

        public string FileExtension
        {
            get { return ".xml"; }
        }

        public string FileFilter
        {
            get { return "Powerbeam Pro XML file|*.xml"; }
        }
    }
}

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

namespace IndoorWorx.Trainers.Computrainer
{
    public class ComputrainerExport : ITrainerExport
    {
        public string CreateExport(ICollection<Telemetry> telemetry)
        {
            throw new NotImplementedException();
        }

        public string Title
        {
            get { return TrainersResources.ComputrainerTitle; }
        }

        public string Description
        {
            get { return TrainersResources.ComputrainerDescription; }
        }
    }
}

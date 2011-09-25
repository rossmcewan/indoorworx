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
using IndoorWorx.Trainers.Resources;

namespace IndoorWorx.Trainers.Helpers
{
    public class ResourceWrapper
    {
        private readonly TrainersResources resources = new TrainersResources();
        public TrainersResources TrainersResources
        {
            get { return resources; }
        }
    }
}

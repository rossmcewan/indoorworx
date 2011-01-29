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
using IndoorWorx.ForMe.Resources;

namespace IndoorWorx.ForMe.Helpers
{
    public sealed class ResourceWrapper
    {
        private readonly ForMeResources forMeResources = new ForMeResources();

        public ForMeResources ForMeResources
        {
            get { return forMeResources; }
        }
    }
}

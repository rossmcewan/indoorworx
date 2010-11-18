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
using IndoorWorx.Designer.Silverlight.Resources;

namespace IndoorWorx.Designer.Silverlight.Helpers
{
    public class ResourceWrapper
    {
        private readonly DesignerResources designerResources = new DesignerResources();

        public DesignerResources DesignerResources
        {
            get { return designerResources; }
        }
    }
}

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
using IndoorWorx.Library.Resources;

namespace IndoorWorx.Library.Helpers
{
    public class ResourceWrapper
    {
        private readonly LibraryResources resources = new LibraryResources();

        public LibraryResources LibraryResources
        {
            get { return resources; }
        }
    }
}

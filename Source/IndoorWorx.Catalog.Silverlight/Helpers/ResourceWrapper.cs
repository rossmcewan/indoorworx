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
using IndoorWorx.Catalog.Resources;

namespace IndoorWorx.Catalog.Helpers
{
    public sealed class ResourceWrapper
    {
        private readonly CatalogResources catalogResources = new CatalogResources();

        public CatalogResources CatalogResources
        {
            get { return catalogResources; }
        }
    }
}

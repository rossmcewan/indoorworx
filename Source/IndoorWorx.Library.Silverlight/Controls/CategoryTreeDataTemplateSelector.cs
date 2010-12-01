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
using Telerik.Windows.Controls;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Library.Controls
{
    public class CategoryTreeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VideoTemplate { get; set; }

        public HierarchicalDataTemplate CategoryTemplate { get; set; }

        public HierarchicalDataTemplate CatalogTemplate { get; set; }

        public CategoryTreeDataTemplateSelector() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Video)
                return VideoTemplate;
            if (item is Category)
                return CategoryTemplate;
            if (item is Catalog)
                return CatalogTemplate;
            throw new Exception(string.Format(Library.Resources.LibraryResources.UnknownItemTypeErrorMessage, item.GetType().ToString()));
        }
    }
}

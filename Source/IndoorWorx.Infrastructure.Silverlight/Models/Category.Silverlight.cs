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
using Microsoft.Practices.Composite.Events;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Category
    {
        public event EventHandler SelectedCatalogChanging;
        public event EventHandler SelectedCatalogChanged;

        private Catalog selectedCatalog;
        public virtual Catalog SelectedCatalog
        {
            get { return selectedCatalog; }
            set
            {
                OnSelectedCategoryChanging();
                bool changed = value != selectedCatalog;
                selectedCatalog = value;
                if (changed)
                    OnSelectedCategoryChanged();
                FirePropertyChanged("SelectedCatalog");
            }
        }

        protected virtual void OnSelectedCategoryChanging()
        {
            if (SelectedCatalogChanging != null)
                SelectedCatalogChanging(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedCategoryChanged()
        {
            if (SelectedCatalogChanged != null)
                SelectedCatalogChanged(this, EventArgs.Empty);
        }
    }
}

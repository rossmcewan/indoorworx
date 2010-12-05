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

namespace IndoorWorx.Catalog.Views
{
    public interface ICatalogPresentationModel
    {
        object SelectedItem { get; set; }

        ICatalogView View { get; set; }

        ICollection<Category> Categories { get; set; }

        Category SelectedCategory { get; set; }

        ICommand DesignTrainingSetCommand { get; set; }

        ICommand PlayTrainingSetCommand { get; set; }

        ICommand PreviewTrainingSetCommand { get; set; }

        void LoadCategories();

        bool IsBusy { get; set; }

        void OnTrainingSetSelectionChanged();
    }
}

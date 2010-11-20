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

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerPresentationModel
    {
        IDesignerView View { get; set; }

        ICommand AddDesignerCommand { get; set; }

        ICollection<Category> Categories { get; set; }

        void LoadCategories();

        bool IsBusy { get; set; }
    }
}

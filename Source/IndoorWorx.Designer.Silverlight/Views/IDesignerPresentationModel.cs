using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using System.Windows.Input;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerPresentationModel : IDesignerPresentationModelBase
    {
        ICommand CancelCommand { get; }

        ICommand SaveCommand { get; }

        IDesignerView View { get; set; }

        bool UseSingleVideo { get; set; }

        bool UseMultipleVideos { get; set; }

        TrainingSetTemplate SelectedTemplate { get; set; }

        Interval SelectedInterval { get; set; }

        void Show();

        void Hide();

        bool IsBusy { get; set; }

        string Title { get; set; }
    }
}

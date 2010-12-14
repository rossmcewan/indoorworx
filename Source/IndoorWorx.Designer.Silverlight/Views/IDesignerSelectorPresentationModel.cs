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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerSelectorPresentationModel
    {
        Video Source { get; set; }

        ICommand AddEntryCommand { get; set; }

        double? SelectionStart { get; set; }

        double? SelectionEnd { get; set; }

        double? SelectionDuration { get; }

        bool IsSelectionValid { get; }

        void OnTrainingSetSelectionChanged();
    }
}

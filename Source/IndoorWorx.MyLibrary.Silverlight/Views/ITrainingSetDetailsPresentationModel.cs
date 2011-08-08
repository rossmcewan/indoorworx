using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITrainingSetDetailsPresentationModel
    {
        event EventHandler TrainingSetRemoved;

        ITrainingSetDetailsView View { get; set; }

        void SelectTrainingSetWithId(Guid guid);

        TrainingSet TrainingSet { get; set; }

        bool IsBusy { get; set; }

        ICommand PlayTrainingSetCommand { get; }
    }
}

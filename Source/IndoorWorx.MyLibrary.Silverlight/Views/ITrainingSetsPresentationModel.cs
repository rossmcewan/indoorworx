using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITrainingSetsPresentationModel
    {
        ITrainingSetsView View { get; set; }

        void Refresh();

        ICommand AddTrainingSetCommand { get; }

        string NumberOfTrainingSetsLabel { get; }

        bool IsBusy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Trainers.Views
{
    public interface ISelectTrainerPresentationModel
    {
        ISelectTrainerView View { get; set; }

        ICollection<ITrainerExport> AvailableTrainers { get; set; }

        ITrainerExport SelectedTrainer { get; set; }
    }
}

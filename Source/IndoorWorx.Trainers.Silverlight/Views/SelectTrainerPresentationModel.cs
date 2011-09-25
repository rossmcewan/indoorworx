using System;
using System.Linq;
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
using System.Collections.Generic;

namespace IndoorWorx.Trainers.Views
{
    public class SelectTrainerPresentationModel : BaseModel, ISelectTrainerPresentationModel
    {
        public ISelectTrainerView View { get; set; }

        private ICollection<ITrainerExport> availableTrainers;
        public virtual ICollection<ITrainerExport> AvailableTrainers
        {
            get { return availableTrainers; }
            set
            {
                availableTrainers = value;
                FirePropertyChanged("AvailableTrainers");
                if (availableTrainers != null)
                    SelectedTrainer = availableTrainers.FirstOrDefault();
            }
        }

        private ITrainerExport selectedTrainer;
        public virtual ITrainerExport SelectedTrainer
        {
            get { return selectedTrainer; }
            set
            {
                selectedTrainer = value;
                FirePropertyChanged("SelectedTrainer");
            }
        }
    }
}

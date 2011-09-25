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
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Trainers.Events;
using IndoorWorx.Trainers.Views;
using IndoorWorx.Trainers.Helpers;

namespace IndoorWorx.Trainers
{
    public class Module : IModule
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;
        public Module(IEventAggregator eventAggregator, IDialogFacade dialogFacade)
        {
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
        }

        public void Initialize()
        {
            Application.Current.Resources.Add("TrainersResources", new ResourceWrapper());
            eventAggregator.GetEvent<ExportTrainerFileEvent>().Subscribe(SaveTrainerFile, ThreadOption.UIThread, true);
        }

        private void SaveTrainerFile(ICollection<Telemetry> telemetry)
        {
            var supportedTrainers = new List<ITrainerExport>();
            eventAggregator.GetEvent<GetSupportedTrainersEvent>().Publish(supportedTrainers);

            var model = new SelectTrainerPresentationModel();
            var view = new SelectTrainerWindow(model);

            model.AvailableTrainers = supportedTrainers;
            view.Closed += (sender, e) =>
                {
                    if (view.DialogResult.GetValueOrDefault())
                        model.SelectedTrainer.CreateExport(telemetry);
                };
            view.Show();
        }
    }
}

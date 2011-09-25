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
using IndoorWorx.Trainers.Events;
using System.Collections.Generic;

namespace IndoorWorx.Trainers.PowerbeamPro
{
    public class PowerbeamProModule : IModule
    {
        private readonly IEventAggregator eventAggregator;
        public PowerbeamProModule(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            eventAggregator.GetEvent<GetSupportedTrainersEvent>().Subscribe(AddSupportedTrainer, true);
        }

        private void AddSupportedTrainer(ICollection<ITrainerExport> supportedTrainers)
        {
            supportedTrainers.Add(new PowerbeamProExport());
        }
    }
}

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
using Microsoft.Practices.Composite.Events;
using System.Collections.Generic;

namespace IndoorWorx.Infrastructure.Services
{
    public interface ISportingHabitsService
    {
        event EventHandler<DataEventArgs<ICollection<TrainingVolume>>> TrainingVolumeOptionsRetrieved;

        event EventHandler<DataEventArgs<Exception>> TrainingVolumeOptionsRetrievalError;

        void RetrieveTrainingVolumeOptions();

        event EventHandler<DataEventArgs<ICollection<IndoorTrainingFrequency>>> IndoorTrainingFrequencyOptionsRetrieved;

        event EventHandler<DataEventArgs<Exception>> IndoorTrainingFrequencyOptionsRetrievalError;

        void RetrieveIndoorTrainingFrequencyOptions();

        event EventHandler<DataEventArgs<ICollection<CompetitiveLevel>>> CompetitiveLevelsRetrieved;

        event EventHandler<DataEventArgs<Exception>> CompetitiveLevelsRetrievalError;

        void RetrieveCompetitiveLevels();

    }
}

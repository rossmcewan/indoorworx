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
using IndoorWorx.Infrastructure.Responses;

namespace IndoorWorx.Infrastructure.Services
{
    public interface ITrainingSetService
    {
        event EventHandler<DataEventArgs<CreateTrainingSetResponse>> TrainingSetCreated;

        event EventHandler<DataEventArgs<Exception>> CreateTrainingSetError;

        void CreateTrainingSet(TrainingSet trainingSet);
    }
}

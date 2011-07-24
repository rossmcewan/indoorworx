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
using Microsoft.Practices.Composite.Events;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Responses;

namespace IndoorWorx.Infrastructure.Services
{
    public interface ITrainingSetTemplateService
    {
        event EventHandler<DataEventArgs<ICollection<TrainingSetTemplate>>> TrainingSetTemplatesRetrieved;

        event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateRetrievalError;

        void RetrieveTrainingSetTemplates();

        event EventHandler<DataEventArgs<SaveTemplateResponse>> TrainingSetTemplateSaved;

        event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateSaveError;

        void SaveTemplate(TrainingSetTemplate template);

        event EventHandler<DataEventArgs<RemoveTemplateResponse>> TrainingSetTemplateRemoved;

        event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateRemoveError;

        void RemoveTemplate(TrainingSetTemplate template);
    }
}

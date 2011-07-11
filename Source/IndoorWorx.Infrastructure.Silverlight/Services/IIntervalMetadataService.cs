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
using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;

namespace IndoorWorx.Infrastructure.Services
{
    public interface IIntervalMetadataService
    {
        event EventHandler<DataEventArgs<ICollection<IntervalLevel>>> IntervalLevelsRetrieved;

        event EventHandler<DataEventArgs<Exception>> IntervalLevelRetrievalError;

        void RetrieveIntervalLevels();

        event EventHandler<DataEventArgs<ICollection<IntervalType>>> IntervalTypesRetrieved;

        event EventHandler<DataEventArgs<Exception>> IntervalTypesRetrievalError;

        void RetrieveIntervalTypes();

        event EventHandler<DataEventArgs<ICollection<EffortType>>> EffortTypesRetrieved;

        event EventHandler<DataEventArgs<Exception>> EffortTypesRetrievalError;

        void RetrieveEffortTypes();
    }
}

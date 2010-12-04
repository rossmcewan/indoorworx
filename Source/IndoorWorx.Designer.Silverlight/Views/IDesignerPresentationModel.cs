﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Designer.Domain;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerPresentationModel
    {
        event EventHandler EntriesChanged;

        event EventHandler CategoriesLoaded;

        event EventHandler<DataEventArgs<TrainingSet>> VideoSelected;

        object SelectedItem { get; set; }

        IDesignerView View { get; set; }

        ICollection<Category> Categories { get; set; }

        TrainingSet SelectedVideo { get; set; }

        TrainingSetDesign Design { get; set; }

        void LoadCategories();

        bool IsBusy { get; set; }

        void PlaySelectedPreview(Action play);

        void StopSelectedPreview(Action play);

        void SelectVideoWithId(Guid guid);

        ICollection<Telemetry> GetDesignedTelemetry();

        void AddDesignerForSelectedTrainingSet();
    }
}

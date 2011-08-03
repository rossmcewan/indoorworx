using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerPresentationModel : IDesignerPresentationModelBase
    {
        IDesignerView View { get; set; }

        bool UseSingleVideo { get; set; }

        bool UseMultipleVideos { get; set; }

        TrainingSetTemplate SelectedTemplate { get; set; }

        Interval SelectedInterval { get; set; }        
    }
}

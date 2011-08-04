using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public interface IIntervalDesignerPresentationModel : IDesignerPresentationModelBase
    {
        bool AllowSingleOrMultipleVideoSelection { get; set; }

        IIntervalDesignerView View { get; set; }

        Interval Interval { get; set; }

        Interval SelectedInterval { get; set; }

        bool UseSingleVideo { get; set; }

        bool UseMultipleVideos { get; set; }
    }
}

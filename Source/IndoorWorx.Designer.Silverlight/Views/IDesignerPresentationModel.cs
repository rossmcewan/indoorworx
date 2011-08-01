using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerPresentationModel
    {
        IDesignerView View { get; set; }

        TrainingSetTemplate SelectedTemplate { get; set; }
    }
}

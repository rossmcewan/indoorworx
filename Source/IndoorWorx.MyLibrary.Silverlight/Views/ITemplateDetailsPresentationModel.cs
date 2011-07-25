using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITemplateDetailsPresentationModel
    {
        event EventHandler TemplateRemoved;

        ITemplateDetailsView View { get; set; }

        void SelectTemplateWithId(Guid guid);

        bool IsBusy { get; set; }

        TrainingSetTemplate Template { get; set; }

        ICommand CreateRideCommand { get; }

        ICommand EditTemplateCommand { get; }

        ICommand RemoveTemplateCommand { get; }
    }
}

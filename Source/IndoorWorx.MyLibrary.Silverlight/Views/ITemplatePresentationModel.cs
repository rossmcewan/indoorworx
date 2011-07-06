using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITemplatePresentationModel
    {
        ITemplateView View { get; set; }

        ICommand CancelCommand { get; }

        CrudOperation TemplateOperation { get; }

        void NewTemplate();

        TrainingSetTemplate Template { get; }        
    }
}

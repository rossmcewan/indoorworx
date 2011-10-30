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
        bool IsBusy { get; set; }

        ITemplateView View { get; set; }

        ICommand CancelCommand { get; }

        ICommand SaveCommand { get; }

        CrudOperation TemplateOperation { get; }        

        ICommand EditIntervalCommand { get; }

        void NewTemplate();

        void EditTemplate(TrainingSetTemplate template);

        TrainingSetTemplate Template { get; }

        IList<Interval> Intervals { get; }

        ICommand AddIntervalCommand { get; }

        ICommand RemoveIntervalCommand { get; }

        bool HasIntervals { get; }

        Interval CurrentInterval { get; }

        ICommand InsertRowCommand { get; }

        ICommand CopyRowsCommand { get; }

        ICommand MoveRowsCommand { get; }

        ICommand DeleteRowsCommand { get; }
    }
}

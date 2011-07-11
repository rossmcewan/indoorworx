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

        ICollection<IntervalType> IntervalTypes { get; }

        ICollection<EffortType> EffortTypes { get; }

        void NewTemplate();

        TrainingSetTemplate Template { get; }

        ICollection<Interval> WarmupIntervals { get; }

        ICommand AddIntervalToWarmupCommand { get; }

        ICommand RemoveIntervalFromWarmupCommand { get; }

        ICommand MoveWarmupIntervalUpCommand { get; }

        ICommand MoveWarmupIntervalDownCommand { get; }

        bool HasWarmupIntervals { get; }

        ICollection<Interval> MainSetIntervals { get; }

        ICommand AddIntervalToMainSetCommand { get; }

        ICommand RemoveIntervalFromMainSetCommand { get; }

        bool HasMainSetIntervals { get; }

        ICollection<Interval> CooldownIntervals { get; }

        ICommand AddIntervalToCooldownCommand { get; }

        ICommand RemoveIntervalFromCooldownCommand { get; }

        bool HasCooldownIntervals { get; }
    }
}

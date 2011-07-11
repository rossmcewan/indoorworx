using System;
using System.Linq;
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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Infrastructure;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IndoorWorx.Infrastructure.Services;

namespace IndoorWorx.MyLibrary.Views
{
    public class TemplatePresentationModel : BaseModel, ITemplatePresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;
        private readonly IShell shell;
        public TemplatePresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator, IDialogFacade dialogFacade, IShell shell)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
            this.shell = shell;            
            this.warmupIntervals = new ObservableCollection<Interval>();
            this.mainSetIntervals = new ObservableCollection<Interval>();
            this.cooldownIntervals = new ObservableCollection<Interval>();
            this.CancelCommand = new DelegateCommand<object>(Cancel);
            this.addIntervalToWarmupCommand = new DelegateCommand<Interval>(AddIntervalToWarmup);
            this.removeIntervalFromWarmupCommand = new DelegateCommand<Interval>(RemoveIntervalFromWarmup);
            this.MoveWarmupIntervalDownCommand = new DelegateCommand<Interval>(MoveWarmupIntervalDown);
            this.MoveWarmupIntervalUpCommand = new DelegateCommand<Interval>(MoveWarmupIntervalUp);
            this.addIntervaltoMainSetCommand = new DelegateCommand<Interval>(AddIntervalToMainSet);
            this.removeIntervalFromMainSetCommand = new DelegateCommand<Interval>(RemoveIntervalFromMainSet);
            this.addIntervalToCooldownCommand = new DelegateCommand<Interval>(AddIntervalToCooldown);
            this.removeIntervalFromCooldownCommand = new DelegateCommand<Interval>(RemoveIntervalFromCooldown);

            var intervalMetadataService = serviceLocator.GetInstance<IIntervalMetadataService>();
            intervalMetadataService.IntervalLevelsRetrieved += (sender, e) =>
                {
                };
            intervalMetadataService.IntervalTypesRetrieved += (sender, e) =>
                {
                    this.IntervalTypes = e.Value;
                    FirePropertyChanged("IntervalTypes");
                };
            intervalMetadataService.EffortTypesRetrieved += (sender, e) =>
                {
                    this.EffortTypes = e.Value;
                    FirePropertyChanged("EffortTypes");
                };
            intervalMetadataService.RetrieveIntervalLevels();
            intervalMetadataService.RetrieveIntervalTypes();
            intervalMetadataService.RetrieveEffortTypes();
        }

        private void Cancel(object arg)
        {
            shell.RemoveFromLayoutRoot(View as UIElement);
        }

        public ITemplateView View { get; set; }

        public ICommand CancelCommand { get; private set; }

        public CrudOperation TemplateOperation { get; private set; }

        public TrainingSetTemplate Template { get; private set; }

        public ICollection<IntervalType> IntervalTypes { get; private set; }

        public ICollection<EffortType> EffortTypes { get; private set; }

        public void NewTemplate()
        {
            this.TemplateOperation = CrudOperation.Create;
            this.Template = new TrainingSetTemplate();
            shell.AddToLayoutRoot(View as UIElement);
        }

        private ObservableCollection<Interval> warmupIntervals;
        public ICollection<Interval> WarmupIntervals
        {
            get { return this.warmupIntervals; }
        }

        private ICommand addIntervalToWarmupCommand;
        public ICommand AddIntervalToWarmupCommand
        {
            get { return this.addIntervalToWarmupCommand; }
        }

        private void AddIntervalToWarmup(Interval arg)
        {
            var index = warmupIntervals.IndexOf(arg);
            if (index != -1)
                warmupIntervals.Insert(++index, new Interval());
            else
                warmupIntervals.Add(new Interval());
            FirePropertyChanged("HasWarmupIntervals");
            RefreshTemplate();
        }

        private ICommand removeIntervalFromWarmupCommand;
        public ICommand RemoveIntervalFromWarmupCommand
        {
            get { return this.removeIntervalFromWarmupCommand; }
        }

        private void RemoveIntervalFromWarmup(Interval arg)
        {
            WarmupIntervals.Remove(arg);
            FirePropertyChanged("HasWarmupIntervals");
            RefreshTemplate();
        }

        public ICommand MoveWarmupIntervalUpCommand { get; private set; }

        private void MoveWarmupIntervalUp(Interval interval)
        {
            var index = warmupIntervals.IndexOf(interval);
            if (index >= 1)
            {
                warmupIntervals.RemoveAt(index);
                warmupIntervals.Insert(--index, interval);
                RefreshTemplate();
            }
        }

        public ICommand MoveWarmupIntervalDownCommand { get; private set; }

        private void MoveWarmupIntervalDown(Interval interval)
        {
            var index = warmupIntervals.IndexOf(interval);
            if (index != -1 && index < (warmupIntervals.Count - 1))
            {
                warmupIntervals.RemoveAt(index);
                warmupIntervals.Insert(++index, interval);
                RefreshTemplate();
            }
        }

        public bool HasWarmupIntervals
        {
            get
            {
                return warmupIntervals.Count > 0;
            }
        }

        private ObservableCollection<Interval> mainSetIntervals;
        public ICollection<Interval> MainSetIntervals
        {
            get { return this.mainSetIntervals; }
        }

        private ICommand addIntervaltoMainSetCommand;
        public ICommand AddIntervalToMainSetCommand
        {
            get { return this.addIntervaltoMainSetCommand; }
        }

        private void AddIntervalToMainSet(Interval arg)
        {
            var index = warmupIntervals.IndexOf(arg);
            if (index != -1)
                warmupIntervals.Insert(++index, new Interval());
            else
                warmupIntervals.Add(new Interval());
            FirePropertyChanged("HasMainSetIntervals");
            RefreshTemplate();
        }

        private ICommand removeIntervalFromMainSetCommand;
        public ICommand RemoveIntervalFromMainSetCommand
        {
            get { return this.removeIntervalFromMainSetCommand; }
        }

        private void RemoveIntervalFromMainSet(Interval arg)
        {
            MainSetIntervals.Remove(arg);
            FirePropertyChanged("HasMainSetIntervals");
            RefreshTemplate();
        }

        public bool HasMainSetIntervals
        {
            get { return mainSetIntervals.Count > 0; }
        }

        private ObservableCollection<Interval> cooldownIntervals;
        public ICollection<Interval> CooldownIntervals
        {
            get { return this.cooldownIntervals; }
        }

        private ICommand addIntervalToCooldownCommand;
        public ICommand AddIntervalToCooldownCommand
        {
            get { return this.addIntervalToCooldownCommand; }
        }

        private void AddIntervalToCooldown(Interval arg)
        {
            var index = warmupIntervals.IndexOf(arg);
            if (index != -1)
                warmupIntervals.Insert(++index, new Interval());
            else
                warmupIntervals.Add(new Interval());
            FirePropertyChanged("HasCooldownIntervals");
            RefreshTemplate();
        }

        private ICommand removeIntervalFromCooldownCommand;
        public ICommand RemoveIntervalFromCooldownCommand
        {
            get { return this.removeIntervalFromCooldownCommand; }
        }

        private void RemoveIntervalFromCooldown(Interval arg)
        {
            CooldownIntervals.Remove(arg);
            FirePropertyChanged("HasCooldownIntervals");
            RefreshTemplate();
        }

        public bool HasCooldownIntervals
        {
            get { return this.cooldownIntervals.Count > 0; }
        }

        protected virtual void RefreshTemplate()
        {
            Template.Intervals.Clear();
            foreach (var interval in warmupIntervals)
            {
                CreateIntervals(interval, x => Template.Intervals.Add(x));
            }
            foreach (var interval in mainSetIntervals)
            {
                CreateIntervals(interval, x => Template.Intervals.Add(x));
            }
            foreach (var interval in cooldownIntervals)
            {
                CreateIntervals(interval, x => Template.Intervals.Add(x));
            }
            Template.SetupIntervalTimes();
        }

        private void CreateIntervals(Interval interval, Action<Interval> add)
        {
            for (int i = 0; i < interval.Repeats; i++)
            {
                add(new Interval()
                {
                    Description = interval.Description,
                    Duration = new TimeSpan(0, interval.DurationMinutes, interval.DurationSeconds),
                    IntervalType = interval.IntervalType,
                    EffortFrom = 200
                    //need to set the other information as well
                });
                add(new Interval()
                {
                    Description = interval.Description,
                    Duration = new TimeSpan(0, interval.RecoveryIntervalMinutes, interval.RecoveryIntervalSeconds),
                    EffortFrom = 100
                    //need to set the interval type to 'Recovery'
                });
            }
        }
    }
}

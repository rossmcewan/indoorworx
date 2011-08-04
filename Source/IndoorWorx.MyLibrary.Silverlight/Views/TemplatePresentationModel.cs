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
using IndoorWorx.MyLibrary.Resources;

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
            this.SaveCommand = new DelegateCommand<object>(Save);
            this.editIntervalCommand = new DelegateCommand<Interval>(EditInterval);
            this.addIntervalToWarmupCommand = new DelegateCommand<Interval>(AddIntervalToWarmup);
            this.removeIntervalFromWarmupCommand = new DelegateCommand<Interval>(RemoveIntervalFromWarmup);
            this.MoveWarmupIntervalDownCommand = new DelegateCommand<Interval>(MoveWarmupIntervalDown);
            this.MoveWarmupIntervalUpCommand = new DelegateCommand<Interval>(MoveWarmupIntervalUp);
            this.addIntervaltoMainSetCommand = new DelegateCommand<Interval>(AddIntervalToMainSet);
            this.removeIntervalFromMainSetCommand = new DelegateCommand<Interval>(RemoveIntervalFromMainSet);
            this.addIntervalToCooldownCommand = new DelegateCommand<Interval>(AddIntervalToCooldown);
            this.removeIntervalFromCooldownCommand = new DelegateCommand<Interval>(RemoveIntervalFromCooldown);
        }

        private bool busy;
        public virtual bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                FirePropertyChanged("IsBusy");
            }
        }

        private ICommand editIntervalCommand;
        public ICommand EditIntervalCommand
        {
            get { return this.editIntervalCommand; }
        }

        private void EditInterval(Interval interval)
        {
            var view = serviceLocator.GetInstance<IIntervalView>();
            view.Model.Interval = interval;
            view.Model.Mode = CrudOperation.Update;
            interval.BeginEdit();
            view.Show();
        }

        private void Cancel(object arg)
        {
            if (Template.IsChanged)
            {
                dialogFacade.Confirm(MyLibraryResources.TemplateChangedCancelConfirmation, (confirmed) =>
                    {
                        if (confirmed)
                        {
                            this.Template.CancelEdit();
                            shell.RemoveFromLayoutRoot(View as UIElement);
                        }
                    });
            }
            else
            {
                this.Template.CancelEdit();
                shell.RemoveFromLayoutRoot(View as UIElement);
            }
        }

        private void Save(object arg)
        {
            if (Template.IsChanged)
            {
                IsBusy = true;
                try
                {
                    var service = serviceLocator.GetInstance<ITrainingSetTemplateService>();
                    service.TrainingSetTemplateSaved += (sender, e) =>
                        {
                            switch (e.Value.Status)
                            {
                                case SaveTemplateStatus.Success:
                                    if (ApplicationUser.CurrentUser.Templates.Contains(e.Value.SavedTemplate))
                                    {
                                        SmartDispatcher.BeginInvoke(() =>
                                            {
                                                var list = ApplicationUser.CurrentUser.Templates as ObservableCollection<TrainingSetTemplate>;
                                                var index = list.IndexOf(e.Value.SavedTemplate);
                                                list.RemoveAt(index);
                                                list.Insert(index, e.Value.SavedTemplate);
                                                shell.RemoveFromLayoutRoot(View as UIElement);
                                            });
                                    }
                                    else
                                    {
                                        SmartDispatcher.BeginInvoke(() =>
                                            {
                                                ApplicationUser.CurrentUser.Templates.Add(e.Value.SavedTemplate);
                                                shell.RemoveFromLayoutRoot(View as UIElement);
                                            });
                                    }
                                    break;
                                case SaveTemplateStatus.Error:
                                    dialogFacade.Alert(MyLibraryResources.ErrorSavingTemplate);                                    
                                    break;
                                default:
                                    break;
                            }
                            IsBusy = false;
                        };
                    service.TrainingSetTemplateSaveError += (sender, e) =>
                        {
                            IsBusy = false;
                        };
                    service.SaveTemplate(Template);
                }
                catch (Exception ex)
                {
                    dialogFacade.Alert(MyLibraryResources.ErrorSavingTemplate); 
                    IsBusy = false;
                }
            }
            else
            {
                shell.RemoveFromLayoutRoot(View as UIElement);
            }
        }

        public ITemplateView View { get; set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public CrudOperation TemplateOperation { get; private set; }

        public TrainingSetTemplate Template { get; private set; }

        public void NewTemplate()
        {
            this.TemplateOperation = CrudOperation.Create;
            this.Template = new TrainingSetTemplate();
            this.Template.EffortType = ApplicationContext.Current.EffortTypes.FirstOrDefault();
            this.Template.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "EffortType")
                    {
                        foreach (var interval in WarmupIntervals)
                            interval.EffortType = this.Template.EffortType;
                        foreach (var interval in MainSetIntervals)
                            interval.EffortType = this.Template.EffortType;
                        foreach (var interval in CooldownIntervals)
                            interval.EffortType = this.Template.EffortType;
                    }
                };
            this.Template.BeginEdit();
            shell.AddToLayoutRoot(View as UIElement);
        }

        public void EditTemplate(TrainingSetTemplate template)
        {
            this.TemplateOperation = CrudOperation.Update;
            this.Template = template;
            var warmups = template.Intervals.Where(x=>x.TemplateSection == Interval.WarmupTag);
            PopulateIntervals(warmups, x => warmupIntervals.Add(x));
            var mainsets = template.Intervals.Where(x=>x.TemplateSection == Interval.MainSetTag);
            PopulateIntervals(mainsets, x => mainSetIntervals.Add(x));
            var cooldowns = template.Intervals.Where(x=>x.TemplateSection == Interval.CooldownTag);
            PopulateIntervals(cooldowns, x => cooldownIntervals.Add(x));
            this.Template.BeginEdit();
            shell.AddToLayoutRoot(View as UIElement);
        }

        private void PopulateIntervals(IEnumerable<Interval> rootIntervals, Action<Interval> add)
        {
            var intervalGroups = rootIntervals.GroupBy(x => x.SectionGroup);
            foreach (var group in intervalGroups)
            {
                var intervals = group.ToList();
                var hasRecovery = intervals.Count > 1 && intervals.GroupBy(x => x.Effort).Count() > 1;
                var firstInterval = intervals.First();

                var interval = new Interval();

                interval.Description = firstInterval.Description;
                interval.Duration = firstInterval.Duration;
                interval.Effort = firstInterval.Effort;
                interval.EffortType = firstInterval.EffortType;
                interval.IntervalDuration = new Infrastructure.Models.Duration() { Hours = firstInterval.Duration.Hours, Minutes = firstInterval.Duration.Minutes, Seconds = firstInterval.Duration.Seconds };
                interval.IntervalLevel = firstInterval.IntervalLevel;
                interval.IntervalType = firstInterval.IntervalType;
                interval.RecoveryInterval = new Infrastructure.Models.Duration() { Hours = 0, Minutes = 0, Seconds = 0 };
                interval.TemplateSection = firstInterval.TemplateSection;
                interval.SectionGroup = firstInterval.SectionGroup;
                interval.Title = firstInterval.Title;
                if (hasRecovery)
                {
                    var recoveryInterval = intervals.ElementAt(1);
                    interval.Repeats = intervals.Count / 2;
                    interval.RecoveryInterval = new Infrastructure.Models.Duration() { Hours = recoveryInterval.Duration.Hours, Minutes = recoveryInterval.Duration.Minutes, Seconds = recoveryInterval.Duration.Seconds };
                }
                else
                {
                    interval.RecoveryInterval = new Infrastructure.Models.Duration() { Hours = 0, Minutes = 0, Seconds = 0 };
                    interval.Repeats = intervals.Count;
                }
                interval.NotifyOnChange(RefreshTemplate);
                add(interval);
            }
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
            var interval = Interval.NewWarmupInterval(Template.EffortType, () => RefreshTemplate());
            if (arg == null)
            {
                warmupIntervals.Add(interval);
            }
            else
            {
                var index = warmupIntervals.IndexOf(arg);
                if (index != -1)
                    warmupIntervals.Insert(++index, interval);
                else
                    warmupIntervals.Add(interval);
            }
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
            var interval = Interval.NewMainSetInterval(Template.EffortType, () => RefreshTemplate());
            if (arg == null)
            {
                mainSetIntervals.Add(interval);
            }
            else
            {
                var index = mainSetIntervals.IndexOf(arg);
                if (index != -1)
                    mainSetIntervals.Insert(++index, interval);
                else
                    mainSetIntervals.Add(interval);
            }
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
            var interval = Interval.NewCooldownInterval(Template.EffortType, () => RefreshTemplate());
            if (arg == null)
            {
                cooldownIntervals.Add(interval);
            }
            else
            {
                var index = cooldownIntervals.IndexOf(arg);
                if (index != -1)
                    cooldownIntervals.Insert(++index, interval);
                else
                    cooldownIntervals.Add(interval);
            }
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
            Template.VideoText.Clear();
            TimeSpan totalDuration = TimeSpan.Zero;
            int sequence = 0;
            foreach (var interval in warmupIntervals)
            {
                CreateIntervals(interval, x => 
                {
                    x.Sequence = sequence++;
                    Template.Intervals.Add(x);
                    totalDuration = totalDuration.Add(x.Duration);
                });
            }
            foreach (var interval in mainSetIntervals)
            {
                CreateIntervals(interval, x =>
                {
                    x.Sequence = sequence++;
                    Template.Intervals.Add(x);
                    totalDuration = totalDuration.Add(x.Duration);
                });
            }
            foreach (var interval in cooldownIntervals)
            {
                CreateIntervals(interval, x =>
                {
                    x.Sequence = sequence++;
                    Template.Intervals.Add(x);
                    totalDuration = totalDuration.Add(x.Duration);
                });
            }
            Template.SetupIntervalTimes();
            Template.Duration = totalDuration;

            TimeSpan start = TimeSpan.Zero;
            foreach (var interval in Template.Intervals)
            {
                if (interval.ToStart.IsActive)
                {
                    var countDownFrom = interval.ToStart.Duration.AsTimeSpan();
                    if (countDownFrom <= start)
                    {
                        var tick = interval.ToStart.Tick.AsTimeSpan();
                        int numberOfCounts = (int)(countDownFrom.TotalSeconds / tick.TotalSeconds);
                        for (int i = numberOfCounts; i > 0; i--)
                        {
                            var textEntry = new VideoText();
                            textEntry.Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter;
                            textEntry.Duration = TimeSpan.FromSeconds(Math.Min(1, tick.TotalSeconds));
                            textEntry.StartTime = start.Subtract(TimeSpan.FromSeconds(tick.TotalSeconds * i));
                            textEntry.MainText = TimeSpan.FromSeconds(tick.TotalSeconds * i).ToString();
                            textEntry.SubText = MyLibraryResources.CountdownToStartText;
                            Template.VideoText.Add(textEntry);
                        }                        
                    }
                    if (!string.IsNullOrWhiteSpace(interval.ToStart.Message))
                    {
                        var textEntry = new VideoText();
                        textEntry.Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter;
                        textEntry.Duration = TimeSpan.FromSeconds(1);
                        textEntry.StartTime = start;
                        textEntry.MainText = interval.ToStart.Message;
                        Template.VideoText.Add(textEntry);
                    }
                }
                if (interval.ToEnd.IsActive)
                {
                    var countDownFrom = interval.ToEnd.Duration.AsTimeSpan();
                    if (countDownFrom < interval.Duration)
                    {
                        var tick = interval.ToEnd.Tick.AsTimeSpan();
                        int numberOfCounts = (int)(countDownFrom.TotalSeconds / tick.TotalSeconds);
                        for (int i = numberOfCounts; i > 0; i--)
                        {
                            var textEntry = new VideoText();
                            textEntry.Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter;
                            textEntry.Duration = TimeSpan.FromSeconds(Math.Min(1, tick.TotalSeconds));
                            textEntry.StartTime = start.Add(interval.Duration).Subtract(TimeSpan.FromSeconds(tick.TotalSeconds * i));
                            textEntry.MainText = TimeSpan.FromSeconds(tick.TotalSeconds * i).ToString();
                            textEntry.SubText = MyLibraryResources.CountdownToEndText;
                            Template.VideoText.Add(textEntry);
                        }                        
                    }
                    if (!string.IsNullOrWhiteSpace(interval.ToEnd.Message))
                    {
                        var textEntry = new VideoText();
                        textEntry.Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter;
                        textEntry.Duration = TimeSpan.FromSeconds(1);
                        textEntry.StartTime = start;
                        textEntry.MainText = interval.ToEnd.Message;
                        Template.VideoText.Add(textEntry);
                    }
                }
                start += interval.Duration;
            }
        }

        private void CreateIntervals(Interval interval, Action<Interval> add)
        {
            var recovery = ApplicationContext.Current.IntervalLevels.FirstOrDefault();
            var recoveryType = ApplicationContext.Current.IntervalTypes.FirstOrDefault(x => x.IsRecovery);
            for (int i = 0; i < interval.Repeats; i++)
            {
                add(new Interval()
                {
                    Description = interval.Description,
                    Duration = interval.IntervalDuration.AsTimeSpan(),
                    IntervalType = interval.IntervalType,
                    Effort = interval.Effort,
                    EffortType = interval.EffortType,
                    IntervalLevel = interval.IntervalLevel,
                    Title = string.Format(MyLibraryResources.IntervalTitle, (i+1)),
                    TemplateSection = interval.TemplateSection,
                    SectionGroup = interval.SectionGroup,
                    ToStart = interval.ToStart,
                    ToEnd = interval.ToEnd
                });
                if (interval.RecoveryInterval.AsTimeSpan() > TimeSpan.Zero)
                {
                    var recoveryInterval = new Interval()
                    {
                        Description = interval.Description,
                        Duration = interval.RecoveryInterval.AsTimeSpan(),
                        IntervalType = recoveryType,
                        EffortType = interval.EffortType,
                        IntervalLevel = recovery,
                        Title = string.Format(MyLibraryResources.RecoveryTitle, (i+1)),
                        TemplateSection = interval.TemplateSection,
                        SectionGroup = interval.SectionGroup
                    };
                    recoveryInterval.Effort = recovery.AverageEffortFor(interval.EffortType);
                    add(recoveryInterval);
                }
            }
        }
    }
}

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
            this.intervals = new ObservableCollection<Interval>();
            this.intervals.CollectionChanged += (sender, e) =>
                {
                    switch (e.Action)
                    {
                        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                            if (IsPopulatingIntervals) return;
                            var interval = e.NewItems[0] as Interval;
                            Interval.Setup(interval, string.Empty, Template.EffortType, () => RefreshTemplate());
                            RefreshTemplate();
                            break;
                        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                            RefreshTemplate();
                            break;
                        case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                            RefreshTemplate();
                            break;
                        case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                            break;
                        default:
                            break;
                    }
                };
            this.CancelCommand = new DelegateCommand<object>(Cancel);
            this.SaveCommand = new DelegateCommand<object>(Save);
            this.editIntervalCommand = new DelegateCommand<Interval>(EditInterval);
            this.addIntervalCommand = new DelegateCommand<Interval>(AddInterval);
            this.removeIntervalCommand = new DelegateCommand<Interval>(RemoveInterval);
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
                            this.Template.ReloadTelemetry();
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
                        foreach (var interval in Intervals)
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
            this.Template.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "EffortType")
                {
                    foreach (var interval in Intervals)
                        interval.EffortType = this.Template.EffortType;
                }
            };
            PopulateIntervals(template.Intervals, x => intervals.Add(x));
            this.Template.BeginEdit();
            shell.AddToLayoutRoot(View as UIElement);
        }

        public bool IsPopulatingIntervals { get; set; }

        private void PopulateIntervals(IEnumerable<Interval> rootIntervals, Action<Interval> add)
        {
            IsPopulatingIntervals = true;
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
            IsPopulatingIntervals = false;
        }

        private ObservableCollection<Interval> intervals;
        public ICollection<Interval> Intervals
        {
            get { return this.intervals; }
        }

        private ICommand addIntervalCommand;
        public ICommand AddIntervalCommand
        {
            get { return this.addIntervalCommand; }
        }

        private void AddInterval(Interval arg)
        {
            var interval = Interval.NewInterval(string.Empty, Template.EffortType, () => RefreshTemplate());
            if (arg == null)
            {
                intervals.Add(interval);
            }
            else
            {
                var index = intervals.IndexOf(arg);
                if (index != -1)
                    intervals.Insert(++index, interval);
                else
                    intervals.Add(interval);
            }
            FirePropertyChanged("HasIntervals");
            RefreshTemplate();
        }

        private ICommand removeIntervalCommand;
        public ICommand RemoveIntervalCommand
        {
            get { return this.removeIntervalCommand; }
        }

        private void RemoveInterval(Interval arg)
        {
            Intervals.Remove(arg);
            FirePropertyChanged("HasIntervals");
            RefreshTemplate();
        }

        public bool HasIntervals
        {
            get { return intervals.Count > 0; }
        }

        protected virtual void RefreshTemplate()
        {
            Template.Intervals.Clear();
            var toRemove = Template.VideoText.Where(x => !string.IsNullOrWhiteSpace(x.Tag)).ToList();
            foreach (var tr in toRemove)
            {
                Template.VideoText.Remove(tr);
            }
            TimeSpan totalDuration = TimeSpan.Zero;
            int sequence = 0;
            foreach (var interval in intervals)
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
                            textEntry.Tag = interval.SectionGroup;
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
                        textEntry.Tag = interval.SectionGroup;
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
                            textEntry.Tag = interval.SectionGroup;
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
                        textEntry.Tag = interval.SectionGroup;
                        textEntry.Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter;
                        textEntry.Duration = TimeSpan.FromSeconds(1);
                        textEntry.StartTime = start.Add(interval.Duration);
                        textEntry.MainText = interval.ToEnd.Message;
                        Template.VideoText.Add(textEntry);
                    }
                }
                start += interval.Duration;
            }
            Template.VideoText = new ObservableCollection<VideoText>(Template.VideoText.OrderBy(x => x.StartTime));
            //if(!Template.IsCancellingEdit)
            //    Template.ReloadTelemetry();
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
                    Title = string.IsNullOrWhiteSpace(interval.Title) ? string.Format(MyLibraryResources.IntervalTitle, (i+1)) : interval.Title,
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

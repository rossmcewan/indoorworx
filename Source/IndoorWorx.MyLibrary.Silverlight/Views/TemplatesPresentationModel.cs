using System;
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
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Library.DragDrop;
using IndoorWorx.Infrastructure;
using IndoorWorx.MyLibrary.Resources;
using IndoorWorx.Infrastructure.Events;
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Facades;

namespace IndoorWorx.MyLibrary.Views
{
    public class TemplatesPresentationModel : BaseModel, ITemplatesPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;

        public TemplatesPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator, IDialogFacade dialogFacade)
        {
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "TemplateCount")
                    FirePropertyChanged("NumberOfTemplatesLabel");
            };
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
            this.AddTemplateCommand = new DelegateCommand<object>(AddTemplate);
            this.RemoveTemplateCommand = new DelegateCommand<TrainingSetTemplate>(RemoveTemplate);
            this.EditTemplateCommand = new DelegateCommand<TrainingSetTemplate>(EditTemplate);
            this.CreateRideCommand = new DelegateCommand<TrainingSetTemplate>(CreateRide);
        }

        private void AddTemplate(object arg)
        {
            var view = serviceLocator.GetInstance<ITemplateView>();
            view.Model.NewTemplate();
        }

        private void RemoveTemplate(TrainingSetTemplate arg)
        {
            dialogFacade.Confirm(MyLibraryResources.RemoveTemplateConfirmation, result =>
            {
                if (result)
                {
                    var service = serviceLocator.GetInstance<ITrainingSetTemplateService>();
                    service.TrainingSetTemplateRemoved += (sender, e) =>
                    {
                        switch (e.Value.Status)
                        {
                            case RemoveTemplateStatus.Success:
                                SmartDispatcher.BeginInvoke(() =>
                                {
                                    ApplicationUser.CurrentUser.Templates.Remove(arg);                                    
                                    this.IsBusy = false;
                                    //OnTemplateRemoved();
                                });
                                break;
                            case RemoveTemplateStatus.Error:
                                this.IsBusy = false;
                                dialogFacade.Alert(MyLibraryResources.ErrorRemovingTemplate);
                                break;
                            default:
                                break;
                        }
                    };
                    service.TrainingSetTemplateRemoveError += (sender, e) =>
                    {
                        throw e.Value;
                    };
                    this.IsBusy = true;
                    try
                    {
                        service.RemoveTemplate(arg);
                    }
                    catch
                    {
                        IsBusy = false;
                        throw;
                    }
                }
            });
        }

        private void EditTemplate(TrainingSetTemplate arg)
        {
            if (arg.IsPublic)
            {
                dialogFacade.Alert(MyLibraryResources.NoEditingOfPublicTemplates);
            }
            else
            {
                var view = serviceLocator.GetInstance<ITemplateView>();
                view.Model.EditTemplate(arg);
            }
        }

        private void CreateRide(TrainingSetTemplate arg)
        {
            eventAggregator.GetEvent<DesignEvent>().Publish(arg);
        }
        #region ITemplatesViewPresentationModel Members

        public ITemplatesView View { get; set; }

        public void Refresh() 
        {
        }

        #endregion

        public string NumberOfTemplatesLabel
        {
            get { return string.Format(Resources.MyLibraryResources.NumberOfTemplatesLabel, ApplicationContext.Current.TemplateCount); }
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

        public ICommand AddTemplateCommand { get; private set; }

        public ICommand CreateRideCommand { get; private set; }

        public ICommand EditTemplateCommand { get; private set; }

        public ICommand RemoveTemplateCommand { get; private set; }
    }
}

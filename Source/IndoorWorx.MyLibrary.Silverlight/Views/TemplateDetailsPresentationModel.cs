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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.MyLibrary.Resources;
using IndoorWorx.Infrastructure;
using IndoorWorx.Infrastructure.Helpers;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary.Views
{
    public class TemplateDetailsPresentationModel : BaseModel, ITemplateDetailsPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;
        public TemplateDetailsPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator, IDialogFacade dialogFacade)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
            this.RemoveTemplateCommand = new DelegateCommand<object>(RemoveTemplate);
            this.EditTemplateCommand = new DelegateCommand<object>(EditTemplate);
            this.CreateRideCommand = new DelegateCommand<object>(CreateRide);
        }

        public event EventHandler TemplateRemoved;

        private void OnTemplateRemoved()
        {
            if (TemplateRemoved != null)
                TemplateRemoved(this, EventArgs.Empty);
        }

        private void RemoveTemplate(object arg)
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
                                                ApplicationUser.CurrentUser.Templates.Remove(Template);
                                                this.Template = null;
                                                this.IsBusy = false;
                                                OnTemplateRemoved();
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
                            service.RemoveTemplate(Template);
                        }
                        catch
                        {
                            IsBusy = false;
                            throw;
                        }
                    }
                });
        }

        private void EditTemplate(object arg)
        {
            if (Template.IsPublic)
            {
                dialogFacade.Alert(MyLibraryResources.NoEditingOfPublicTemplates);
            }
            else
            {
                var view = serviceLocator.GetInstance<ITemplateView>();
                view.Model.EditTemplate(Template);
            }
        }

        private void CreateRide(object arg)
        {
            eventAggregator.GetEvent<DesignEvent>().Publish(Template);
        }

        public ITemplateDetailsView View { get; set; }

        public void SelectTemplateWithId(Guid id)
        {
            var template = ApplicationUser.CurrentUser.Templates.FirstOrDefault(x => x.Id == id);
            if (template != null)
            {
                Template = template;
            }
            else
            {
                var templateService = serviceLocator.GetInstance<ITrainingSetTemplateService>();
                templateService.TrainingSetTemplatesRetrieved += (sender, e) =>
                {
                    var templates = e.Value;
                    Template = templates.FirstOrDefault(x => x.Id == id);                    
                    IsBusy = false;
                };
                templateService.TrainingSetTemplateRetrievalError += (sender, e) =>
                {
                    IsBusy = false;
                };
                IsBusy = true;
                templateService.RetrieveTrainingSetTemplates();
            }
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

        private TrainingSetTemplate template;
        public virtual TrainingSetTemplate Template
        {
            get { return template; }
            set
            {
                template = value;
                FirePropertyChanged("Template");
                SmartDispatcher.BeginInvoke(() => EditTemplateCommand.RaiseCanExecuteChanged<object>());
            }
        }

        public ICommand CreateRideCommand { get; private set; }

        public ICommand EditTemplateCommand { get; private set; }

        public ICommand RemoveTemplateCommand { get; private set; }
    }
}

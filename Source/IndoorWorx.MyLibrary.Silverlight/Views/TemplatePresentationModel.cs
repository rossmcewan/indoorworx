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
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Facades;

namespace IndoorWorx.MyLibrary.Views
{
    public class TemplatePresentationModel : BaseModel, ITemplatePresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;
        public TemplatePresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator, IDialogFacade dialogFacade)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
            this.CancelCommand = new DelegateCommand<object>(Cancel);
        }

        private void Cancel(object arg)
        {
        }

        public ITemplateView View { get; set; }

        public ICommand CancelCommand { get; private set; }

        public CrudOperation TemplateOperation { get; private set; }

        public TrainingSetTemplate Template { get; private set; }

        public void NewTemplate()
        {
            this.TemplateOperation = CrudOperation.Create;
            this.Template = new TrainingSetTemplate();
            dialogFacade.Show(View);
        }        
    }
}

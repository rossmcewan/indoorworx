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

namespace IndoorWorx.MyLibrary.Views
{
    public class TemplateDetailsPresentationModel : BaseModel, ITemplateDetailsPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public TemplateDetailsPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
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
            }
        }
    }
}

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
using IndoorWorx.Catalog.Resources;
using IndoorWorx.Infrastructure.Events;
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace IndoorWorx.Catalog.Views
{
    public class TemplatesPresentationModel : BaseModel, ITemplatesPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;

        public TemplatesPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;            
            this.templateDropTarget = new DropTarget(
                (target, payload) =>
                {
                    target.IsBusy = true;
                    ApplicationUser.CurrentUser.AddTemplateToLibrary(payload as TrainingSetTemplate, () => target.IsBusy = false);
                },
                (target, payload) => payload is TrainingSetTemplate, (target) => ApplicationContext.Current.TemplateCount)
            {
                Id = Guid.NewGuid(),
                Image = new Uri("/IndoorWorx.Catalog.Silverlight;component/Images/templates.png", UriKind.Relative),
                Title = CatalogResources.MyLibraryOfTemplates
            };
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "TemplateCount")
                    this.templateDropTarget.ItemCount = ApplicationContext.Current.TemplateCount;
            };
            this.AddToMyLibraryCommand = new DelegateCommand<TrainingSetTemplate>(AddToMyLibrary);
        }

        private void AddToMyLibrary(TrainingSetTemplate template)
        {
            ApplicationUser.CurrentUser.AddTemplateToLibrary(template, () => { });
        }

        #region ITemplatesViewPresentationModel Members

        private IDropTarget templateDropTarget;
        public IDropTarget TemplateDropTarget
        {
            get { return this.templateDropTarget; }
            set
            {
                this.templateDropTarget = value;
                FirePropertyChanged("TemplateDropTarget");
            }
        }

        public ITemplatesView View { get; set; }

        public void Refresh() 
        {
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            IsBusy = true;
            var service = serviceLocator.GetInstance<ITrainingSetTemplateService>();
            service.TrainingSetTemplateRetrievalError += (sender, e) =>
                {
                    IsBusy = false;
                    throw e.Value;
                };
            service.TrainingSetTemplatesRetrieved += (sender, e) =>
                {
                    Templates = e.Value;
                    foreach (var template in Templates)
                        template.LoadTelemetry();
                    IsBusy = false;
                };
            service.RetrieveTrainingSetTemplates();
        }

        private ICollection<TrainingSetTemplate> templates = new List<TrainingSetTemplate>();
        public virtual ICollection<TrainingSetTemplate> Templates
        {
            get { return templates; }
            set
            {
                templates = value;
                FirePropertyChanged("Templates");
                FirePropertyChanged("NumberOfTemplatesLabel");
            }
        }

        #endregion

        public void OnMyLibrarySelected()
        {
            eventAggregator.GetEvent<MyLibraryEvent>().Publish(LibraryPart.Templates);
        }

        public string NumberOfTemplatesLabel
        {
            get { return string.Format(Resources.CatalogResources.NumberOfTemplatesLabel, Templates.Count); }
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

        public ICommand AddToMyLibraryCommand { get; private set; }
    }
}

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
                    //ApplicationUser.CurrentUser.AddVideoToLibrary(payload as Video, () => target.IsBusy = false);
                },
                (target, payload) => false, (target) => 0)
            {
                Id = Guid.NewGuid(),
                Image = new Uri("/IndoorWorx.Catalog.Silverlight;component/Images/templates.png", UriKind.Relative),
                Title = CatalogResources.MyLibraryOfTemplates
            };
            //ApplicationContext.Current.PropertyChanged += (sender, e) =>
            //{
            //    if (e.PropertyName == "TemplateCount")
            //        this.templateDropTarget.ItemCount = ApplicationContext.Current.TemplateCount;
            //};
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

        public void Refresh() { }

        #endregion
    }
}

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
using IndoorWorx.Infrastructure.Navigation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Designer.Events;

namespace IndoorWorx.Designer.Navigation
{
    public class UseSelectedVideoMenuItem : IMenuItem
    {
        private readonly IEventAggregator eventAggregator;
        public UseSelectedVideoMenuItem(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.command = new DelegateCommand<Video>(UseSelectedVideo);
        }

        public void UseSelectedVideo(Video video)
        {
            eventAggregator.GetEvent<UseSelectedVideoEvent>().Publish(video);
        }

        #region IMenuItem Members

        public string Title
        {
            get { return Resources.DesignerResources.UseSelectedTrainingSetMenuItem; }
        }

        private ICommand command;
        public ICommand Command
        {
            get { return this.command; }
        }

        #endregion
    }
}

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
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary.Views
{
    public class MyLibraryPresentationModel : BaseModel, IMyLibraryPresentationModel
    {
        private IEventAggregator eventAggregator;
        public MyLibraryPresentationModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.playVideoCommand = new DelegateCommand<Video>(PlayVideo);
        }

        public void PlayVideo(Video video)
        {
            eventAggregator.GetEvent<PlayVideoEvent>().Publish(video);
        }

        public IMyLibraryView View { get; set; }

        private ICommand playVideoCommand;
        public ICommand PlayVideoCommand
        {
            get { return this.playVideoCommand; }
        }
    }
}

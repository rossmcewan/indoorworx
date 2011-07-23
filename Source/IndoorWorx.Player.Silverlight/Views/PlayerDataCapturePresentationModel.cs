using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Player.Resources;

namespace IndoorWorx.Player.Views
{
    public class PlayerDataCapturePresentationModel : BaseModel, IPlayerDataCapturePresentationModel
    {
        public IPlayerDataCaptureView View { get; set; }

        private Video video;
        public virtual Video Video
        {
            get { return video; }
            set
            {
                video = value;
                FirePropertyChanged("Video");
            }
        }

        public string Title
        {
            get { return PlayerResources.PlayerDataCaptureTitle; }
        }
    }
}

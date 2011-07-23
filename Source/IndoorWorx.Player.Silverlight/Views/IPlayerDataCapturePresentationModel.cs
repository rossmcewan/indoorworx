using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Player.Views
{
    public interface IPlayerDataCapturePresentationModel
    {
        IPlayerDataCaptureView View { get; set; }

        Video Video { get; set; }
    }
}

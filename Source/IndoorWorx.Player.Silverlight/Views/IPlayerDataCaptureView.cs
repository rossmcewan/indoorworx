using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Player.Views
{
    public interface IPlayerDataCaptureView
    {
        IPlayerDataCapturePresentationModel Model { get; }

        void Show(Action ok, Action cancel);

        void Hide();
    }
}

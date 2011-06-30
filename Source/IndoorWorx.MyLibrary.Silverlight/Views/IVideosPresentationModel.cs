using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Infrastructure.Models;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IVideosPresentationModel
    {
        IVideosView View { get; set; }

        ICommand PlayVideoCommand { get; }

        string NumberOfVideosLabel { get; }

        void Refresh();
    }
}

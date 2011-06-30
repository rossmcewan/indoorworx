using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IVideosPresentationModel
    {
        IVideosView View { get; set; }

        ICollection<Category> Categories { get; set; }

        Category SelectedCategory { get; set; }

        string NumberOfVideosLabel { get; }

        void Refresh();
    }
}

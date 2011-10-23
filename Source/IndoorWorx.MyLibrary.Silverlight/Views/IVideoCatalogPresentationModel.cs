using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IVideoCatalogPresentationModel
    {
        ICommand AddVideoCommand { get; }

        IVideoCatalogView View { get; set; }

        void Refresh();

        IVideoCatalogPresentationModel FilterVideosBy(string filter);

        IVideoCatalogPresentationModel OrderVideosBy(string orderBy);
    }
}

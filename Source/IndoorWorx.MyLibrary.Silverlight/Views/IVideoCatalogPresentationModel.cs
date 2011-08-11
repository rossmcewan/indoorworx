using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IVideoCatalogPresentationModel
    {
        IVideoCatalogView View { get; set; }

        void Refresh();

        IVideoCatalogPresentationModel FilterVideosBy(string filter);

        IVideoCatalogPresentationModel OrderVideosBy(string orderBy);
    }
}

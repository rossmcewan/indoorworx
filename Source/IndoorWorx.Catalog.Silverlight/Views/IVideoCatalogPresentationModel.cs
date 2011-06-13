using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Catalog.Views
{
    public interface IVideoCatalogPresentationModel
    {
        IVideoCatalogView View { get; set; }

        void FilterVideosBy(string filter);

        void Refresh();
    }
}

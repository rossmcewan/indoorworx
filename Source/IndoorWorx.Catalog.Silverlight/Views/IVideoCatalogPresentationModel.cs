using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Catalog.Views
{
    public interface IVideoCatalogPresentationModel
    {
        IVideoCatalogView View { get; set; }

        string NumberOfVideosLabel { get; }

        ICollection<Category> FilteredCategories { get; }

        ICollection<Video> AllFilteredVideos { get; }

        string OrderBy { get; }

        IVideoCatalogPresentationModel FilterVideosBy(string filter);

        IVideoCatalogPresentationModel OrderVideosBy(string orderBy);

        void Refresh();
    }
}

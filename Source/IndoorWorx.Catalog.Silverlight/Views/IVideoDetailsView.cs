using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Catalog.Views
{
    public interface IVideoDetailsView
    {
        IVideoDetailsPresentationModel Model { get; }
    }
}

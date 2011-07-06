using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IVideoDetailsPresentationModel
    {
        ICommand PreviewVideoCommand { get; }

        IVideoDetailsView View { get; set; }

        void SelectVideoWithId(Guid id);
    }
}

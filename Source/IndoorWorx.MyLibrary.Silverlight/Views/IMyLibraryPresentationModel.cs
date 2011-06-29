using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IMyLibraryPresentationModel
    {
        IMyLibraryView View { get; set; }

        ICommand PlayVideoCommand { get; }
    }
}

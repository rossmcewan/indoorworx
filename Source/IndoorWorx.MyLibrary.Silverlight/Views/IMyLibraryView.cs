using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IMyLibraryView
    {
        IMyLibraryPresentationModel Model { get; }
    }
}

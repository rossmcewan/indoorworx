using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITrainingSetDetailsView
    {
        ITrainingSetDetailsPresentationModel Model { get; }
    }
}

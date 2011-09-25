using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Trainers.Views
{
    public interface ISelectTrainerView
    {
        ISelectTrainerPresentationModel Model { get; }
    }
}

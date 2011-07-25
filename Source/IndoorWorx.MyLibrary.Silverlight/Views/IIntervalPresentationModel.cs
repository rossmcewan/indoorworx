using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.MyLibrary.Views
{
    public interface IIntervalPresentationModel
    {
        CrudOperation Mode { get; set; }

        Interval Interval { get; set; }

        IIntervalView View { get; set; }

        string Title { get; }

        void OnAccepted(Action accepted);

        void OnCancelled(Action cancelled);
    }
}

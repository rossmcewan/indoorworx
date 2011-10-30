using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITemplateView
    {
        ITemplatePresentationModel Model { get; }

        //Interval CurrentInterval { get; }
        //IEnumerable<Interval> SelectedIntervals { get; }
    }
}

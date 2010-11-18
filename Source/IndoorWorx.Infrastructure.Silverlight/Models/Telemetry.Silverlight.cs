using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Telemetry
    {
        public virtual DateTime TimePositionAsDateTime
        {
            get
            {
                var today = DateTime.Today;
                return new DateTime(today.Year, today.Month, today.Day, TimePosition.Hours, TimePosition.Minutes, TimePosition.Seconds);
            }
        }
    }
}

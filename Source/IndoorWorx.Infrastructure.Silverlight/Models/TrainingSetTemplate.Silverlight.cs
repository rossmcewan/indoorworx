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
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class TrainingSetTemplate
    {
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.Intervals == null)
                this.Intervals = new ObservableCollection<Interval>();
        }
    }
}

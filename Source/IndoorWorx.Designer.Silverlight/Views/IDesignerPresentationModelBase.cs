using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using System.Windows.Input;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerPresentationModelBase
    {       
        TimeSpan VideoFrom { get; set; }

        TimeSpan VideoTo { get; set; }

        Video Video { get; set; }

        ICollection<Video> SelectableVideos { get; }
    }
}

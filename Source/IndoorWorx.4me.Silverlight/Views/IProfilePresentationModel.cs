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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.ForMe.Views
{
    public interface IProfilePresentationModel
    {
        IProfileView View { get; set; }

        ApplicationUser User { get; set; }
       
        bool IsBusy { get; set; }

    }
}

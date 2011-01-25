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
    public partial class Colour
    {
        public SolidColorBrush Brush
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(alpha, Red, Green, Blue));
            }
            set
            {
                this.Alpha = value.Color.A;
                this.Blue = value.Color.B;
                this.Red = value.Color.R;
                this.Green = value.Color.G;
            }
        }
    }
}

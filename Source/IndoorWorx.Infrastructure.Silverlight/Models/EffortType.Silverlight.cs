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
    public partial class EffortType
    {
        public static readonly string HRTag = "HR";

        public static readonly string PowerTag = "POWER";

        public static readonly string RPETag = "RPE";

        public bool IsHR
        {
            get { return Tag == HRTag; }
        }

        public bool IsPower
        {
            get { return Tag == PowerTag; }
        }

        public bool IsRPE
        {
            get { return Tag == RPETag; }
        }
    }
}

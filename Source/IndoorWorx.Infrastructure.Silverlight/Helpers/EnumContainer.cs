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

namespace IndoorWorx.Infrastructure.Helpers
{
    public sealed class EnumContainer
    {
        public int EnumValue { get; set; }
        public string EnumDescription { get; set; }
        public object EnumOriginalValue { get; set; }
        public override string ToString()
        {
            return EnumDescription;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return EnumValue.Equals((int)obj);
        }

        public override int GetHashCode()
        {
            return EnumValue.GetHashCode();
        }

    }
}

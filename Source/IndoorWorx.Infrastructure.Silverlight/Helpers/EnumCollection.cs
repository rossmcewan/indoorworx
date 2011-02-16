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
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace IndoorWorx.Infrastructure.Helpers
{
    public class EnumCollection<T> : List<EnumContainer> where T : struct
    {
        public EnumCollection()
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("This class only supports Enum types");
            var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                var container = new EnumContainer();
                container.EnumOriginalValue = field.GetValue(null);
                container.EnumValue = (int)field.GetValue(null);
                container.EnumDescription = field.Name;
                var atts = field.GetCustomAttributes(false);
                foreach (var att in atts)
                    if (att is DescriptionAttribute)
                    {
                        container.EnumDescription = ((DescriptionAttribute)att).Description;
                        break;
                    }
                Add(container);
            }

        }
    }
}

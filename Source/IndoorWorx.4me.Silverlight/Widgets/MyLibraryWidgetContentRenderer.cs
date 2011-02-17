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
using IndoorWorx.Infrastructure.Widgets;

namespace IndoorWorx.ForMe.Widgets
{
    public class MyLibraryWidgetContentRenderer : IWidgetContentRenderer
    {
        #region IWidgetContentRenderer Members

        public object Render()
        {
            return new TextBlock() { Text = "My Library" };
        }

        #endregion
    }
}

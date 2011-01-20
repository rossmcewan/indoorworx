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
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Model;

namespace IndoorWorx.Library.Controls
{
    public class PageViewModel
    {
        private ImageSource image;
        private string name;

        public ImageSource Image
        {
            get { return image; }
            set { image = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Text { get; set; }
    }
}

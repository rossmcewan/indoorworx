using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IndoorWorx.MyLibrary.Views
{
    public partial class MyLibraryView : UserControl, IMyLibraryView
    {
        public MyLibraryView(IMyLibraryPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public IMyLibraryPresentationModel Model
        {
            get { return this.DataContext as IMyLibraryPresentationModel; }
        }

        private void MyLibraryContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UpdateSelectedLinks(e.Uri);
        }

        private void UpdateSelectedLinks(Uri uri)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    var uriString = uri.ToString();
                    var compareTo = uriString;
                    if (uriString.Contains('?'))
                        compareTo = uriString.Substring(0, uriString.IndexOf('?'));
                    if (hb.NavigateUri.ToString().Equals(compareTo))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        public Uri VideosPage
        {
            get { return new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/VideosPage.xaml", UriKind.Relative); }
        }

        public Uri TemplatesPage
        {
            get { return new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/TemplatesPage.xaml", UriKind.Relative); }
        }

        public Uri ProgramsPage
        {
            get { return new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/TrainingProgramsPage.xaml", UriKind.Relative); }
        }

        public void NavigateToLibraryPart(Infrastructure.Events.LibraryPart libraryPartEnum)
        {
            switch (libraryPartEnum)
            {
                case IndoorWorx.Infrastructure.Events.LibraryPart.Videos:
                    this.MyLibraryContentFrame.Navigate(VideosPage);
                    break;
                case IndoorWorx.Infrastructure.Events.LibraryPart.Templates:
                    this.MyLibraryContentFrame.Navigate(TemplatesPage);
                    break;
                case IndoorWorx.Infrastructure.Events.LibraryPart.Programs:
                    this.MyLibraryContentFrame.Navigate(ProgramsPage);
                    break;
                default:
                    break;
            }
        }
    }
}

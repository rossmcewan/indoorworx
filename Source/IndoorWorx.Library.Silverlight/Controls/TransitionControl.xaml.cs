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
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls.TransitionEffects;

namespace IndoorWorx.Library.Controls
{
    public partial class TransitionControl : UserControl
    {
        private PagerViewModel pagerViewModel;

        public TransitionControl()
        {
            InitializeComponent();
            this.radTransitionControl.Transition = new MotionBlurredZoomTransition();
            this.pagerViewModel = new PagerViewModel();
            this.pagerViewModel.Pages = this.LoadPages();
            this.Pager.DataContext = this.pagerViewModel;
        }

        private IEnumerable<PageViewModel> LoadPages()
        {
            return new List<PageViewModel>()
            {
                new PageViewModel() { Name = "1", Image = new BitmapImage(new Uri("/IndoorWorx.Silverlight;component/Images/suncity.jpg", UriKind.Relative)), Text = "We all know how boring indoor training can be, yet it is a necessary part of most people training regimes.  Be it because of the weather, the traffic or just the necessity to get a quality session done in a controlled environment."},
                new PageViewModel() { Name = "2", Image = new BitmapImage(new Uri("/IndoorWorx.Silverlight;component/Images/tri1.jpg", UriKind.Relative)), Text="Indoor training works, but how do we make it more enjoyable, and overcome the boredom to push ourselves to new limits. IndoorWorx. That’s how."},
                new PageViewModel() { Name = "3", Image = new BitmapImage(new Uri("/IndoorWorx.Silverlight;component/Images/tri2.jpg", UriKind.Relative)), Text="IndoorWorx is an online video streaming application that streams training videos from the internet, to be used whilst training indoors. Sessions can range from group training sets, intervals sessions, time trials, triathlons, track sessions, the list is endless.  Power and/or HR profiles will show the levels that you should be working in."},
                new PageViewModel() { Name = "4", Image = new BitmapImage(new Uri("/IndoorWorx.Silverlight;component/Images/tri3.jpg", UriKind.Relative)), Text="If a computrainer is your tool of choice, we can provide a means to automatically adjust the tension of your trainer, so all that is required from you is to turns the pedals. Easier said than done!"},
                new PageViewModel() { Name = "5", Image = new BitmapImage(new Uri("/IndoorWorx.Silverlight;component/Images/suikerbosrand.jpg", UriKind.Relative)), Text="Experience Indoor Training like never before."}
            };
        }

        private void ButtonLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.pagerViewModel.SelectPrev();
        }

        private void ButtonRight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.pagerViewModel.SelectNext();
        }
    }
}

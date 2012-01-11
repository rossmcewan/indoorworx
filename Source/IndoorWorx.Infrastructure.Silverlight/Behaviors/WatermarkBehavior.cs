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
using System.Windows.Interactivity;

namespace IndoorWorx.Infrastructure.Behaviors
{
    public class Watermark : Behavior<TextBox>
    {
        private bool _hasWatermark;
        private Brush _textBoxForeground;

        //public String Text { get; set; }
        public Brush Foreground { get; set; }

        protected override void OnAttached()
        {
            _textBoxForeground = AssociatedObject.Foreground;

            base.OnAttached();
            if (Text != null)
                SetWatermarkText();
            AssociatedObject.Loaded += Loaded;
            AssociatedObject.GotFocus += GotFocus;
            AssociatedObject.LostFocus += LostFocus;
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (_hasWatermark)
                SetWatermarkText();
            else
                RemoveWatermarkText();
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Text.Length == 0)
                if (Text != null)
                    SetWatermarkText();
        }

        private void GotFocus(object sender, RoutedEventArgs e)
        {
            if (_hasWatermark)
                RemoveWatermarkText();
        }

        private void RemoveWatermarkText()
        {
            AssociatedObject.Foreground = _textBoxForeground;
            AssociatedObject.Text = "";
            _hasWatermark = false;
            SetIsWatermarked(AssociatedObject, _hasWatermark);
        }

        private void SetWatermarkText()
        {
            AssociatedObject.Foreground = Foreground;
            AssociatedObject.Text = Text;
            _hasWatermark = true;
            SetIsWatermarked(AssociatedObject, _hasWatermark);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= GotFocus;
            AssociatedObject.LostFocus -= LostFocus;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                   "Text",
                   typeof(string),
                   typeof(Watermark),
                   new PropertyMetadata("Search..."));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static bool GetIsWatermarked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWatermarkedProperty);
        }

        public static void SetIsWatermarked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWatermarkedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsWatermarked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsWatermarkedProperty =
            DependencyProperty.RegisterAttached("IsWatermarked", typeof(bool), typeof(TextBox), new PropertyMetadata(false));


    }
}

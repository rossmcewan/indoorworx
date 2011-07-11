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
using System.Collections;
using System.ComponentModel;

namespace IndoorWorx.MyLibrary.Controls
{
    public partial class IntervalsCaptureControl : UserControl, INotifyPropertyChanged
    {
        public IntervalsCaptureControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public IEnumerable IntervalTypes
        {
            get { return (IEnumerable)GetValue(IntervalTypesProperty); }
            set { SetValue(IntervalTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IntervalTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntervalTypesProperty =
            DependencyProperty.Register("IntervalTypes", typeof(IEnumerable), typeof(IntervalsCaptureControl), new PropertyMetadata(null, IntervalTypesChanged));

        private static void IntervalTypesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (IntervalsCaptureControl)obj;
            control.FirePropertyChanged("IntervalTypes");
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(IntervalsCaptureControl), new PropertyMetadata(null, ItemsSourceChanged));

        private static void ItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (IntervalsCaptureControl)obj;
            control.IntervalItems.ItemsSource = control.ItemsSource;
        }

        public ICommand AddIntervalCommand
        {
            get { return (ICommand)GetValue(AddIntervalCommandProperty); }
            set { SetValue(AddIntervalCommandProperty, value); }
        }

        public static readonly DependencyProperty AddIntervalCommandProperty =
            DependencyProperty.Register("AddIntervalCommand", typeof(ICommand), typeof(IntervalsCaptureControl), new PropertyMetadata(null, AddIntervalCommandChanged));

        private static void AddIntervalCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (IntervalsCaptureControl)obj;
            control.FirePropertyChanged("AddIntervalCommand");
        }

        public ICommand RemoveIntervalCommand
        {
            get { return (ICommand)GetValue(RemoveIntervalCommandProperty); }
            set { SetValue(RemoveIntervalCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveIntervalCommandProperty =
            DependencyProperty.Register("RemoveIntervalCommand", typeof(ICommand), typeof(IntervalsCaptureControl), new PropertyMetadata(null, RemoveIntervalCommandChanged));

        private static void RemoveIntervalCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (IntervalsCaptureControl)obj;
            control.FirePropertyChanged("RemoveIntervalCommand");
        }

        public ICommand MoveUpCommand
        {
            get { return (ICommand)GetValue(MoveUpCommandProperty); }
            set { SetValue(MoveUpCommandProperty, value); }
        }

        public static readonly DependencyProperty MoveUpCommandProperty =
            DependencyProperty.Register("MoveUpCommand", typeof(ICommand), typeof(IntervalsCaptureControl), new PropertyMetadata(null, MoveUpCommandChanged));

        private static void MoveUpCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (IntervalsCaptureControl)obj;
            control.FirePropertyChanged("MoveUpCommand");
        }

        public ICommand MoveDownCommand
        {
            get { return (ICommand)GetValue(MoveDownCommandProperty); }
            set { SetValue(MoveDownCommandProperty, value); }
        }

        public static readonly DependencyProperty MoveDownCommandProperty =
            DependencyProperty.Register("MoveDownCommand", typeof(ICommand), typeof(IntervalsCaptureControl), new PropertyMetadata(null, MoveDownCommandChanged));

        private static void MoveDownCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (IntervalsCaptureControl)obj;
            control.FirePropertyChanged("MoveDownCommand");
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }               
    }
}

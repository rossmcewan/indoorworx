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
using IndoorWorx.Infrastructure;
using Telerik.Windows.Data;
using IndoorWorx.Infrastructure.Enums;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.MyLibrary.Views
{
    public partial class TemplateView : UserControl, ITemplateView
    {
        private readonly IShell shell;
        public TemplateView(ITemplatePresentationModel model, IShell shell)
        {
            this.shell = shell;
            this.DataContext = model;
            InitializeComponent();
            model.View = this;

            var animations = EnumDataSource.FromType<VideoTextAnimations>();
            var animationColumn = textGridView.Columns[5] as GridViewComboBoxColumn;
            animationColumn.ItemsSource = animations;
        }

        public ITemplatePresentationModel Model
        {
            get { return this.DataContext as ITemplatePresentationModel; }
        }

        private void timeBar_Loaded(object sender, RoutedEventArgs e)
        {
            Model.Template.ReloadTelemetry();
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if(Model.EditIntervalCommand.CanExecute(element.DataContext))
                Model.EditIntervalCommand.Execute(element.DataContext);
        }

        private void RadContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            GridViewRow row = menu.GetClickedElement<GridViewRow>();
            
            if (row != null)
            {
                row.IsSelected = row.IsCurrent = true;
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {
                    cell.IsCurrent = true;
                }
            }
            else
            {
                menu.IsOpen = false;
            }

        }

        //public Interval CurrentInterval
        //{
        //    get
        //    {
        //        return intervalGridView.CurrentItem as Interval;
        //    }
        //}

        //public IEnumerable<Interval> SelectedIntervals
        //{
        //    get { return intervalGridView.SelectedItems.Cast<Interval>(); }
        //}
    }
}

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
            var animationColumn = textGridView.Columns[4] as GridViewComboBoxColumn;
            animationColumn.ItemsSource = animations;
        }

        public ITemplatePresentationModel Model
        {
            get { return this.DataContext as ITemplatePresentationModel; }
        }
    }
}

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
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IndoorWorx.Infrastructure.Behaviors
{
    public class SearchTextAwareBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            IoC.Resolve<IEventAggregator>().GetEvent<SearchTextChangedEvent>().Subscribe(SearchTextChanged, ThreadOption.UIThread, true);
        }

        protected override void OnDetaching()
        {
            IoC.Resolve<IEventAggregator>().GetEvent<SearchTextChangedEvent>().Unsubscribe(SearchTextChanged);
        }

        private void SearchTextChanged(string searchText)
        {
            var model = AssociatedObject.DataContext;
            if (model is ISearchable)
            {
                var validSearch = (model as ISearchable).IsValid(searchText);
                if (validSearch)
                {
                    AssociatedObject.Visibility = Visibility.Visible;
                }
                else
                {
                    AssociatedObject.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}

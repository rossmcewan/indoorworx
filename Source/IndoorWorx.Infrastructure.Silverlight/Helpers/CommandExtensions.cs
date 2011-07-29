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
using Microsoft.Practices.Composite.Presentation.Commands;

namespace IndoorWorx.Infrastructure.Helpers
{
    public static class CommandExtensions
    {
        public static void RaiseCanExecuteChanged<T>(this ICommand command)
        {
            if (command is DelegateCommand<T>)
                (command as DelegateCommand<T>).RaiseCanExecuteChanged();
        }
    }
}

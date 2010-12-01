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
using IndoorWorx.Infrastructure.Facades;
using Telerik.Windows.Controls;

namespace IndoorWorx.Library.Facades
{
    public class DialogFacade : IDialogFacade
    {
        #region IDialogFacade Members

        public void Alert(object content)
        {
            RadWindow.Alert(content);
        }

        public void Confirm(object content, Action<bool> closed)
        {
            RadWindow.Confirm(content, (sender, args) => closed(args.DialogResult.GetValueOrDefault()));
        }

        #endregion
    }
}

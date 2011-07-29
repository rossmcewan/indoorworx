using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Designer.Views
{
    public interface IDesignerView
    {
        IDesignerPresentationModel Model { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Catalog.Views
{
    public interface ITemplateDetailsPresentationModel
    {
        ITemplateDetailsView View { get; set; }

        void SelectTemplateWithId(Guid guid);
    }
}

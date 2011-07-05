using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Catalog.Views
{
    public interface ITemplateDetailsPresentationModel
    {
        ITemplateDetailsView View { get; set; }

        void SelectTemplateWithId(Guid guid);

        bool IsBusy { get; set; }

        TrainingSetTemplate Template { get; set; }
    }
}

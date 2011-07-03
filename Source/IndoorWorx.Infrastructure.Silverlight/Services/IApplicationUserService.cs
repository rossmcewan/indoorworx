using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Responses;

namespace IndoorWorx.Infrastructure.Services
{
    public interface IApplicationUserService
    {
        event EventHandler<DataEventArgs<ApplicationUser>> ApplicationUserSaved;

        event EventHandler<DataEventArgs<Exception>> ApplicationUserSaveError;

        void SaveApplicationUser(ApplicationUser user);

        event EventHandler<DataEventArgs<ApplicationUser>> ApplicationUserRetrieved;

        event EventHandler<DataEventArgs<Exception>> ApplicationUserRetrievalError;

        void RetrieveApplicationUser(string username);

        event EventHandler<DataEventArgs<ICollection<Occupation>>> OccupationsRetrieved;

        event EventHandler<DataEventArgs<Exception>> OccupationsRetrievalError;

        void RetrievOccupations();

        event EventHandler<DataEventArgs<ICollection<ReferralSource>>> ReferralSourcesRetrieved;

        event EventHandler<DataEventArgs<Exception>> ReferralSourcesRetrievalError;

        void RetrievReferralSources();

        event EventHandler<DataEventArgs<AddVideoResponse>> AddVideoCompleted;

        event EventHandler<DataEventArgs<Exception>> AddVideoError;

        void AddVideoToLibrary(Video video);

        event EventHandler<DataEventArgs<AddTemplateResponse>> AddTemplateCompleted;

        event EventHandler<DataEventArgs<Exception>> AddTemplateError;

        void AddTemplateToLibrary(TrainingSetTemplate template);
    }
}

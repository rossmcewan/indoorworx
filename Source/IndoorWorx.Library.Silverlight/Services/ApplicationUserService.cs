using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure;
using IndoorWorx.Library.ApplicationUserServiceReference;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Responses;
using IndoorWorx.Infrastructure.Requests;

namespace IndoorWorx.Library.Services
{
    public class ApplicationUserService : IndoorWorx.Infrastructure.Services.IApplicationUserService
    {
        private readonly IServiceLocator serviceLocator;
        public ApplicationUserService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
        }


        private ApplicationUserServiceClient proxy = new ApplicationUserServiceClient();
        public ApplicationUserServiceClient Proxy
        {
            get { return proxy; }
        }

        #region Occupation Methods

        public event EventHandler<DataEventArgs<ICollection<Occupation>>> OccupationsRetrieved;

        public event EventHandler<DataEventArgs<Exception>> OccupationsRetrievalError;

        public void RetrievOccupations()
        {
            var occupation = Cache.Get("Occupations") as ICollection<Occupation>;
            if (occupation != null)
            {
                if (OccupationsRetrieved != null)
                    OccupationsRetrieved(this, new DataEventArgs<ICollection<Occupation>>(occupation));
            }
            else
            {
                Proxy.RetrieveAllOccupationsCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (OccupationsRetrievalError != null)
                            OccupationsRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (OccupationsRetrieved != null)
                            OccupationsRetrieved(this, new DataEventArgs<ICollection<Occupation>>(e.Result));
                    }
                };
                proxy.RetrieveAllOccupationsAsync();
            }
        }

        #endregion

        #region ReferralSource Methods

        public event EventHandler<DataEventArgs<ICollection<ReferralSource>>> ReferralSourcesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> ReferralSourcesRetrievalError;

        public void RetrievReferralSources()
        {
            var referralSource = Cache.Get("ReferralSources") as ICollection<ReferralSource>;
            if (referralSource != null)
            {
                if (ReferralSourcesRetrieved != null)
                    ReferralSourcesRetrieved(this, new DataEventArgs<ICollection<ReferralSource>>(referralSource));
            }
            else
            {
                Proxy.RetrieveAllReferralSourcesCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (ReferralSourcesRetrievalError != null)
                            ReferralSourcesRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (ReferralSourcesRetrieved != null)
                            ReferralSourcesRetrieved(this, new DataEventArgs<ICollection<ReferralSource>>(e.Result));
                    }
                };
                proxy.RetrieveAllOccupationsAsync();
            }
        }

        #endregion

        #region ApplicationUser Methods

        public event EventHandler<DataEventArgs<ApplicationUser>> ApplicationUserSaved;

        public event EventHandler<DataEventArgs<Exception>> ApplicationUserSaveError;

        public void SaveApplicationUser(ApplicationUser user)
        {
            Proxy.SaveApplicationUserCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (ApplicationUserSaveError != null)
                        ApplicationUserSaveError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (ApplicationUserSaved != null)
                        ApplicationUserSaved(this, new DataEventArgs<ApplicationUser>(e.Result));
                }
            };
            proxy.SaveApplicationUserAsync(user);
        }

        public event EventHandler<DataEventArgs<ApplicationUser>> ApplicationUserRetrieved;

        public event EventHandler<DataEventArgs<Exception>> ApplicationUserRetrievalError;

        public void RetrieveApplicationUser(string username)
        {
            Proxy.RetrieveApplicationUserCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (ApplicationUserRetrievalError != null)
                        ApplicationUserRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (ApplicationUserRetrieved != null)
                        ApplicationUserRetrieved(this, new DataEventArgs<ApplicationUser>(e.Result));
                }
            };
            proxy.RetrieveApplicationUserAsync(new ApplicationUserFindCriteria() { Username = username } );
        }

        public event EventHandler<DataEventArgs<AddVideoResponse>> AddVideoCompleted;

        public event EventHandler<DataEventArgs<Exception>> AddVideoError;

        public void AddVideoToLibrary(Video video)
        {
            Proxy.AddVideoToLibraryCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (AddVideoError != null)
                            AddVideoError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (AddVideoCompleted != null)
                            AddVideoCompleted(this, new DataEventArgs<AddVideoResponse>(e.Result));
                    }
                };
            Proxy.AddVideoToLibraryAsync(new AddVideoRequest()
            {
                User = ApplicationUser.CurrentUser.Username,
                VideoId = video.Id
            });
        }

        #endregion
    }
}

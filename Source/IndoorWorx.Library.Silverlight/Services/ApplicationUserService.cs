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
using System.ServiceModel;

namespace IndoorWorx.Library.Services
{
    public class ApplicationUserService : IndoorWorx.Infrastructure.Services.IApplicationUserService
    {
        private readonly IServiceLocator serviceLocator;
        private readonly Uri serviceUri;
        public ApplicationUserService(IServiceLocator serviceLocator, IConfigurationService configurationService)
        {
            this.serviceLocator = serviceLocator;
            this.serviceUri = new Uri(configurationService.GetParameterValue("ApplicationUserServiceUri"), UriKind.Absolute);
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
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
                return;
            }
            else
            {
                var proxy = CreateApplicationUserServiceClient();
                proxy.RetrieveAllOccupationsCompleted += (sender, e) =>
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
                return;
            }
            else
            {
                var proxy = CreateApplicationUserServiceClient();
                proxy.RetrieveAllReferralSourcesCompleted += (sender, e) =>
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
            var proxy = CreateApplicationUserServiceClient();
            proxy.SaveApplicationUserCompleted += (sender, e) =>
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
            var proxy = CreateApplicationUserServiceClient();
            proxy.RetrieveApplicationUserCompleted += (sender, e) =>
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
            var proxy = CreateApplicationUserServiceClient();
            proxy.AddVideoToLibraryCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (AddVideoError != null)
                            AddVideoError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (AddVideoCompleted != null)
                        {
                            if (e.Result.AddVideoStatus == AddVideoStatus.Success)
                                ApplicationUser.CurrentUser.Credits = e.Result.Credits;
                            AddVideoCompleted(this, new DataEventArgs<AddVideoResponse>(e.Result));
                        }
                    }
                };
            proxy.AddVideoToLibraryAsync(new AddVideoRequest()
            {
                User = ApplicationUser.CurrentUser.Username,
                VideoId = video.Id
            });
        }

        #endregion

        public event EventHandler<DataEventArgs<AddTemplateResponse>> AddTemplateCompleted;

        public event EventHandler<DataEventArgs<Exception>> AddTemplateError;

        public void AddTemplateToLibrary(TrainingSetTemplate template)
        {
            var proxy = CreateApplicationUserServiceClient();
            proxy.AddTemplateToLibraryCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (AddTemplateError != null)
                        AddTemplateError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (AddTemplateCompleted != null)
                    {
                        if (e.Result.AddTemplateStatus == AddTemplateStatus.Success)
                            ApplicationUser.CurrentUser.Credits = e.Result.Credits;
                        AddTemplateCompleted(this, new DataEventArgs<AddTemplateResponse>(e.Result));
                    }
                }
            };
            proxy.AddTemplateToLibraryAsync(new AddTemplateRequest()
            {
                User = ApplicationUser.CurrentUser.Username,
                TemplateId = template.Id
            });
        }

        public event EventHandler<DataEventArgs<PlayVideoResponse>> PlayVideoCompleted;

        public event EventHandler<DataEventArgs<Exception>> PlayVideoError;

        public void PlayVideo(Video video)
        {
            var proxy = CreateApplicationUserServiceClient();
            proxy.PlayVideoCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (PlayVideoError != null)
                            PlayVideoError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (PlayVideoCompleted != null)
                        {
                            if (e.Result.Status == PlayVideoStatus.Success)
                                ApplicationUser.CurrentUser.Credits = e.Result.Credits;
                            PlayVideoCompleted(this, new DataEventArgs<PlayVideoResponse>(e.Result));
                        }
                    }
                };
            proxy.PlayVideoAsync(new PlayVideoRequest()
            {
                User = ApplicationUser.CurrentUser.Username,
                VideoId = video.Id
            });
        }

        private ApplicationUserServiceClient CreateApplicationUserServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                Name = "ApplicationUserServiceBinding",
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647,
            };

            EndpointAddress endpointAddress = new EndpointAddress(this.serviceUri);

            return new ApplicationUserServiceClient(binding, endpointAddress);
        }        
    }
}

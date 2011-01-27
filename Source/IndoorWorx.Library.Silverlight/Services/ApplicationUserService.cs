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

namespace IndoorWorx.Library.Services
{
    public class ApplicationUserService : IndoorWorx.Infrastructure.Services.IApplicationUserService
    {
        private readonly IServiceLocator serviceLocator;
        public ApplicationUserService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        private ApplicationUserServiceClient proxy = new ApplicationUserServiceClient();
        public ApplicationUserServiceClient Proxy
        {
            get { return proxy; }
        }

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
    }
}

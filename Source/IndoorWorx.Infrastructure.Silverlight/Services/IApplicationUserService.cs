﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Events;

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
    }
}
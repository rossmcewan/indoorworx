﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Criteria;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface IApplicationUserService
    {
        [OperationContract]
        ApplicationUser SaveApplicationUser(ApplicationUser user);

        [OperationContract]
        ApplicationUser RetrieveApplicationUser(ApplicationUserFindCriteria criteria);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        ICollection<Category> FindAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface IActivityService
    {
        [OperationContract]
        ICollection<ActivityType> RetrieveAllActivityTypes();

        [OperationContract]
        ICollection<Equipment> RetrieveAllEquipment();

        [OperationContract]
        ICollection<EquipmentFeatures> RetrieveAllEquipmentFeatures();

        [OperationContract]
        ICollection<Manufacturer> RetrieveAllManufacturers();
    }
}

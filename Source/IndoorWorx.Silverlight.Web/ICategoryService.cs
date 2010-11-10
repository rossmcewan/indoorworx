using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IndoorWorx.Silverlight.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICategoryService" in both code and config file together.
    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        void DoWork();
    }
}

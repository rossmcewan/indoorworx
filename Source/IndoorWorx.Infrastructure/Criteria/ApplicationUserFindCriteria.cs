using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Criteria
{
    [DataContract(IsReference=true)]
    public class ApplicationUserFindCriteria : BaseModel
    {
        private string username = string.Empty;
        [DataMember]
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                FirePropertyChanged("Username");
            }
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Silverlight.Web
{
    [DataContract(IsReference=true)]
    public class CreateUserResponse : BaseModel
    {
        private CreateUserStatus userStatus;
        [DataMember]
        public CreateUserStatus UserStatus
        {
            get { return userStatus; }
            set
            {
                userStatus = value;
                FirePropertyChanged("UserStatus");
            }
        }

        private ApplicationUser applicationUser;
        [DataMember]
        public ApplicationUser ApplicationUser
        {
            get { return applicationUser; }
            set
            {
                applicationUser = value;
                FirePropertyChanged("ApplicationUser");
            }
        }
    }
}

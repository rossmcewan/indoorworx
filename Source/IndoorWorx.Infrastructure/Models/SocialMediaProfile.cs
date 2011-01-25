using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class SocialMediaProfile : BaseModel
    {
        private SocialMediaType socialMediaType = new SocialMediaType();
        [DataMember]
        public virtual SocialMediaType SocialMediaType
        {
            get { return socialMediaType; }
            set
            {
                socialMediaType = value;
                FirePropertyChanged("SocialMediaType");
            }
        }

        private string username = string.Empty;
        [DataMember]
        public virtual string Username
        {
            get { return username; }
            set
            {
                username = value;
                FirePropertyChanged("Username");
            }
        }

        private string password = string.Empty;
        [DataMember]
        public virtual string Password
        {
            get { return password; }
            set
            {
                password = value;
                FirePropertyChanged("Password");
            }
        }

        private ICollection<SocialMediaNotification> notificationOptions = new List<SocialMediaNotification>();
        [DataMember]
        public virtual ICollection<SocialMediaNotification> NotificationOptions
        {
            get { return notificationOptions; }
            set
            {
                notificationOptions = value;
                FirePropertyChanged("NotificationOptions");
            }
        }
    }
}

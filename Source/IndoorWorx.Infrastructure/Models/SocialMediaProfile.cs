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
        private SocialMediaTypes socialMediaType;
        [DataMember]
        public virtual SocialMediaTypes SocialMediaType
        {
            get { return socialMediaType; }
            set
            {
                socialMediaType = value;
                FirePropertyChanged(" SocialMediaType");
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

        private ICollection<SocialMediaNotificationOptions> notificationOptions = new List<SocialMediaNotificationOptions>();
        [DataMember]
        public virtual ICollection<SocialMediaNotificationOptions> NotificationOptions
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

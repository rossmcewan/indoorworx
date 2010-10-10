using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class NavigationInfo : BaseModel
    {
        private string content;
        public virtual string Content
        {
            get { return content; }
            set
            {
                content = value;
                FirePropertyChanged("Content");
            }
        }

        private string navigationUri;
        public virtual string NavigationUri
        {
            get { return navigationUri; }
            set
            {
                navigationUri = value;
                FirePropertyChanged("NavigationUri");
            }
        }

        private string packageName;
        public virtual string PackageName
        {
            get { return packageName; }
            set
            {
                packageName = value;
                FirePropertyChanged("PackageName");
            }
        }

        private bool authenticationRequired;
        public virtual bool IsAuthenticationRequired
        {
            get { return authenticationRequired; }
            set
            {
                authenticationRequired = value;
                FirePropertyChanged("IsAuthenticationRequired");
            }
        }

        private ICollection<string> allow = new List<string>();
        public virtual ICollection<string> Allow
        {
            get { return allow; }
            set
            {
                allow = value;
                FirePropertyChanged("Roles");
            }
        }

        private ICollection<string> deny = new List<string>();
        public virtual ICollection<string> Deny
        {
            get { return deny; }
            set
            {
                deny = value;
                FirePropertyChanged("Deny");
            }
        }
    }
}

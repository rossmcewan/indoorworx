using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class Video : AuditableModel
    {
        public virtual Guid Id { get; set; }

        private Uri resourceUri;
        public virtual Uri ResourceUri
        {
            get { return resourceUri; }
            set
            {
                resourceUri = value;
                FirePropertyChanged("ResourceUri");
            }
        }

        private ICollection<Telemetry> telemetry = new List<Telemetry>();
        public virtual ICollection<Telemetry> Telemetry
        {
            get { return telemetry; }
            set
            {
                telemetry = value;
                FirePropertyChanged("Telemetry");
            }
        }
    }
}

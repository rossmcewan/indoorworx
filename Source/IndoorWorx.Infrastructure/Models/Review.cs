using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(VideoReview))]
    public partial class Review : AuditableModel
    {
        private int rating;
        [DataMember]
        public virtual int Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                FirePropertyChanged("Rating");
            }
        }

        private string title;
        [DataMember]
        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                FirePropertyChanged("Title");
            }
        }

        private string comment;
        [DataMember]
        public virtual string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                FirePropertyChanged("Comment");
            }
        }
    }
}

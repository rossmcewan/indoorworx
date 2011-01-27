using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class ApplicationUser : BaseModel
    {
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

        private string firstname = string.Empty;
        [DataMember]
        public virtual string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;
                FirePropertyChanged("Firstname");
            }
        }

        private string lastname = string.Empty;
        [DataMember]
        public virtual string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                FirePropertyChanged("Lastname");
            }
        }

        private Gender gender;
        [DataMember]
        public virtual Gender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                FirePropertyChanged("Gender");
            }
        }

        private string email  = string.Empty;
        [DataMember]
        public virtual string Email
        {
            get { return email; }
            set
            {
                email = value;
                FirePropertyChanged("Email");
            }
        }

        private DateTime dateOfBirth;
        [DataMember]
        public virtual DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                FirePropertyChanged("DateOfBirth");
            }
        }

        private Country country;
        [DataMember]
        public virtual Country Country
        {
            get { return country; }
            set
            {
                country = value;
                FirePropertyChanged("Country");
            }
        }

        private ICollection<Activity> activities = new List<Activity>();
        [DataMember]
        public virtual ICollection<Activity> Activities
        {
            get { return activities; }
            set
            {
                activities = value;
                FirePropertyChanged("Activities");
            }
        }

        private ICollection<SocialMediaProfile> socialProfiles = new List<SocialMediaProfile>();
        [DataMember]
        public virtual ICollection<SocialMediaProfile> SocialProfile
        {
            get { return socialProfiles; }
            set
            {
                socialProfiles = value;
                FirePropertyChanged("SocialProfile");
            }
        }

        private static ApplicationUser currentUser = null;
        public static ApplicationUser CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
    }
}

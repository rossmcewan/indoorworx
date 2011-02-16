using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Enums;

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

        private Genders gender;
        [DataMember]
        public virtual Genders Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                FirePropertyChanged("Gender");
            }
        }

        private Occupation occupation = new Occupation();
        [DataMember]
        public virtual Occupation Occupation
        {
            get { return occupation; }
            set
            {
                occupation = value;
                FirePropertyChanged("Occupation");
            }
        }

        private SportingHabits sportingHabits = new SportingHabits();
        [DataMember]
        public virtual SportingHabits SportingHabits
        {
            get { return sportingHabits; }
            set
            {
                sportingHabits = value;
                FirePropertyChanged("SportingHabits");
            }
        }

        private ReferralSource referralSource = new ReferralSource();
        [DataMember]
        public virtual ReferralSource ReferralSource
        {
            get { return referralSource; }
            set
            {
                referralSource = value;
                FirePropertyChanged("ReferralSource");
            }
        }

        private string about = string.Empty;
        [DataMember]
        public virtual string About
        {
            get { return about; }
            set
            {
                about = value;
                FirePropertyChanged("About");
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

        private Countries country;
        [DataMember]
        public virtual Countries Country
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

        private static ApplicationUser currentUser = new ApplicationUser();
        public static ApplicationUser CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
    }
}

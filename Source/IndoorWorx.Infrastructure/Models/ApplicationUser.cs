﻿using System;
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

        private Occupation occupation;
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

        private SportingHabits sportingHabits;
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

        private ReferralSource referralSource;
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

        private int credits;
        [DataMember]
        public virtual int Credits
        {
            get { return credits; }
            set 
            { 
                credits = value;
                FirePropertyChanged("Credits");
            }
        }


        private ICollection<Video> videos = new List<Video>();
        [DataMember]
        public virtual ICollection<Video> Videos
        {
            get { return videos; }
            set
            {
                videos = value;
                FirePropertyChanged("Videos");
            }
        }

        private ICollection<TrainingSetTemplate> templates = new List<TrainingSetTemplate>();
        [DataMember]
        public virtual ICollection<TrainingSetTemplate> Templates
        {
            get { return templates; }
            set
            {
                templates = value;
                FirePropertyChanged("Templates");
            }
        }

        private int? ftp;
        [DataMember]
        public virtual int? FTP
        {
            get { return ftp; }
            set
            {
                ftp = value;
                FirePropertyChanged("FTP");
            }
        }

        private int? fthr;
        [DataMember]
        public virtual int? FTHR
        {
            get { return fthr; }
            set
            {
                fthr = value;
                FirePropertyChanged("FTHR");
            }
        }

        //private ICollection<VideoHistoryItem> videoHistory = new List<VideoHistoryItem>();
        //[DataMember]
        //public virtual ICollection<VideoHistoryItem> VideoHistory
        //{
        //    get { return videoHistory; }
        //    set
        //    {
        //        videoHistory = value;
        //        FirePropertyChanged("VideoHistory");
        //    }
        //}

        public static event EventHandler CurrentUserChanged;

        private static ApplicationUser currentUser = new ApplicationUser();
        public static ApplicationUser CurrentUser
        {
            get { return currentUser; }
            set 
            { 
                currentUser = value;
                OnCurrentUserChanged();
            }
        }

        private static void OnCurrentUserChanged()
        {
            if (CurrentUserChanged != null)
                CurrentUserChanged(null, EventArgs.Empty);
        }
    }
}

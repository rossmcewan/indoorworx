using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Helpers;
using IndoorWorx.Infrastructure.Enums;
using System.Collections.ObjectModel;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class ApplicationUser
    {
        private ICollection<Occupation> occupations = new List<Occupation>();
        [DataMember]
        public ICollection<Occupation> Occupations
        {
            get { return occupations; }
            set
            {
                occupations = value;
                FirePropertyChanged("Occupations");
            }
        }


        private ICollection<ReferralSource> referralSources = new List<ReferralSource>();
        [DataMember]
        public ICollection<ReferralSource> ReferralSources
        {
            get { return referralSources; }
            set
            {
                referralSources = value;
                FirePropertyChanged("ReferralSources");
            }
        }

        private EnumCollection<Countries> countries = new EnumCollection<Countries>();
        public EnumCollection<Countries> Countries
        {
            get
            {
                if (countries == null)
                    countries = new EnumCollection<Countries>();
                return countries;
            }
            set
            {
                countries = value;
                FirePropertyChanged("Countries");
            }
        }

        private EnumCollection<Genders> genders = new EnumCollection<Genders>();
        public EnumCollection<Genders> Genders
        {
            get
            {
                if (genders == null)
                    genders = new EnumCollection<Genders>();
                return genders;
            }
            set
            {
                genders = value;
                FirePropertyChanged("Genders");
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext streamingContext)
        {
            if (this.Videos == null)
                this.Videos = new ObservableCollection<Video>();
            else
                this.Videos = new ObservableCollection<Video>(this.Videos);
        }
    }
}

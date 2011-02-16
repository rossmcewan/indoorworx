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
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;

namespace IndoorWorx.ForMe.Views
{
    public class ActivitiesViewPresentationModel :  BaseModel, IActivitiesViewPresentationModel
    {
        private ICollection<Activity> activites = null;
        public ICollection<Activity> Activities
        {
            get
            {
                if (activites == null)
                {
                    activites = ApplicationUser.CurrentUser.Activities;
                }
                return activites;
            }
            set 
            {
                this.activites = value;
                FirePropertyChanged("Activities");
            }
        }

        private Activity selectedItem = new Activity();
        public Activity SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                FirePropertyChanged("SelectedItem");
            }
        }

        private ICollection<ActivityType> activityTypes = null;
        public ICollection<ActivityType> ActivityTypes
        {
            get
            {
                if (activites == null)
                {
                    activites = ApplicationUser.CurrentUser.Activities;
                }
                return activityTypes;
            }
            set
            {
                this.activityTypes = value;
                FirePropertyChanged("ActivityTypes");
            }
        }


    }
}

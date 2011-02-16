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
using IndoorWorx.Infrastructure.Enums;
using IndoorWorx.Infrastructure.Helpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IndoorWorx.ForMe.Views
{
    public class ProfilePresentationModel : BaseModel,IProfilePresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public ProfilePresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            LoadItems();
        }

        private void LoadItems()
        {
            var applicationUserService = serviceLocator.GetInstance<IApplicationUserService>();
            applicationUserService.OccupationsRetrievalError += (sender, e) =>
            {
                this.IsBusy = false;
                throw e.Value;
            };
            applicationUserService.OccupationsRetrieved += (sender, e) =>
            {
                User.Occupations = e.Value as ObservableCollection<Occupation>;
                this.IsBusy = false;
            };
            this.IsBusy = true;
            applicationUserService.RetrievOccupations();

            applicationUserService.ReferralSourcesRetrievalError += (sender, e) =>
            {
                this.IsBusy = false;
                throw e.Value;
            };
            applicationUserService.ReferralSourcesRetrieved += (sender, e) =>
            {
                User.ReferralSources = e.Value as ICollection<ReferralSource>;
                this.IsBusy = false;
            };
            this.IsBusy = true;
            applicationUserService.RetrievReferralSources();

            var sportingHabitsService = serviceLocator.GetInstance<ISportingHabitsService>();
            sportingHabitsService.CompetitiveLevelsRetrievalError += (sender, e) =>
            {
                this.IsBusy = false;
                throw e.Value;
            };
            sportingHabitsService.CompetitiveLevelsRetrieved += (sender, e) =>
            {
                User.SportingHabits.CompetitiveLevels = e.Value as ICollection<CompetitiveLevel>;
                this.IsBusy = false;
            };
            this.IsBusy = true;
            sportingHabitsService.RetrieveCompetitiveLevels();

            sportingHabitsService.IndoorTrainingFrequencyOptionsRetrievalError += (sender, e) =>
            {
                this.IsBusy = false;
                throw e.Value;
            };
            sportingHabitsService.IndoorTrainingFrequencyOptionsRetrieved += (sender, e) =>
            {
                User.SportingHabits.IndoorTrainingFrequencyOptions = e.Value as ICollection<IndoorTrainingFrequency>;
                this.IsBusy = false;
            };
            this.IsBusy = true;
            sportingHabitsService.RetrieveIndoorTrainingFrequencyOptions();

            sportingHabitsService.TrainingVolumeOptionsRetrievalError += (sender, e) =>
            {
                this.IsBusy = false;
                throw e.Value;
            };
            sportingHabitsService.TrainingVolumeOptionsRetrieved += (sender, e) =>
            {
                User.SportingHabits.TrainingVolumeOptions = e.Value as ICollection<TrainingVolume>;
                this.IsBusy = false;
            };
            this.IsBusy = true;
            sportingHabitsService.RetrieveTrainingVolumeOptions();
        }


        public ApplicationUser User
        {
            get
            {
                return ApplicationUser.CurrentUser;
            }
            set 
            {
                ApplicationUser.CurrentUser = value;
                FirePropertyChanged("User");
            }
        }

        private IProfileView view = null;
        public IProfileView View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
                FirePropertyChanged("View");
            }
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get
            {
                return isBusy; 
            }
            set
            {
                isBusy = value;
                FirePropertyChanged("IsBusy");
            }
        }
    }
}

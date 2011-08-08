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
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Infrastructure.Resources;

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

        public ApplicationUser AddTemplateToLibrary(TrainingSetTemplate template, Action complete)
        {
            var userService = IoC.Resolve<IApplicationUserService>();
            userService.AddTemplateError += (sender, e) =>
                {
                    complete();
                    throw e.Value;
                };
            userService.AddTemplateCompleted += (sender, e) =>
                {
                    var dialogFacade = IoC.Resolve<IDialogFacade>();
                    switch (e.Value.AddTemplateStatus)
                    {
                        case AddTemplateStatus.Success:
                            ApplicationUser.CurrentUser.Templates.Add(template);
                            break;
                        case AddTemplateStatus.InsufficientCredits:
                            dialogFacade.Alert(ApplicationResources.InsufficientCredits);
                            break;
                        case AddTemplateStatus.TemplateAlreadyAdded:
                            dialogFacade.Alert(ApplicationResources.TemplateAlreadyAdded);
                            break;
                        case AddTemplateStatus.Error:
                            dialogFacade.Alert(e.Value.Message);
                            break;
                        default:
                            break;
                    }
                    complete();
                };
            userService.AddTemplateToLibrary(template);
            return this;
        }

        public ApplicationUser AddVideoToLibrary(Video video, Action complete)
        {
            var userService = IoC.Resolve<IApplicationUserService>();
            userService.AddVideoError += (sender, e) =>
            {
                complete();
                throw e.Value;
            };
            userService.AddVideoCompleted += (sender, e) =>
            {
                var dialogFacade = IoC.Resolve<IDialogFacade>();
                switch (e.Value.AddVideoStatus)
                {
                    case AddVideoStatus.Success:
                        ApplicationUser.CurrentUser.Videos.Add(video);
                        break;
                    case AddVideoStatus.InsufficientCredits:
                        dialogFacade.Alert(ApplicationResources.InsufficientCredits);
                        break;
                    case AddVideoStatus.VideoAlreadyAdded:
                        dialogFacade.Alert(ApplicationResources.VideoAlreadyAdded);
                        break;
                    case AddVideoStatus.Error:
                        dialogFacade.Alert(e.Value.Message);
                        break;
                    default:
                        break;
                }
                complete();
            };
            userService.AddVideoToLibrary(video);
            return this;
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext streamingContext)
        {
            if (this.Videos == null)
                this.Videos = new ObservableCollection<Video>();
            else
                this.Videos = new ObservableCollection<Video>(videos);

            if (this.Templates == null)
                this.Templates = new ObservableCollection<TrainingSetTemplate>();
            else
                this.Templates = new ObservableCollection<TrainingSetTemplate>(templates);

            if (this.TrainingSets == null)
                this.TrainingSets = new ObservableCollection<TrainingSet>();
            else
                this.TrainingSets = new ObservableCollection<TrainingSet>(trainingSets);
        }
    }
}

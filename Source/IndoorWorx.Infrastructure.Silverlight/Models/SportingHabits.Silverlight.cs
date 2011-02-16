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

namespace IndoorWorx.Infrastructure.Models
{
    public partial class SportingHabits
    {
        private ICollection<Sport> allSports = new List<Sport>();
        [DataMember]
        public ICollection<Sport> AllSports
        {
            get { return allSports; }
            set
            {
                allSports = value;
                FirePropertyChanged("AllSports");
            }
        }

        private ICollection<TrainingVolume> trainingVolumeOptions = new List<TrainingVolume>();
        [DataMember]
        public ICollection<TrainingVolume> TrainingVolumeOptions
        {
            get { return trainingVolumeOptions; }
            set
            {
                trainingVolumeOptions = value;
                FirePropertyChanged("TrainingVolumeOptions");
            }
        }

        private ICollection<IndoorTrainingFrequency> indoorTrainingFrequencyOptions = new List<IndoorTrainingFrequency>();
        [DataMember]
        public ICollection<IndoorTrainingFrequency> IndoorTrainingFrequencyOptions
        {
            get { return indoorTrainingFrequencyOptions; }
            set
            {
                indoorTrainingFrequencyOptions = value;
                FirePropertyChanged("IndoorTrainingFrequencyOptions");
            }
        }

        private ICollection<CompetitiveLevel> competitiveLevels = new List<CompetitiveLevel>();
        [DataMember]
        public ICollection<CompetitiveLevel> CompetitiveLevels
        {
            get { return competitiveLevels; }
            set
            {
                competitiveLevels = value;
                FirePropertyChanged("CompetitiveLevels");
            }
        }



        private Sport selectedSport = new Sport();
        [DataMember]
        public Sport SelectedSport
        {
            get { return selectedSport; }
            set
            {
                selectedSport = value;
                FirePropertyChanged("SelectedSport");
            }
        }

        private Sport mySelectedSport = new Sport();
        [DataMember]
        public Sport MySelectedSport
        {
            get { return mySelectedSport; }
            set
            {
                mySelectedSport = value;
                FirePropertyChanged("MySelectedSport");
            }
        }
    }
}

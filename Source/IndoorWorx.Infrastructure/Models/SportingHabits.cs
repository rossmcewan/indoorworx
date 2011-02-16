using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference=true)]
    public partial class SportingHabits : BaseModel
    {
        private ICollection<Sport> mySports = new List<Sport>();
        [DataMember]
        public virtual ICollection<Sport> MySports
        {
            get
            {
                return mySports;
            }
            set
            {
                mySports = value;
                FirePropertyChanged("MySports");
            }
        }

        private TrainingVolume trainingVolume;
        [DataMember]
        public virtual TrainingVolume TrainingVolume
        {
            get { return trainingVolume; }
            set
            {
                trainingVolume = value;
                FirePropertyChanged("TrainingVolume");
            }
        }

        private IndoorTrainingFrequency indoorTrainingFrequency;
        [DataMember]
        public virtual IndoorTrainingFrequency IndoorTrainingFrequency
        {
            get { return indoorTrainingFrequency; }
            set
            {
                indoorTrainingFrequency = value;
                FirePropertyChanged("IndoorTrainingFrequency");
            }
        }

        private CompetitiveLevel competitiveLevel;
        [DataMember]
        public virtual CompetitiveLevel CompetitiveLevel
        {
            get { return competitiveLevel; }
            set
            {
                competitiveLevel = value;
                FirePropertyChanged("CompetitiveLevel");
            }
        }

       
    }
}

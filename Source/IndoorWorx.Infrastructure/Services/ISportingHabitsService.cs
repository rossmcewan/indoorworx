using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface ISportingHabitsService
    {
        [OperationContract]
        ICollection<TrainingVolume> RetrieveTrainingVolumeOptions();

        [OperationContract]
        ICollection<IndoorTrainingFrequency> RetrieveIndoorTrainingFrequency();

        [OperationContract]
        ICollection<CompetitiveLevel> RetrieveCompetitiveLevels();

    }
}

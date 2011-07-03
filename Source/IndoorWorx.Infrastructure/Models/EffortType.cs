using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IndoorWorx.Infrastructure.Models
{
    public enum EffortType
    {
        [Description("Power")]
        Power,
        [Description("HeartRate")]
        HeartRate,
        [Description("RPE")]
        RPE
    }
}

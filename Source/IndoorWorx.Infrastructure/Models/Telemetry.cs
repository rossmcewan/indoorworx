using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class Telemetry : BaseModel
    {
        public virtual Guid Id { get; set; }

        public virtual Video Video { get; set; }

        //TimePosition - the position of this telemetry element, relative to the entire duration

        //Torque - cycling/rowing specific - anything with a ergometer

        //Watts

        //Speed - in km/h

        //Distance - in km

        //Cadence

        //Heart Rate

        //Altitude

        //GPS co-ordinates

        //Gear
    }
}

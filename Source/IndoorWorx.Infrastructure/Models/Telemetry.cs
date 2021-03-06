﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class Telemetry : BaseModel
    {
        [DataMember]
        public virtual TimeSpan TimePosition { get; set; }

        [DataMember]
        public virtual double Torque { get; set; }

        [DataMember]
        public virtual double Speed { get; set; }

        [DataMember]
        public virtual double Watts { get; set; }

        [DataMember]
        public virtual double Distance { get; set; }

        [DataMember]
        public virtual int Cadence { get; set; }

        [DataMember]
        public virtual int HeartRate { get; set; }

        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual double Altitude { get; set; }

        public virtual double PercentageThreshold { get; set; }
        
        public static Telemetry Parse(string s)
        {
            var elements = s.Split(',');
            return new Telemetry()
            {
                TimePosition = TimeSpan.FromMinutes(Convert.ToDouble(elements[0])),
                Torque = Convert.ToDouble(elements[1]),
                Speed = Convert.ToDouble(elements[2]),
                Watts = Convert.ToDouble(elements[3]),
                Distance = Convert.ToDouble(elements[4]),
                Cadence = Convert.ToInt32(elements[5]),
                HeartRate = Convert.ToInt32(elements[6]),
                Id = Convert.ToInt32(elements[7]),
                Altitude = Convert.ToDouble(elements[8]),
                PercentageThreshold = Convert.ToDouble(elements[9])
            };
        }

        public string ToDelimitedString(char delimiter)
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}",
                delimiter,
                TimePosition.TotalMinutes,
                Torque,
                Speed,
                Watts,
                Distance,
                Cadence,
                HeartRate,
                Id,
                Altitude,
                PercentageThreshold);
        }

        public Telemetry Clone()
        {
            var result = this.MemberwiseClone() as Telemetry;
            return result;
        }

        DateTime today = DateTime.Now;
        public double OATimePosition
        {
            get
            {
                var seconds = TimePosition.TotalSeconds;
                var valueAsSeconds = System.Convert.ToDouble(seconds);
                var timespan = TimeSpan.FromSeconds(valueAsSeconds);
                var datetime = DateTime.MinValue.Add(timespan);
                //var datetime = new DateTime(today.Year, today.Month, today.Day, timespan.Hours, timespan.Minutes, timespan.Seconds);
                return datetime.ToOADate();
            }
        }        
    }
}

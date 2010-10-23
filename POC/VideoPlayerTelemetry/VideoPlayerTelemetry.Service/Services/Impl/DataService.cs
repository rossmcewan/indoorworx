using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoPlayerTelemetry.Models;
using System.IO;

namespace VideoPlayerTelemetry.Service.Impl
{
    public class DataService : IDataService
    {
        public ICollection<Telemetry> ProvideTelemetryData()
        {
           LoadRootTelemetry();
           return rootTelemetry;
        }

        private static bool loaded = false;
        private static bool loading = false;
        private static List<Telemetry> rootTelemetry = new List<Telemetry>();
        private static void LoadRootTelemetry()
        {
            if (!loading && !loaded)
            {
                loading = true;
                var lines = File.ReadAllLines(@"C:\Temp\Telemetry.csv");
                int lineNumber = 1;
                foreach (var line in lines)
                {
                    if(true)// (lineNumber % 15 == 0)
                    {
                        var values = line.Split(',');
                        var time = TimeSpan.FromMinutes(Convert.ToDouble(values[0]));
                        var watts = Convert.ToDecimal(values[1]);
                        var percentageOfThreshold = (watts * 100M) / 300M;
                        rootTelemetry.Add(new Telemetry() { TimePosition = time, PercentageOfThreshold = percentageOfThreshold });
                    }
                    lineNumber++;
                }
                loaded = true;
            }
        }
    }
}
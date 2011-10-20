using System;
using System.Linq;
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
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Trainers.Resources;
using System.Text;

namespace IndoorWorx.Trainers.Computrainer
{
    public class ComputrainerExport : ITrainerExport
    {
        public string CreateExport(ICollection<Telemetry> telemetry)
        {
            var fileContent = new StringBuilder();
            fileContent.AppendLine("[COURSE HEADER]");
            fileContent.AppendLine("VERSION = 2");
            fileContent.AppendLine("UNITS = ENGLISH");
            fileContent.AppendLine("DESCRIPTION = INDOORWORX");
            fileContent.AppendLine("FILE NAME = INDOORWORX.ERG");
            fileContent.AppendLine("MINUTES\tWATTS");
            fileContent.AppendLine("[END COURSE HEADER]");
            fileContent.AppendLine("");
            fileContent.AppendLine("[COURSE DATA]");
            var lines = new Dictionary<string, string>();
            foreach (var t in telemetry)
            {
                var key = Math.Round(t.TimePosition.TotalMinutes, 2).ToString();
                var watts = (t.PercentageThreshold * 300).ToString();//need to change this to the current users threshold
                if (!lines.ContainsKey(key))
                {
                    lines.Add(key, watts);
                }
                //fileContent.AppendLine(decimal.Round(Convert.ToDecimal(t.TimePosition.TotalMinutes), 1) + "\t" + t.PercentageThreshold * 300);//need to change this to the current users threshold
            }
            foreach (var line in lines)
            {
                fileContent.AppendLine(string.Format("{0}\t{1}", line.Key, line.Value));
            }
            fileContent.AppendLine("[END COURSE DATA]");
            return fileContent.ToString();
        }

        public string Title
        {
            get { return TrainersResources.ComputrainerTitle; }
        }

        public string Description
        {
            get { return TrainersResources.ComputrainerDescription; }
        }

        public string FileExtension
        {
            get { return ".erg"; }
        }

        public string FileFilter
        {
            get { return "Computrainer ERG file|*.erg"; }
        }
    }
}

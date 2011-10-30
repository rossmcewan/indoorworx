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
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace IndoorWorx.Trainers.PowerbeamPro
{
    public class PowerbeamProExport : ITrainerExport
    {
        public string CreateExport(Video video)
        {
            var template = new Template()
            {
                Name = video.Title,
                Description = video.Description,
                Type = 0,
                Version = "1.0",
                Segments = new List<Segment>()
            };
            foreach (var interval in video.Intervals.OrderBy(x=>x.Sequence))
            {
                template.Segments.Add(new Segment()
                {
                    Control = new Control()
                    {
                        Type = "target",
                        Param = new Param()
                        {
                            Name = "target",
                            Value = ((interval.Effort / 100.0) * ApplicationUser.CurrentUser.FTP).ToString()
                        }
                    },
                    Duration = new Duration()
                    {
                        Type = "time",
                        Param = new Param()
                        {
                            Name = "seconds",
                            Value = interval.Duration.TotalSeconds.ToString()
                        }
                    }
                });
            }
            var serializer = new XmlSerializer(typeof(Template));
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms, Encoding.UTF8);
            serializer.Serialize(writer, template);
            ms = (MemoryStream)writer.BaseStream;
            var bytes = ms.ToArray();
            string xml = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return xml;
        }

        //public string CreateExport(ICollection<Telemetry> telemetry)
        //{
        //    var template = new Template()
        //    {
        //        Name = "IndoorWorx",
        //        Description = "IndoorWorx",
        //        Type = 0,
        //        Version = "1.0",
        //        Segments = new List<Segment>()
        //    };
        //    string lastPower = string.Empty;
        //    var intervalDuration = 0;
        //    foreach (var t in telemetry)
        //    {
        //        var tPower = (t.PercentageThreshold * ApplicationUser.CurrentUser.FTP).ToString(); 
        //        if (string.IsNullOrEmpty(lastPower))
        //            lastPower = tPower;
        //        else
        //            intervalDuration += 2;
        //        if (!string.Equals(tPower, lastPower))
        //        {
        //            lastPower = tPower;

        //            var s = new Segment();
        //            s.Control = new Control()
        //            {
        //                Type = "target",
        //                Param = new Param()
        //                {
        //                    Name = "target",
        //                    Value = tPower
        //                }
        //            };
        //            s.Duration = new Duration()
        //            {
        //                Type = "time",
        //                Param = new Param()
        //                {
        //                    Name = "seconds",
        //                    Value = intervalDuration.ToString()
        //                }
        //            };
        //            template.Segments.Add(s);
        //            intervalDuration = 0;
        //        }                
        //    }
        //    if (intervalDuration != 0)
        //    {
        //        intervalDuration += 2;
        //        var s = new Segment();
        //        s.Control = new Control()
        //        {
        //            Type = "target",
        //            Param = new Param()
        //            {
        //                Name = "target",
        //                Value = lastPower
        //            }
        //        };
        //        s.Duration = new Duration()
        //        {
        //            Type = "time",
        //            Param = new Param()
        //            {
        //                Name = "seconds",
        //                Value = intervalDuration.ToString()
        //            }
        //        };
        //        template.Segments.Add(s);
        //    }
        //    var serializer = new XmlSerializer(typeof(Template));
        //    MemoryStream ms = new MemoryStream();
        //    StreamWriter writer = new StreamWriter(ms, Encoding.UTF8);
        //    serializer.Serialize(writer, template);
        //    ms = (MemoryStream)writer.BaseStream;
        //    var bytes = ms.ToArray();
        //    string xml =  Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        //    return xml;
        //}

        public string Title
        {
            get { return TrainersResources.PowerbeamProTitle; }
        }

        public string Description
        {
            get { return TrainersResources.PowerbeamProDescription; }
        }

        public string FileExtension
        {
            get { return ".xml"; }
        }

        public string FileFilter
        {
            get { return "Powerbeam Pro XML file|*.xml"; }
        }
    }
}

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
using System.Xml.Serialization;

namespace IndoorWorx.Trainers.PowerbeamPro
{
    [XmlRoot("template")]
    public class Template
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("type")]
        public int Type { get; set; }

        [XmlElement("segment")]
        public List<Segment> Segments { get; set; }
    }

    [XmlRoot("segment")]
    public class Segment
    {
        [XmlElement("control")]
        public Control Control { get; set; }

        [XmlElement("duration")]
        public Duration Duration { get; set; }
    }

    public class Control
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("param")]
        public Param Param { get; set; }
    }

    public class Duration
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("param")]
        public Param Param { get; set; }
    }

    public class Param
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}

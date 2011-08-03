﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public class DesignerPresentationModel : BaseModel, IDesignerPresentationModel
    {
        public IDesignerView View { get; set; }

        private bool useSingleVideo = false;
        public virtual bool UseSingleVideo
        {
            get { return useSingleVideo; }
            set
            {
                useSingleVideo = value;
                useMultipleVideos = !useSingleVideo;                
                FirePropertyChanged("UseSingleVideo");
                FirePropertyChanged("UseMultipleVideos");
            }
        }

        private bool useMultipleVideos = true;
        public virtual bool UseMultipleVideos
        {
            get { return useMultipleVideos; }
            set
            {
                useMultipleVideos = value;
                useSingleVideo = !useMultipleVideos;
                FirePropertyChanged("UseMultipleVideos");
                FirePropertyChanged("UseSingleVideo");
            }
        }

        private TrainingSetTemplate selectedTemplate;
        public virtual TrainingSetTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                if (selectedTemplate != null)
                    selectedTemplate.ParseSets();
                FirePropertyChanged("SelectedTemplate");
            }
        }

        private Interval selectedInterval;
        public virtual Interval SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                selectedInterval = value;
                FirePropertyChanged("SelectedInterval");
            }
        }
    }
}

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
using System.Collections.Generic;

namespace IndoorWorx.Trainers
{
    public interface ITrainerExport
    {
        string CreateExport(Video video);

        string Title { get; }

        string Description { get; }

        string FileExtension { get; }

        string FileFilter { get; }
    }
}

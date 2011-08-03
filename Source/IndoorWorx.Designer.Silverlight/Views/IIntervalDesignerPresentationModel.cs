﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public interface IIntervalDesignerPresentationModel
    {
        IIntervalDesignerView View { get; set; }

        Interval Interval { get; set; }

        bool UseSingleVideo { get; set; }

        bool UseMultipleVideos { get; set; }
    }
}

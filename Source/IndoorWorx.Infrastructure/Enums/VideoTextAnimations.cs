using System;
using System.Net;
using System.ComponentModel;

namespace IndoorWorx.Infrastructure.Enums
{
    public enum VideoTextAnimations
    {
        [Description("Fade to Center")]
        FadeCenter,

        [Description("Zoom to Center")]
        ZoomCenter,

        [Description("Scroll to Center")]
        ScrollingCenter,

        [Description("Spin")]
        Spinner 
    }
}

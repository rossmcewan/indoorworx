// <copyright file="BlankMediaData.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: BlankMediaData.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player.Models
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Infrastructure.Models;

    /// <summary>
    /// To play a blank element in the player when there is some gap between
    /// two assets in the timeline.
    /// </summary>
    public class BlankMediaData : MediaData
    {
        /// <summary>
        /// The <see cref="canvas"/> control.
        /// </summary>
        private readonly Canvas canvas;
        
        /// <summary>
        /// The <see cref="DispatcherTimer"/> to have the position while playing <see cref="ImageMediaData"/>.
        /// </summary>
        private readonly DispatcherTimer timer;

        /// <summary>
        /// The <see cref="TimelineElement"/> for the <see cref="BlankMediaData"/>
        /// to have the position of the mediadata in the timeline.
        /// </summary>
        private readonly TimelineElement timelineElement;

        /// <summary>
        /// To have the time when the asset starts playing.
        /// </summary>
        private DateTime processTime;

        /// <summary>
        /// The <see cref="TimeSpan"/> position of the <see cref="ImageMediaData"/> while playing.
        /// </summary>
        private TimeSpan inPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlankMediaData"/> class.
        /// </summary>
        /// <param name="timelineElement">The timeline element.</param>
        public BlankMediaData(TimelineElement timelineElement)
        {
            this.timelineElement = timelineElement;
            this.In = new TimeSpan(0);

            this.canvas = new Canvas
                             {
                                 Opacity = 0
                             };

            this.timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 25) };
            this.timer.Tick += (sender, e) =>
                                   {
                                       this.timer.Stop();

                                       if (this.Playing)
                                       {
                                           DateTime now = DateTime.Now;
                                           this.Position += TimeSpan.FromMilliseconds((now - this.processTime).TotalMilliseconds);
                                           this.processTime = now;
                                           this.timer.Start();
                                       }
                                   };
        }

        /// <summary>
        /// Gets the control corresponding to the <see cref="BlankMediaData"/> which
        /// is used to play the <see cref="BlankMediaData"/>.
        /// </summary>
        /// <value>The user control.</value>
        public override object Media
        {
            get { return this.canvas; }
        }

        /// <summary>
        /// Gets the timeline element for the <see cref="BlankMediaData"/>.
        /// </summary>
        /// <value>The timeline element.</value>
        public override TimelineElement TimelineElement
        {
            get { return this.timelineElement; }
        }

        /// <summary>
        /// Gets or sets start position of the asset of the <see cref="BlankMediaData"/>.
        /// </summary>
        /// <value>
        /// Start position from where <see cref="BlankMediaData"/> will start playing.
        /// </value>
        public override TimeSpan In
        {
            get
            {
                return this.inPosition;
            }

            set
            {
                this.inPosition = value;
                this.Position = value;
            }
        }

        public override TimeSpan Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                TimeSpan duration = this.Position + (this.Out - this.In);

                if (duration.TotalSeconds == 0)
                {
                    base.Position = value;
                }
                else if (value.TotalSeconds >= 0 && value <= duration)
                {
                    base.Position = value;
                }
            }
        }

        public override TimeSpan Out
        {
            get
            {
                return base.Out;
            }

            set
            {
                if (this.Position > value)
                {
                    base.Position = value;
                }

                base.Out = value;
            }
        }

        /// <summary>
        /// Plays this <see cref="BlankMediaData"/>.
        /// </summary>
        public override void Play()
        {
            this.Playing = true;
            this.processTime = DateTime.Now;
            this.timer.Start();
        }

        /// <summary>
        /// Pauses this <see cref="BlankMediaData"/>.
        /// </summary>
        public override void Pause()
        {
            this.Playing = false;
        }

        /// <summary>
        /// Stops this <see cref="BlankMediaData"/>.
        /// </summary>
        public override void Stop()
        {
            this.Position = new TimeSpan(0);
            this.Playing = false;
        }

        /// <summary>
        /// Hides this <see cref="BlankMediaData"/>.
        /// </summary>
        public override void Hide()
        {
            base.Hide();
            this.canvas.Opacity = 0;
        }

        /// <summary>
        /// Shows this <see cref="BlankMediaData"/>.
        /// </summary>
        public override void Show()
        {
            base.Show();
            this.canvas.Opacity = 1;
        }
    }
}

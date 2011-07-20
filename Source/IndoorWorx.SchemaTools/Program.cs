﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using IndoorWorx.NHibernate;
using NHibernate.Tool.hbm2ddl;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Enums;

namespace IndoorWorx.SchemaTools
{
    class Program
    {
        static ICollection<VideoText> GetVideoText()
        {
            return new List<VideoText>() 
            {
                new VideoText() 
                {
                    Animation = VideoTextAnimations.Spinner, 
                    Duration = new TimeSpan(0,0,9), 
                    MainText = "Let's Get Ready to Rumble!",
                    SubText = "your video is about to begin",
                    StartTime = new TimeSpan(0,0,0)
                },
                new VideoText() 
                {
                    Animation = VideoTextAnimations.Spinner, 
                    Duration = new TimeSpan(0,0,1), 
                    MainText = "5",
                    SubText = "five",
                    StartTime = new TimeSpan(0,0,10)
                },
                 new VideoText() 
                {
                    Animation = VideoTextAnimations.Spinner, 
                    Duration = new TimeSpan(0,0,1), 
                    MainText = "4",
                    SubText = "four",
                    StartTime = new TimeSpan(0,0,11)
                },
                 new VideoText() 
                {
                    Animation = VideoTextAnimations.Spinner, 
                    Duration = new TimeSpan(0,0,1), 
                    MainText = "3",
                    SubText = "three",
                    StartTime = new TimeSpan(0,0,12)
                },
                 new VideoText() 
                {
                    Animation = VideoTextAnimations.Spinner, 
                    Duration = new TimeSpan(0,0,1), 
                    MainText = "2",
                    SubText = "two",
                    StartTime = new TimeSpan(0,0,13)
                },
                 new VideoText() 
                {
                    Animation = VideoTextAnimations.Spinner, 
                    Duration = new TimeSpan(0,0,1), 
                    MainText = "1",
                    SubText = "one",
                    StartTime = new TimeSpan(0,0,14)
                },
                new VideoText() 
                {
                    Animation = VideoTextAnimations.ZoomCenter, 
                    Duration = new TimeSpan(0,0,10), 
                    MainText = "It's Go time",
                    SubText = "allez allez allez hop hop hop",
                    StartTime = new TimeSpan(0,0,15)
                },
            };
        }

        static Category GetRidesCategory()
        {
            var category = new Category()
            {
                Title = "RIDES",
                Description = "Indoor cycling training videos.",
                Sequence = 1,
                CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=RIDES&orderBy=CATALOG", UriKind.RelativeOrAbsolute),
                Catalogs = new List<Catalog>()
                    {                        
                        new Catalog()
                        {
                            Title = "IndoorWorx",
                            Sequence = 1,
                            Description = "Training videos filmed by us on our favourite training routes with our favourite training buddies.",
                            Videos = new List<Video>()
                            {
#region Rand Waterboard
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/randwaterboard.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Rand Waterboard - 2s and 5s",
                                    Description = "The entire ride entails a quick warm up, followed by 2 by 2 minutes at 120 % FTP with 2 minutes RI; followed by 5 minutes at 110% FTP with 5 minutes RI. We repeat this 5 times before a quick cool down. This is a great set ... dig deep.",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                },
#endregion

#region Suikerbosrand
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/suikerbosrand.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Suikerbosrand Nature Reserve",
                                    Description = "This is a ride through the Suikerbosrand Nature Reserve. About two and a half hours with some good climbing. Go hard up the climbs, there's recovery on the other side. Keep your eyes peeled for eland and baboon!",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                },
#endregion

#region Sun City
                                new Video()
                                {
                                    Sequence = 3,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/suncity.jpg", UriKind.Absolute),
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/sun_city/FILE001.ism/Manifest", UriKind.Absolute),
                                    Title = "11 Global - Sun City",
                                    Description = "Ride the 11 Global Sun City triathlon bike leg. A near threshold effort for just over an hour. 2 significant climbs. Hang tough!",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/suncity.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                }
#endregion
                            }
                        },
                        new Catalog()
                        {
                            Title = "MTN Energade",
                            ImageUri = new Uri("http://localhost:3415/Mock/mtn_energade.jpg", UriKind.Absolute),
                            Sequence = 2,
                            Description = "Training videos filmed by the MTN Energade cycling team.",
                            Videos = new List<Video>()
                            {
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/cyclechallenge.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Pick n Pay 94.7",
                                    Description = "Ride with the men's elite champions of the MTN Energade cycling team racing the world's biggest race! Suffer up the climbs, hang with them on the descent, set yourself up for the win!",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                },
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/cape-argus.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Pick n Pay Cape Argus",
                                    Description = "Ride with the men's elite champions of the MTN Energade cycling team racing the world's biggest race! Suffer up the climbs, hang with them on the descent, set yourself up for the win!",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }
                                }
                            }
                        }
                    }
            };
            return category;
        }

        static Category GetPartsCategory()
        {
            var category = new Category()
            {
                Title = "PARTS",
                Description = "Indoor cycling training videos.",
                Sequence = 1,
                CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=PARTS&orderBy=CATALOG", UriKind.RelativeOrAbsolute),
                Catalogs = new List<Catalog>()
                    {                        
                        new Catalog()
                        {
                            Title = "Hill climbs",
                            Sequence = 1,
                            Description = "Watch the best riding the hills as they should be riddne ... HARD!",
                            Videos = new List<Video>()
                            {
#region Rand Waterboard
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/hillclimb1.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Hill climb 1",
                                    Description = "",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                },
#endregion

#region Suikerbosrand
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/hillclimb2.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Hill climb 2",
                                    Description = "",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                }
#endregion
                            }
                        },
                        new Catalog()
                        {
                            Title = "Time trials",
                            ImageUri = new Uri("http://localhost:3415/Mock/mtn_energade.jpg", UriKind.Absolute),
                            Sequence = 2,
                            Description = "The deciding moment in a race ... maybe!",
                            Videos = new List<Video>()
                            {
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/timetrial1.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Time trial 1",
                                    Description = "",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                },
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/timetrial2.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Time trial 2",
                                    Description = "",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }
                                }
                            }
                        },
                        new Catalog()
                        {
                            Title = "Breakaways",
                            ImageUri = new Uri("http://localhost:3415/Mock/mtn_energade.jpg", UriKind.Absolute),
                            Sequence = 2,
                            Description = "The deciding moment in a race ... maybe!",
                            Videos = new List<Video>()
                            {
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/breakaway1.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Breakaway 1",
                                    Description = "",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }                                    
                                },
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/breakaway2.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Breakaway 2",
                                    Description = "",
                                    TelemetryInfo = new TelemetryInfo()
                                    {
                                        RecordingInterval = 2,
                                        TelemetryUri = new Uri("http://localhost:3415/Mock/telemetry.csv", UriKind.Absolute)
                                    },
                                    Duration = new TimeSpan(1, 18, 0),
                                    VideoMetadata = new VideoMetadata()
                                    {
                                        WhenFilmed = DateTime.Now.AddYears(-1),
                                        FilmedWith = "Contour HD 1080p",
                                        FilmedBy = "Ross McEwan"
                                    }
                                }
                            }
                        }
                    }
            };
            return category;
        }

        static void Main(string[] args)
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("IndoorWorx")))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Module>())
                .ExposeConfiguration(x => new SchemaExport(x).SetOutputFile("../../Schema/createdb.sql").Create(true, true))
                .BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {                
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(new ApplicationUser() { Email = "rossmcewan@gmail.com", Firstname = "Ross", Gender = Genders.Male, Lastname = "McEwan", Username = "rossmcewan" });

                    var power = new EffortType() { Title = "Power" };
                    session.Save(power);
                    var hr = new EffortType() { Title = "Heart Rate" };
                    session.Save(hr);
                    var rpe = new EffortType() { Title = "RPE", Description = "Rate of perceived exertion" };
                    session.Save(rpe);
                    
                    var l1 = new IntervalLevel() { Title = "Active Recovery", MaximumPercentageOfFthr = 68, MaximumPercentageOfFtp = 55, MinRPE = 0, MaxRPE = 2 };
                    session.Save(l1);
                    var l2 = new IntervalLevel() { Title = "Endurance", MinimumPercentageOfFthr = 69, MaximumPercentageOfFthr = 83, MaximumPercentageOfFtp = 75, MinimumPercentageOfFtp = 56, MinRPE = 2, MaxRPE = 3 };
                    session.Save(l2);
                    var l3 = new IntervalLevel() { Title = "Tempo", MinimumPercentageOfFthr = 84, MaximumPercentageOfFthr = 94, MaximumPercentageOfFtp = 90, MinimumPercentageOfFtp = 76, MinRPE = 3, MaxRPE = 4 };
                    session.Save(l3);
                    var l4 = new IntervalLevel() { Title = "Lactate Threshold", MinimumPercentageOfFthr = 95, MaximumPercentageOfFthr = 105, MaximumPercentageOfFtp = 105, MinimumPercentageOfFtp = 91, MinRPE = 4, MaxRPE = 5, TypicalMinDuration = TimeSpan.FromMinutes(8), TypicalMaxDuration = TimeSpan.FromMinutes(30) };
                    session.Save(l4);
                    var l5 = new IntervalLevel() { Title = "VO2max", MinimumPercentageOfFthr = 106, MaximumPercentageOfFtp = 120, MinimumPercentageOfFtp = 106, MinRPE = 6, MaxRPE = 7, TypicalMinDuration = TimeSpan.FromMinutes(3), TypicalMaxDuration = TimeSpan.FromMinutes(8) };
                    session.Save(l5);
                    var l6 = new IntervalLevel() { Title = "Anaerobic Capacity", MaximumPercentageOfFtp = 150, MinimumPercentageOfFtp = 121, MinRPE = 7, TypicalMinDuration = TimeSpan.FromSeconds(30), TypicalMaxDuration = TimeSpan.FromMinutes(3) };
                    session.Save(l6);
                    var l7 = new IntervalLevel() { Title = "Neuromuscular Power", TypicalMaxDuration = TimeSpan.FromSeconds(30) };
                    session.Save(l7);

                    var warmup = new IntervalType() { Name = "Warm up", DefaultLevel = l2 };
                    session.Save(warmup);
                    var cooldown = new IntervalType() { Name = "Cool down", DefaultLevel = l1 };
                    session.Save(cooldown);
                    var recover = new IntervalType() { Name = "Recovery", DefaultLevel = l1 };
                    session.Save(recover);
                    var longHills = new IntervalType() { Name = "Long Hills", DefaultLevel = l3 };
                    session.Save(longHills);
                    var shortHills = new IntervalType() { Name = "Short Hills", DefaultLevel = l4 };
                    session.Save(shortHills);
                    var tt = new IntervalType() { Name = "Time Trial", DefaultLevel = l3 };
                    session.Save(tt);
                    var sprints = new IntervalType() { Name = "Sprints", DefaultLevel = l5 };
                    session.Save(sprints);

                    #region 4x10
                    var trainingTemplate = new TrainingSetTemplate();
                    trainingTemplate.EffortType = power;
                    trainingTemplate.Duration = TimeSpan.FromMinutes(70);
                    trainingTemplate.Title = "4 x 10";
                    trainingTemplate.Description = "An easy 10 minute warm up followed by 4 10 minute intervals at threshold with 5 minutes recovery. This is followed by an easy 5 minute cool down.";
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Warm up",
                        IntervalType = warmup,
                        IntervalLevel = warmup.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        EffortFrom = warmup.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = warmup.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 0
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #1",
                        IntervalType = tt,
                        IntervalLevel = tt.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        EffortFrom = tt.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = tt.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 1
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        IntervalType = recover,
                        IntervalLevel = recover.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        EffortFrom = recover.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = recover.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 2
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #2",
                        IntervalType = tt,
                        IntervalLevel = tt.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        EffortFrom = tt.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = tt.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 3
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        IntervalType = recover,
                        IntervalLevel = recover.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        EffortFrom = recover.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = recover.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 4
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #3",
                        IntervalType = tt,
                        IntervalLevel = tt.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        EffortFrom = tt.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = tt.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 5
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        IntervalType = recover,
                        IntervalLevel = recover.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        EffortFrom = recover.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = recover.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 6
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #4",
                        IntervalType = tt,
                        IntervalLevel = tt.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        EffortFrom = tt.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = tt.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 7
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Cool down",
                        IntervalType = cooldown,
                        IntervalLevel = cooldown.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        EffortFrom = cooldown.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = cooldown.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 8
                    });
                    session.Save(trainingTemplate);
                    #endregion

                    #region 2x20
                    var trainingTemplate1 = new TrainingSetTemplate();
                    trainingTemplate1.EffortType = power;
                    trainingTemplate1.Duration = TimeSpan.FromHours(1);
                    trainingTemplate1.Title = "2 x 20";
                    trainingTemplate1.Description = "An easy 10 minute warm up followed by 2 20 minute intervals at threshold with 5 minutes recovery. This is followed by an easy 5 minute cool down.";
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Warm up",
                        IntervalType = warmup,
                        IntervalLevel = warmup.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        EffortFrom = warmup.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = warmup.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 0
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Interval #1",
                        IntervalType = tt,
                        IntervalLevel = tt.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(20),
                        EffortType = power,
                        EffortFrom = tt.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = tt.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 1
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        IntervalType = recover,
                        IntervalLevel = recover.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        EffortFrom = recover.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = recover.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 2
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Interval #2",
                        IntervalType = tt,
                        IntervalLevel = tt.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(20),
                        EffortType = power,
                        EffortFrom = tt.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = tt.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 3
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Cool down",
                        IntervalType = cooldown,
                        IntervalLevel = cooldown.DefaultLevel,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        EffortFrom = cooldown.DefaultLevel.MinimumPercentageOfFtp,
                        EffortTo = cooldown.DefaultLevel.MaximumPercentageOfFtp,
                        Sequence = 8
                    });
                    session.Save(trainingTemplate1);
                    #endregion

                    session.Save(new Category() { Title = "ALL", Sequence = 0, CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=ALL&orderBy=CATEGORY", UriKind.RelativeOrAbsolute) });
                    session.Save(GetRidesCategory());
                    session.Save(GetPartsCategory());
                    transaction.Commit();
                }
            }
        }
    }
}

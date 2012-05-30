using System;
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

        static Category BuildRidesCategory()
        {
            return new Category()
            {
                Title = "RIDES",
                Description = "Longer videos.",
                Sequence = 1,
                CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=RIDES&orderBy=CATALOG", UriKind.RelativeOrAbsolute),
                LibraryUri = new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=RIDES&orderBy=CATALOG", UriKind.RelativeOrAbsolute),
                Catalogs = new List<Catalog>()
                {
                    new Catalog()
                    {
                        Title = "CycleFilm",
                        Sequence = 1,
                        Description =   "CYCLEFILM is a European based production company specializing in informative cycling documentaries, how-to ride and reconnaissance films as well as news gathering from cycling events and trade shows around the world. "+
                                        "We own over 500 hours of road cycling related stock footage available for commercial use.  Please get in touch with us for details on licensing and syndication. "+
                                        "From backstage coverage of pro events like the Tour de France and Giro d'Italia, to daily diaries of charity rides for the Geoff Thomas Foundation and the Leuka Fireflies, to event and recon films of European Sportives & Gran Fondos, Cyclefilm covers it all. "+
                                        "Cyclefilm also offers New Media Production services for small businesses and corporations, helping to build stronger customer relationships with emerging media tools. "+
                                        "Cyclefilm can formulate a comprehensive online strategy, produce powerful, targeted content and help deploy it effectively across the web.",
                        Videos = new List<Video>()
                    }
                }
            };
        }

        static Category BuildSnippetsCategory()
        {
            return new Category()
            {
                Title = "SNIPPETS",
                Description = "Video snippets to create your perfect indoor training experience.",
                Sequence = 2,
                CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=SNIPPETS&orderBy=CATALOG", UriKind.RelativeOrAbsolute),
                LibraryUri = new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=SNIPPETS&orderBy=CATALOG", UriKind.RelativeOrAbsolute),
                Catalogs = new List<Catalog>()
                {
                    new Catalog()
                    {
                        Title = "CycleFilm",
                        Sequence = 1,
                        Description =   "CYCLEFILM is a European based production company specializing in informative cycling documentaries, how-to ride and reconnaissance films as well as news gathering from cycling events and trade shows around the world. "+
                                        "We own over 500 hours of road cycling related stock footage available for commercial use.  Please get in touch with us for details on licensing and syndication. "+
                                        "From backstage coverage of pro events like the Tour de France and Giro d'Italia, to daily diaries of charity rides for the Geoff Thomas Foundation and the Leuka Fireflies, to event and recon films of European Sportives & Gran Fondos, Cyclefilm covers it all. "+
                                        "Cyclefilm also offers New Media Production services for small businesses and corporations, helping to build stronger customer relationships with emerging media tools. "+
                                        "Cyclefilm can formulate a comprehensive online strategy, produce powerful, targeted content and help deploy it effectively across the web.",
                        Videos = new List<Video>()
                        {
                            new Video()
                            {
                                ImageUri = new Uri("http://www.indoorworx.com/IndoorWorx/Media/CycleFilm/Climb/DDL_SSG_SD_Thumb.jpg", UriKind.Absolute),
                                StreamUri = new Uri("http://www.indoorworx.com/IndoorWorx/Media/CycleFilm/Climb/DDL_SSG_SD.ism/Manifest", UriKind.Absolute),
                                Title = "Survival Guide - Climbing",
                                Description = "Narrated by UK endurance rider and Sportive Specialist Michael Cotty from Cannondale.",
                                TelemetryInfo = new TelemetryInfo()
                                {
                                    RecordingInterval = 2,
                                    TelemetryUri = new Uri("http://www.indoorworx.com/IndoorWorx/Mock/telemetry.csv", UriKind.Absolute)
                                },
                                Duration = new TimeSpan(0,6,20),
                                VideoMetadata = new VideoMetadata()
                                {
                                    WhenFilmed = null,
                                    FilmedBy = "CycleFilm"
                                }
                            },
                            new Video()
                            {
                                ImageUri = new Uri("http://www.indoorworx.com/IndoorWorx/Media/CycleFilm/Descent/DDL_SSG_SD_Thumb.jpg", UriKind.Absolute),
                                StreamUri = new Uri("http://www.indoorworx.com/IndoorWorx/Media/CycleFilm/Descent/DDL_SSG_SD.ism/manifest", UriKind.Absolute),
                                Title = "Survival Guide - Descending",
                                Description = "Narrated by UK endurance rider and Sportive Specialist Michael Cotty from Cannondale.",
                                TelemetryInfo = new TelemetryInfo()
                                {
                                    RecordingInterval = 2,
                                    TelemetryUri = new Uri("http://www.indoorworx.com/IndoorWorx/Mock/telemetry.csv", UriKind.Absolute)
                                },
                                Duration = new TimeSpan(0,7,31),
                                VideoMetadata = new VideoMetadata()
                                {
                                    WhenFilmed = null,
                                    FilmedBy = "CycleFilm"
                                }
                            }
                        }
                    }
                }
            };
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
                    session.Save(new ApplicationUser() { Email = "rossmcewan@gmail.com", Firstname = "Ross", Gender = Genders.Male, Lastname = "McEwan", Username = "rossmcewan", Credits = 100 });

                    var power = new EffortType() { Title = "Power", Sequence = 10, Tag = "POWER" };
                    session.Save(power);
                    var hr = new EffortType() { Title = "Heart Rate", Sequence = 20, Tag = "HR" };
                    session.Save(hr);
                    var rpe = new EffortType() { Title = "RPE", Description = "Rate of perceived exertion", Sequence = 30, Tag = "RPE" };
                    session.Save(rpe);
                    
                    var l1 = new IntervalLevel() { Title = "L1 - Active Recovery", Sequence = 10, MaximumPercentageOfFthr = 68, MaximumPercentageOfFtp = 55, MinRPE = 0, MaxRPE = 2 };
                    session.Save(l1);
                    var l2 = new IntervalLevel() { Title = "L2 - Endurance", Sequence = 20, MinimumPercentageOfFthr = 69, MaximumPercentageOfFthr = 83, MaximumPercentageOfFtp = 75, MinimumPercentageOfFtp = 56, MinRPE = 2, MaxRPE = 3 };
                    session.Save(l2);
                    var l3 = new IntervalLevel() { Title = "L3 - Tempo", Sequence = 30, MinimumPercentageOfFthr = 84, MaximumPercentageOfFthr = 94, MaximumPercentageOfFtp = 90, MinimumPercentageOfFtp = 76, MinRPE = 3, MaxRPE = 4 };
                    session.Save(l3);
                    var l4 = new IntervalLevel() { Title = "L4 - Lactate Threshold", Sequence = 40, MinimumPercentageOfFthr = 95, MaximumPercentageOfFthr = 105, MaximumPercentageOfFtp = 105, MinimumPercentageOfFtp = 91, MinRPE = 4, MaxRPE = 5, TypicalMinDuration = TimeSpan.FromMinutes(8), TypicalMaxDuration = TimeSpan.FromMinutes(30) };
                    session.Save(l4);
                    var l5 = new IntervalLevel() { Title = "L5 - VO2max", Sequence = 50, MinimumPercentageOfFthr = 106, MaximumPercentageOfFtp = 120, MinimumPercentageOfFtp = 106, MinRPE = 6, MaxRPE = 7, TypicalMinDuration = TimeSpan.FromMinutes(3), TypicalMaxDuration = TimeSpan.FromMinutes(8) };
                    session.Save(l5);
                    var l6 = new IntervalLevel() { Title = "L6 - Anaerobic Capacity", Sequence = 60, MaximumPercentageOfFtp = 150, MinimumPercentageOfFtp = 121, MinRPE = 7, TypicalMinDuration = TimeSpan.FromSeconds(30), TypicalMaxDuration = TimeSpan.FromMinutes(3) };
                    session.Save(l6);
                    var l7 = new IntervalLevel() { Title = "L7 - Neuromuscular Power", Sequence = 70, TypicalMaxDuration = TimeSpan.FromSeconds(30) };
                    session.Save(l7);

                    //var warmup = new IntervalType() { Name = "Warm up", Tag = "WARMUP", Sequence = 10 };
                    //session.Save(warmup);
                    //var cooldown = new IntervalType() { Name = "Cool down", Tag = "COOLDOWN", Sequence = 20 };
                    //session.Save(cooldown);
                    //var recover = new IntervalType() { Name = "Recovery", Tag = "RECOVERY", Sequence = 30 };
                    //session.Save(recover);
                    //var longHills = new IntervalType() { Name = "Long Hills", Tag = "LONGHILLS", Sequence = 40 };
                    //session.Save(longHills);
                    //var shortHills = new IntervalType() { Name = "Short Hills", Tag = "SHORTHILLS", Sequence = 50 };
                    //session.Save(shortHills);
                    //var tt = new IntervalType() { Name = "Time Trial", Tag = "TIMETRIAL", Sequence = 60 };
                    //session.Save(tt);
                    //var sprints = new IntervalType() { Name = "Sprints", Tag = "SPRINTS", Sequence = 70 };
                    //session.Save(sprints);
                    //var breakaways = new IntervalType() { Name = "Breakaways", Tag = "BREAKAWAYS", Sequence = 80 };
                    //session.Save(breakaways);

                    var levels = new IntervalType() { Name = "Levels", Sequence = 10, Tag = IntervalType.LevelsTag };
                    session.Save(levels);

                    var stepped = new IntervalType() { Name = "Stepped", Sequence = 20, Tag = IntervalType.SteppedTag };
                    session.Save(stepped);

                    var recover = new IntervalType() { Name = "Recovery", Tag = IntervalType.RecoveryTag, Sequence = 30 };
                    session.Save(recover);

                    #region 4x10
                    var trainingTemplate = new TrainingSetTemplate();
                    trainingTemplate.IsPublic = true;
                    trainingTemplate.EffortType = power;
                    trainingTemplate.Duration = TimeSpan.FromMinutes(70);
                    trainingTemplate.Title = "4 x 10";
                    trainingTemplate.Description = "An easy 10 minute warm up followed by 4 10 minute intervals at threshold with 5 minutes recovery. This is followed by an easy 5 minute cool down.";
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Warm up",
                        TemplateSection = "WARMUP",
                        SectionGroup = "W1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 0
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #1",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l3,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        Effort = (l3.MinimumPercentageOfFtp + l3.MaximumPercentageOfFtp) / 2,
                        Sequence = 1
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 2
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #2",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l3,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        Effort = (l3.MinimumPercentageOfFtp + l3.MaximumPercentageOfFtp) / 2,
                        Sequence = 3
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 4
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #3",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l3,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        Effort = (l3.MinimumPercentageOfFtp + l3.MaximumPercentageOfFtp) / 2,
                        Sequence = 5
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 6
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Interval #4",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l3,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        Effort = (l3.MinimumPercentageOfFtp + l3.MaximumPercentageOfFtp) / 2,
                        Sequence = 7
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 8
                    });
                    trainingTemplate.Intervals.Add(new Interval()
                    {
                        Title = "Cool down",
                        TemplateSection = "COOLDOWN",
                        SectionGroup = "C1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 9
                    });
                    session.Save(trainingTemplate);
                    #endregion

                    #region 2x20
                    var trainingTemplate1 = new TrainingSetTemplate();
                    trainingTemplate1.IsPublic = true;
                    trainingTemplate1.EffortType = power;
                    trainingTemplate1.Duration = TimeSpan.FromHours(1);
                    trainingTemplate1.Title = "2 x 20";
                    trainingTemplate1.Description = "An easy 10 minute warm up followed by 2 20 minute intervals at threshold with 5 minutes recovery. This is followed by an easy 5 minute cool down.";
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Warm up",
                        TemplateSection = "WARMUP",
                        SectionGroup = "W1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(10),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 0
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Interval #1",
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalType = levels,
                        IntervalLevel = l3,
                        Duration = TimeSpan.FromMinutes(20),
                        EffortType = power,
                        Effort = (l3.MinimumPercentageOfFtp + l3.MaximumPercentageOfFtp) / 2,
                        Sequence = 1
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        IntervalType = levels,
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 2
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Interval #2",
                        IntervalType = levels,
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalLevel = l3,
                        Duration = TimeSpan.FromMinutes(20),
                        EffortType = power,
                        Effort = (l3.MinimumPercentageOfFtp + l3.MaximumPercentageOfFtp) / 2,
                        Sequence = 3
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Recovery",
                        IntervalType = levels,
                        TemplateSection = "MAINSET",
                        SectionGroup = "M1",
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 4
                    });
                    trainingTemplate1.Intervals.Add(new Interval()
                    {
                        Title = "Cool down",
                        TemplateSection = "COOLDOWN",
                        SectionGroup = "C1",
                        IntervalType = levels,
                        IntervalLevel = l1,
                        Duration = TimeSpan.FromMinutes(5),
                        EffortType = power,
                        Effort = (l1.MinimumPercentageOfFtp + l1.MaximumPercentageOfFtp) / 2,
                        Sequence = 5
                    });
                    session.Save(trainingTemplate1);
                    #endregion

                    session.Save(new Category() { Title = "ALL", Sequence = 0, CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=ALL&orderBy=CATEGORY", UriKind.RelativeOrAbsolute), LibraryUri = new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=ALL&orderBy=CATEGORY", UriKind.RelativeOrAbsolute) });
                    session.Save(BuildRidesCategory());
                    session.Save(BuildSnippetsCategory());
                    session.Save(new Category() { Title = "WORKOUTS", Sequence = 3, CatalogUri = new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=WORKOUTS&orderBy=CATEGORY", UriKind.RelativeOrAbsolute), LibraryUri = new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/VideoCatalogPage.xaml?filter=WORKOUTS&orderBy=CATEGORY", UriKind.RelativeOrAbsolute) });
                    transaction.Commit();
                }
            }
        }
    }
}

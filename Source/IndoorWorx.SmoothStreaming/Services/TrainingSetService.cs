﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure.Responses;
using IndoorWorx.Infrastructure.Requests;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;
using IndoorWorx.SmoothStreaming.Models;
using System.IO;
using System.Globalization;
using System.Web;
using IndoorWorx.Infrastructure.Models;
using System.Transactions;
using System.ServiceModel.Channels;
using IndoorWorx.Infrastructure.Criteria;

namespace IndoorWorx.SmoothStreaming.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TrainingSetService : ITrainingSetService
    {
        const ulong Timescale = 10000000;

        private readonly IServiceLocator serviceLocator;
        public TrainingSetService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public CreateTrainingSetResponse CreateTrainingSet(CreateTrainingSetRequest request)
        {            
            var response = new CreateTrainingSetResponse();
            try
            {
                var downloadManager = new DownloaderManager();
                var compositeManifestInfo = new CompositeManifestInfo(2, 0);
                var videoRepository = serviceLocator.GetInstance<IVideoRepository>();
                var catalogRepository = serviceLocator.GetInstance<ICatalogRepository>();
                var templateRepository = serviceLocator.GetInstance<ITrainingSetTemplateRepository>();
                var totalDuration = TimeSpan.Zero;
                foreach (var videoPart in request.TrainingSet.VideoParts)
                {
                    var video = videoRepository.Get(videoPart.VideoId);
                    var manifestStream = downloadManager.DownloadManifest(video.StreamUri, true, null);
                    if (manifestStream != null)
                    {
                        var parser = new SmoothStreamingManifestParser(manifestStream);
                        double startPosition = videoPart.From.TotalSeconds;
                        double endPosition = videoPart.To.TotalSeconds;
                        compositeManifestInfo.AddClip(video.StreamUri, (ulong)(startPosition * Timescale), (ulong)(endPosition * Timescale), parser.ManifestInfo);
                        totalDuration = totalDuration.Add(videoPart.To.Subtract(videoPart.From));
                    }
                }
                var writer = new SmoothStreamingManifestWriter();
                var manifest = writer.GenerateCompositeManifest(compositeManifestInfo, false, false);

                string csmPath = HttpContext.Current.Server.MapPath("csm");
                if (!Directory.Exists(csmPath))
                    Directory.CreateDirectory(csmPath);
                var uniqueName = Guid.NewGuid().ToString();
                string tmpFilePath = Path.Combine(csmPath, string.Format("{0}.tmpcsm", uniqueName));
                string finalFilePath = Path.Combine(csmPath, string.Format("{0}.csm", uniqueName));

                File.WriteAllText(tmpFilePath, manifest, Encoding.UTF8);

                if (File.Exists(tmpFilePath))
                {
                    if (File.Exists(finalFilePath))
                    {
                        File.Delete(finalFilePath);
                    }

                    File.Move(tmpFilePath, finalFilePath);
                }

                var userService = serviceLocator.GetInstance<IApplicationUserService>();
                var user = userService.RetrieveApplicationUser(new ApplicationUserFindCriteria()
                {
                    Username = request.User
                });

                var template = templateRepository.Get(request.TrainingSet.TrainingSetTemplateId);
                //var _telemetry = new List<Telemetry>();
                var timer = TimeSpan.Zero;
                var recordingInterval = TimeSpan.FromSeconds(2);
                var telemetryFile = new StringBuilder().AppendLine("\"Minutes\",\"Torq(N-m)\",\"Km/h\",\"Watts\",\"Km\",\"Cadence\",\"Hrate\",\"ID\",\"Altitude(m)\"");
                foreach (var interval in template.Intervals)
                {
                    var numberOfElements = interval.Duration.TotalSeconds / recordingInterval.TotalSeconds;
                    for (int i = 0; i < numberOfElements; i++)
                    {
                        var telemetry = new Telemetry();
                        telemetry.PercentageThreshold = Convert.ToDouble(interval.Effort) / 100;
                        telemetry.Watts = ((telemetry.PercentageThreshold) * user.FTP.GetValueOrDefault());
                        telemetry.TimePosition = timer;
                        timer = timer.Add(recordingInterval);
                        telemetryFile.AppendLine(telemetry.ToDelimitedString(','));

                        //telemetry.PercentageThreshold = Convert.ToDouble(interval.Effort);
                        //telemetry.TimePosition = timer;
                        //telemetryFile.AppendLine(telemetry.ToDelimitedString(','));
                    }
                }
                string telemetryPath = HttpContext.Current.Server.MapPath("telemetry");
                if (!Directory.Exists(telemetryPath))
                    Directory.CreateDirectory(telemetryPath);
                string filePath = Path.Combine(telemetryPath, string.Format("{0}.csv", uniqueName));
                File.WriteAllText(filePath, telemetryFile.ToString());

                //var context = System.ServiceModel.OperationContext.Current;

                //RemoteEndpointMessageProperty property = (RemoteEndpointMessageProperty)context.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];

                string host = "www.indoorworx.com";// property.Address;

                //var host = "localhost";

                //now create the video
                var workout = new Video();
                workout.StreamUri = new Uri(string.Format("http://{0}/IndoorWorx/csm/{1}.csm", host, uniqueName), UriKind.Absolute);
                workout.Title = request.TrainingSet.Title;
                workout.Description = template.Description;
                workout.Duration = totalDuration;
                workout.TelemetryInfo.TelemetryUri = new Uri(string.Format("http://{0}/IndoorWorx/telemetry/{1}.csv", host, uniqueName), UriKind.Absolute);
                foreach (var text in template.VideoText)
                {
                    workout.VideoText.Add(text.Clone());
                }
                workout.Intervals = template.Intervals.Select(x => new VideoInterval(x.Duration, x.Effort, x.Sequence)).ToList();
                var savedWorkout = videoRepository.Save(workout);

                userService.AddVideoToLibrary(new AddVideoRequest()
                {
                    User = request.User,
                    VideoId = savedWorkout.Id
                });

                response.Status = CreateTrainingSetStatus.Success;
                response.TrainingSet = savedWorkout;
            }
            catch (Exception ex)
            {
                response.Status = CreateTrainingSetStatus.Error;
                response.Message = ex.StackTrace;
                //response.Message = ex.Message;                
            }
            return response;
        }
    }
}

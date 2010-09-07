using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RCE.Services.Contracts.Output;
using RCE.Services.Contracts;
using System.IO;
using SmoothStreamingManifestGenerator.Models;
using SmoothStreamingManifestGenerator;
using System.Web;
using System.Globalization;
using System.ServiceModel.Activation;

namespace RCE.Services.CSM
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CompositeStreamManifestService : ICompositeStreamManifestService
    {
        #region ICompositeStreamManifestService Members

        public void CreateCompositeStream(Contracts.Project project)
        {
            CompositeManifestInfo compositeManifestInfo = new CompositeManifestInfo(2, 1);

            compositeManifestInfo.PlayByPlayDataStreamName = "PBP";
            compositeManifestInfo.AdsDataStreamName = "ADS";

            DownloaderManager manager = new DownloaderManager();

            const ulong Timescale = 10000000; 
            if (project.Timeline != null)
            {
                Track track = project.Timeline.SingleOrDefault(x => x.TrackType.ToUpperInvariant() == "VISUAL");

                if (track != null && track.Shots != null)
                {
                    foreach (Shot shot in track.Shots)
                    {
                        if (shot.Source != null && shot.Source is VideoItem && shot.Source.Resources.Count > 0 && shot.SourceAnchor != null)
                        {
                            Resource resource = shot.Source.Resources.SingleOrDefault(x => !String.IsNullOrEmpty(x.Ref));

                            Uri assetUri;

                            if (resource != null && Uri.TryCreate(resource.Ref, UriKind.Absolute, out assetUri))
                            {
                                Stream manifestStream = manager.DownloadManifest(assetUri, true, null);

                                if (manifestStream != null)
                                {
                                    double startPosition = (shot.Source is SmoothStreamingVideoItem) ? ((SmoothStreamingVideoItem)shot.Source).StartPosition : 0;

                                    SmoothStreamingManifestParser parser = new SmoothStreamingManifestParser(manifestStream);

                                    ulong clipBegin = (ulong)((shot.SourceAnchor.MarkIn.GetValueOrDefault() * Timescale) + (startPosition * Timescale));
                                    ulong clipEnd = (ulong)((shot.SourceAnchor.MarkOut.GetValueOrDefault() * Timescale) + (startPosition * Timescale));

                                    compositeManifestInfo.AddClip(assetUri, clipBegin, clipEnd, parser.ManifestInfo);
                                }
                            }
                        }
                    }
                }
            }

            if (project.Titles != null)
            {
                foreach (var title in project.Titles)
                {
                    //compositeManifestInfo
                }
            }

            if (project.AdOpportunities != null)
            {
                foreach (RCE.Services.Contracts.AdOpportunity adOpportunity in project.AdOpportunities)
                {
                    compositeManifestInfo.AddAdOpportunity(adOpportunity.ID, adOpportunity.TemplateType, adOpportunity.Time);
                }
            }

            if (project.Markers != null)
            {
                foreach (Marker marker in project.Markers)
                {
                    compositeManifestInfo.AddPlayByPlay(marker.ID, marker.Text, marker.Time);
                }
            }

            SmoothStreamingManifestWriter writer = new SmoothStreamingManifestWriter();

            string manifest = writer.GenerateCompositeManifest(compositeManifestInfo, false);

            string csmPath = HttpContext.Current.Server.MapPath("csm");
            if (!Directory.Exists(csmPath))
                Directory.CreateDirectory(csmPath);

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            string tmpFilePath = Path.Combine(csmPath, string.Format(CultureInfo.InvariantCulture, "{0}-{1}.tmpcsm", project.Title.ToString(), datetime));
            string finalFilePath = Path.Combine(csmPath, string.Format(CultureInfo.InvariantCulture, "{0}-{1}.csm", project.Title.ToString(), datetime));

            File.WriteAllText(tmpFilePath, manifest, Encoding.UTF8);
            
            if (File.Exists(tmpFilePath))
            {
                if (File.Exists(finalFilePath))
                {
                    File.Delete(finalFilePath);
                }

                File.Move(tmpFilePath, finalFilePath);
            }
        }

        #endregion
    }
}

// <copyright file="TimelinePresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TimelinePresenter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using Infrastructure.DragDrop;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using RCE.Infrastructure;
    using RCE.Infrastructure.Events;
    using RCE.Infrastructure.Models;
    using RCE.Modules.Timeline.Commands;
    using Services.Contracts;
    using SMPTETimecode;
    using Comment = RCE.Infrastructure.Models.Comment;
    using Project = RCE.Infrastructure.Models.Project;
    using TitleTemplate = RCE.Infrastructure.Models.TitleTemplate;
    using Track = RCE.Infrastructure.Models.Track;
    using TrackType = RCE.Infrastructure.Models.TrackType;

    /// <summary>
    /// Presenter for TimeLine View.
    /// </summary>
    public class TimelinePresenter : BaseModel, ITimelinePresenter
    {
        /// <summary>
        /// The default timeline duration.
        /// </summary>
        public const int DefaultTimelineDuration = 7200;

        /// <summary>
        /// Minimum Element Duration for the asset in the timeline.
        /// </summary>
        public const int MinimumElementDuration = 1;

        /// <summary>
        /// Default Asset Duration (For Images).
        /// </summary>
        public const int DefaultAssetDuration = 600;

        /// <summary>
        /// The <seealso cref="IEventAggregator"/> used to publish and subscribe to events.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The timeline model to manage all the timeline operations.
        /// </summary>
        private readonly ITimelineModel timelineModel;

        /// <summary>
        /// The project service used to interact with the current project.
        /// </summary>
        private readonly IProjectService projectService;

        /// <summary>
        /// Used to manage the undo/redo operations.
        /// </summary>
        private readonly ICaretaker caretaker;

        /// <summary>
        /// Contains the max number of Audio Tracks allowables.
        /// </summary>
        private readonly int maxNumberOfAudioTracks;

        /// <summary>
        /// Default Timeline Duration.
        /// </summary>
        private readonly int defaultTimelineDuration;

        /// <summary>
        /// Contains the current edit mode of the timeline.
        /// </summary>
        private EditMode editMode;

        /// <summary>
        /// Contians the current selected element.
        /// </summary>
        private TimelineElement selectedElement;

        /// <summary>
        /// Contains a list of layer snapshots used for the undo/redo operatios that involves a track.
        /// </summary>
        private IList<TimelineElement> layerSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelinePresenter"/> class.
        /// </summary>
        /// <param name="view">The <see cref="ITimelineView"/>.</param>
        /// <param name="eventAggregator">The <see cref="IEventAggregator"/>.</param>
        /// <param name="timelineModel">The <see cref="ITimelineModel"/>.</param>
        /// <param name="projectService">The <see cref="IProjectService"/>.</param>
        /// <param name="caretaker">The <see cref="ICaretaker"/>.</param>
        /// <param name="configurationService">The <see cref="IConfigurationService"/>.</param>
        public TimelinePresenter(ITimelineView view, IEventAggregator eventAggregator, ITimelineModel timelineModel, IProjectService projectService, ICaretaker caretaker, IConfigurationService configurationService)
        {
            this.caretaker = caretaker;
            this.caretaker.SetUndoLevel(configurationService.GetUndoLevel());
            this.eventAggregator = eventAggregator;
            this.timelineModel = timelineModel;
            this.AudioTracks = new ObservableCollection<Track>();

            this.timelineModel.ElementAdded += this.TimelineModel_ElementAdded;
            this.timelineModel.ElementRemoved += this.TimelineModel_ElementRemoved;
            this.timelineModel.ElementMoved += this.TimelineModel_ElementMoved;
            this.timelineModel.ElementLinked += this.TimelineModel_ElementLinked;
            this.timelineModel.ElementUnlinked += this.TimelineModel_ElementUnlinked;
            this.timelineModel.Tracks.CollectionChanged += this.Tracks_CollectionChanged;

            this.projectService = projectService;

            this.eventAggregator.GetEvent<PositionUpdatedEvent>().Subscribe(this.UpdatePlayHead, true);

            this.eventAggregator.GetEvent<EditModeChangedEvent>().Subscribe(this.SetEditingMode, true);

            this.eventAggregator.GetEvent<DeleteMediaBinAssetEvent>().Subscribe(this.DeleteAsset, true);

            this.eventAggregator.GetEvent<SmpteTimeCodeChangedEvent>().Subscribe(this.UpdateSmpteFrameRate, true);

            this.eventAggregator.GetEvent<AddAssetToTimelineEvent>().Subscribe(this.AddAssetAtCurrentPosition, true);

            this.eventAggregator.GetEvent<StartTimeCodeChangedEvent>().Subscribe(this.UpdateStartTimeCode, true);

            this.eventAggregator.GetEvent<DownloadProgressChangedEvent>().Subscribe(this.ShowAssetDownloadProgress, true);

            this.editMode = EditMode.Gap;
            this.IsInSnapMode = configurationService.GetParameterValueAsBoolean("SnapModeEnabled").GetValueOrDefault();

            this.maxNumberOfAudioTracks = configurationService.GetParameterValueAsInt("MaxNumberOfAudioTracks").GetValueOrDefault(1);

            this.AddAudioTrackCommand = new DelegateCommand<object>(this.AddAudioTrack, this.CanAddAudioTrack);
            this.RemoveAudioTrackCommand = new DelegateCommand<object>(this.RemoveAudioTrack, this.CanRemoveAudioTrack);
            this.DropCommand = new DelegateCommand<DropPayload>(this.DropItem, FilterDropItem);
            this.MoveFrameCommand = new DelegateCommand<object>(this.MoveFrame);
            this.MoveNextClipCommand = new DelegateCommand<object>(this.MoveToNextClip);
            this.MovePreviousClipCommand = new DelegateCommand<object>(this.MoveToPreviousClip);

            this.View = view;
            this.View.Model = this;
            this.View.ElementPositionChange += this.View_ChangeElementPosition;
            this.View.ElementSelect += this.View_ElementSelect;
            this.View.PositionChange += this.View_PositionChange;
            this.View.ShowingLinks += this.View_ShowingLinks;
            this.View.HidingLinks += this.View_HidingLinks;
            this.View.LinkingElement += this.View_LinkingElement;
            this.View.Split += this.View_Split;
            this.View.MovingPlayHead += this.View_MovingPlayhead;
            this.View.Delete += this.View_Delete;
            this.View.TogglePlay += this.View_TogglePlay;
            this.View.TopBarDoubleClicked += this.View_TopBarDoubleClicked;
            this.View.RefreshingElements += this.View_RefreshingElements;
            this.View.Undo += this.View_Undo;
            this.View.Redo += this.View_Redo;
            this.View.StartMoving += this.View_StartMoving;
            this.View.StopMoving += this.View_StopMoving;
            this.View.ToggleEditMode += this.View_ToggleEditMode;
            this.View.TrimElementToCurrentPosition += this.View_TrimElementAtCurrentPositionChange;
            this.View.PickThumbnail += this.View_PickThumbnail;

            this.defaultTimelineDuration = configurationService.GetParameterValueAsInt("DefaultTimelineDurationInSeconds").GetValueOrDefault(DefaultTimelineDuration);
            this.SetDuration(TimeCode.FromAbsoluteTime(this.defaultTimelineDuration, SmpteFrameRate.Smpte2997NonDrop));

            bool timelineHandlersEnabled = configurationService.GetParameterValueAsBoolean("EnableTimelineHandlers").GetValueOrDefault(true);

            this.View.UpdateTimelineHandlers(timelineHandlersEnabled);

            this.LoadTimeline();
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The <see cref="ITimelineView"/>.</value>
        public ITimelineView View { get; private set; }

        /// <summary>
        /// Gets the command to add audio tracks.
        /// </summary>
        /// <value>The delegate command used to add audio tracks.</value>
        public DelegateCommand<object> AddAudioTrackCommand { get; private set; }

        /// <summary>
        /// Gets the command to remove audio tracks.
        /// </summary>
        /// <value>The delegate command used to remove audio tracks.</value>
        public DelegateCommand<object> RemoveAudioTrackCommand { get; private set; }

        /// <summary>
        /// Gets the command executed on drop elements.
        /// </summary>
        /// <value>The delegate command used to drop the elements.</value>
        public DelegateCommand<DropPayload> DropCommand { get; private set; }

        /// <summary>
        /// Gets the command to move frame backward and forward.
        /// </summary>
        /// <value>The move frame command.</value>
        public DelegateCommand<object> MoveFrameCommand { get; private set; }

        /// <summary>
        /// Gets the command to move to the next clip.
        /// </summary>
        /// <value>The command to move to the next clip.</value>
        public DelegateCommand<object> MoveNextClipCommand { get; private set; }

        /// <summary>
        /// Gets the command to move to the previous clip.
        /// </summary>
        /// <value>The command to move to the previous clip.</value>
        public DelegateCommand<object> MovePreviousClipCommand { get; private set; }

        /// <summary>
        /// Gets the list of available audio tracks.
        /// </summary>
        /// <value>The list of available audio tracks.</value>
        public ObservableCollection<Track> AudioTracks { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timeline is in snap mode or not.
        /// </summary>
        /// <value>A true if the timeline is in snap mode;otherwise false.</value>
        public bool IsInSnapMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timeline is in ripple mode or not.
        /// </summary>
        /// <value>A true if the timeline is in ripple mode;otherwise false.</value>
        public bool IsInRippleMode
        {
            get
            {
                return this.editMode == EditMode.Ripple;
            }

            set
            {
                EditMode newEditMode = value ? EditMode.Ripple : EditMode.Gap;

                this.NotifyNewEditingMode(newEditMode);
            }
        }

        public TimeCode TimelineDuration
        {
            get
            {
                double timelineDuration = 0;
                TimelineElement lastElement;
                foreach (Track track in this.timelineModel.Tracks)
                {
                    if (track == null || track.Shots == null || track.Shots.Count == 0)
                    {
                        continue;
                    }

                    lastElement = track.Shots.Where(x => x.Position == track.Shots.Max(y => y.Position)).FirstOrDefault();
                    if (lastElement != null)
                    {
                        double duration = lastElement.Position.TotalSeconds + lastElement.OutPosition.TotalSeconds -
                                          lastElement.InPosition.TotalSeconds;

                        timelineDuration = Math.Max(timelineDuration, duration);
                    }
                }

                return TimeCode.FromSeconds(timelineDuration, this.timelineModel.Duration.FrameRate);
            }
        }

        /// <summary>
        /// Filters the dropped item if it has valid arguments.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>true if the arguments are valid else false.</returns>
        private static bool FilterDropItem(DropPayload payload)
        {
            return payload.MouseEventArgs != null && payload.DraggedItem != null;
        }

        /// <summary>
        /// Shows the asset download progress.
        /// </summary>
        /// <param name="payload">The payload.</param>
        private void ShowAssetDownloadProgress(AssetDownloadProgressEventArgs payload)
        {
            this.View.ShowAssetDownloadProgress(payload.Element, payload.Progress, payload.Offset);
        }

        /// <summary>
        /// Sets the editing mode.
        /// </summary>
        /// <param name="mode">The <see cref="EditMode"/>.</param>
        private void SetEditingMode(EditMode mode)
        {
            this.editMode = mode;
            this.OnPropertyChanged("IsInRippleMode");
        }

        /// <summary>
        /// Adds the asset at current position.
        /// </summary>
        /// <param name="asset">The <see cref="Asset"/>.</param>
        private void AddAssetAtCurrentPosition(Asset asset)
        {
            if (asset != null)
            {
                Track layer = this.GetAssetTrack(asset);
                this.AddAssetToLayer(this.timelineModel.CurrentPosition, layer, asset);
            }
        }

        /// <summary>
        /// Updates the play head position to the given position.
        /// </summary>
        /// <param name="payload">The <see cref="RCE.Infrastructure.Events.PositionPayloadEventArgs"/> instance containing the event data.</param>
        private void UpdatePlayHead(PositionPayloadEventArgs payload)
        {
            TimeCode timeCode = TimeCode.FromSeconds(payload.Position.TotalSeconds, this.timelineModel.Duration.FrameRate);
            this.timelineModel.CurrentPosition = timeCode;
            this.View.SetPlayHeadPosition(timeCode);
        }

        /// <summary>
        /// Adds the dropped item in the timeline to the appropiate layer(Visual/Audio).
        /// </summary>
        /// <param name="payload">The <see cref="DropPayload"/>.</param>
        private void DropItem(DropPayload payload)
        {
            TitleTemplate titleTemplate = payload.DraggedItem as TitleTemplate;
            Asset asset;

            if (titleTemplate != null)
            {
                asset = new TitleAsset
                {
                    Title = titleTemplate.Title,
                    MainText = titleTemplate.MainText,
                    SubText = titleTemplate.SubText,
                    TitleTemplate = titleTemplate
                };
            }
            else
            {
                asset = payload.DraggedItem as Asset;
            }

            if (asset != null)
            {
                LayerPosition layerPosition = this.View.ResolveLayerPositionFromRelativePosition(payload.MouseEventArgs);

                if (layerPosition == null)
                {
                    return;
                }

                Track track = layerPosition.Track ?? this.GetAssetTrack(asset);

                if (track != null)
                {
                    this.AddAssetToLayer(layerPosition.Position, track, asset);

                    // Don't publish the AddAssetEvent if the asset is Title asset
                    if (!(asset is TitleAsset))
                    {
                        this.eventAggregator.GetEvent<AddAssetEvent>().Publish(asset);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the start time code from where the timeline starts.
        /// </summary>
        /// <param name="timeCode">The <see cref="TimeCode"/>.</param>
        private void UpdateStartTimeCode(TimeCode timeCode)
        {
            this.View.SetStartTimeCode(timeCode);
        }

        /// <summary>
        /// Deletes the given asset.
        /// </summary>
        /// <param name="asset">The <see cref="Asset"/>.</param>
        private void DeleteAsset(Asset asset)
        {
            IEnumerable<Track> tracks = this.GetAssetTracks(asset);

            if (tracks != null)
            {
                foreach (Track track in tracks)
                {
                    if (asset.ProviderUri != null)
                    {
                        track.Shots.Where(x => x.Asset.ProviderUri == asset.ProviderUri)
                            .ToList()
                            .ForEach(this.DeleteElement);
                    }
                    else
                    {
                        track.Shots.Where(x => x.Asset.Id == asset.Id)
                            .ToList()
                            .ForEach(this.DeleteElement);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the smpte frame rate to the given framerate.
        /// </summary>
        /// <param name="frameRate">The <see cref="SmpteFrameRate"/>.</param>
        private void UpdateSmpteFrameRate(SmpteFrameRate frameRate)
        {
            this.SetDuration(TimeCode.FromAbsoluteTime(this.timelineModel.Duration.TotalSeconds, frameRate));
        }

        /// <summary>
        /// Returns the track for the specified Asset.
        /// </summary>
        /// <param name="asset">Asset to validate.</param>
        /// <returns>Timeline's Model Track.</returns>
        private Track GetAssetTrack(Asset asset)
        {
            var videoAsset = asset as VideoAsset;
            var audioAsset = asset as AudioAsset;
            var imageAsset = asset as ImageAsset;
            var titlesAsset = asset as TitleAsset;

            if (videoAsset != null || imageAsset != null)
            {
                Track track = this.timelineModel.Tracks.FirstOrDefault(x => x.TrackType == TrackType.Visual);

                if (track == null)
                {
                    track = new Track { TrackType = TrackType.Visual };
                    this.timelineModel.Tracks.Add(track);
                    this.projectService.GetCurrentProject().Timeline.Add(track);
                }

                return track;
            }

            if (audioAsset != null)
            {
                Track track = this.timelineModel.Tracks.FirstOrDefault(x => x.TrackType == TrackType.Audio);

                if (track == null)
                {
                    track = new Track { Number = 1, TrackType = TrackType.Audio };
                    this.timelineModel.Tracks.Add(track);
                    this.projectService.GetCurrentProject().Timeline.Add(track);
                }

                return track;
            }

            if (titlesAsset != null)
            {
                Track track = this.timelineModel.Tracks.FirstOrDefault(x => x.TrackType == TrackType.Title);

                if (track == null)
                {
                    track = new Track { TrackType = TrackType.Title };
                    this.timelineModel.Tracks.Add(track);
                    this.projectService.GetCurrentProject().Timeline.Add(track);
                }

                return track;
            }

            return null;
        }

        /// <summary>
        /// Returns the track for the specified Asset.
        /// </summary>
        /// <param name="asset">Asset to validate.</param>
        /// <returns>Timeline's Model Track.</returns>
        private IEnumerable<Track> GetAssetTracks(Asset asset)
        {
            var videoAsset = asset as VideoAsset;
            var audioAsset = asset as AudioAsset;
            var imageAsset = asset as ImageAsset;
            var titlesAsset = asset as TitleAsset;

            if (videoAsset != null || imageAsset != null || titlesAsset != null)
            {
                Track track = this.GetAssetTrack(asset);
                return new List<Track> { track };
            }

            if (audioAsset != null)
            {
                return this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Audio).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the track associated with the timeline element.
        /// </summary>
        /// <param name="element">The timeline element used to look for a track.</param>
        /// <returns>The track associated with the timeline element.</returns>
        private Track GetElementTrack(TimelineElement element)
        {
            if (element.Asset is AudioAsset)
            {
                Track track = this.timelineModel.Tracks.SingleOrDefault(x => x.TrackType == TrackType.Audio && x.Shots.Contains(element));

                return track;
            }
            else
            {
                return this.GetAssetTrack(element.Asset);
            }
        }

        /// <summary>
        /// Sets the duration of the timeline.
        /// </summary>
        /// <param name="timeCode">The duration.</param>
        private void SetDuration(TimeCode timeCode)
        {
            this.timelineModel.Duration = timeCode;
            this.View.SetDuration(timeCode);
            this.SetCurrentPosition(TimeCode.FromAbsoluteTime(0, timeCode.FrameRate));
        }

        /// <summary>
        /// Get all the elements at the current playhead position.
        /// </summary>
        /// <returns>A <see cref="IList{T}"/> of <seealso cref="TimelineElement"/>.</returns>
        private IList<TimelineElement> GetElementsAtCurrentPosition()
        {
            IList<TimelineElement> elements = new List<TimelineElement>();

            foreach (Track track in this.timelineModel.Tracks)
            {
                TimelineElement element = this.timelineModel.GetElementAtPosition(this.timelineModel.CurrentPosition, track, null);
                if (element != null)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        /// <summary>
        /// Adds the asset to the timeline at the given position.
        /// </summary>
        /// <param name="position">Position where element is going to be added.</param>
        /// <param name="layer">Layer where the element is going to be added.</param>
        /// <param name="asset">Asset to be added in the timeline layer.</param>
        private void AddAssetToLayer(TimeCode position, Track layer, Asset asset)
        {
            if (layer != null)
            {
                VideoAssetInOut videoAssetInOut = asset as VideoAssetInOut;

                if (videoAssetInOut != null && videoAssetInOut.InPosition != -1 && videoAssetInOut.OutPosition != -1)
                {
                    this.AddElement(videoAssetInOut.VideoAsset, layer, position, videoAssetInOut.InPosition, videoAssetInOut.OutPosition);
                }
                else
                {
                    TimeCode duration = this.GetAssetDuration(asset);

                    this.AddElement(asset, layer, duration, position);
                }
            }
        }

        /// <summary>
        /// Returns the Asset's default duration.
        /// If the Asset does not contain a duration, the DefaultAssetDuration is returned.
        /// </summary>
        /// <param name="asset">Asset to validate.</param>
        /// <returns>TimeCode with default duration.</returns>
        private TimeCode GetAssetDuration(Asset asset)
        {
            var videoAsset = asset as VideoAsset;
            var audioAsset = asset as AudioAsset;

            if (videoAsset != null)
            {
                return videoAsset.Duration;
            }

            if (audioAsset != null)
            {
                return TimeCode.FromAbsoluteTime(audioAsset.Duration, this.timelineModel.Duration.FrameRate);
            }

            return TimeCode.FromAbsoluteTime(DefaultAssetDuration, this.timelineModel.Duration.FrameRate);
        }

        /// <summary>
        /// Returns the Asset maximun duration according to its type.
        /// </summary>
        /// <param name="asset">Asset to validate.</param>
        /// <returns>TimeCode with max duration.</returns>
        private TimeCode GetAssetMaxDuration(Asset asset)
        {
            var videoAsset = asset as VideoAsset;
            var audioAsset = asset as AudioAsset;

            if (videoAsset != null)
            {
                return videoAsset.Duration;
            }

            if (audioAsset != null)
            {
                return TimeCode.FromAbsoluteTime(audioAsset.Duration, this.timelineModel.Duration.FrameRate);
            }

            return TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate);
        }

        /// <summary>
        /// Delete current selected element.
        /// </summary>
        /// <param name="currentElement">The current element.</param>
        private void DeleteElement(TimelineElement currentElement)
        {
            if (currentElement == null)
            {
                return;
            }

            Track layer = this.GetElementTrack(currentElement);

            RemoveElementCommand command = new RemoveElementCommand(this.timelineModel, layer, this.editMode, currentElement);

            this.caretaker.ExecuteCommand(command);

            this.OnPropertyChanged("TimelineDuration");
        }

        /// <summary>
        /// Add asset to timeline with In/Out Position.
        /// </summary>
        /// <param name="asset">Asset instance.</param>
        /// <param name="layer">Timeline layer.</param>
        /// <param name="duration">Duration of the element.</param>
        /// <param name="position">Position of drop.</param>
        private void AddElement(Asset asset, Track layer, TimeCode duration, TimeCode position)
        {
            this.AddElement(asset, layer, position, 0, duration.TotalSeconds);
        }

        /// <summary>
        /// Add asset to timeline with In/Out Position.
        /// </summary>
        /// <param name="asset">Asset instance.</param>
        /// <param name="layer">Timeline layer.</param>
        /// <param name="position">Position of drop.</param>
        /// <param name="inPosition">InPosition in second from the begining.</param>
        /// <param name="outPosition">OutPosition in second from the begining.</param>
        private void AddElement(Asset asset, Track layer, TimeCode position, double inPosition, double outPosition)
        {
            TimeCode duration = TimeCode.FromAbsoluteTime(outPosition - inPosition, position.FrameRate);
            TimeCode outOffset = duration;
            bool offsetFix = false;

            if (this.editMode == EditMode.Ripple)
            {
                TimelineElement overlapElement = this.timelineModel.GetElementAtPosition(position, layer, null);

                if (overlapElement != null)
                {
                    TimelineElement nextOverlapElement = this.timelineModel.GetElementAtPosition(overlapElement.Position + overlapElement.Duration, layer, overlapElement);

                    if (nextOverlapElement != null)
                    {
                        TimeCode newEndPosition = nextOverlapElement.Position + duration;

                        while (nextOverlapElement != null)
                        {
                            TimeCode nextElementOldPosition = nextOverlapElement.Position;
                            TimeCode nextElementNewPosition = TimeCode.FromAbsoluteTime(newEndPosition.TotalSeconds, overlapElement.Position.FrameRate);

                            this.timelineModel.MoveElement(nextOverlapElement, layer, nextElementNewPosition);
                            newEndPosition = newEndPosition + nextOverlapElement.Duration;
                            this.View.RefreshElement(nextOverlapElement);
                            this.PublishElementMovedEvent(nextOverlapElement, ElementPositionType.Position, nextElementOldPosition, nextElementNewPosition);

                            nextOverlapElement = this.timelineModel.GetElementAtPosition(newEndPosition, layer, nextOverlapElement);
                        }

                        position = overlapElement.OutPosition - overlapElement.InPosition;
                    }
                }

                if (this.timelineModel.Duration.TotalSeconds <= this.TimelineDuration.TotalSeconds)
                {
                    double exceededDuration = this.TimelineDuration.TotalSeconds - this.timelineModel.Duration.TotalSeconds;
                    this.IncreaseTimelineDuration(exceededDuration);
                }
            }

            if (this.IsInSnapMode)
            {
                TimelineElement previousElement = this.timelineModel.GetPreviousElement(position, layer);

                if (previousElement == null)
                {
                    position = TimeCode.FromSeconds(0d, position.FrameRate);
                }
                else
                {
                    position = TimeCode.FromAbsoluteTime(previousElement.Position.TotalSeconds + previousElement.Duration.TotalSeconds, position.FrameRate);
                }
            }

            // fix start position (do not overlay with other assets, move to next available position)
            TimelineElement nextElement = this.timelineModel.GetElementAtPosition(position, layer, null);
            while (nextElement != null)
            {
                offsetFix = true;
                position = TimeCode.FromAbsoluteTime(nextElement.Position.TotalSeconds + nextElement.Duration.TotalSeconds, position.FrameRate);
                nextElement = this.timelineModel.GetElementAtPosition(position, layer, nextElement);
            }

            // fix end position (do not overlay with other assets, trim to fit)
            nextElement = this.timelineModel.GetElementWithinRange(position, position + duration, layer, null);
            if (nextElement != null)
            {
                outOffset = TimeCode.FromAbsoluteTime(nextElement.Position.TotalSeconds - position.TotalSeconds, position.FrameRate);
                offsetFix = true;
            }

            // do not exceed timeline duration (trim to fit)
            if (position + duration >= this.timelineModel.Duration)
            {
                if (!offsetFix)
                {
                    // move to fit
                    // TimeCode newPosition = TimeCode.FromAbsoluteTime(this.timelineModel.Duration.TotalSeconds - duration.TotalSeconds, this.timelineModel.Duration.FrameRate);
                    if (this.timelineModel.GetElementWithinRange(position, this.timelineModel.Duration, layer, null) != null)
                    {
                        TimelineElement lastElement = layer.Shots[layer.Shots.Count - 1];

                        position = TimeCode.FromAbsoluteTime(lastElement.Position.TotalSeconds + lastElement.Duration.TotalSeconds, position.FrameRate);
                    }
                }

                if (outOffset == duration)
                {
                    // trim to fit
                    // outOffset = TimeCode.FromAbsoluteTime(duration.TotalSeconds - ((position.TotalSeconds + duration.TotalSeconds) - this.timelineModel.Duration.TotalSeconds), position.FrameRate);

                    // Increase Timeline Duration
                    double exceededDuration = (position.TotalSeconds + duration.TotalSeconds) - this.timelineModel.Duration.TotalSeconds;

                    this.IncreaseTimelineDuration(exceededDuration);
                }
            }

            TimelineElement element = new TimelineElement
            {
                Asset = asset,
                InPosition = TimeCode.FromAbsoluteTime(inPosition, position.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(outOffset.TotalSeconds + inPosition, position.FrameRate),
                Position = position
            };

            if (element.Duration.TotalSeconds > 0.0)
            {
                AddElementCommand command = new AddElementCommand(this.timelineModel, layer, element);

                this.caretaker.ExecuteCommand(command);

                this.OnPropertyChanged("TimelineDuration");
            }
        }

        private void IncreaseTimelineDuration(double exceededDuration)
        {
            TimeCode newDuration = this.timelineModel.Duration + TimeCode.FromSeconds(exceededDuration, this.timelineModel.Duration.FrameRate) + TimeCode.FromMinutes(15, this.timelineModel.Duration.FrameRate);
            this.SetDuration(newDuration);
        }

        /// <summary>
        /// Move the selected element to a <paramref name="newPosition">new position.</paramref>
        /// </summary>
        /// <param name="newPosition">The new position where the selected element is being positioned.</param>
        private void MoveSelectedElement(TimeCode newPosition)
        {
            if (this.selectedElement == null)
            {
                throw new InvalidOperationException("No elements are selected");
            }

            Track track = this.GetElementTrack(this.selectedElement);

            if (track != null)
            {
                this.MoveElement(this.selectedElement, track, newPosition);
            }
        }

        /// <summary>
        /// Moves an element of a specific layer to a new position.
        /// </summary>
        /// <param name="element">The element being moved.</param>
        /// <param name="layer">The track where the element belongs to.</param>
        /// <param name="newPosition">The new position where the selected element ins being positioned.</param>
        private void MoveElement(TimelineElement element, Track layer, TimeCode newPosition)
        {
            var oldPosition = element.Position;
            var offsetFix = false;

            if (newPosition.TotalSeconds < 0)
            {
                newPosition = TimeCode.FromAbsoluteTime(0, newPosition.FrameRate);
            }

            if (newPosition > oldPosition)
            {
                element = this.timelineModel.FindLastElementLinking(element, layer);
                newPosition = TimeCode.FromSeconds(element.Position.TotalSeconds + (newPosition.TotalSeconds - oldPosition.TotalSeconds), newPosition.FrameRate);
                oldPosition = element.Position;
            }
            else if (newPosition < oldPosition)
            {
                element = this.timelineModel.FindFirstElementLinking(element, layer);
                newPosition = TimeCode.FromSeconds(element.Position.TotalSeconds - (oldPosition.TotalSeconds - newPosition.TotalSeconds), newPosition.FrameRate);
                oldPosition = element.Position;
            }

            // fix start position (do not overlay with other assets)
            // var nextElement = this.timelineModel.GetElementAtPosition(newPosition, layer, element);
            TimeCode offset = TimeCode.FromSeconds((element.Duration.TotalSeconds * 15) / 100, element.Duration.FrameRate);

            var nextElement = this.timelineModel.GetElementWithinRange(newPosition, newPosition + element.Duration - offset, layer, element);
            while (nextElement != null && nextElement != element)
            {
                offsetFix = true;
                newPosition = nextElement.Position + nextElement.Duration;

                // nextElement = this.timelineModel.GetElementAtPosition(newPosition, layer, nextElement);
                nextElement = this.timelineModel.GetElementWithinRange(newPosition, newPosition + element.Duration, layer, nextElement);
            }

            // fix end position (do not overlay with other assets)
            nextElement = this.timelineModel.GetElementWithinRange(newPosition, newPosition + element.Duration, layer, element);
            while (nextElement != null && nextElement != element)
            {
                offsetFix = true;
                newPosition = TimeCode.FromSeconds(nextElement.Position.TotalSeconds - element.Duration.TotalSeconds, newPosition.FrameRate);

                // nextElement = this.timelineModel.GetElementAtPosition(newPosition, layer, nextElement);
                nextElement = this.timelineModel.GetElementWithinRange(newPosition, newPosition + element.Duration, layer, nextElement);

                if (nextElement != null && nextElement.Position + nextElement.Duration == newPosition)
                {
                    nextElement = null;
                }
            }

            if (newPosition < TimeCode.FromAbsoluteTime(0, newPosition.FrameRate))
            {
                if (!offsetFix && this.timelineModel.GetElementWithinRange(TimeCode.FromAbsoluteTime(0, newPosition.FrameRate), element.Duration, layer, element) == null)
                {
                    newPosition = TimeCode.FromAbsoluteTime(0, newPosition.FrameRate);
                }
                else
                {
                    // already fixed, invalidate new position
                    newPosition = oldPosition;
                }
            }
            else if (newPosition + element.Duration > this.timelineModel.Duration)
            {
                if (!offsetFix && this.timelineModel.GetElementWithinRange(this.timelineModel.Duration - element.Duration, this.timelineModel.Duration, layer, element) == null)
                {
                    newPosition = TimeCode.FromAbsoluteTime(this.timelineModel.Duration.TotalSeconds - element.Duration.TotalSeconds, this.timelineModel.Duration.FrameRate);
                }
                else
                {
                    // already fixed, invalidate new position
                    newPosition = oldPosition;
                }
            }

            this.timelineModel.MoveElement(element, layer, newPosition);
            this.View.RefreshElement(element);
            this.PublishElementMovedEvent(element, ElementPositionType.Position, oldPosition, newPosition);

            // tooltip
            var tooltipPosition = newPosition;
            var layerPosition = new LayerPosition
            {
                Track = layer,
                Position = tooltipPosition,
                LayerType = layer.TrackType
            };

            this.View.ShowTooltip(tooltipPosition.ToString(), layerPosition);

            TimelineElementLink link = this.timelineModel.GetElementLink(element);

            if (newPosition > oldPosition)
            {
                TimelineElement previousElement = layer.Shots.Where(e => e.Id == link.PreviousElementId).SingleOrDefault();

                while (previousElement != null)
                {
                    TimeCode previousElementOldPosition = previousElement.Position;
                    TimeCode previousElementNewPosition = previousElement.Position + (newPosition - oldPosition);
                    this.timelineModel.MoveElement(previousElement, layer, previousElementNewPosition);
                    this.View.RefreshElement(previousElement);
                    this.PublishElementMovedEvent(previousElement, ElementPositionType.Position, previousElementOldPosition, previousElementNewPosition);
                    link = this.timelineModel.GetElementLink(previousElement);
                    previousElement = layer.Shots.Where(e => e.Id == link.PreviousElementId).SingleOrDefault();
                }
            }
            else if (newPosition < oldPosition)
            {
                nextElement = layer.Shots.Where(e => e.Id == link.NextElementId).SingleOrDefault();

                while (nextElement != null)
                {
                    TimeCode nextElementOldPosition = nextElement.Position;
                    TimeCode nextElementNewPosition = nextElement.Position - (oldPosition - newPosition);
                    this.timelineModel.MoveElement(nextElement, layer, nextElementNewPosition);
                    this.View.RefreshElement(nextElement);
                    this.PublishElementMovedEvent(nextElement, ElementPositionType.Position, nextElementOldPosition, nextElementNewPosition);
                    link = this.timelineModel.GetElementLink(nextElement);
                    nextElement = layer.Shots.Where(e => e.Id == link.NextElementId).SingleOrDefault();
                }
            }
        }

        /// <summary>
        /// Publishes the ElementMovedEvent every time an element is being moved.
        /// </summary>
        /// <param name="element">The element that was moved.</param>
        /// <param name="positionType">The type of position.</param>
        /// <param name="oldPosition">The old position of the element.</param>
        /// <param name="newPosition">The new position of the element.</param>
        private void PublishElementMovedEvent(TimelineElement element, ElementPositionType positionType, TimeCode oldPosition, TimeCode newPosition)
        {
            ElementMovedPayload payload = new ElementMovedPayload(element, positionType, oldPosition, newPosition);
            this.eventAggregator.GetEvent<ElementMovedEvent>().Publish(payload);
            this.OnPropertyChanged("TimelineDuration");
        }

        /// <summary>
        /// Sets the current position of the timeline.
        /// </summary>
        /// <param name="timeCode">The new position.</param>
        private void SetCurrentPosition(TimeCode timeCode)
        {
            this.timelineModel.CurrentPosition = timeCode;
            this.eventAggregator.GetEvent<PlayheadMovedEvent>().Publish(new PositionPayloadEventArgs(TimeSpan.FromSeconds(timeCode.TotalSeconds)));
            this.View.SetPlayHeadPosition(timeCode);
        }

        /// <summary>
        /// Trims the left position of the selected element.
        /// </summary>
        /// <param name="absolutePosition">The new position of the element.</param>
        private void TrimLeftSelectedElement(TimeCode absolutePosition)
        {
            if (this.selectedElement == null)
            {
                throw new InvalidOperationException("No element is selected");
            }

            Track track = this.GetElementTrack(this.selectedElement);
            double maxDuration = this.GetAssetMaxDuration(this.selectedElement.Asset).TotalSeconds;

            TimeCode offset = TimeCode.FromAbsoluteTime(absolutePosition.TotalSeconds - this.selectedElement.Position.TotalSeconds, absolutePosition.FrameRate);
            TimeCode newInPosition = this.selectedElement.InPosition + offset;
            TimeCode newDuration = TimeCode.FromSeconds(this.selectedElement.OutPosition.TotalSeconds - newInPosition.TotalSeconds, this.selectedElement.Duration.FrameRate);

            // validate trim (MINIMAL LENGTH)
            TimeCode minimumDuration = TimeCode.FromAbsoluteTime(MinimumElementDuration, newDuration.FrameRate);
            if (newDuration < minimumDuration)
            {
                newInPosition = this.selectedElement.OutPosition - minimumDuration;
                offset = TimeCode.FromSeconds(newInPosition.TotalSeconds - this.selectedElement.InPosition.TotalSeconds, newInPosition.FrameRate);
                newDuration = minimumDuration;
            }

            // validate trim (VIDEO/AUDIO MAX LENGTH)
            if (maxDuration > 0)
            {
                if (newInPosition.TotalSeconds < 0)
                {
                    newInPosition = TimeCode.FromAbsoluteTime(0, newInPosition.FrameRate);
                    offset = TimeCode.FromSeconds(newInPosition.TotalSeconds - this.selectedElement.InPosition.TotalSeconds, newInPosition.FrameRate);
                }
            }

            TimeCode newPosition = this.selectedElement.Position + offset;

            EditMode currentEditMode = this.editMode;

            TimelineElementLink link = this.timelineModel.GetElementLink(this.selectedElement);

            // If element has link, should behavior as in Ripple Mode
            if (link.PreviousElementId != Guid.Empty)
            {
                currentEditMode = EditMode.Ripple;
            }

            switch (currentEditMode)
            {
                case EditMode.Gap:
                    {
                        // GAP MODE, validate OVERLAPPING
                        TimelineElement prevElement = this.timelineModel.GetElementWithinRange(newPosition, newPosition + newDuration, track, this.selectedElement);
                        if (prevElement != null)
                        {
                            TimeCode oldPosition = newPosition;
                            newPosition = prevElement.Position + prevElement.Duration;
                            offset = TimeCode.FromAbsoluteTime(oldPosition.TotalSeconds - newPosition.TotalSeconds, newPosition.FrameRate);
                            newInPosition = TimeCode.FromAbsoluteTime(newInPosition.TotalSeconds - offset.TotalSeconds, newInPosition.FrameRate);
                        }
                    }

                    break;
                case EditMode.Ripple:
                    {
                        TimelineElement currElement = this.selectedElement;
                        TimelineElement prevElement = this.timelineModel.GetElementAtPosition(currElement.Position, track, currElement) ??
                                          this.timelineModel.GetElementWithinRange(newPosition, newPosition + currElement.Duration, track, currElement);

                        if (prevElement != null)
                        {
                            // RIPPLE MODE, move any adjacent elements
                            if (newPosition < this.selectedElement.Position)
                            {
                                // Move previous elements backward
                                while (prevElement != null)
                                {
                                    TimeCode prevElementOldPosition = prevElement.Position;
                                    TimeCode prevElementNewPosition = TimeCode.FromAbsoluteTime(currElement.Position.TotalSeconds - prevElement.Duration.TotalSeconds + offset.TotalSeconds, prevElement.Position.FrameRate);

                                    this.timelineModel.MoveElement(prevElement, track, prevElementNewPosition);
                                    this.View.RefreshElement(prevElement);
                                    this.PublishElementMovedEvent(prevElement, ElementPositionType.Position, prevElementOldPosition, prevElementNewPosition);

                                    currElement = prevElement;
                                    prevElement = this.timelineModel.GetElementAtPosition(currElement.Position, track, currElement);
                                    offset = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate);
                                }
                            }
                            else
                            {
                                // Move previous elements forward
                                TimeCode newPrevPosition = newPosition;
                                while (prevElement != null)
                                {
                                    TimeCode prevElementOldPosition = prevElement.Position;
                                    TimeCode prevElementNewPosition = TimeCode.FromAbsoluteTime(newPrevPosition.TotalSeconds - prevElement.Duration.TotalSeconds, prevElement.Position.FrameRate);

                                    this.timelineModel.MoveElement(prevElement, track, prevElementNewPosition);
                                    newPrevPosition = prevElement.Position;
                                    this.View.RefreshElement(prevElement);
                                    this.PublishElementMovedEvent(prevElement, ElementPositionType.Position, prevElementOldPosition, prevElementNewPosition);

                                    currElement = prevElement;
                                    prevElement = this.timelineModel.GetElementAtPosition(prevElementOldPosition, track, currElement);
                                }
                            }
                        }
                    }

                    break;
            }

            this.timelineModel.MoveElement(this.selectedElement, track, newPosition);
            TimeCode oldInPosition = this.selectedElement.InPosition;
            this.selectedElement.InPosition = newInPosition;
            this.View.RefreshElement(this.selectedElement);
            this.PublishElementMovedEvent(this.selectedElement, ElementPositionType.InPosition, oldInPosition, newInPosition);

            // Tooltip
            LayerPosition layerPosition = new LayerPosition
            {
                Track = track,
                Position = newPosition,
                LayerType = track.TrackType
            };

            this.View.ShowTooltip(newPosition.ToString(), layerPosition);
        }

        /// <summary>
        /// Trims the right position of the element.
        /// </summary>
        /// <param name="absolutePosition">The new position of the element.</param>
        private void TrimRightSelectedElement(TimeCode absolutePosition)
        {
            if (this.selectedElement == null)
            {
                throw new InvalidOperationException("No element is selected");
            }

            var layer = this.GetElementTrack(this.selectedElement);
            var maxDuration = this.GetAssetMaxDuration(this.selectedElement.Asset).TotalSeconds;

            var newOutPosition = TimeCode.FromAbsoluteTime(absolutePosition.TotalSeconds - this.selectedElement.Position.TotalSeconds + this.selectedElement.InPosition.TotalSeconds, absolutePosition.FrameRate);
            var newDuration = TimeCode.FromAbsoluteTime(newOutPosition.TotalSeconds - this.selectedElement.InPosition.TotalSeconds, newOutPosition.FrameRate);

            // validate trim (MINIMAL LENGTH)
            var minimumDuration = TimeCode.FromAbsoluteTime(MinimumElementDuration, newDuration.FrameRate);
            if (newDuration < minimumDuration)
            {
                newOutPosition = TimeCode.FromAbsoluteTime(this.selectedElement.InPosition.TotalSeconds + minimumDuration.TotalSeconds, newOutPosition.FrameRate);
            }

            // validate trim (VIDEO/AUDIO MAX LENGTH)
            if (maxDuration > 0)
            {
                if (newOutPosition.TotalSeconds > maxDuration)
                {
                    newOutPosition = TimeCode.FromAbsoluteTime(maxDuration, newOutPosition.FrameRate);
                }
            }

            var newEndPosition = this.selectedElement.Position + (newOutPosition - this.selectedElement.InPosition);
            var offset = TimeCode.FromSeconds(newOutPosition.TotalSeconds - this.selectedElement.OutPosition.TotalSeconds, newOutPosition.FrameRate);

            var currentEditMode = this.editMode;

            TimelineElementLink link = this.timelineModel.GetElementLink(this.selectedElement);

            // If element has link, shoud behavior as in Ripple Mode
            if (link.NextElementId != Guid.Empty)
            {
                currentEditMode = EditMode.Ripple;
            }

            switch (currentEditMode)
            {
                case EditMode.Gap:
                    {
                        // GAP MODE, validate trim (OVERLAPPING)
                        var nextElement = this.timelineModel.GetElementWithinRange(this.selectedElement.Position, newEndPosition, layer, this.selectedElement);
                        if (nextElement != null)
                        {
                            newEndPosition = nextElement.Position;

                            newOutPosition = TimeCode.FromAbsoluteTime(newEndPosition.TotalSeconds - this.selectedElement.Position.TotalSeconds + this.selectedElement.InPosition.TotalSeconds, this.selectedElement.Position.FrameRate);
                        }
                    }

                    break;
                case EditMode.Ripple:
                    {
                        var currElement = this.selectedElement;
                        var nextElement = this.timelineModel.GetElementAtPosition(currElement.Position + currElement.Duration, layer, currElement) ??
                                          this.timelineModel.GetElementWithinRange(currElement.Position + currElement.Duration, currElement.Position + newDuration, layer, currElement);

                        // RIPPLE MODE, move any adjacent elements
                        if (newOutPosition < this.selectedElement.OutPosition)
                        {
                            // Move next elements backward
                            while (nextElement != null)
                            {
                                TimeCode nextElementOldPosition = nextElement.Position;
                                TimeCode nextElementNewPosition = TimeCode.FromAbsoluteTime(nextElement.Position.TotalSeconds + offset.TotalSeconds, nextElement.Position.FrameRate);

                                this.timelineModel.MoveElement(nextElement, layer, nextElementNewPosition);
                                this.View.RefreshElement(nextElement);
                                this.PublishElementMovedEvent(nextElement, ElementPositionType.Position, nextElementOldPosition, nextElementNewPosition);

                                currElement = nextElement;
                                nextElement = this.timelineModel.GetElementAtPosition(nextElementOldPosition + currElement.Duration, layer, currElement);

                                // offset = TimeCode.FromAbsoluteTime(0, this.Model.Duration.FrameRate);
                            }
                        }
                        else
                        {
                            // Move next elements fordward
                            while (nextElement != null)
                            {
                                TimeCode nextElementOldPosition = nextElement.Position;
                                TimeCode nextElementNewPosition = TimeCode.FromAbsoluteTime(newEndPosition.TotalSeconds, nextElement.Position.FrameRate);

                                this.timelineModel.MoveElement(nextElement, layer, nextElementNewPosition);
                                newEndPosition = newEndPosition + nextElement.Duration;
                                this.View.RefreshElement(nextElement);
                                this.PublishElementMovedEvent(nextElement, ElementPositionType.Position, nextElementOldPosition, nextElementNewPosition);

                                currElement = nextElement;
                                nextElement = this.timelineModel.GetElementAtPosition(newEndPosition, layer, currElement);
                            }
                        }
                    }

                    break;
            }

            TimeCode oldOutPosition = this.selectedElement.OutPosition;
            this.selectedElement.OutPosition = newOutPosition;
            this.View.RefreshElement(this.selectedElement);
            this.PublishElementMovedEvent(this.selectedElement, ElementPositionType.OutPosition, oldOutPosition, newOutPosition);

            // tooltip
            var tooltipPosition = TimeCode.FromAbsoluteTime(this.selectedElement.Duration.TotalSeconds + this.selectedElement.Position.TotalSeconds, this.timelineModel.Duration.FrameRate);
            var layerPosition = new LayerPosition
            {
                Track = layer,
                Position = tooltipPosition,
                LayerType = layer.TrackType
            };

            this.View.ShowTooltip(tooltipPosition.ToString(), layerPosition);
        }

        /// <summary>
        /// Handles the TrimElementAtCurrentPositionChange event and 
        /// trims the element(In/Out) by the given time code value.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">ElementPositionChangeEventArgs event args.</param>
        private void View_TrimElementAtCurrentPositionChange(object sender, ElementPositionChangeEventArgs e)
        {
            TimelineElement currentSelectedElement = this.selectedElement;
            IList<TimelineElement> elements = this.GetElementsAtCurrentPosition();

            if (elements != null && elements.Count > 0)
            {
                IList<LayerSnapshotCommand> layerSnapshotCommands = new List<LayerSnapshotCommand>();

                foreach (TimelineElement element in elements)
                {
                    Track track = this.GetElementTrack(element);
                    IList<TimelineElement> snapshot = track.GetMemento();

                    this.selectedElement = element;

                    switch (e.PositionType)
                    {
                        case ElementPositionType.InPosition:
                            this.TrimLeftSelectedElement(this.timelineModel.CurrentPosition);
                            this.View.HideTooltip();
                            break;
                        case ElementPositionType.OutPosition:
                            this.TrimRightSelectedElement(this.timelineModel.CurrentPosition);
                            this.View.HideTooltip();
                            break;
                        default:
                            break;
                    }

                    LayerSnapshotCommand command = new LayerSnapshotCommand(this.timelineModel, track, snapshot, track.GetMemento());
                    layerSnapshotCommands.Add(command);
                }

                TimelineSnapshotCommand timelineSnapshotCommand = new TimelineSnapshotCommand(layerSnapshotCommands);
                this.caretaker.ExecuteCommand(timelineSnapshotCommand);
            }

            this.selectedElement = currentSelectedElement;
        }

        /// <summary>
        /// Handles the PositionChange event of the View. Sets the current position to the model 
        /// and notifies about the new position to others.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the new position.</param>
        private void View_PositionChange(object sender, PositionChangeEventArgs e)
        {
            this.SetCurrentPosition(e.NewPosition);
        }

        /// <summary>
        /// Handles the ElementSelect event of the View. Unselects the current element and select the new one.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element to select.</param>
        private void View_ElementSelect(object sender, ElementSelectEventArgs e)
        {
            if (e.Element != null)
            {
                if (this.selectedElement != null)
                {
                    this.View.UnselectElement(this.selectedElement);
                    this.selectedElement = null;
                }

                this.selectedElement = e.Element;
                this.View.SelectElement(this.selectedElement);
            }
            else
            {
                this.View.HideTooltip();
            }
        }

        /// <summary>
        /// Handles the ChangeElementPosition event of the View. Based on the type of position change decides what action to do.
        /// (Moves the selected element, trims left the selected element or trims right the selected element).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element to select.</param>
        private void View_ChangeElementPosition(object sender, ElementPositionChangeEventArgs e)
        {
            switch (e.PositionType)
            {
                case ElementPositionType.Position:
                    this.MoveSelectedElement(e.NewPosition);
                    break;
                case ElementPositionType.InPosition:
                    this.TrimLeftSelectedElement(e.NewPosition);
                    break;
                case ElementPositionType.OutPosition:
                    this.TrimRightSelectedElement(e.NewPosition);
                    break;
            }
        }

        /// <summary>
        /// Handles the ShowingLinks event of the View. Shows the links of an element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element which links are going to be shown.</param>
        private void View_ShowingLinks(object sender, ElementLinkEventArgs e)
        {
            this.ShowLinks(e.Element);
        }

        /// <summary>
        /// Handles the HidingLinks event of the View. Hides the links of an element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element which links are going to be hidden.</param>
        private void View_HidingLinks(object sender, ElementLinkEventArgs e)
        {
            this.HideLinks(e.Element);
        }

        /// <summary>
        /// Handles the LinkingElement event of the View. Toggle the linking of an element by executing the a ToggleLinkElementCommand.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element.</param>
        private void View_LinkingElement(object sender, LinkElementEventArgs e)
        {
            if (e.Element == null)
            {
                return;
            }

            Track track = this.GetElementTrack(e.Element);

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, e.Element, e.LinkPosition);
            this.caretaker.ExecuteCommand(command);
        }

        /// <summary>
        /// Shows the links of the <paramref name="element"/> passed.
        /// </summary>
        /// <param name="element">The timeline element to show links.</param>
        private void ShowLinks(TimelineElement element)
        {
            if (element == null)
            {
                return;
            }

            Track track = this.GetElementTrack(element);

            TimelineElement previousElement = this.timelineModel.GetElementAtPosition(element.Position, track, element);

            if (previousElement != null)
            {
                bool linked = this.timelineModel.IsElementLinkedTo(element, previousElement);
                this.View.ShowLink(LinkPosition.In, linked, element);
            }

            TimelineElement nextElement = this.timelineModel.GetElementAtPosition(element.Position + element.Duration, track, element);

            if (nextElement != null)
            {
                bool linked = this.timelineModel.IsElementLinkedTo(element, nextElement);
                this.View.ShowLink(LinkPosition.Out, linked, element);
            }
        }

        /// <summary>
        /// Hides the links of the <paramref name="element"/> passed.
        /// </summary>
        /// <param name="element">The timeline element to hide links.</param>
        private void HideLinks(TimelineElement element)
        {
            if (element == null)
            {
                return;
            }

            this.View.HideLink(LinkPosition.In, element);
            this.View.HideLink(LinkPosition.Out, element);
        }

        /// <summary>
        /// Handles the Slit event of the View. Split the elements under the current position.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void View_Split(object sender, EventArgs e)
        {
            this.SplitElements();
        }

        /// <summary>
        /// Handles the MovingPlayhead event of the view. Publishes the <see cref="PauseEvent"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void View_MovingPlayhead(object sender, EventArgs e)
        {
            this.eventAggregator.GetEvent<PauseEvent>().Publish(null);
        }

        /// <summary>
        /// Handles the TogglePlay event of the view. Publishes the <see cref="PlayerEvent"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void View_TogglePlay(object sender, EventArgs e)
        {
            this.eventAggregator.GetEvent<PlayerEvent>().Publish(new PlayerEventPayload { PlayerMode = PlayerMode.Timeline });
        }

        /// <summary>
        /// Handles the Delete event of the view. Deletes the selected element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void View_Delete(object sender, EventArgs e)
        {
            this.DeleteElement(this.selectedElement);
        }

        /// <summary>
        /// Splits the elements under the current position.
        /// </summary>
        private void SplitElements()
        {
            TimelineElement currentSelectedElement = this.selectedElement;

            foreach (Track track in this.timelineModel.Tracks)
            {
                IList<TimelineElement> elements = this.timelineModel.GetElementsAtPosition(this.timelineModel.CurrentPosition, track);

                if (elements != null && elements.Count > 0)
                {
                    foreach (TimelineElement element in elements)
                    {
                        var newElement = new TimelineElement
                        {
                            Asset = element.Asset,
                            InPosition = element.InPosition,
                            OutPosition = element.OutPosition,
                            Position = element.Position,
                            Volume = element.Volume
                        };

                        element.OutPosition = TimeCode.FromSeconds(this.timelineModel.CurrentPosition.TotalSeconds - element.Position.TotalSeconds + element.InPosition.TotalSeconds, element.Position.FrameRate);

                        newElement.InPosition = TimeCode.FromSeconds(element.OutPosition.TotalSeconds, element.Position.FrameRate);
                        newElement.Position += element.Duration;

                        this.View.RefreshElement(element);
                        this.AddElement(newElement.Asset, track, newElement.Position, newElement.InPosition.TotalSeconds, newElement.OutPosition.TotalSeconds);
                    }
                }
            }

            this.View.SelectElement(currentSelectedElement);
        }

        /// <summary>
        /// Handles the TopBarDoubleClicked event of the View. Publishes the <see cref="PositionDoubleClickedEvent"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the position being clicked.</param>
        private void View_TopBarDoubleClicked(object sender, PositionPayloadEventArgs e)
        {
            this.eventAggregator.GetEvent<PositionDoubleClickedEvent>().Publish(e);
        }

        /// <summary>
        /// Handles the RefreshingElements event of the View. Publishes the <see cref="RefreshElementsEvent"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void View_RefreshingElements(object sender, RefreshElementsEventArgs e)
        {
            this.eventAggregator.GetEvent<RefreshElementsEvent>().Publish(e);
        }

        /// <summary>
        /// Handles the Undo event of the View. Undo the latest operation done. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void View_Undo(object sender, EventArgs e)
        {
            this.caretaker.Undo();
        }

        /// <summary>
        /// Handles the Redo event of the View. Redo the latest operation undoned.
        /// </summary>
        /// <param name="sender">Thhe sender.</param>
        /// <param name="e">The event args.</param>
        private void View_Redo(object sender, EventArgs e)
        {
            this.caretaker.Redo();
        }

        /// <summary>
        /// Handles the StopMoving event of the view. Executes a <see cref="LayerSnapshotCommand"/> using the track of the asset of the element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element.</param>
        private void View_StopMoving(object sender, Infrastructure.DataEventArgs<TimelineElement> e)
        {
            if (this.layerSnapshot != null)
            {
                Track track = this.GetElementTrack(e.Data);

                LayerSnapshotCommand command = new LayerSnapshotCommand(this.timelineModel, track, this.layerSnapshot, track.GetMemento());
                this.caretaker.ExecuteCommand(command);

                this.layerSnapshot = null;
            }
        }

        /// <summary>
        /// Event handler for the ToggleEditMode event of the View. Publish the EditModeChangedEvent.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void View_ToggleEditMode(object sender, EventArgs e)
        {
            EditMode newEditMode = this.editMode == EditMode.Gap ? EditMode.Ripple : EditMode.Gap;

            this.NotifyNewEditingMode(newEditMode);
        }

        private void NotifyNewEditingMode(EditMode newEditMode)
        {
            this.editMode = newEditMode;
            this.eventAggregator.GetEvent<EditModeChangedEvent>().Publish(this.editMode);
            this.OnPropertyChanged("IsInRippleMode");
        }

        /// <summary>
        /// Handles the StartMoving event of the View. Gets a memento of the layer of the asset.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element being moved.</param>
        private void View_StartMoving(object sender, Infrastructure.DataEventArgs<TimelineElement> e)
        {
            Track track = this.GetElementTrack(e.Data);
            this.layerSnapshot = track.GetMemento();
        }

        /// <summary>
        /// Handles the PickThumbnail event. Publish the PickThumbnailEvent.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void View_PickThumbnail(object sender, EventArgs e)
        {
            this.eventAggregator.GetEvent<PickThumbnailEvent>().Publish(null);
        }

        /// <summary>
        /// Handles the ElementAdded event of the TimelineModel. Addes the new element to the view, unselects the current element 
        /// and selects the just added element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element added.</param>
        private void TimelineModel_ElementAdded(object sender, TimelineElementEventArgs e)
        {
            if (e.Element != null)
            {
                this.View.AddElement(e.Element);

                if (this.selectedElement != null)
                {
                    this.View.UnselectElement(this.selectedElement);
                }

                this.selectedElement = e.Element;
                this.View.SelectElement(e.Element);
            }
        }

        /// <summary>
        /// Handles the ElementRemoved event of the TimelineModel. Removes the element from the view, 
        /// and cleans the unselected element if was the removed element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element removed.</param>
        private void TimelineModel_ElementRemoved(object sender, TimelineElementEventArgs e)
        {
            if (e.Element != null)
            {
                this.View.RemoveElement(e.Element);

                if (e.Element == this.selectedElement)
                {
                    this.selectedElement = null;
                }
            }
        }

        /// <summary>
        /// Handles the ElementMoved event of the TimelineModel. Refreshes the element moved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element moved.</param>
        private void TimelineModel_ElementMoved(object sender, TimelineElementEventArgs e)
        {
            if (e.Element != null)
            {
                this.View.RefreshElement(e.Element);
            }
        }

        /// <summary>
        /// Handles the ElementUnliked event of the TimelineModel. Hides the link of the element unliked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element unlinked.</param>
        private void TimelineModel_ElementUnlinked(object sender, TimelineElementEventArgs e)
        {
            if (e.Element != null)
            {
                this.HideLinks(e.Element);
            }
        }

        /// <summary>
        /// Handles the ElementLinked event of the TimelineModel. Shows the links of the element linked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the element moved.</param>
        private void TimelineModel_ElementLinked(object sender, TimelineElementEventArgs e)
        {
            if (e.Element != null)
            {
                this.ShowLinks(e.Element);
            }
        }

        /// <summary>
        /// Loads the timeline of the current project.
        /// </summary>
        private void LoadTimeline()
        {
            if (this.projectService.State != ProjectState.Retrieved)
            {
                this.projectService.ProjectRetrieved += (sender, e) => this.LoadTimeline(this.projectService.GetCurrentProject());
            }
            else
            {
                this.LoadTimeline(this.projectService.GetCurrentProject());
            }
        }

        /// <summary>
        /// Loads the timeline of the given project.
        /// </summary>
        /// <param name="project">The project with the timeline to be loaded.</param>
        private void LoadTimeline(Project project)
        {
            if (project != null)
            {
                this.View.SetStartTimeCode(project.StartTimeCode);

                foreach (Track track in project.Timeline)
                {
                    this.timelineModel.AddTrack(track);

                    TimelineElement[] currentShots = new TimelineElement[track.Shots.Count];
                    track.Shots.CopyTo(currentShots);

                    track.Shots.Clear();

                    foreach (TimelineElement element in currentShots)
                    {
                        // TODO: Add overloads that receives an element
                        this.AddElement(element.Asset, track, element.Position, element.InPosition.TotalSeconds, element.OutPosition.TotalSeconds);

                        this.selectedElement.Volume = element.Volume;
                        this.selectedElement.ProviderUri = element.ProviderUri;

                        foreach (Comment comment in element.Comments)
                        {
                            this.selectedElement.Comments.Add(comment);
                        }
                    }
                }

                AdOpportunity[] currentAdOpportunities = new AdOpportunity[project.AdOpportunities.Count];
                project.AdOpportunities.CopyTo(currentAdOpportunities, 0);

                foreach (AdOpportunity adOpportunity in currentAdOpportunities)
                {
                    this.eventAggregator.GetEvent<AddPreviewEvent>().Publish(new AddPreviewPayload("Ad", adOpportunity));
                }

                Marker[] currentMarkers = new Marker[project.Markers.Count];
                project.Markers.CopyTo(currentMarkers, 0);

                foreach (Marker marker in currentMarkers)
                {
                    this.eventAggregator.GetEvent<AddPreviewEvent>().Publish(new AddPreviewPayload("Marker", marker));
                }
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the tracks collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args containing event data.</param>
        private void Tracks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems.Count > 0)
                    {
                        Track track = e.NewItems[0] as Track;

                        if (track != null && track.TrackType == TrackType.Audio)
                        {
                            this.AudioTracks.Add(track);

                            this.AddAudioTrackCommand.RaiseCanExecuteChanged();
                            this.RemoveAudioTrackCommand.RaiseCanExecuteChanged();
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count > 0)
                    {
                        Track track = e.OldItems[0] as Track;

                        if (track != null && track.TrackType == TrackType.Audio)
                        {
                            this.AudioTracks.Remove(track);

                            this.AddAudioTrackCommand.RaiseCanExecuteChanged();
                            this.RemoveAudioTrackCommand.RaiseCanExecuteChanged();
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds an audio track to the tracks collection.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void AddAudioTrack(object parameter)
        {
            int number = this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Audio).Max(x => x.Number);

            Track track = new Track { Number = number + 1, TrackType = TrackType.Audio };

            this.projectService.GetCurrentProject().Timeline.Add(track);
            this.timelineModel.Tracks.Add(track);
        }

        /// <summary>
        /// Evaluates if an audio track can be added or not.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>A true if the track can be added;otherwise false.</returns>
        private bool CanAddAudioTrack(object parameter)
        {
            return this.timelineModel.Tracks.Count(x => x.TrackType == TrackType.Audio) < this.maxNumberOfAudioTracks;
        }

        /// <summary>
        /// Removes the last audio tracks from the tracks collection.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void RemoveAudioTrack(object parameter)
        {
            int number = this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Audio).Max(x => x.Number);
            Track track = this.timelineModel.Tracks.Single(x => x.TrackType == TrackType.Audio && x.Number == number);

            TimelineElement[] currentShots = new TimelineElement[track.Shots.Count];
            track.Shots.CopyTo(currentShots);

            foreach (TimelineElement element in currentShots)
            {
                this.DeleteElement(element);
            }

            this.projectService.GetCurrentProject().Timeline.Remove(track);
            this.timelineModel.Tracks.Remove(track);
        }

        /// <summary>
        /// Evaluates if an audio track can be removed.
        /// </summary>
        /// <param name="parameter">The command paramter.</param>
        /// <returns>A true if the audio track can be removed;otherwise false.</returns>
        private bool CanRemoveAudioTrack(object parameter)
        {
            return this.timelineModel.Tracks.Count(x => x.TrackType == TrackType.Audio) > 1;
        }

        /// <summary>
        /// Move the current position by one frame (backward or forward).
        /// </summary>
        /// <param name="frames">The number of frames being added/removed to the current position.</param>
        private void MoveFrame(object frames)
        {
            TimeCode currentTimeCode = this.timelineModel.CurrentPosition;

            bool add = long.Parse(frames.ToString(), CultureInfo.InvariantCulture) > 0;

            TimeCode frameTimeCode = TimeCode.FromFrames(1, this.timelineModel.CurrentPosition.FrameRate);

            currentTimeCode = add ? currentTimeCode.Add(frameTimeCode) : currentTimeCode.Subtract(frameTimeCode);

            this.SetCurrentPosition(currentTimeCode);
        }

        private void MoveToNextClip(object payload)
        {
            Track track = this.timelineModel.Tracks.Single(x => x.TrackType == TrackType.Visual);

            TimeCode currentPosition = this.timelineModel.CurrentPosition;

            TimelineElement nextElement = this.timelineModel.GetNextElement(currentPosition, track);

            if (nextElement != null)
            {
                this.SetCurrentPosition(nextElement.Position.Add(TimeCode.FromFrames(1, nextElement.Position.FrameRate)));
            }
        }

        private void MoveToPreviousClip(object payload)
        {
            Track track = this.timelineModel.Tracks.Single(x => x.TrackType == TrackType.Visual);

            TimeCode currentPosition = this.timelineModel.CurrentPosition;

            TimelineElement previousElement = this.timelineModel.GetPreviousElement(currentPosition, track);

            if (previousElement != null)
            {
                TimeCode position = previousElement.Position.TotalSeconds == 0 ? previousElement.Position : previousElement.Position.Add(TimeCode.FromFrames(1, previousElement.Position.FrameRate));
                this.SetCurrentPosition(position);
            }
        }
    }
}
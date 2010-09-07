// <copyright file="PlayerViewPresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayerViewPresenter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Browser;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using Infrastructure.DragDrop;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Presentation.Events;
    using RCE.Infrastructure.Events;
    using RCE.Infrastructure.Models;
    using RCE.Modules.Player.Models;
    using Services.Contracts;
    using SMPTETimecode;
    using Comment = RCE.Infrastructure.Models.Comment;
    using Track = RCE.Infrastructure.Models.Track;
    using TrackType = RCE.Infrastructure.Models.TrackType;

    /// <summary>
    /// Presenter for the <see cref="PlayerView"/>.
    /// </summary>
    public class PlayerViewPresenter : BaseModel, IPlayerViewPresenter
    {
        /// <summary>
        /// The <seealso cref="IEventAggregator"/> instance used to 
        /// publish and subscribe to events.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The <see cref="ITimelineModel"/> which has the assets in the timeline.
        /// </summary>
        private readonly ITimelineModel timelineModel;

        /// <summary>
        /// The <see cref="IAggregateMediaModel"/> which handles <see cref="VideoAsset"/>
        /// and <see cref="ImageAsset"/> from the timeline model.
        /// </summary>
        private readonly IAggregateMediaModel visualMediaModel;

        /// <summary>
        /// The <see cref="IAggregateMediaModel"/> which handles <see cref="AudioAsset"/>
        /// from the timeline model.
        /// </summary>
        private readonly IAggregateMediaModel audioMediaModel;

        /// <summary>
        /// The <see cref="IAggregateMediaModel"/> which handles <see cref="TitleAsset"/>
        /// from the timeline model.
        /// </summary>
        private readonly IAggregateMediaModel titleMediaModel;

        /// <summary>
        /// The <see cref="DispatcherTimer"/> to frame rewind/forward the currently playing model.
        /// </summary>
        private readonly DispatcherTimer frameRewindForwardTimer;

        /// <summary>
        /// To have the current player mode.
        /// </summary>
        private PlayerMode playerMode;

        /// <summary>
        /// Value indicating if the forwar/rewind is going on.
        /// </summary>
        private int currentSkipDirection;

        /// <summary>
        /// Value indicating if the loop back is on for the player.
        /// </summary>
        private bool loopPlayback;

        /// <summary>
        /// The current playing comment.
        /// </summary>
        private List<Comment> currentPlayingComments;

        /// <summary>
        /// The <seealso cref="Comment"/> instance used to store the comment for which 
        /// the detail is being displayed.
        /// </summary>
        private Comment currentPlayingComment;

        /// <summary>
        /// The <seealso cref="Asset"/> instance used to store the asset for which 
        /// the metadata is currently displayed in the metadata region.
        /// </summary>
        private Asset currentAssetPlaying;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewPresenter"/> class.
        /// </summary>
        /// <param name="view">The <see cref="IPlayerView"/> instance as view.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="timelineModel">The timeline model.</param>
        /// <param name="visualMediaModel">The visual media model.</param>
        /// <param name="audioMediaModel">The audio media model.</param>
        /// <param name="titleMediaModel">The title media model.</param>
        public PlayerViewPresenter(IPlayerView view, IEventAggregator eventAggregator, ITimelineModel timelineModel, IAggregateMediaModel visualMediaModel, IAggregateMediaModel audioMediaModel, IAggregateMediaModel titleMediaModel)
        {
            this.PropertyChanged += this.PlayerViewPresenter_PropertyChanged;
            this.visualMediaModel = visualMediaModel;
            this.visualMediaModel.FinishedPlaying += this.OnPlayFinished;
            this.visualMediaModel.PositionUpdated += this.OnPositionUpdated;
            this.visualMediaModel.BufferStart += this.OnBufferStart;
            this.visualMediaModel.BufferEnd += this.OnBufferEnd;
            this.visualMediaModel.DownloadProgressChanged += this.OnDownloadProgressChanged;

            this.audioMediaModel = audioMediaModel;
            this.audioMediaModel.FinishedPlaying += this.OnPlayFinished;
            this.audioMediaModel.PositionUpdated += this.OnPositionUpdated;
            this.audioMediaModel.BufferStart += this.OnBufferStart;
            this.audioMediaModel.BufferEnd += this.OnBufferEnd;
            this.audioMediaModel.DownloadProgressChanged += this.OnDownloadProgressChanged;

            this.titleMediaModel = titleMediaModel;
            this.titleMediaModel.PositionUpdated += this.OnPositionUpdated;
            this.titleMediaModel.FinishedPlaying += this.OnPlayFinished;

            this.eventAggregator = eventAggregator;

            this.timelineModel = timelineModel;
            this.timelineModel.ElementAdded += (sender, e) => this.AddElement(e.Element);
            this.timelineModel.ElementRemoved += (sender, e) => this.RemoveElement(e.Element);
            this.timelineModel.ElementMoved += (sender, e) => this.MoveElement(e.Element);

            this.eventAggregator.GetEvent<KeyMappingEvent>().Subscribe(this.OnKeyAction, ThreadOption.PublisherThread, true, Filter);
            this.eventAggregator.GetEvent<SmpteTimeCodeChangedEvent>().Subscribe(this.UpdateSmpteFrameRate, true);
            this.eventAggregator.GetEvent<AspectRatioChangedEvent>().Subscribe(this.UpdatePlayerAspectRatio, true);
            this.eventAggregator.GetEvent<PauseEvent>().Subscribe(this.OnPauseEventPublished, true);
            this.eventAggregator.GetEvent<PlayerEvent>().Subscribe(this.OnPlayerEventPublished, true);
            this.eventAggregator.GetEvent<PlayheadMovedEvent>().Subscribe(this.UpdatePosition, true);
            this.eventAggregator.GetEvent<PlayCommentEvent>().Subscribe(this.PlayComment, ThreadOption.PublisherThread, true, CanPlayComment);
            this.eventAggregator.GetEvent<PickThumbnailEvent>().Subscribe(this.PickThumbnail, true);

            this.frameRewindForwardTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.frameRewindForwardTimer.Tick += this.FrameRewindForwardTimerTick;

            this.DropCommand = new DelegateCommand<DropPayload>(this.DropItem, FilterDropItem);
            this.FastRewindCommand = new DelegateCommand<object>(this.FastRewind, this.CanFastRewindForward);
            this.FastForwardCommand = new DelegateCommand<object>(this.FastForward, this.CanFastRewindForward);

            this.View = view;
            this.View.Model = this;
            this.UpdateSmpteFrameRate(SmpteFrameRate.Smpte2997NonDrop);

            this.View.FullScreenChanged += this.View_FullScreenChanged;
            this.View.PlayClicked += (sender, e) => this.TogglePlay();
            this.View.PauseClicked += (sender, e) => this.TogglePlay();
            this.View.MoveToStartClicked += (sender, e) => this.MoveToStart();
            this.View.MoveToEndClicked += (sender, e) => this.MoveToEnd();
            this.View.FrameRewindStarted += (sender, e) => this.StartFrameRewindForward(-1);
            this.View.FrameRewindEnded += (sender, e) => this.EndFrameForwardRewind();
            this.View.FrameForwardStarted += (sender, e) => this.StartFrameRewindForward(1);
            this.View.FrameForwardEnded += (sender, e) => this.EndFrameForwardRewind();
            this.View.LoopPlaybackClicked += (sender, e) => this.ToggleLoopPlayback();
            this.View.AddCommentClicked += this.AddCommentAtCurrentPosition;
            this.View.MetadataClicked += this.OnShowMetadata;
            this.View.MuteClicked += this.MutePlayer;
            this.View.PickThumbnailClicked += (sender, e) => this.PickThumbnail(null);
            this.View.SlowMotionClicked += (sender, e) => this.ToggleSlowMotion();

            HtmlPage.RegisterScriptableObject("Player", this);
        }

        /// <summary>
        /// Gets or sets the instance of <see cref="IPlayerView"/> as the view.
        /// </summary>
        /// <value>The <see cref="IPlayerView"/> instance as view.</value>
        public IPlayerView View { get; set; }

        /// <summary>
        /// Gets or sets the player mode.
        /// </summary>
        /// <value>The player mode.</value>
        public PlayerMode PlayerMode
        {
            get
            {
                return this.playerMode;
            }

            set
            {
                bool changed = value != this.PlayerMode;
                this.playerMode = value;

                if (changed)
                {
                    this.OnPropertyChanged("PlayerMode");
                }
            }
        }

        /// <summary>
        /// Gets the command executed on drop elements.
        /// </summary>
        /// <value>The delegate command used to drop the elements.</value>
        public DelegateCommand<DropPayload> DropCommand { get; private set; }

        /// <summary>
        /// Gets the command executed on fast rewind.
        /// </summary>
        /// <value>The delegate command used to start/stop fast rewind.</value>
        public DelegateCommand<object> FastRewindCommand { get; private set; }

        /// <summary>
        /// Gets the command executed on fast forward.
        /// </summary>
        /// <value>The delegate command used to start/stop fast forward.</value>
        public DelegateCommand<object> FastForwardCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timeline is playing.
        /// </summary>
        /// <value>
        /// <c>True</c> if timeline is playing; otherwise, <c>false</c>.
        /// </value>
        private bool IsPlayModelPlaying
        {
            get
            {
                return this.visualMediaModel.IsPlaying || this.audioMediaModel.IsPlaying || this.titleMediaModel.IsPlaying;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the AggregateMode(Visual/Audio) is muted.
        /// </summary>
        private bool MuteModel
        {
            set
            {
                this.visualMediaModel.Mute = value;
                this.audioMediaModel.Mute = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the Visual/Audio is visible.
        /// </summary>
        private bool IsVisibleModel
        {
            set
            {
                this.visualMediaModel.IsVisible = value;
                this.audioMediaModel.IsVisible = value;
                this.titleMediaModel.IsVisible = value;

                if (this.currentPlayingComments != null && !value)
                {
                    this.View.HideComments();
                }
            }
        }

        /// <summary>
        /// Toggles the play timeline.
        /// </summary>
        [ScriptableMember]
        public void TogglePlayTimeline()
        {
            this.PlayerMode = PlayerMode.Timeline;
            this.TogglePlayModel();
        }

        /// <summary>
        /// Stops the timeline.
        /// </summary>
        [ScriptableMember]
        public void StopTimeline()
        {
            this.PlayerMode = PlayerMode.Timeline;
            this.PauseModel();
            TimeSpan position = TimeSpan.FromSeconds(0);
            this.SetCurrentPosition(position);
            this.OnPositionUpdated(this, new PositionPayloadEventArgs(position));
        }

        /// <summary>
        /// Gets the MediaData associated to the visual element at current position.
        /// </summary>
        /// <returns>The MediaData of the current position.</returns>
        public MediaData GetMediaDataAtCurrentPosition()
        {
            if (this.PlayerMode == PlayerMode.Timeline)
            {
                Track track = this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Visual).FirstOrDefault();

                if (track != null)
                {
                    TimelineElement element = this.timelineModel.GetElementAtPosition(this.timelineModel.CurrentPosition, track, null);

                    if (element != null)
                    {
                        IAggregateMediaModel mediaModel = this.GetModelByElement(element);
                        return mediaModel.FindMediaByElement(element);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Publishes the ThumbnailEvent.
        /// </summary>
        /// <param name="bitmap">The bitmap being published.</param>
        public void SetThumbnail(WriteableBitmap bitmap)
        {
            this.eventAggregator.GetEvent<ThumbnailEvent>().Publish(bitmap);
        }

        /// <summary>
        /// Validates whether is control is dropped on the player region or outside.
        /// </summary>
        /// <param name="payload">Asset that is dropped on the player.</param>
        /// <returns>True asset is dropped at the right position, otherwise [False].</returns>
        private static bool FilterDropItem(DropPayload payload)
        {
            return payload.DraggedItem != null;
        }

        /// <summary>
        /// Filter that indicates whether a comment can be played or not.
        /// </summary>
        /// <param name="comment">Instance of the comment.</param>
        /// <returns>True if the comment can be played, otherwise [False].</returns>
        private static bool CanPlayComment(Comment comment)
        {
            return comment != null && comment.MarkIn != null && comment.MarkOut != null
                && comment.MarkOut >= comment.MarkIn;
        }

        /// <summary>
        /// Filter for KeyMappingEvent event.
        /// </summary>
        /// <param name="keyMappingAction">Returns true if KeyMappingAction is 
        /// PlayTimeLine, PauseTimeLine or Toggle.<see cref="RCE.Infrastructure.Models.KeyMappingAction"/>.</param>
        /// <returns>True if KeyMappingAction is PlayTimeline, PauseTimeline, Toggle.</returns>
        private static bool Filter(KeyMappingAction keyMappingAction)
        {
            switch (keyMappingAction)
            {
                case KeyMappingAction.PlayTimeline:
                case KeyMappingAction.PausePlayer:
                case KeyMappingAction.Toggle:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handlers the asset drop event on the player. Also, raises an event to 
        /// display the metadata for the asset dropped on the player.
        /// </summary>
        /// <param name="payload">Asset that is dropped on the player.</param>
        private void DropItem(DropPayload payload)
        {
            Asset asset = payload.DraggedItem as Asset;

            if (asset != null && !(asset is FolderAsset))
            {
                this.PlayerMode = PlayerMode.MediaLibrary;
                this.View.HidePreviewImage();
                this.View.SetSource(asset);
                this.ShowMetadata(asset);
            }
        }

        private bool CanFastRewindForward(object payload)
        {
            if (this.PlayerMode == PlayerMode.Timeline)
            {
                return this.visualMediaModel.IsPlaying;
            }
            else
            {
                return false;
            }
        }

        private void FastForward(object payload)
        {
            this.visualMediaModel.FastForward();
        }

        private void FastRewind(object payload)
        {
            this.visualMediaModel.FastRewind();
        }

        /// <summary>
        /// Take the action corresponding to the given key action.
        /// </summary>
        /// <param name="keyAction">Key Action Value.<seealso cref="RCE.Infrastructure.Models.KeyMappingAction"/>.</param>
        private void OnKeyAction(KeyMappingAction keyAction)
        {
            if (keyAction == KeyMappingAction.Toggle)
            {
                if (this.PlayerMode == PlayerMode.Timeline)
                {
                    this.TogglePlayModel();
                }
                else if (this.PlayerMode == PlayerMode.MediaBin || this.PlayerMode == PlayerMode.MediaLibrary)
                {
                    this.View.TogglePlay();
                }
            }
            else if (keyAction == KeyMappingAction.PlayTimeline && !this.IsPlayModelPlaying)
            {
                this.PlayerMode = PlayerMode.Timeline;
                this.PlayModel();
            }
            else if (keyAction == KeyMappingAction.PausePlayer)
            {
                if (this.PlayerMode == PlayerMode.Timeline && this.IsPlayModelPlaying)
                {
                    this.PauseModel();
                }
                else if (this.PlayerMode == PlayerMode.MediaLibrary || this.PlayerMode == PlayerMode.MediaBin)
                {
                    this.View.PausePlayer();
                }
            }
        }

        /// <summary>
        /// Picks a thumbnail from the view.
        /// </summary>
        /// <param name="payload">The payload.</param>
        private void PickThumbnail(object payload)
        {
            MediaData mediaData = this.GetMediaDataAtCurrentPosition();

            this.View.PickThumbnail(mediaData);
        }

        /// <summary>
        /// Updates the smpte frame rate.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        private void UpdateSmpteFrameRate(SmpteFrameRate frameRate)
        {
            this.View.SetCurrentSmpteFrameRate(frameRate);
        }

        /// <summary>
        /// Called when [player event published].
        /// </summary>
        /// <param name="payload">The payload.</param>
        private void OnPlayerEventPublished(PlayerEventPayload payload)
        {
            this.PlayerMode = payload.PlayerMode;

            if (payload.PlayerMode == PlayerMode.Timeline)
            {
                this.TogglePlayModel();
            }
            else
            {
                if (!(payload.Asset is FolderAsset))
                {
                    this.View.HidePreviewImage();
                    this.View.SetSource(payload.Asset);
                    this.ShowMetadata(payload.Asset);
                }
            }
        }

        /// <summary>
        /// Called when [pause event published].
        /// </summary>
        /// <param name="payload">The payload.</param>
        private void OnPauseEventPublished(object payload)
        {
            this.PauseModel();
        }

        /// <summary>
        /// Handles the playhead position update event.
        /// </summary>
        /// <param name="payload">Holds the new position of the playhead.</param>
        private void UpdatePosition(PositionPayloadEventArgs payload)
        {
            if (this.PlayerMode != PlayerMode.Timeline)
            {
                this.PlayerMode = PlayerMode.Timeline;
            }

            this.SetCurrentPosition(payload.Position);
            this.View.SetCurrentTime(payload.Position);
            this.HandleCommentsAtCurrentPosition(payload.Position.TotalSeconds);
            this.HandleMetadataAtCurrentPosition();
        }

        /// <summary>
        /// Updates the aspect ratio for the asset for the player control.
        /// </summary>
        /// <param name="selectedAspectRatio">New aspect ratio.</param>
        private void UpdatePlayerAspectRatio(AspectRatio selectedAspectRatio)
        {
            this.View.SetAspectRatio(selectedAspectRatio);
        }

        /// <summary>
        /// Handles the PlayCommentEvent event. <seealso cref="RCE.Infrastructure.Events.PlayCommentEvent"/>.
        /// </summary>
        /// <param name="comment">Comment to be played.</param>
        private void PlayComment(Comment comment)
        {
            if (comment != null)
            {
                this.PlayerMode = PlayerMode.Comment;
                TimeSpan commentPosition = TimeSpan.FromSeconds(comment.MarkIn.GetValueOrDefault());
                
                // To pause the aggregate model so that the OnFrameRendered event could not be triggered.
                this.PauseModel();
                this.SetCurrentPosition(commentPosition);
                this.View.SetCurrentTime(commentPosition);
                this.eventAggregator.GetEvent<PositionUpdatedEvent>().Publish(new PositionPayloadEventArgs(commentPosition));
                this.currentPlayingComment = comment;
                this.PlayModel();
            }
        }

        /// <summary>
        /// Handles the Metadata icon click event for the MediaBin/Library view.
        /// Retrieves the metadata information for the asset and handles the 
        /// visibility of the metadata region.
        /// </summary>
        /// <param name="sender">Event source - the select asset.</param>
        /// <param name="e">Event argument.</param>
        private void OnShowMetadata(object sender, EventArgs e)
        {
            Asset payload = null;
            bool isAudioOrVisualAsset = false;

            if (this.playerMode == PlayerMode.Timeline)
            {
                if (this.visualMediaModel != null && this.visualMediaModel.CurrentAsset != null)
                {
                    payload = this.visualMediaModel.CurrentAsset;
                    isAudioOrVisualAsset = true;
                }
                else if (this.audioMediaModel != null && this.audioMediaModel.CurrentAsset != null)
                {
                    payload = this.audioMediaModel.CurrentAsset;
                    isAudioOrVisualAsset = true;
                }
            }
            else if (this.playerMode == PlayerMode.MediaBin || this.playerMode == PlayerMode.MediaLibrary)
            {
                payload = sender as Asset;
            }

            if (payload != null && isAudioOrVisualAsset)
            {
                this.ShowMetadata(payload);
            }
            else
            {
                this.HideMetadata();
            }
        }

        /// <summary>
        /// Sets the position of all the <see cref="IAggregateMediaModel"/>(visual/audio/title).
        /// </summary>
        /// <param name="position">The position.</param>
        private void SetCurrentPosition(TimeSpan position)
        {
            this.visualMediaModel.Position = position;
            this.audioMediaModel.Position = position;
            this.titleMediaModel.Position = position;
        }

        /// <summary>
        /// Toggles between play/pause.
        /// </summary>
        private void TogglePlay()
        {
            if (this.PlayerMode == PlayerMode.Timeline || this.PlayerMode == PlayerMode.Comment)
            {
                this.TogglePlayModel();
            }
            else
            {
                this.View.TogglePlay();
            }

            this.FastRewindCommand.RaiseCanExecuteChanged();
            this.FastForwardCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Toggles the SlowMotion of the current Media Data.
        /// </summary>
        private void ToggleSlowMotion()
        {
            if (this.PlayerMode == PlayerMode.Timeline)
            {
                MediaData mediaData = this.GetMediaDataAtCurrentPosition();
                this.View.ToggleSlowMotion(mediaData);
            }
            else
            {
                this.View.ToggleSlowMotion(null);
            }
        }

        /// <summary>
        /// Moves to start of the current playing media in the player.
        /// </summary>
        private void MoveToStart()
        {
            if (this.PlayerMode == PlayerMode.Timeline)
            {
                this.MoveToStartModel();
            }
            else if (this.playerMode == PlayerMode.Comment)
            {
                this.MoveToStartOfComment();
            }
            else
            {
                this.View.MoveToStart();
            }
        }

        /// <summary>
        /// Moves to start of current playing comment.
        /// </summary>
        private void MoveToStartOfComment()
        {
            if (this.currentPlayingComment != null)
            {
                TimeSpan position = TimeSpan.FromMilliseconds(this.currentPlayingComment.MarkIn.GetValueOrDefault() * 1000);
                this.SetCurrentPosition(position);
                this.OnPositionUpdated(this, new PositionPayloadEventArgs(position));
            }
        }

        /// <summary>
        /// Moves to end of current playing media.
        /// </summary>
        private void MoveToEnd()
        {
            if (this.PlayerMode == PlayerMode.Timeline)
            {
                this.MoveToEndModel();
            }
            else if (this.playerMode == PlayerMode.Comment)
            {
                this.MoveToEndOfComment();
            }
            else
            {
                this.View.MoveToEnd();
            }

            this.HideMetadata();
        }

        /// <summary>
        /// Starts the rewind forward.
        /// </summary>
        /// <param name="skipDirection">The skip direction.</param>
        private void StartFrameRewindForward(int skipDirection)
        {
            if (this.PlayerMode == PlayerMode.Timeline || this.playerMode == PlayerMode.Comment)
            {
                this.StartFrameForwardRewindModel(skipDirection);
            }
            else
            {
                this.View.StartFrameRewindForward(skipDirection);
            }
        }

        /// <summary>
        /// Ends the forward rewind.
        /// </summary>
        private void EndFrameForwardRewind()
        {
            if (this.PlayerMode == PlayerMode.Timeline || this.playerMode == PlayerMode.Comment)
            {
                this.StopFrameForwardRewindModel();
            }
            else
            {
                this.View.EndFrameRewindForward();
            }
        }

        /// <summary>
        /// Toggles the loop playback.
        /// </summary>
        private void ToggleLoopPlayback()
        {
            this.ToggleLoopPlaybackModel();
            this.View.ToggleLoopPlayback();
        }

        /// <summary>
        /// Toggles the play model between play/pause.
        /// </summary>
        private void TogglePlayModel()
        {
            if (this.IsPlayModelPlaying)
            {
                this.PauseModel();
            }
            else
            {
                this.PlayModel();
            }
        }

        /// <summary>
        /// Toggles the loop playback.
        /// </summary>
        private void ToggleLoopPlaybackModel()
        {
            this.loopPlayback = !this.loopPlayback;
        }

        /// <summary>
        /// Plays all the <see cref="IAggregateMediaModel"/>(visual/audio/title).
        /// </summary>
        private void PlayModel()
        {
            this.visualMediaModel.Play();
            this.audioMediaModel.Play();
            this.titleMediaModel.Play();
            this.MuteModel = this.View.IsMuted;
            this.View.TogglePlayVisibility(this.IsPlayModelPlaying);
        }

        /// <summary>
        /// Pauses all the <see cref="IAggregateMediaModel"/>(visual/audio/title).
        /// </summary>
        private void PauseModel()
        {
            this.visualMediaModel.Pause();
            this.audioMediaModel.Pause();
            this.titleMediaModel.Pause();
            this.View.TogglePlayVisibility(this.IsPlayModelPlaying);
        }

        /// <summary>
        /// Moves to start of the <see cref="IAggregateMediaModel"/>.
        /// </summary>
        private void MoveToStartModel()
        {
            TimeSpan position = TimeSpan.FromSeconds(0);
            this.SetCurrentPosition(position);
            this.OnPositionUpdated(this, new PositionPayloadEventArgs(position));
        }

        /// <summary>
        /// Handles the FullScreenChanged event of the View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RCE.Infrastructure.Models.FullScreenModeEventArgs"/> instance containing the event data.</param>
        private void View_FullScreenChanged(object sender, FullScreenModeEventArgs e)
        {
            this.eventAggregator.GetEvent<FullScreenEvent>().Publish(e.Mode);
        }

        /// <summary>
        /// Moves to end of the timeline element.
        /// </summary>
        private void MoveToEndModel()
        {
            this.PauseModel();
            List<TimeSpan> tempList = new List<TimeSpan>
                                          {
                                              this.visualMediaModel.Duration, // .Subtract(TimeSpan.FromSeconds(0.1));
                                              this.audioMediaModel.Duration, // .Subtract(TimeSpan.FromSeconds(0.1));
                                              this.titleMediaModel.Duration, // .Subtract(TimeSpan.FromSeconds(0.1));
                                          };

            TimeSpan position = tempList.Max();
            this.visualMediaModel.Position = this.visualMediaModel.Duration;
            this.audioMediaModel.Position = this.audioMediaModel.Duration;
            this.titleMediaModel.Position = this.titleMediaModel.Duration;
            this.OnPositionUpdated(this, new PositionPayloadEventArgs(position));
            this.HideMetadata();
        }

        /// <summary>
        /// Sets the position to the end of the comment.
        /// </summary>
        private void MoveToEndOfComment()
        {
            if (this.currentPlayingComment != null)
            {
                this.PauseModel();
                TimeSpan position = TimeSpan.FromSeconds((double)this.currentPlayingComment.MarkOut);
                this.PauseModel();
                this.SetCurrentPosition(position);
                this.OnPositionUpdated(this, new PositionPayloadEventArgs(position));
            }
        }

        /// <summary>
        /// Starts the forward/rewind model.
        /// </summary>
        /// <param name="skipDirection">The skip direction.</param>
        private void StartFrameForwardRewindModel(int skipDirection)
        {
            this.PauseModel();
            this.currentSkipDirection = skipDirection;
            this.frameRewindForwardTimer.Start();
        }

        /// <summary>
        /// Stops the forward/rewind model.
        /// </summary>
        private void StopFrameForwardRewindModel()
        {
            this.currentSkipDirection = 0;
            this.frameRewindForwardTimer.Stop();
        }

        /// <summary>
        /// Adds the <see cref="MediaData"/> to the <see cref="IAggregateMediaModel"/>.
        /// </summary>
        /// <param name="element">The <see cref="TimelineModel"/>.</param>
        private void AddElement(TimelineElement element)
        {
            element.PropertyChanged += this.Element_PropertyChanged;

            IAggregateMediaModel mediaModel = this.GetModelByElement(element);
            
            MediaData blankMediaData = mediaModel.AddBlank(element);
            MediaData mediaData = mediaModel.AddElement(element);

            if (mediaData != null && blankMediaData != null)
            {
                this.ReorderElements(element);

                int width = 1;
                int height = 1;

                VideoAsset videoAsset = element.Asset as VideoAsset;
                ImageAsset imageAsset = element.Asset as ImageAsset;

                if (videoAsset != null)
                {
                    width = videoAsset.Width.GetValueOrDefault();
                    height = videoAsset.Height.GetValueOrDefault();
                }

                if (imageAsset != null)
                {
                    width = imageAsset.Width;
                    height = imageAsset.Height;
                }

                this.View.AddElement(blankMediaData, width, height);
                this.View.AddElement(mediaData, width, height);
            }

            mediaModel.ResetCurrent();
            this.SetCurrentPosition(TimeSpan.FromSeconds(this.timelineModel.CurrentPosition.TotalSeconds));
            this.HandleCommentsAtCurrentPosition(this.timelineModel.CurrentPosition.TotalSeconds);
            this.HandleMetadataAtCurrentPosition();
            this.PauseModel();
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Element control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InPosition" || e.PropertyName == "OutPosition")
            {
                TimelineElement element = (TimelineElement) sender;

                IAggregateMediaModel mediaModel = this.GetModelByElement(element);

                this.ReorderElements(element);

                mediaModel.ResetCurrent();
                this.SetCurrentPosition(TimeSpan.FromSeconds(this.timelineModel.CurrentPosition.TotalSeconds));
                this.HandleCommentsAtCurrentPosition(this.timelineModel.CurrentPosition.TotalSeconds);
                this.HandleMetadataAtCurrentPosition();
                this.PauseModel();
            }
        }

        /// <summary>
        /// Removes the element.
        /// </summary>
        /// <param name="element">The element.</param>
        private void RemoveElement(TimelineElement element)
        {
            element.PropertyChanged -= this.Element_PropertyChanged;

            IAggregateMediaModel mediaModel = this.GetModelByElement(element);

            MediaData blankMediaData = mediaModel.RemoveBlankElement(element);
            MediaData mediaData = mediaModel.RemoveElement(element);

            if (mediaData != null && blankMediaData != null)
            {
                this.ReorderElements(element);
                this.View.RemoveElement(blankMediaData);
                this.View.RemoveElement(mediaData);
            }

            mediaModel.ResetCurrent();

            if (element.Position.TotalSeconds <= this.timelineModel.CurrentPosition.TotalSeconds 
                && (element.Position.TotalSeconds + element.Duration.TotalSeconds) >= this.timelineModel.CurrentPosition.TotalSeconds)
            {
                this.View.EndBuffer();
            }

            this.SetCurrentPosition(TimeSpan.FromSeconds(this.timelineModel.CurrentPosition.TotalSeconds));
            this.HandleCommentsAtCurrentPosition(this.timelineModel.CurrentPosition.TotalSeconds);
            this.HandleMetadataAtCurrentPosition();
            this.PauseModel();
            this.HideMetadata();
        }

        /// <summary>
        /// Moves the element to the new dropped position.
        /// </summary>
        /// <param name="element">The element.</param>
        private void MoveElement(TimelineElement element)
        {
            IAggregateMediaModel mediaModel = this.GetModelByElement(element);

            this.ReorderElements(element);

            mediaModel.ResetCurrent();
            this.SetCurrentPosition(TimeSpan.FromSeconds(this.timelineModel.CurrentPosition.TotalSeconds));
            this.HandleCommentsAtCurrentPosition(this.timelineModel.CurrentPosition.TotalSeconds);
            this.HandleMetadataAtCurrentPosition();
            this.PauseModel();
        }

        /// <summary>
        /// Gets the <see cref="IAggregateMediaModel"/> by element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="IAggregateMediaModel"/>.</returns>
        private IAggregateMediaModel GetModelByElement(TimelineElement element)
        {
            if (element.Asset is VideoAsset || element.Asset is ImageAsset)
            {
                return this.visualMediaModel;
            }

            if (element.Asset is AudioAsset)
            {
                return this.audioMediaModel;
            }

            if (element.Asset is TitleAsset)
            {
                return this.titleMediaModel;
            }

            return null;
        }

        /// <summary>
        /// Reorders the elements of the track of the given elements asset.
        /// </summary>
        /// <param name="element">The element.</param>
        private void ReorderElements(TimelineElement element)
        {
            IEnumerable<Track> tracks = this.GetElementTracks(element);

            if (tracks != null && tracks.Count() > 0)
            {
                List<TimelineElement> elements = tracks.SelectMany(track => track.Shots).ToList();

                elements.Sort((element1, element2) => element1.Position.CompareTo(element2.Position));

                if (tracks.First().TrackType == TrackType.Visual)
                {
                    this.visualMediaModel.ReorderElements(elements);
                }

                if (tracks.First().TrackType == TrackType.Audio)
                {
                    this.audioMediaModel.ReorderElements(elements);
                }

                if (tracks.First().TrackType == TrackType.Title)
                {
                    this.titleMediaModel.ReorderElements(elements);
                }
            }
        }

        /// <summary>
        /// Gets the element track of the given <see cref="TimelineElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="TimelineElement"/>.</param>
        /// <returns>The list of <see cref="Track"/>s.</returns>
        private IEnumerable<Track> GetElementTracks(TimelineElement element)
        {
            var videoAsset = element.Asset as VideoAsset;
            var audioAsset = element.Asset as AudioAsset;
            var imageAsset = element.Asset as ImageAsset;
            var titlesAsset = element.Asset as TitleAsset;

            if (videoAsset != null || imageAsset != null)
            {
                return this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Visual);
            }

            if (audioAsset != null)
            {
                return this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Audio);
            }

            if (titlesAsset != null)
            {
                return this.timelineModel.Tracks.Where(x => x.TrackType == TrackType.Title);
            }

            return null;
        }

        /// <summary>
        /// Handles the Rewind/Forward of the <see cref="IAggregateMediaModel"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FrameRewindForwardTimerTick(object sender, EventArgs e)
        {
            if (this.currentSkipDirection == 0)
            {
                return;
            }

            List<TimeSpan> tempList = new List<TimeSpan>
                                          {
                                              this.visualMediaModel.Position,
                                              this.audioMediaModel.Position,
                                              this.titleMediaModel.Position,
                                          };

            bool add = this.currentSkipDirection > 0;
            long newSkipDirection = 1;

            TimeCode currentTimeCode = TimeCode.FromTimeSpan(tempList.Max(), this.timelineModel.Duration.FrameRate);
            long currentTotalFrames = currentTimeCode.TotalFrames;

            TimeCode frameTimeCode = TimeCode.FromFrames(newSkipDirection, this.timelineModel.Duration.FrameRate);

            currentTimeCode = add ? currentTimeCode.Add(frameTimeCode) : currentTimeCode.Subtract(frameTimeCode);
            newSkipDirection++;

            while (currentTimeCode.TotalFrames == currentTotalFrames)
            {
                frameTimeCode = TimeCode.FromFrames(newSkipDirection, this.timelineModel.Duration.FrameRate);
                currentTimeCode = add ? currentTimeCode.Add(frameTimeCode) : currentTimeCode.Subtract(frameTimeCode);
                newSkipDirection++;
            }

            TimeSpan position = TimeSpan.FromSeconds(Math.Max(0, currentTimeCode.TotalSeconds));

            // Check if the playing mode is comment. If yes then don't allow to go forwar/backwar
            // beyond the comment Markin and MarkOut position.
            if (this.PlayerMode == PlayerMode.Comment && this.currentPlayingComment != null)
            {
                if (position.TotalSeconds < this.currentPlayingComment.MarkIn)
                {
                    position = TimeSpan.FromMilliseconds(this.currentPlayingComment.MarkIn.GetValueOrDefault() * 1000);
                }
                else if (position.TotalSeconds > this.currentPlayingComment.MarkOut)
                {
                    position = TimeSpan.FromMilliseconds(this.currentPlayingComment.MarkOut.GetValueOrDefault() * 1000);
                }
            }

            this.SetCurrentPosition(position);
            this.OnPositionUpdated(this, new PositionPayloadEventArgs(position));
        }

        /// <summary>
        /// Called when position of the <see cref="IAggregateMediaModel"/> is updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RCE.Infrastructure.Events.PositionPayloadEventArgs"/> instance containing the event data.</param>
        private void OnPositionUpdated(object sender, PositionPayloadEventArgs e)
        {
            this.View.SetCurrentTime(e.Position);
            this.eventAggregator.GetEvent<PositionUpdatedEvent>().Publish(e);
            this.HandleCommentsAtCurrentPosition(e.Position.TotalSeconds);
            this.HandleMetadataAtCurrentPosition();
            this.StopPlayingComment(e.Position.TotalSeconds);
        }

        /// <summary>
        /// Handles the comment at current position.
        /// It shows if there is any comment at the current position and hides if the current 
        /// comment is not null and the current position is not in between the In and Out position 
        /// of the current comment.
        /// </summary>
        /// <param name="timePosition">The time position.</param>
        private void HandleCommentsAtCurrentPosition(double timePosition)
        {
            List<Comment> comments = this.timelineModel.CommentElements.Where(
                        x => x.MarkIn <= timePosition && x.MarkOut >= timePosition).ToList();

            if (comments.Count > 0 && this.currentPlayingComments != comments)
            {
                this.View.ShowComments(comments);
                this.currentPlayingComments = comments;
            }
            else
            {
                this.HideComments();
            }
        }

        /// <summary>
        /// Handles the display of the metadata information for the asset
        /// at the current playhead position.
        /// </summary>
        private void HandleMetadataAtCurrentPosition()
        {
            Asset audioAsset = null;
            Asset visualAsset = null;

            ObservableCollection<Track> tracks = this.timelineModel.Tracks;

            foreach (Track track in tracks)
            {
                if (track.TrackType == TrackType.Title || track.TrackType == TrackType.Undefined)
                {
                    continue;
                }

                IList<TimelineElement> elements = this.timelineModel.GetElementsAtPosition(this.timelineModel.CurrentPosition, track);

                foreach (TimelineElement element in elements)
                {
                    if (element.Asset is AudioAsset)
                    {
                        audioAsset = element.Asset;
                    }
                    else if (element.Asset is VideoAsset || element.Asset is ImageAsset)
                    {
                        visualAsset = element.Asset;
                        break;
                    }
                }
            }

            Asset currentAsset = visualAsset ?? audioAsset;

            if (currentAsset != null)
            {
                if (currentAsset != this.currentAssetPlaying)
                {
                    this.ShowMetadata(currentAsset);
                }
            }
            else
            {
                this.HideMetadata();
            }

            this.currentAssetPlaying = currentAsset;
        }

        /// <summary>
        /// Stops playing comment if the position reaches to the out position of the comment.
        /// </summary>
        /// <param name="position">The current position.</param>
        private void StopPlayingComment(double position)
        {
            if (this.playerMode == PlayerMode.Comment && this.currentPlayingComment != null
                && this.IsPlayModelPlaying && position > this.currentPlayingComment.MarkOut)
            {
                this.PauseModel();
            }
        }

        /// <summary>
        /// Hides the comment.
        /// </summary>
        private void HideComments()
        {
            this.View.HideComments();
            this.currentPlayingComments = null;
        }

        /// <summary>
        /// Called when <see cref="MediaData"/> reaches to end.
        /// It hides the current <see cref="MediaData"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPlayFinished(object sender, EventArgs e)
        {
            this.HideMetadata();

            if (this.IsPlayModelPlaying)
            {
                return;
            }

            this.View.TogglePlayVisibility(this.IsPlayModelPlaying);

            if (this.loopPlayback)
            {
                this.SetCurrentPosition(TimeSpan.FromSeconds(0));
                this.PlayModel();
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">PropertyChangedEventArgs arguments.</param>
        private void PlayerViewPresenter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PlayerMode")
            {
                if (this.PlayerMode == PlayerMode.Timeline || this.PlayerMode == PlayerMode.Comment)
                {
                    this.View.Stop();
                    this.View.HidePreviewImage();
                    this.IsVisibleModel = true;
                }
                else
                {
                    this.PauseModel();
                    this.IsVisibleModel = false;
                }
            }
        }

        /// <summary>
        /// Adds the comment at the current position if the playermode is Timeline.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddCommentAtCurrentPosition(object sender, EventArgs e)
        {
            if (this.playerMode == PlayerMode.Timeline)
            {
                this.eventAggregator.GetEvent<PositionDoubleClickedEvent>().Publish(new PositionPayloadEventArgs(TimeSpan.FromSeconds(this.timelineModel.CurrentPosition.TotalSeconds)));
            }
        }

        /// <summary>
        /// Handles the MuteClicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MutePlayer(object sender, EventArgs e)
        {
            if (this.playerMode == PlayerMode.Timeline)
            {
                this.MuteModel = !this.View.IsMuted;
            }

            this.View.IsMuted = !this.View.IsMuted;
        }

        /// <summary>
        /// Handles the StartBuffer event of the <see cref="MediaData"/> element in the <see cref="AggregateMediaModel"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MediaData"/>.</param>
        /// <param name="e">The <see cref="EventArgs"/>.</param>
        private void OnBufferStart(object sender, EventArgs e)
        {
            this.View.StartBuffer();
        }

        /// <summary>
        /// Handles EndBuffer event of the <see cref="MediaData"/> element in the <see cref="AggregateMediaModel"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MediaData"/>.</param>
        /// <param name="e">The <see cref="EventArgs"/>.</param>
        private void OnBufferEnd(object sender, EventArgs e)
        {
            this.View.EndBuffer();
        }

        /// <summary>
        /// Handles the hiding of the metadata region when no asset is at the current
        /// playhead or when no asset is playing.
        /// </summary>
        private void HideMetadata()
        {
            this.eventAggregator.GetEvent<HideMetadataEvent>().Publish(null);
        }

        /// <summary>
        /// Displays the metadata information of the given asset.
        /// </summary>
        /// <param name="asset"><see cref="Asset"/> for which the metadata needs to be displayed.</param>
        private void ShowMetadata(Asset asset)
        {
            this.eventAggregator.GetEvent<ShowMetadataEvent>().Publish(asset);
        }

        /// <summary>
        /// Called when download progress of the current <see cref="MediaData"/> changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AssetDownloadProgressEventArgs"/> instance containing the event data.</param>
        private void OnDownloadProgressChanged(object sender, AssetDownloadProgressEventArgs args)
        {
            this.eventAggregator.GetEvent<DownloadProgressChangedEvent>().Publish(args);
        }
    }
}

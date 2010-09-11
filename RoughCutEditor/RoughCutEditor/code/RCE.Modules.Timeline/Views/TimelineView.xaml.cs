// <copyright file="TimelineView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TimelineView.xaml.cs                     
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
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Infrastructure;
    using Infrastructure.Models;
    using RCE.Infrastructure.Events;
    using SMPTETimecode;
    using Key = System.Windows.Input.Key;

    /// <summary>
    /// The Timeline view.
    /// </summary>
    public partial class TimelineView : ITimelineView
    {
        /// <summary>
        /// Default zoom value.
        /// </summary>
        private const double ZoomValue = 2;

        /// <summary>
        /// Minimum zoom slider size.
        /// </summary>
        private const int MinimunZoomSliderSize = 10;

        /// <summary>
        /// Main marker brush.
        /// </summary>
        private readonly SolidColorBrush mainMarkerStroke;

        /// <summary>
        /// Top marker brush.
        /// </summary>
        private readonly SolidColorBrush topMarkerStroke;

        /// <summary>
        /// Contains the element views of the current elements in the timeline.
        /// </summary>
        private readonly IDictionary<Guid, TimelineElementView> elementViews;

        /// <summary>
        /// Contains the main bar markers of the timeline.
        /// </summary>
        private readonly List<Line> mainBarMarkers;

        /// <summary>
        /// Contains the top bar markers of the timeline.
        /// </summary>
        private readonly List<Line> topBarMarkers;

        /// <summary>
        /// Contains the top bar labels of the timeline.
        /// </summary>
        private readonly List<TextBlock> topBarLabels;

        /// <summary>
        /// The active element of the timeline.
        /// </summary>
        private TimelineElementView activeElement;

        /// <summary>
        /// The dragging option of the active element.
        /// </summary>
        private ElementPositionType activeElementDraggingOption;

        /// <summary>
        /// The element drag offset.
        /// </summary>
        private double elementDragOffset;

        /// <summary>
        /// The ticks of the last clicks used to detect double clicks.
        /// </summary>
        private long lastClickTicks;

        /// <summary>
        /// Determines whether the playhead is being moved or not. 
        /// </summary>
        private bool movingPlayhead;

        /// <summary>
        /// The timeline duration.
        /// </summary>
        private TimeCode duration;

        /// <summary>
        /// The start position of the timeline.
        /// </summary>
        private TimeCode viewStartPosition;

        /// <summary>
        /// The end position of the timeline.
        /// </summary>
        private TimeCode viewEndPosition;

        /// <summary>
        /// The current position of the timeline.
        /// </summary>
        private TimeCode currentPosition;

        /// <summary>
        /// The Z Index of the current view.
        /// </summary>
        private int currentZIndex;

        /// <summary>
        /// Last known position of the mouse.
        /// </summary>
        private double lastKnownMousePosition;

        /// <summary>
        /// The active zoom handler.
        /// </summary>
        private ZoomSliderHandler activeZoomHandler;

        /// <summary>
        /// Determines whether elements are being moved or not.
        /// </summary>
        private bool movingElements;

        /// <summary>
        /// Determines whether the timeline is locked or not.
        /// </summary>
        private bool isTimelineLocked;

        /// <summary>
        /// Determines whether the timeline handlers are enabled or not.
        /// </summary>
        private bool areTimelineHandlersEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineView"/> class.
        /// </summary>
        public TimelineView()
        {
            InitializeComponent();

            this.TopBar.MouseLeftButtonDown += this.TimeBar_MouseLeftButtonDown;
            this.Playhead.MouseLeftButtonDown += this.TimeBar_MouseLeftButtonDown;

            this.ZoomSliderRightHandler.MouseLeftButtonDown += this.ZoomSliderRightHandler_MouseLeftButtonDown;
            this.ZoomSliderLeftHandler.MouseLeftButtonDown += this.ZoomSliderLeftHandler_MouseLeftButtonDown;
            this.ZoomSliderMiddleHandler.MouseLeftButtonDown += this.ZoomSliderMiddleHandler_MouseLeftButtonDown;

            // Resize
            this.Width = Application.Current.Host.Content.ActualWidth;
            Application.Current.Host.Content.Resized += (sender, args) => this.Width = Application.Current.Host.Content.ActualWidth;
            Application.Current.Host.Content.FullScreenChanged += (sender, args) => this.Width = Application.Current.Host.Content.ActualWidth;

            // Element views
            this.elementViews = new Dictionary<Guid, TimelineElementView>();

            this.mainBarMarkers = new List<Line>();
            this.topBarMarkers = new List<Line>();
            this.topBarLabels = new List<TextBlock>();

            // Key Commands
            if (Application.Current.RootVisual != null)
            {
                Application.Current.RootVisual.KeyDown += this.RootVisual_KeyDown;
                Application.Current.RootVisual.MouseWheel += this.TimelineGrid_MouseWheel;
            }

            this.AudioTracks.LayoutUpdated += this.OnAudioTracksLayoutUpdated;

            HtmlPage.RegisterScriptableObject("Timeline", this);

            this.mainMarkerStroke = (SolidColorBrush)this.Resources["MainMarkerStroke"];
            this.topMarkerStroke = (SolidColorBrush)this.Resources["TopMarkerStroke"];
        }

        /// <summary>
        /// Occurs when an element is being trimmed to the current position.
        /// </summary>
        public event EventHandler<ElementPositionChangeEventArgs> TrimElementToCurrentPosition;

        /// <summary>
        /// Occurs when the position of the element change.
        /// </summary>
        public event EventHandler<ElementPositionChangeEventArgs> ElementPositionChange;

        /// <summary>
        /// Occurs when an element is being selected.
        /// </summary>
        public event EventHandler<ElementSelectEventArgs> ElementSelect;

        /// <summary>
        /// Occurs when the position of the playhead change.
        /// </summary>
        public event EventHandler<PositionChangeEventArgs> PositionChange;

        /// <summary>
        /// Occurs when an element is being entered.
        /// </summary>
        public event EventHandler<ElementLinkEventArgs> ShowingLinks;

        /// <summary>
        /// Occurs when an element is being left.
        /// </summary>
        public event EventHandler<ElementLinkEventArgs> HidingLinks;

        /// <summary>
        /// Occurs when two elements are being linked.
        /// </summary>
        public event EventHandler<LinkElementEventArgs> LinkingElement;

        /// <summary>
        /// Occurs when the top bar is being double clicked.
        /// </summary>
        public event EventHandler<PositionPayloadEventArgs> TopBarDoubleClicked;

        /// <summary>
        /// Occurs when the elements are being refreshed.
        /// </summary>
        public event EventHandler<RefreshElementsEventArgs> RefreshingElements;

        /// <summary>
        /// Occurs when a split operation is being performed.
        /// </summary>
        public event EventHandler Split;

        /// <summary>
        /// Occurs when the playhead is being moved.
        /// </summary>
        public event EventHandler MovingPlayHead;

        /// <summary>
        /// Occurs when a delete operation is being performed.
        /// </summary>
        public event EventHandler Delete;

        /// <summary>
        /// Occurs when the status of the timeline is being changed.
        /// </summary>
        public event EventHandler TogglePlay;

        /// <summary>
        /// Occurs when the <see cref="EditMode"/> of the timeline is being changed.
        /// </summary>
        public event EventHandler ToggleEditMode;

        /// <summary>
        /// Occurs when elements start being moved.
        /// </summary>
        public event EventHandler<DataEventArgs<TimelineElement>> StartMoving;

        /// <summary>
        /// Occurs when elements stop being moved.
        /// </summary>
        public event EventHandler<DataEventArgs<TimelineElement>> StopMoving;

        /// <summary>
        /// Occurs when an undo operation is being performed.
        /// </summary>
        public event EventHandler Undo;

        /// <summary>
        /// Occurs when a redo operation is being performed.
        /// </summary>
        public event EventHandler Redo;

        /// <summary>
        /// Occurs when a thumbnail is being requested.
        /// </summary>
        public event EventHandler PickThumbnail;

        /// <summary>
        /// Defines the slider handlers that are available.
        /// </summary>
        private enum ZoomSliderHandler
        {
            /// <summary>
            /// No handler selected.
            /// </summary>
            None = 0,

            /// <summary>
            /// Left handler selected.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Middle handler selected.
            /// </summary>
            Middle = 2,

            /// <summary>
            /// Right handler selected.
            /// </summary>
            Right = 3
        }

        /// <summary>
        /// Defines the range of zoom options that are available.
        /// </summary>
        private enum Zoom
        {
            /// <summary>
            /// Represents the In zoom.
            /// </summary>
            In,

            /// <summary>
            /// Represents the Out zoom.
            /// </summary>
            Out
        }

        /// <summary>
        /// Gets or sets the PresentationModel associated with the view.
        /// </summary>
        /// <value>The presentation model.</value>
        public ITimelinePresenter Model
        {
            get { return this.DataContext as ITimelinePresenter; }
            set { this.DataContext = value; }
        }

        /// <summary>
        /// Gets or sets the start position of the timeline.
        /// </summary>
        /// <value>A value indicating the start position of the timeline.</value>
        public TimeCode ViewStartPosition
        {
            get
            {
                return this.viewStartPosition;
            }

            set
            {
                this.viewStartPosition = value;
                this.ValidateViewPositions();
                this.RefreshZoomSlider();
                this.RefreshTimeMarks();
                this.RefreshElements();
                this.UpdateTime();
            }
        }

        /// <summary>
        /// Gets or sets the end position of the timeline.
        /// </summary>
        /// <value>A value indicating the end position of the timeline.</value>
        public TimeCode ViewEndPosition
        {
            get
            {
                return this.viewEndPosition;
            }

            set
            {
                this.viewEndPosition = value;
                this.ValidateViewPositions();
                this.RefreshZoomSlider();
                this.RefreshTimeMarks();
                this.RefreshElements();
                this.UpdateTime();
            }
        }

        /// <summary>
        /// Gets or sets the start timecode offset used by the timeline.
        /// </summary>
        /// <value>The start offset of the timeline.</value>
        private TimeCode StartOffset { get; set; }

        /// <summary>
        /// Gets the scale of the view.
        /// </summary>
        /// <value>A value indicating the scale of the view.</value>
        private double ViewScale
        {
            get
            {
                var visibleTime = this.viewEndPosition.TotalSeconds - this.viewStartPosition.TotalSeconds;
                var scale = this.TimelineGrid.ActualWidth / visibleTime;
                return scale;
            }
        }

        /// <summary>
        /// Gets the <see cref="TimelineElementView"/>s of the timeline.
        /// </summary>
        /// <value>A list of the current element views.</value>
        private IDictionary<Guid, TimelineElementView> ElementViews
        {
            get
            {
                return this.elementViews;
            }
        }

        /// <summary>
        /// Sets the duration of the timeline and resets the zoom.
        /// </summary>
        /// <param name="value">The duration.</param>
        public void SetDuration(TimeCode value)
        {
            this.duration = value;
            this.ResetZoom();
        }

        /// <summary>
        /// Resets the slider zoom and refreshes the time marks and elements based on the new zoom.
        /// </summary>
        public void ResetZoom()
        {
            this.viewStartPosition = TimeCode.FromAbsoluteTime(0, this.duration.FrameRate) + this.StartOffset;
            this.viewEndPosition = this.duration;
            this.RefreshZoomSlider();
            this.RefreshTimeMarks();
            this.RefreshElements();
            this.UpdateTime();
        }

        /// <summary>
        /// Sets the playhead position to the given timecode.
        /// </summary>
        /// <param name="timeCode">The new position of the playhead.</param>
        public void SetPlayHeadPosition(TimeCode timeCode)
        {
            this.currentPosition = timeCode;
            var totalFrames = this.duration.TotalFrames;
            var visibleFrames = this.viewEndPosition.TotalFrames - this.viewStartPosition.TotalFrames;
            
            if (visibleFrames <= 0)
            {
                visibleFrames += this.StartOffset.TotalFrames;
            }

            var startFrame = this.viewStartPosition.TotalFrames;

            var layersWidth = this.TimelineGrid.ActualWidth;
            var markerOffset = layersWidth / visibleFrames;
            var newPosition = markerOffset * startFrame;
            this.PlayheadCanvas.Width = (this.TimelineGrid.ActualWidth / visibleFrames) * totalFrames;
            this.PlayheadCanvas.Margin = new Thickness(-newPosition, 0, 0, 0);
            this.MenuCanvas.Margin = new Thickness(-newPosition, this.MenuCanvas.Margin.Top, 0, 0);
            var x = this.TimeCodeToPixel(timeCode);
            Canvas.SetLeft(this.Playhead, x);
            Canvas.SetLeft(this.StripMenu, x);
        }

        /// <summary>
        /// Adds a <see cref="TimelineElement"/> to the timeline.
        /// </summary>
        /// <param name="element">The timeline element being added.</param>
        public void AddElement(TimelineElement element)
        {
            TimelineElementView elementView = new TimelineElementView(element) { IsTabStop = true };
            elementView.SetViewScale(this.ViewScale);
            elementView.EnableTimelineHandlers(this.areTimelineHandlersEnabled);
            elementView.StartDrag += this.ElementView_StartDrag;
            elementView.InSliceStartDrag += this.ElementView_InSliceStartDrag;
            elementView.OutSliceStartDrag += this.ElementView_OutSliceStartDrag;
            elementView.LinkClicked += this.ElementView_LinkClicked;
            elementView.MouseEnter += this.ElementView_MouseEnter;
            elementView.MouseLeave += this.ElementView_MouseLeave;
            elementView.Model.Volume = this.VolumeControl.Volume;
            elementView.DeleteClicked += this.ElementView_DeleteClicked;
            ToolTipService.SetToolTip(elementView, element.Asset.Title);

            this.elementViews.Add(element.Id, elementView);

            if (element.Asset is VideoAsset || element.Asset is ImageAsset)
            {
                this.VideoLayerCanvas.Children.Add(elementView);
            }
            else if (element.Asset is AudioAsset)
            {
                // this.AudioLayerCanvas.Items.Add(elementView);
                IList<Canvas> audioCanvas = this.AudioTracks.GetChildControls<Canvas>();
                foreach (Canvas canvas in audioCanvas)
                {
                    Track track = canvas.Tag as Track;

                    if (track != null && track.Shots.Contains(element))
                    {
                        canvas.Children.Add(elementView);
                    }
                }
            }
            else if (element.Asset is TitleAsset)
            {
                this.TitlesLayerCanvas.Children.Add(elementView);
            }

            elementView.RefreshPreview();
        }

        /// <summary>
        /// Removes a <see cref="TimelineElement"/> from the timeline.
        /// </summary>
        /// <param name="element">The timeline element being removed.</param>
        public void RemoveElement(TimelineElement element)
        {
            TimelineElementView elementView = this.elementViews[element.Id];
            this.VideoLayerCanvas.Children.Remove(elementView);

            IList<Canvas> audioCanvas = this.AudioTracks.GetChildControls<Canvas>();

            foreach (Canvas canvas in audioCanvas)
            {
               canvas.Children.Remove(elementView);
            }

            // this.AudioLayerCanvas.Items.Remove(elementView);
            this.TitlesLayerCanvas.Children.Remove(elementView);
            this.elementViews.Remove(element.Id);
            elementView.StartDrag -= this.ElementView_StartDrag;
            elementView.InSliceStartDrag -= this.ElementView_InSliceStartDrag;
            elementView.OutSliceStartDrag -= this.ElementView_OutSliceStartDrag;
            elementView.LinkClicked -= this.ElementView_LinkClicked;
            elementView.MouseEnter -= this.ElementView_MouseEnter;
            elementView.MouseLeave -= this.ElementView_MouseLeave;
            elementView.DeleteClicked -= this.ElementView_DeleteClicked;
        }

        /// <summary>
        /// Refreshes the given element.
        /// </summary>
        /// <param name="element">The element to refresh.</param>
        public void RefreshElement(TimelineElement element)
        {
            this.ElementViews[element.Id].Refresh();
        }

        /// <summary>
        /// Selects the given element.
        /// </summary>
        /// <param name="element">The element to select.</param>
        public void SelectElement(TimelineElement element)
        {
            this.ElementViews[element.Id].SetSelected(true);
        }

        /// <summary>
        /// Shows the download progress bar.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="offset">The offset.</param>
        public void ShowAssetDownloadProgress(TimelineElement element, double progress, double offset)
        {
            this.ElementViews[element.Id].ShowDownloadProgressBar(progress, offset);
        }

        /// <summary>
        /// Sets if the timeline handlers should be enabled or not.
        /// </summary>
        /// <param name="timelineHandlersEnabled">A true if the timeline handlers should be enabled; otherwise false.</param>
        public void UpdateTimelineHandlers(bool timelineHandlersEnabled)
        {
            this.areTimelineHandlersEnabled = timelineHandlersEnabled;
        }

        /// <summary>
        /// Unselects the given element.
        /// </summary>
        /// <param name="element">The element to unselect.</param>
        public void UnselectElement(TimelineElement element)
        {
            this.ElementViews[element.Id].SetSelected(false);
        }

       /// <summary>
        /// Resolves the layer position based on relative position.
        /// </summary>
        /// <param name="e">The relative position.</param>
        /// <returns>The layer position.</returns>
        public LayerPosition ResolveLayerPositionFromRelativePosition(MouseEventArgs e)
        {
           Point point = e.GetPosition(this.VideoLayerCanvas);
            if ((point.X >= 0 && point.X <= this.VideoLayerCanvas.ActualWidth) &&
                (point.Y >= 0 && point.Y <= this.VideoLayerCanvas.ActualHeight))
            {
                return new LayerPosition
                {
                    LayerType = TrackType.Visual,
                    Position = this.PixelToTimeCode(point.X)
                };
            }

            IList<Canvas> audioCanvas = this.AudioTracks.GetChildControls<Canvas>();

            foreach (Canvas canvas in audioCanvas)
            {
                point = e.GetPosition(canvas);
                if ((point.X >= 0 && point.X <= canvas.ActualWidth) &&
                    (point.Y >= 0 && point.Y <= canvas.ActualHeight))
                {
                    return new LayerPosition
                    {
                        Track = canvas.Tag as Track,
                        LayerType = TrackType.Audio,
                        Position = this.PixelToTimeCode(point.X)
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Shows a tooltip at an specific position.
        /// </summary>
        /// <param name="text">The text being displayed in the tooltip.</param>
        /// <param name="layerPosition">The layer position that defines the position and the layer where the tooltip is being showed.</param>
        public void ShowTooltip(string text, LayerPosition layerPosition)
        {
            this.TooltipBox.Text = text;
            var left = this.TimeCodeToPixel(layerPosition.Position);
            left += this.VideoLayerCanvas.Margin.Left;

            double top;
            switch (layerPosition.LayerType)
            {
                case TrackType.Visual:
                case TrackType.Title:
                    this.TooltipBorder.SetValue(Grid.RowProperty, 1);
                    top = this.VideoBar.TransformToVisual(this).Transform(new Point()).Y +
                          (this.VideoBar.ActualHeight / 3) - this.TopBar.ActualHeight;
                    break;
                case TrackType.Audio:
                    this.TooltipBorder.SetValue(Grid.RowProperty, 2);
                    FrameworkElement element = this.AudioTracks;

                    if (layerPosition.Track != null)
                    {
                        IList<Canvas> audioCanvas = this.AudioTracks.GetChildControls<Canvas>();

                        foreach (Canvas canvas in audioCanvas)
                        {
                            Track track = canvas.Tag as Track;

                            if (track == layerPosition.Track)
                            {
                                element = canvas;
                                break;
                            }
                        }
                    }

                    top = element.TransformToVisual(this).Transform(new Point()).Y +
                          (element.ActualHeight / 2) - this.VideoBar.ActualHeight - this.TopBar.ActualHeight;
                    break;
                default:
                    top = 0;
                    break;
            }

            left -= this.TooltipBorder.ActualWidth / 2;
            top -= this.TooltipBorder.ActualHeight / 2;

            if (left < 0)
            {
                left = 0;
            }

            if ((left + this.TooltipBorder.ActualWidth) > this.TimelineGrid.ActualWidth)
            {
                left = this.TimelineGrid.ActualWidth - this.TooltipBorder.ActualWidth;
            }

            this.TooltipBorder.Margin = new Thickness(left, top, 0, 0);
            this.TooltipBorder.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        public void HideTooltip()
        {
            this.TooltipBorder.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows a link of a timeline element.
        /// </summary>
        /// <param name="position">The position of the link being showed.</param>
        /// <param name="linked">If the link should be shown linked or not.</param>
        /// <param name="element">The element that contains the link being showed.</param>
        public void ShowLink(LinkPosition position, bool linked, TimelineElement element)
        {
            TimelineElementView view = this.ElementViews[element.Id];
            view.ShowLink(position, linked);
        }

        /// <summary>
        /// Hides a link of a timeline element.
        /// </summary>
        /// <param name="position">The position of the link.</param>
        /// <param name="element">The element that contains the link being hide.</param>
        public void HideLink(LinkPosition position, TimelineElement element)
        {
            TimelineElementView view = this.ElementViews[element.Id];
            view.HideLink(position);
        }

        /// <summary>
        /// Sets the start Timecode of the timeline.
        /// </summary>
        /// <param name="timeCode">The new start timecode.</param>
        public void SetStartTimeCode(TimeCode timeCode)
        {
            this.StartOffset = timeCode;
            this.ResetZoom();

            if (this.currentPosition >= timeCode)
            {
                this.SetPlayHeadPosition(this.currentPosition);
            }
            else
            {
                this.SetPlayHeadPosition(timeCode);
            }
        }

        /// <summary>
        /// Sets the timeline position.
        /// </summary>
        /// <param name="seconds">The new position of the timeline expressed in seconds.</param>
        [ScriptableMember]
        public void SetCurrentPosition(double seconds)
        {
            if (this.PositionChange != null)
            {
                TimeCode newPosition = TimeCode.FromSeconds(seconds, this.duration.FrameRate);
                this.PositionChange(this, new PositionChangeEventArgs { NewPosition = newPosition });
            }
        }

        /// <summary>
        /// Gets a string representation of a <seealso cref="TimeCode"/>.
        /// </summary>
        /// <param name="timecode">The timecode being used.</param>
        /// <returns>A string representation of the timecode.</returns>
        private static string GetTimeString(TimeCode timecode)
        {
            var hoursString = timecode.HoursSegment.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            var minutesString = timecode.MinutesSegment.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            var secondsString = timecode.SecondsSegment.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", hoursString, minutesString, secondsString);
        }

        /// <summary>
        /// Creates a mark line for the timeline.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="h">The height.</param>
        /// <param name="stroke">The stroke of the line.</param>
        /// <returns>The line created using the parameters.</returns>
        private static Line CreateMark(double x, double y, double h, Brush stroke)
        {
            var l = new Line
                        {
                            X1 = x,
                            X2 = x,
                            Y1 = y,
                            Y2 = y + h,
                            Stroke = stroke,
                            StrokeThickness = 1,
                            UseLayoutRounding = true
                        };

            return l;
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the TimeBar. Notifies about the playhead moves and raises the TopBarDoubleClicked event when applies. 
        /// Attaches to root visual events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void TimeBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnMovingPlayhead();

            // TODO: Avoid having to use RootVisual
            if (!this.movingPlayhead)
            {
                Application.Current.RootVisual.MouseMove += this.TimeBar_MouseMove;
                this.movingPlayhead = true;
                this.PlayheadContentControl.Focus();
            }

            Application.Current.RootVisual.MouseLeftButtonUp += this.TimeBar_MouseLeftButtonUp;
            this.TimeBar_MouseMove(sender, e);

            if ((DateTime.Now.Ticks - this.lastClickTicks) < UtilityHelper.MouseDoubleClickDurationValue)
            {
                double positionClicked = this.PixelToTimeCode(e.GetPosition(this.VideoLayerCanvas).X).TotalSeconds;
                this.OnTopBarDoubleClicked(TimeSpan.FromSeconds(positionClicked));

                this.lastClickTicks = 0;
            }

            this.lastClickTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Handles the MouseMove event of the TimeBar. Notifies about the position change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void TimeBar_MouseMove(object sender, MouseEventArgs e)
        {
            var x = e.GetPosition(this.VideoLayerCanvas).X;

            var timecode = this.PixelToTimeCode(x);

            timecode = timecode.TotalSecondsPrecision < 0 ? TimeCode.FromAbsoluteTime(0, timecode.FrameRate) : timecode;

            if (this.PositionChange != null)
            {
                this.PositionChange(this, new PositionChangeEventArgs { NewPosition = timecode });
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the TimeBar. Detaches from the root visual events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void TimeBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.RootVisual.MouseMove -= this.TimeBar_MouseMove;
            Application.Current.RootVisual.MouseLeftButtonUp -= this.TimeBar_MouseLeftButtonUp;
            this.movingPlayhead = false;
        }

        /// <summary>
        /// Handles the InSliceStartDrag event of the ElementView. Start observing changes on the element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_InSliceStartDrag(object sender, MouseButtonEventArgs e)
        {
            ElementPositionType elementPositionType = this.areTimelineHandlersEnabled ? ElementPositionType.InPosition : ElementPositionType.Position;
            
            var elementView = sender as TimelineElementView;
            if (elementView != null)
            {
                this.StartObservingElementChanges(elementView, elementPositionType, e);
            }
        }

        /// <summary>
        /// Handles the OutSliceStartDrag event of the ElementView. Start observing changes on the element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_OutSliceStartDrag(object sender, MouseButtonEventArgs e)
        {
            ElementPositionType elementPositionType = this.areTimelineHandlersEnabled ? ElementPositionType.OutPosition : ElementPositionType.Position;

            var elementView = sender as TimelineElementView;
            if (elementView != null)
            {
                this.StartObservingElementChanges(elementView, elementPositionType, e);
            }
        }

        /// <summary>
        /// Handles the StartDrag envet of the ElementView. Start observing changes on the element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_StartDrag(object sender, MouseButtonEventArgs e)
        {
            TimelineElementView elementView = sender as TimelineElementView;
            if (elementView != null)
            {
                this.StartObservingElementChanges(elementView, ElementPositionType.Position, e);
            }
        }

        /// <summary>
        /// Observes element changes.
        /// </summary>
        /// <param name="elementView">The element view that contains the element being observed.</param>
        /// <param name="positionType">The position type to monitor.</param>
        /// <param name="e">The event args.</param>
        private void StartObservingElementChanges(TimelineElementView elementView, ElementPositionType positionType, MouseEventArgs e)
        {
            this.activeElement = elementView;
            this.elementDragOffset = e.GetPosition(elementView).X;

            Application.Current.RootVisual.MouseMove += this.ElementView_OnDrag;
            Application.Current.RootVisual.MouseLeftButtonUp += this.ElementView_StopDrag;
            this.activeElementDraggingOption = positionType;

            ElementSelectEventArgs args = new ElementSelectEventArgs
                                                  {
                                                      Element = this.activeElement.Model,
                                                      Position = this.PixelToTimeCode(e.GetPosition(this.VideoLayerCanvas).X)
                                                  };

            this.OnElementSelect(args);
        }
        
        /// <summary>
        /// Handles the StopDrag event of the ElementView. Stop the drag of the active element.
        /// Detaches from the root visual events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_StopDrag(object sender, MouseButtonEventArgs e)
        {
            Application.Current.RootVisual.MouseMove -= this.ElementView_OnDrag;
            Application.Current.RootVisual.MouseLeftButtonUp -= this.ElementView_StopDrag;

            if (this.activeElement != null && this.activeElementDraggingOption != ElementPositionType.Position &&
                this.activeElementDraggingOption != ElementPositionType.None)
            {
                this.activeElement.RefreshPreview(RefreshSource.Drag);
            }

            if (this.movingElements && this.activeElement != null)
            {
                this.movingElements = false;
                this.OnStopMoving(this.activeElement.Model);
            }

            this.activeElement = null;
            this.activeElementDraggingOption = ElementPositionType.None;
            this.elementDragOffset = 0;

            this.OnElementSelect(new ElementSelectEventArgs());
        }

        /// <summary>
        /// Handles the OnDrag event of the Element view.  Starts the drag of the active element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_OnDrag(object sender, MouseEventArgs e)
        {
            if (!this.isTimelineLocked)
            {
                TimelineElementView element = this.activeElement;
                if (element == null)
                {
                    return;
                }

                if (!this.movingElements)
                {
                    this.movingElements = true;
                    this.OnStartMoving(element.Model);
                }

                if (this.activeElementDraggingOption != ElementPositionType.None && this.ElementPositionChange != null)
                {
                    var args = new ElementPositionChangeEventArgs
                                   {
                                       PositionType = this.activeElementDraggingOption
                                   };

                    var x = e.GetPosition(this.VideoLayerCanvas).X;
                    switch (this.activeElementDraggingOption)
                    {
                        case ElementPositionType.Position:
                            args.NewPosition = this.PixelToTimeCode(x - this.elementDragOffset);
                            break;
                        case ElementPositionType.InPosition:
                            // HACK: 
                            args.NewPosition = this.PixelToTimeCode(x);
                            break;
                        case ElementPositionType.OutPosition:
                            // HACK: 
                            args.NewPosition = this.PixelToTimeCode(x);
                            break;
                    }

                    this.ElementPositionChange(this, args);
                }
            }
        }

        /// <summary>
        /// Handles the MouseEnter event of the Element View. Tries to show the links of the active element if applies.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_MouseEnter(object sender, MouseEventArgs e)
        {
            TimelineElementView view = sender as TimelineElementView;

            if (view != null)
            {
                this.currentZIndex = Canvas.GetZIndex(view);
                Canvas.SetZIndex(view, 200);
                this.OnShowingLinks(view.Model);
            }
        }

        /// <summary>
        /// Handles the MouseLeave event of the Element View. Tries to hide the links of the active element if applies.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ElementView_MouseLeave(object sender, MouseEventArgs e)
        {
            TimelineElementView view = sender as TimelineElementView;

            if (view != null)
            {
                Canvas.SetZIndex(view, this.currentZIndex);
                this.OnHidingLinks(view.Model);
            }
        }

        /// <summary>
        /// Handles the LinkClicked event of the ElementView. Links/Unlink the element.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args that contains the element and the link position.</param>
        private void ElementView_LinkClicked(object sender, LinkElementEventArgs e)
        {
            TimelineElementView view = sender as TimelineElementView;

            if (view != null)
            {
                this.OnLinkingElement(e.Element, e.LinkPosition);
            }
        }

        /// <summary>
        /// Handles the DeleteClicked event of the ElementView. Deletes the elemenet.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args that contains the element.</param>
        private void ElementView_DeleteClicked(object sender, DataEventArgs<TimelineElement> e)
        {
            this.OnElementSelect(new ElementSelectEventArgs { Element = e.Data });
            this.OnDelete();
        }

        /// <summary>
        /// Valides the view positions to prevent inconsistencies.
        /// </summary>
        private void ValidateViewPositions()
        {
            if (this.viewEndPosition > this.duration)
            {
                this.viewEndPosition = this.duration;
            }

            if (this.viewStartPosition == this.viewEndPosition)
            {
                this.viewEndPosition = this.duration;
            }

            if (this.viewStartPosition > this.duration)
            {
                this.viewStartPosition = TimeCode.FromAbsoluteTime(0, this.duration.FrameRate);
            }
        }

        /// <summary>
        /// Converts a timecode into a pixel representation.
        /// </summary>
        /// <param name="timecode">The timecode being converted.</param>
        /// <returns>The pixel representation of the timecode.</returns>
        private double TimeCodeToPixel(TimeCode timecode)
        {
            return (this.VideoLayerCanvas.Width * timecode.TotalSeconds) / this.duration.TotalSeconds;
        }

        /// <summary>
        /// Converts a pixel into a timecode representation.
        /// </summary>
        /// <param name="px">The pixel being converted.</param>
        /// <returns>The timecode representation of the pixel.</returns>
        private TimeCode PixelToTimeCode(double px)
        {
            px = px < 0 ? 0 : px;
            return TimeCode.FromAbsoluteTime((this.duration.TotalSeconds * px) / this.VideoLayerCanvas.ActualWidth, this.duration.FrameRate);
        }

        /// <summary>
        /// Refreshes the elements using the current view scale. Refreshes also the layers margins.
        /// </summary>
        private void RefreshElements()
        {
            this.RefreshLayersMargin();

            var scale = this.ViewScale;

            // Video elements
            foreach (TimelineElementView assetView in this.VideoLayerCanvas.Children)
            {
                assetView.SetViewScale(scale);
            }

            // Audio elements
            IList<Canvas> audioCanvas = this.AudioTracks.GetChildControls<Canvas>();
            foreach (Canvas canvas in audioCanvas)
            {
                foreach (TimelineElementView assetView in canvas.Children)
                {
                    assetView.SetViewScale(scale);
                }
            }

            // foreach (TimelineElementView assetView in this.AudioLayerCanvas.Items)
            // {
            //    assetView.SetViewScale(scale);
            // }

            // Title elements
            foreach (TimelineElementView assetView in this.TitlesLayerCanvas.Children)
            {
                assetView.SetViewScale(scale);
            }

            double width = this.VideoLayerCanvas.Width;
            this.OnRefreshingElements(width);
        }

        /// <summary>
        /// Refreshes layers margins.
        /// </summary>
        private void RefreshLayersMargin()
        {
            var totalTime = this.duration.TotalSeconds;
            var visibleTime = this.viewEndPosition.TotalSeconds - this.viewStartPosition.TotalSeconds;

            if (visibleTime < 0)
            {
                visibleTime += this.StartOffset.TotalSeconds;
            }

            var startTime = this.viewStartPosition.TotalSeconds;

            var layersWidth = this.TimelineGrid.ActualWidth;
            var markerOffset = layersWidth / visibleTime;
            var newPosition = markerOffset * startTime;
            this.VideoLayerCanvas.Width = (this.TimelineGrid.ActualWidth / visibleTime) * totalTime;
            var layersMargin = new Thickness(-newPosition, 0, 0, 0);

            this.VideoLayerCanvas.Margin = layersMargin;
            this.AudioTracks.Margin = layersMargin;
            this.CommentsBarPlaceholder.Margin = layersMargin;
            this.TitlesLayerCanvas.Margin = layersMargin;
            this.TimeMarksCanvas.Margin = layersMargin;
        }

        /// <summary>
        /// Refreshes the time marks.
        /// </summary>
        private void RefreshTimeMarks()
        {
            if (Double.IsNaN(this.TimelineGrid.ActualWidth) || this.TimelineGrid.ActualWidth <= 0 ||
                this.ViewStartPosition >= (this.ViewEndPosition + this.StartOffset) || this.duration.TotalSeconds <= 0)
            {
                return;
            }

            // Get time values
            var totalTime = TimeCode.FromFrames(this.duration.TotalFrames + 1, this.duration.FrameRate).TotalSeconds;
            var visibleTime = this.viewEndPosition.TotalSeconds - this.viewStartPosition.TotalSeconds;

            if (visibleTime < 0)
            {
                visibleTime += this.StartOffset.TotalSeconds;
            }

            // Get max widths and heights
            var layersTop = this.TopBar.ActualHeight;
            var layersHeight = this.VideoBar.ActualHeight + this.AudioTracks.ActualHeight + 4;
            var layersWidth = this.TimelineGrid.ActualWidth;
            var markerOffset = layersWidth / visibleTime;

            // Marks position
            this.TimeMarksCanvas.Height = layersHeight;

            // ix, start/end, 1sec width
            var markIx = 0;
            var labelMarkIx = 0;
            var st = this.viewStartPosition.TotalSeconds;
            var et = this.viewEndPosition.TotalSeconds + this.StartOffset.TotalSeconds;
            var minWidth = markerOffset;
            var modifier = 1;
            while (markerOffset < 10)
            {
                markerOffset += markerOffset;
                modifier += modifier;
            }

            for (var t = 0; t < totalTime; t += modifier)
            {
                if (t < st || t > et)
                {
                    continue;
                }

                Line mark;
                var x = t * minWidth;
                if (markIx >= this.mainBarMarkers.Count)
                {
                    // create
                    mark = CreateMark(x, layersTop, layersHeight, this.mainMarkerStroke);
                    this.TimeMarksCanvas.Children.Add(mark);
                    this.mainBarMarkers.Add(mark);
                }
                else
                {
                    // update
                    mark = this.mainBarMarkers[markIx];
                    mark.Visibility = Visibility.Visible;
                    mark.X1 = x;
                    mark.X2 = x;
                }

                Line topMark;
                if (markIx >= this.topBarMarkers.Count)
                {
                    // create
                    topMark = CreateMark(x, 1, 1, this.topMarkerStroke);
                    this.TimeMarksCanvas.Children.Add(topMark);
                    this.topBarMarkers.Add(topMark);
                }
                else
                {
                    // update
                    topMark = this.topBarMarkers[markIx];
                    topMark.Visibility = Visibility.Visible;
                    topMark.X1 = x;
                    topMark.X2 = x;
                }

                markIx++;

                if (t % (10 * modifier) != 0)
                {
                    topMark.StrokeThickness = 1;
                    topMark.Y1 = layersTop - 8;
                    topMark.Y2 = layersTop - 8 + 6;
                }
                else
                {
                    topMark.StrokeThickness = 2;
                    topMark.Y1 = 3;
                    topMark.Y2 = layersTop - 2;

                    // LABEL
                    TextBlock label;
                    if (labelMarkIx >= this.topBarLabels.Count)
                    {
                        label = new TextBlock
                                    {
                                        FontFamily = new FontFamily("Verdana"),
                                        FontSize = 9,
                                        Foreground = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120))
                                    };

                        this.TimeMarksCanvas.Children.Add(label);
                        this.topBarLabels.Add(label);
                    }
                    else
                    {
                        label = this.topBarLabels[labelMarkIx];
                        label.Visibility = Visibility.Visible;
                    }

                    label.Text = GetTimeString(TimeCode.FromAbsoluteTime(t, this.duration.FrameRate));
                    Canvas.SetLeft(label, x - label.ActualWidth - 3);
                    Canvas.SetTop(label, 10);

                    labelMarkIx++;
                }
            }

            // hide any remaining marks
            for (var i = markIx; i < this.mainBarMarkers.Count; i++)
            {
                this.mainBarMarkers[i].Visibility = Visibility.Collapsed;
                this.topBarMarkers[i].Visibility = Visibility.Collapsed;
            }

            for (var i = labelMarkIx; i < this.topBarLabels.Count; i++)
            {
                this.topBarLabels[i].Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Refresh the TimeMarks height.
        /// </summary>
        private void RefreshTimeMarksHeight()
        {
            double layersHeight = this.VideoBar.ActualHeight + this.AudioTracks.ActualHeight + 4;

            IEnumerable<Line> lines = this.TimeMarksCanvas.Children.Where(x => x is Line).Cast<Line>();
            
            foreach (Line line in lines)
            {
                double newHeight = line.Y1 + layersHeight;
                if (newHeight != line.Y2)
                {
                    line.Y2 = newHeight;
                }
            }
        }

        /// <summary>
        /// Updates the StartPosition, the CurrentRange and the EndPosition text.
        /// </summary>
        private void UpdateTime()
        {
            // time
            this.StartPositionText.Text = GetTimeString(this.viewStartPosition);

            double totalSeconds = this.viewEndPosition.TotalSeconds - this.viewStartPosition.TotalSeconds;

            if (totalSeconds < 0)
            {
                this.CurrentRangeText.Text =
                    GetTimeString((this.viewEndPosition + this.StartOffset) - this.viewStartPosition);
            }
            else
            {
                this.CurrentRangeText.Text = GetTimeString(this.viewEndPosition - this.viewStartPosition);
            }

            this.EndPositionText.Text = GetTimeString(this.viewEndPosition);
        }

        /// <summary>
        /// Refreshes the zoom slider.
        /// </summary>
        private void RefreshZoomSlider()
        {
            if (this.TimelineGrid.ActualWidth <= 0 || this.duration.TotalSeconds <= 0)
            {
                return;
            }

            var modifier = this.TimelineGrid.ActualWidth / (this.duration.TotalSeconds - this.StartOffset.TotalSeconds);
            var x1 = (this.ViewStartPosition.TotalSeconds - this.StartOffset.TotalSeconds) * modifier;
            var x2 = (this.ViewEndPosition.TotalSeconds - this.StartOffset.TotalSeconds) * modifier;

            Canvas.SetLeft(this.ZoomSliderLeftHandler, x1);
            Canvas.SetLeft(this.ZoomSliderRightHandler, x2 - this.ZoomSliderRightHandler.ActualWidth);

            this.RefreshZoomSliderMiddleHandler();
        }

        /// <summary>
        /// Refreshes the zoom slider middle handler.
        /// </summary>
        private void RefreshZoomSliderMiddleHandler()
        {
            Canvas.SetLeft(this.ZoomSliderMiddleHandler, Canvas.GetLeft(this.ZoomSliderLeftHandler) + this.ZoomSliderLeftHandler.ActualWidth);
            this.ZoomSliderMiddleHandler.Width = Canvas.GetLeft(this.ZoomSliderRightHandler) -
                                                 Canvas.GetLeft(this.ZoomSliderMiddleHandler);
        }

        /// <summary>
        /// Handles the MouseWheel event of the Timeline Grid. Used to Zoom In / Zoom out the timeline.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void TimelineGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
              Point point = e.GetPosition(this.TimelineGrid);
              if ((point.X >= 0 && point.X <= this.TimelineGrid.ActualWidth) &&
                  (point.Y >= 0 && point.Y <= this.TimelineGrid.ActualHeight))
              {
                  double delta = e.Delta;

                  if (delta > 0)
                  {
                      this.ZoomHandler(Zoom.In, 10);
                  }
                  else if (delta < 0)
                  {
                      this.ZoomHandler(Zoom.Out, 10);
                  }

                  e.Handled = true;
              }
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the ZoomSliderLeftHandler. Attaches to root visual events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void ZoomSliderLeftHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.activeZoomHandler = ZoomSliderHandler.Left;
            this.lastKnownMousePosition = e.GetPosition(this.ZoomSliderCanvas).X;
            this.AttachToRootMouseEvents();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the ZoomMiddleLeftHandler. Attaches to root visual events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void ZoomSliderMiddleHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.activeZoomHandler = ZoomSliderHandler.Middle;
            this.lastKnownMousePosition = e.GetPosition(this.ZoomSliderCanvas).X;
            this.AttachToRootMouseEvents();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the ZoomSliderRightHandler. Attaches to the root visual mouse events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void ZoomSliderRightHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.activeZoomHandler = ZoomSliderHandler.Right;
            this.lastKnownMousePosition = e.GetPosition(this.ZoomSliderCanvas).X;
            this.AttachToRootMouseEvents();
        }

        /// <summary>
        /// Handles the MouseMove event of the ZoomSliderHandler. Refreshes the zoom slider.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ZoomSliderHandler_MouseMove(object sender, MouseEventArgs e)
        {
            double offset = this.lastKnownMousePosition - e.GetPosition(this.ZoomSliderCanvas).X;
            if (this.RefreshZoomSlider(this.activeZoomHandler, offset))
            {
                this.lastKnownMousePosition = e.GetPosition(this.ZoomSliderCanvas).X;
            }
        }

        /// <summary>
        /// Set ZoomSlider's position corresponding to the offset.
        /// </summary>
        /// <param name="zoomHandler">ZoomSliderHandler value.<see cref="ZoomSliderHandler"/>.</param>
        /// <param name="offset">Mouse move offset.</param>
        /// <returns>True if the same slider can move in the same direction else false.</returns>
        private bool RefreshZoomSlider(ZoomSliderHandler zoomHandler, double offset)
        {
            var updated = false;
            double newX;
            bool setMousePosition = false;
            bool isChanged = false;

            switch (zoomHandler)
            {
                case ZoomSliderHandler.Left:
                    newX = Canvas.GetLeft(this.ZoomSliderLeftHandler) - offset;
                    if (newX < 0)
                    {
                        newX = 0;
                    }
                    else if (newX >
                             Canvas.GetLeft(this.ZoomSliderRightHandler) - MinimunZoomSliderSize -
                             this.ZoomSliderLeftHandler.ActualWidth)
                    {
                        newX = Canvas.GetLeft(this.ZoomSliderRightHandler) - MinimunZoomSliderSize -
                               this.ZoomSliderLeftHandler.ActualWidth;
                    }
                    else
                    {
                        setMousePosition = true;
                    }

                    // Check if there is any change.
                    if ((double)this.ZoomSliderLeftHandler.GetValue(Canvas.LeftProperty) != newX)
                    {
                        Canvas.SetLeft(this.ZoomSliderLeftHandler, newX);
                        this.RefreshZoomSliderMiddleHandler();
                        isChanged = true;
                    }

                    updated = true;
                    break;
                case ZoomSliderHandler.Right:
                    newX = Canvas.GetLeft(this.ZoomSliderRightHandler) - offset;
                    if (newX > this.TimelineGrid.ActualWidth - this.ZoomSliderRightHandler.ActualWidth)
                    {
                        newX = this.TimelineGrid.ActualWidth - this.ZoomSliderRightHandler.ActualWidth;
                    }
                    else if (newX <
                             Canvas.GetLeft(this.ZoomSliderLeftHandler) + this.ZoomSliderLeftHandler.ActualWidth +
                             MinimunZoomSliderSize)
                    {
                        newX = Canvas.GetLeft(this.ZoomSliderLeftHandler) + this.ZoomSliderLeftHandler.ActualWidth +
                               MinimunZoomSliderSize;
                    }
                    else
                    {
                        setMousePosition = true;
                    }

                    // Check if there is any change.
                    if ((double)this.ZoomSliderRightHandler.GetValue(Canvas.LeftProperty) != newX)
                    {
                        Canvas.SetLeft(this.ZoomSliderRightHandler, newX);
                        this.RefreshZoomSliderMiddleHandler();
                        isChanged = true;
                    }

                    updated = true;
                    break;
                case ZoomSliderHandler.Middle:
                    newX = Canvas.GetLeft(this.ZoomSliderMiddleHandler) - offset;

                    if (newX - this.ZoomSliderLeftHandler.ActualWidth < 0)
                    {
                        newX = this.ZoomSliderLeftHandler.ActualWidth;
                    }
                    else if (newX + this.ZoomSliderMiddleHandler.ActualWidth + this.ZoomSliderRightHandler.ActualWidth >
                             this.TimelineGrid.ActualWidth)
                    {
                        newX = this.TimelineGrid.ActualWidth -
                               (this.ZoomSliderMiddleHandler.ActualWidth + this.ZoomSliderRightHandler.ActualWidth);
                    }
                    else
                    {
                        setMousePosition = true;
                    }

                    if ((double)this.ZoomSliderLeftHandler.GetValue(Canvas.LeftProperty) != newX - this.ZoomSliderLeftHandler.ActualWidth)
                    {
                        Canvas.SetLeft(this.ZoomSliderLeftHandler, newX - this.ZoomSliderLeftHandler.ActualWidth);
                        isChanged = true;
                    }

                    if ((double)this.ZoomSliderRightHandler.GetValue(Canvas.LeftProperty) != newX + this.ZoomSliderMiddleHandler.ActualWidth)
                    {
                        Canvas.SetLeft(this.ZoomSliderRightHandler, newX + this.ZoomSliderMiddleHandler.ActualWidth);
                        isChanged = true;
                    }

                    if ((double)this.ZoomSliderMiddleHandler.GetValue(Canvas.LeftProperty) != newX)
                    {
                        Canvas.SetLeft(this.ZoomSliderMiddleHandler, newX);
                        isChanged = true;
                    }

                    this.UpdateTime();

                    break;
            }

            if (isChanged)
            {
                this.viewStartPosition =
                    TimeCode.FromAbsoluteTime(
                        ((this.duration.TotalSeconds - this.StartOffset.TotalSeconds) / this.TimelineGrid.ActualWidth) * Canvas.GetLeft(this.ZoomSliderLeftHandler),
                        this.duration.FrameRate) + this.StartOffset;
                this.viewEndPosition =
                    TimeCode.FromAbsoluteTime(
                        ((this.duration.TotalSeconds - this.StartOffset.TotalSeconds) / this.TimelineGrid.ActualWidth) *
                        (Canvas.GetLeft(this.ZoomSliderRightHandler) + this.ZoomSliderRightHandler.ActualWidth),
                        this.duration.FrameRate) + this.StartOffset;

                this.UpdateTime();

                this.RefreshTimeMarks();
                if (!updated)
                {
                    this.RefreshLayersMargin();
                }
                else
                {
                    this.RefreshElements();
                }

                this.SetPlayHeadPosition(this.currentPosition);
            }

            return setMousePosition;
        }

        /// <summary>
        /// Handle the keyboard Zoom In/Out command and mouse wheel operation.
        /// It tries to put the playhead at the center of the visible portion of the slider canvas 
        /// and then decides on if the ZoomSliderRightHandler should move or ZoomSliderLeftHandler should move.
        /// </summary>
        /// <param name="zoom">The <see cref="Zoom"/>value.</param>
        private void ZoomHandler(Zoom zoom)
        {
            this.ZoomHandler(zoom, ZoomValue);
        }

        /// <summary>
        /// Handle the keyboard Zoom In/Out command and mouse wheel operation.
        /// It tries to put the playhead at the center of the visible portion of the slider canvas 
        /// and then decides on if the ZoomSliderRightHandler should move or ZoomSliderLeftHandler should move.
        /// </summary>
        /// <param name="zoom">The <see cref="Zoom"/>value.</param>
        /// /// <param name="zoomValue">The zoom value.</param>
        private void ZoomHandler(Zoom zoom, double zoomValue)
        {
            ZoomSliderHandler zoomSliderHandler = this.currentPosition.TotalSeconds >= this.ViewStartPosition.TotalSeconds + ((this.viewEndPosition - this.viewStartPosition).TotalSeconds / 2)
                                                      ? zoom == Zoom.In ? ZoomSliderHandler.Left : ZoomSliderHandler.Right
                                                      : zoom == Zoom.In ? ZoomSliderHandler.Right : ZoomSliderHandler.Left;

            double offset = zoomSliderHandler == ZoomSliderHandler.Left
                                ? zoom == Zoom.In ? 0 - zoomValue : zoomValue
                                : zoom == Zoom.In ? zoomValue : 0 - zoomValue;

            // if RefreshZoomSlider returns false then this means that the slider didn't moved by the given amount
            // so move the other slider by the same amount.
            if (!this.RefreshZoomSlider(zoomSliderHandler, offset))
            {
                zoomSliderHandler = zoomSliderHandler == ZoomSliderHandler.Left ? ZoomSliderHandler.Right : ZoomSliderHandler.Left;
                offset = zoomSliderHandler == ZoomSliderHandler.Left
                             ? zoom == Zoom.In ? 0 - zoomValue : zoomValue
                             : zoom == Zoom.In ? zoomValue : 0 - zoomValue;
                this.RefreshZoomSlider(zoomSliderHandler, offset);
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the Zoom Slider Handler. Refreshes the elemnts previews and detaches of the root visual mouse events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ZoomSliderHandler_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.activeZoomHandler = ZoomSliderHandler.None;
            this.DetachZoomSliderToRootMouseEvents();

            foreach (TimelineElementView elementView in this.elementViews.Values)
            {
                elementView.RefreshPreview(RefreshSource.Zoom);
            }
        }

        /// <summary>
        /// Occurs when the user control is being resized. Refresh the zoom slider, the time mars and the elements. Also sets the playhead to the current position.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshZoomSlider();
            this.RefreshTimeMarks();
            this.RefreshElements();
            this.SetPlayHeadPosition(this.currentPosition);

            this.TimelineGrid.Clip = new RectangleGeometry
                                         {
                                             Rect = new Rect(0, 0, this.TimelineGrid.ActualWidth, this.TimelineGrid.ActualHeight)
                                         };
        }

        /// <summary>
        /// Attachs to the root visual mouse events.
        /// </summary>
        private void AttachToRootMouseEvents()
        {
            Application.Current.RootVisual.MouseLeftButtonUp += this.ZoomSliderHandler_MouseLeftButtonUp;
            Application.Current.RootVisual.MouseMove += this.ZoomSliderHandler_MouseMove;
        }

        /// <summary>
        /// Detach fom the root visual mouse events.
        /// </summary>
        private void DetachZoomSliderToRootMouseEvents()
        {
            Application.Current.RootVisual.MouseLeftButtonUp -= this.ZoomSliderHandler_MouseLeftButtonUp;
            Application.Current.RootVisual.MouseMove -= this.ZoomSliderHandler_MouseMove;
        }

        /// <summary>
        /// Handle the KeyDown event of the root visual. Executes different operation based on the key triggered.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args that contains the key triggered.</param>
        private void RootVisual_KeyDown(object sender, KeyEventArgs e)
        {
            ContentControl contentControl = e.OriginalSource as ContentControl;

            bool handleKey = (e.OriginalSource is TimelineView) ||
                             (e.OriginalSource is TimelineElementView) ||
                             (contentControl != null &&
                              contentControl.Name.ToUpper(CultureInfo.InvariantCulture).StartsWith("PLAYHEAD", StringComparison.OrdinalIgnoreCase));

            if (handleKey)
            {
                switch (e.Key)
                {
                    case Key.S:
                        if (!this.isTimelineLocked)
                        {
                            this.OnSplit();
                        }

                        e.Handled = true;
                        break;

                    case Key.Space:
                        this.OnTogglePlay();
                        e.Handled = true;
                        break;

                    case Key.Delete:
                    case Key.Back:
                        if (!this.isTimelineLocked)
                        {
                            this.OnDelete();
                        }

                        e.Handled = true;
                        break;

                    case Key.Z:
                        if (ModifierKeys.Control == Keyboard.Modifiers && !this.isTimelineLocked)
                        {
                            this.OnUndo();
                        }

                        break;

                    case Key.Y:
                        if (ModifierKeys.Control == Keyboard.Modifiers && !this.isTimelineLocked)
                        {
                            this.OnRedo();
                        }

                        break;

                    case Key.F:
                        if (!this.isTimelineLocked)
                        {
                            this.OnToggleEditMode();
                        }

                        e.Handled = true;
                        break;

                    case Key.Up:
                        this.ZoomHandler(Zoom.In);
                        e.Handled = true;
                        break;

                    case Key.Down:
                        this.ZoomHandler(Zoom.Out);
                        e.Handled = true;
                        break;

                    case Key.I:
                        if (!this.isTimelineLocked)
                        {
                            this.OnTrimElementToCurrentPosition(ElementPositionType.InPosition);
                        }

                        break;

                    case Key.O:
                        if (!this.isTimelineLocked)
                        {
                            this.OnTrimElementToCurrentPosition(ElementPositionType.OutPosition);
                        }

                        break;

                    case Key.T:
                        this.OnPickThumbnail();

                        e.Handled = true;
                        break;

                    case Key.J:
                        this.Model.MovePreviousClipCommand.Execute(null);
                        e.Handled = true;
                        break;

                    case Key.K:
                        this.Model.MoveNextClipCommand.Execute(null);
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Raises the ToggleEditMode event.
        /// </summary>
        private void OnToggleEditMode()
        {
            EventHandler toggleEditModeHandler = this.ToggleEditMode;
            if (toggleEditModeHandler != null)
            {
                toggleEditModeHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the Split event.
        /// </summary>
        private void OnSplit()
        {
            EventHandler splitHandler = this.Split;
            if (splitHandler != null)
            {
                splitHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the MovingPlayhead event.
        /// </summary>
        private void OnMovingPlayhead()
        {
            EventHandler movingPlayheadHandler = this.MovingPlayHead;
            if (movingPlayheadHandler != null)
            {
                movingPlayheadHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the Delete event.
        /// </summary>
        private void OnDelete()
        {
            EventHandler deleteHandler = this.Delete;
            if (deleteHandler != null)
            {
                deleteHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the OnPickThumbnail event.
        /// </summary>
        private void OnPickThumbnail()
        {
            EventHandler thumbnail = this.PickThumbnail;
            if (thumbnail != null)
            {
                thumbnail(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the TogglePlay event.
        /// </summary>
        private void OnTogglePlay()
        {
            EventHandler togglePlayHandler = this.TogglePlay;
            if (togglePlayHandler != null)
            {
                togglePlayHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the ShowingLinks event.
        /// </summary>
        /// <param name="element">The timeline element passed in the arguments of the event.</param>
        private void OnShowingLinks(TimelineElement element)
        {
            EventHandler<ElementLinkEventArgs> showingLinksHandler = this.ShowingLinks;
            if (showingLinksHandler != null)
            {
                showingLinksHandler(this, new ElementLinkEventArgs(element));
            }
        }

        /// <summary>
        /// Raises the HidinngLinks event.
        /// </summary>
        /// <param name="element">The timeline element passed in the arguments of the event.</param>
        private void OnHidingLinks(TimelineElement element)
        {
            EventHandler<ElementLinkEventArgs> hidingLinksHandler = this.HidingLinks;
            if (hidingLinksHandler != null)
            {
                hidingLinksHandler(this, new ElementLinkEventArgs(element));
            }
        }

        /// <summary>
        /// Raises the LinkingElement event.
        /// </summary>
        /// <param name="element">The element being linking that is used in the event args.</param>
        /// <param name="linkPosition">The link position being used in the event args.</param>
        private void OnLinkingElement(TimelineElement element, LinkPosition linkPosition)
        {
            EventHandler<LinkElementEventArgs> linkingElementHandler = this.LinkingElement;
            if (linkingElementHandler != null)
            {
                linkingElementHandler(this, new LinkElementEventArgs(element, linkPosition));
            }
        }

        /// <summary>
        /// Raises the TopBarDoubleClicked event.
        /// </summary>
        /// <param name="position">The position being used in the event args.</param>
        private void OnTopBarDoubleClicked(TimeSpan position)
        {
            EventHandler<PositionPayloadEventArgs> topBarDoubleClickedHandler = this.TopBarDoubleClicked;
            if (topBarDoubleClickedHandler != null)
            {
                topBarDoubleClickedHandler(this, new PositionPayloadEventArgs(position));
            }
        }

        /// <summary>
        /// Raises the RefreshingElements event.
        /// </summary>
        /// <param name="width">The width being used in the event args.</param>
        private void OnRefreshingElements(double width)
        {
            EventHandler<RefreshElementsEventArgs> refreshingElementsHandler = this.RefreshingElements;
            if (refreshingElementsHandler != null)
            {
                refreshingElementsHandler(this, new RefreshElementsEventArgs(width));
            }
        }

        /// <summary>
        /// Raises the Undo event.
        /// </summary>
        private void OnUndo()
        {
            EventHandler undoHandler = this.Undo;
            if (undoHandler != null)
            {
                undoHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the Redo event.
        /// </summary>
        private void OnRedo()
        {
            EventHandler redoHandler = this.Redo;
            if (redoHandler != null)
            {
                redoHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the TrimElementToCurrentPosition event.
        /// </summary>
        /// <param name="postiontype">The position type being used in the event args.</param>
        private void OnTrimElementToCurrentPosition(ElementPositionType postiontype)
        {
            EventHandler<ElementPositionChangeEventArgs> trimElementEventHandler = this.TrimElementToCurrentPosition;
            if (trimElementEventHandler != null)
            {
                trimElementEventHandler(this, new ElementPositionChangeEventArgs { PositionType = postiontype });
            }
        }

        /// <summary>
        /// Raises the StartMoving event.
        /// </summary>
        /// <param name="element">The element being used in the event args.</param>
        private void OnStartMoving(TimelineElement element)
        {
            EventHandler<DataEventArgs<TimelineElement>> startMovingHandler = this.StartMoving;
            if (startMovingHandler != null)
            {
                startMovingHandler(this, new DataEventArgs<TimelineElement>(element));
            }
        }

        /// <summary>
        /// Raises the StopMoving event.
        /// </summary>
        /// <param name="element">The element being used in the event args.</param>
        private void OnStopMoving(TimelineElement element)
        {
            EventHandler<DataEventArgs<TimelineElement>> stopMovingHandler = this.StopMoving;
            if (stopMovingHandler != null)
            {
                stopMovingHandler(this, new DataEventArgs<TimelineElement>(element));
            }
        }

        /// <summary>
        /// Raises the ElementSelect event.
        /// </summary>
        /// <param name="e">The event args that contains event data.</param>
        private void OnElementSelect(ElementSelectEventArgs e)
        {
            EventHandler<ElementSelectEventArgs> elementSelectHandler = this.ElementSelect;
            if (elementSelectHandler != null)
            {
                elementSelectHandler(this, e);
            }
        }

        /// <summary>
        /// Toggle the lock of the timeline.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void LockTimeline_Click(object sender, RoutedEventArgs e)
        {
            this.isTimelineLocked = !this.isTimelineLocked;
        }

        /// <summary>
        /// Changes the volume of the timeline elements.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void VolumeControl_VolumeChanged(object sender, EventArgs e)
        {
            if (this.ElementViews != null)
            {
                foreach (TimelineElementView view in this.ElementViews.Values)
                {                   
                    view.Model.Volume = this.VolumeControl.Volume;
                }
            }
        }

        /// <summary>
        /// Updates the PlayHead size based on the timeline size.
        /// </summary>
        private void UpdatePlayHeadSize()
        {
            double height = this.TopBar.ActualHeight + this.VideoBar.ActualHeight + this.AudioTracks.ActualHeight - this.PlayHeadTop.Height + 2;

            if (height != this.PlayHeadRectangle.Height)
            {
                this.PlayHeadInnerCanvas.Height = height;
                this.PlayHeadRectangle.Height = height;
            }
        }

        /// <summary>
        /// Handles the LayoutUpdated event of the AudioTracks control. 
        /// Updates the PlayHead size and the refresh the time marks height.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void OnAudioTracksLayoutUpdated(object sender, EventArgs e)
        {
            this.UpdatePlayHeadSize();
            this.RefreshTimeMarksHeight();
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Binding dropCommand = new Binding("DropCommand") { Source = this.DataContext };
            ((BindingHelper)this.Resources["DropItemCommand"]).SetBinding(BindingHelper.ValueProperty, dropCommand);
        }
    }
}
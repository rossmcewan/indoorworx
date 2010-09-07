// <copyright file="MockAggregateMediaModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockAggregateMediaModel.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Player.Models;

    public class MockAggregateMediaModel : IAggregateMediaModel
    {
        private TimeSpan position;

        public event EventHandler<PositionPayloadEventArgs> PositionUpdated;

        public event EventHandler FinishedPlaying;

        /// <summary>
        /// Occurs when [download progress start].
        /// </summary>
        public event EventHandler<AssetDownloadProgressEventArgs> DownloadProgressChanged;

        public event EventHandler BufferStart;

        public event EventHandler BufferEnd;

        public TimeSpan Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.PositionSet = true;
            }
        }

        // TODO
        public TimeSpan Duration
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public bool Mute { get; set; }

        public Asset CurrentAsset { get; set; }

        public bool IsPlaying { get; set; }

        public bool PositionSet { get; set; }

        public bool PlayCalled { get; set; }

        public bool PauseCalled { get; set; }

        public bool AddElementCalled { get; set; }

        public bool AddBlankCalled { get; set; }

        public bool ReorderElementsCalled { get; set; } 

        public bool RemoveElementCalled { get; set; }

        public bool RemoveBlankElementCalled { get; set; }

        public bool IsVisible { get; set; }

        public bool FindMediaByElementCalled { get; set; }

        public TimelineElement FindMediaByElementArgument { get; set; }

        public IList<TimelineElement> ReorderElementsArguments { get; set; }

        public void Play()
        {
            this.PlayCalled = true;
        }

        public void Pause()
        {
            this.PauseCalled = true;
        }

        public MediaData AddElement(TimelineElement element)
        {
            this.AddElementCalled = true;
            return new MockMediaData();
        }

        public MediaData AddBlank(TimelineElement element)
        {
            this.AddBlankCalled = true;
            return new MockMediaData();
        }

        public MediaData RemoveElement(TimelineElement element)
        {
            this.RemoveElementCalled = true;
            return new MockMediaData();
        }

        public MediaData RemoveBlankElement(TimelineElement element)
        {
            this.RemoveBlankElementCalled = true;
            return new MockMediaData();
        }

        public void ReorderElements(IList<TimelineElement> elements)
        {
            this.ReorderElementsCalled = true;
            this.ReorderElementsArguments = elements;
        }

        public void ResetCurrent()
        {
        }

        public MediaData FindMediaByElement(TimelineElement timelineElement)
        {
            this.FindMediaByElementCalled = true;
            this.FindMediaByElementArgument = timelineElement;

            return null;
        }

        public void FastForward()
        {
        }

        public void FastRewind()
        {
        }

        public void InvokePositionUpdated(TimeSpan position)
        {
            EventHandler<PositionPayloadEventArgs> positionUpdatedHandler = this.PositionUpdated;
            if (positionUpdatedHandler != null)
            {
                positionUpdatedHandler(this, new PositionPayloadEventArgs(position));
            }
        }

        public void InvokeBufferStart()
        {
            EventHandler bufferStart = this.BufferStart;
            if (bufferStart != null)
            {
                bufferStart(null, EventArgs.Empty);
            }
        }

        public void InvokeBufferEnd()
        {
            EventHandler bufferEnd = this.BufferEnd;
            if (bufferEnd != null)
            {
                bufferEnd(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Invokes the download progress changed.
        /// </summary>
        public void InvokeDownloadProgressChanged()
        {
            EventHandler<AssetDownloadProgressEventArgs> downloadProgressChanged = this.DownloadProgressChanged;

            if (downloadProgressChanged != null)
            {
                downloadProgressChanged(null, new AssetDownloadProgressEventArgs(new TimelineElement(), 0.5, 5));
            }
        }
    }
}

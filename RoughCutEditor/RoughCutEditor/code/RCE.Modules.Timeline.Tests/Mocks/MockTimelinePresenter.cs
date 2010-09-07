// <copyright file="MockTimelinePresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockTimelinePresenter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline.Tests.Mocks
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Infrastructure.DragDrop;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using SMPTETimecode;

    internal class MockTimelinePresenter : ITimelinePresenter
    {
        private readonly ITimelineView view = new MockTimelineView();

        private readonly TimelineModel model = new TimelineModel();

        public event PropertyChangedEventHandler PropertyChanged;
        
        public bool SetEditingModeCalled { get; set; }
        
        public EditMode GetEditingMode { get; set; }
        
        public ITimelineView View
        {
            get { return this.view; }
        }

        public bool IsInRippleMode { get; set; }

        public TimeCode TimelineDuration { get; private set; }

        public bool IsInSnapMode { get; set; }

        public DelegateCommand<object> AddAudioTrackCommand { get; private set; }
        
        public DelegateCommand<object> RemoveAudioTrackCommand { get; private set; }
        
        public DelegateCommand<DropPayload> DropCommand { get; private set; }

        public DelegateCommand<object> MoveFrameCommand { get; private set; }

        public DelegateCommand<object> MoveNextClipCommand
        {
            get { throw new NotImplementedException(); }
        }

        public DelegateCommand<object> MovePreviousClipCommand
        {
            get { throw new NotImplementedException(); }
        }

        public ObservableCollection<Track> AudioTracks { get; private set; }

        public ITimelineModel Model
        {
            get { return this.model; }
        }

        public void SetEditingMode(EditMode mode)
        {
            this.SetEditingModeCalled = true;
            this.GetEditingMode = mode;
        }

        public void SetDuration(TimeCode timeCode)
        {
            throw new System.NotImplementedException();
        }
    }
}
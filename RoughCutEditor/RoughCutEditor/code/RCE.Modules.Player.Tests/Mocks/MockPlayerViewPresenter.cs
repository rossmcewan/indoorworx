// <copyright file="MockPlayerViewPresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockPlayerViewPresenter.cs                     
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
    using System.Windows.Media.Imaging;
    using Infrastructure.DragDrop;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Player.Models;

    public class MockPlayerViewPresenter : IPlayerViewPresenter
    {
        private IPlayerView view = new MockPlayerView();

        public IPlayerView View
        {
            get
            {
                return this.view;
            }

            set
            {
                this.view = value;
            }
        }

        public PlayerMode PlayerMode { get; set; }

        public DelegateCommand<DropPayload> DropCommand { get; private set; }

        public DelegateCommand<object> FastRewindCommand { get; private set; }

        public DelegateCommand<object> FastForwardCommand { get; private set; }

        public MediaData GetMediaDataAtCurrentPosition()
        {
            throw new NotImplementedException();
        }

        public void SetThumbnail(WriteableBitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public void SetAsset(Asset asset)
        {
            throw new System.NotImplementedException();
        }
    }
}
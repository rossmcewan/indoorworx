// <copyright file="MockMediaBinViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockMediaBinViewPresentationModel.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.MediaBin.Tests.Mocks
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Infrastructure.DragDrop;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Commands;
    
    public class MockMediaBinViewPresentationModel : IMediaBinViewPresentationModel
    {
        public MockMediaBinViewPresentationModel()
        {
            this.View = new MockMediaBinView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IMediaBinView View { get; set; }

        public ObservableCollection<Asset> Assets { get; set; }

        public bool ShowVideos { get; set; }

        public bool ShowAudio { get; set; }

        public bool ShowImages { get; set; }

        public bool IsHelpWindowOpen { get; set; }

        public double Scale { get; set; }

        public DelegateCommand<string> SearchCommand { get; set; }

        public DelegateCommand<object> PlaySelectedAssetCommand { get; set; }

        public DelegateCommand<string> ShiftSliderScaleCommand { get; set; }

        public DelegateCommand<string> HelpButtonCommand { get; set; }

        public DelegateCommand<DropPayload> DropCommand { get; private set; }

        public string HeaderInfo { get; set; }

        public string FolderTitle { get; set; }

        public bool IsThumbChecked { get; set; }

        public DelegateCommand<string> AddFolderCommand { get; set; }
        
        public DelegateCommand<string> UpArrowCommand { get; set; }

        public DelegateCommand<string> DeleteAssetCommand { get; set; }

        public string HeaderIconOn { get; set; }

        public string HeaderIconOff { get; set; }

        public bool IsActive { get; set; }

        public Asset SelectedAsset { get; set; }
        
        public void OnAssetSelected(Asset asset)
        {
            throw new System.NotImplementedException();
        }

        public void OnDropItem(Asset asset, MouseButtonEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void RefreshCurrentAssets()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAsset(Asset asset)
        {
            throw new System.NotImplementedException();
        }

        public void CopyAssetFromClipboardToCurrentFolder(Asset asset)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetMediaBin()
        {
            throw new System.NotImplementedException();
        }

        public void AddAssetToTimeline(Asset asset)
        {
        }

        public void ShowMetadata(Asset asset)
        {
        }

        public void AddAssetToFolder(Asset asset, Guid folderId)
        {
            throw new System.NotImplementedException();
        }

        public void Activate()
        {
        }

        public void DeleteCurrentAsset()
        {
        }
    }
}

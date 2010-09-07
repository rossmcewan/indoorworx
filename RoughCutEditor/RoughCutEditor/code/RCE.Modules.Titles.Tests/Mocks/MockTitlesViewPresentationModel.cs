// <copyright file="MockTitlesViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockTitlesViewPresentationModel.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles.Tests.Mocks
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Infrastructure.Models;

    public class MockTitlesViewPresentationModel : ITitlesViewPresentationModel
    {
        public MockTitlesViewPresentationModel()
        {
            this.View = new MockTitlesView();
        }

        public event PropertyChangedEventHandler PropertyChanged; 

        public ITitlesView View { get; set; }

        public string MainTitle { get; set; }
        
        public string SubTitle { get; set; }

        public string HeaderInfo { get; set; }

        public string HeaderIconOn { get; set; }

        public string HeaderIconOff { get; set; }

        public bool IsActive { get; private set; }

        public ObservableCollection<TitleTemplate> TitleTemplates { get; set; }

        public void OnDropItem(TitleTemplate titleAsset, MouseButtonEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void AddTitleAssetToTimeline(TitleTemplate titleTemplate)
        {
            throw new System.NotImplementedException();
        }

        public void Activate()
        {
        }
    }
}

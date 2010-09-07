// <copyright file="MockAddPreviewEvent.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockRefreshElementsEvent.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.CommentsBar.Tests.Mocks
{
    using System;
    using Infrastructure.Events;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    public class MockAddPreviewEvent : AddPreviewEvent
    {
        public Action<AddPreviewPayload> SubscribeArgumentAction { get; set; }

        public Predicate<AddPreviewPayload> SubscribeArgumentFilter { get; set; }

        public ThreadOption SubscribeArgumentThreadOption { get; set; }

        public override SubscriptionToken Subscribe(Action<AddPreviewPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<AddPreviewPayload> filter)
        {
            this.SubscribeArgumentAction = action;
            this.SubscribeArgumentFilter = filter;
            this.SubscribeArgumentThreadOption = threadOption;
            return null;
        }
    }
}
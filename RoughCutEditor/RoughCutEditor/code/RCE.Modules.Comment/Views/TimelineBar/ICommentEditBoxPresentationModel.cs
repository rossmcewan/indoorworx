// <copyright file="ICommentEditBoxPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ICommentEditBoxPresentationModel.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Comment
{
    using System.ComponentModel;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Commands;

    public interface ICommentEditBoxPresentationModel : ITimelineBarElementModel, INotifyPropertyChanged
    {
       ICommentEditBox View { get; }
        
        DelegateCommand<object> CloseCommand { get; }
        
        DelegateCommand<object> SaveCommand { get; }
        
        DelegateCommand<object> DeleteCommand { get; }
        
        DelegateCommand<object> PlayCommand { get; }
        
        double MarkIn { get; set; }
        
        double MarkOut { get; set; }
        
        string Text { get; set; }
        
        void ShowEditBox();
    }
}
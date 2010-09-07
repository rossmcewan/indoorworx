// <copyright file="CommentEditBox.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: CommentEditBox.xaml.cs                     
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
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Provides the implementation for CommentEditBox view.
    /// </summary>
    public partial class CommentEditBox : ICommentEditBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentEditBox"/> class.
        /// </summary>
        public CommentEditBox()
        {
            InitializeComponent();
        }

        public ICommentEditBoxPresentationModel Model
        {
            get { return this.DataContext as CommentEditBoxPresentationModel; }
            set { this.DataContext = value; }
        }

        public void Close()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            this.CommentBox.Focus();
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the key up event of the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/>.</param>
        private void CommentBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.Model.SaveCommand.CanExecute(null))
            {
                this.Model.Text = this.CommentBox.Text;
                this.Model.SaveCommand.Execute(null);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CommentBox.Focus();
        }
    }
}
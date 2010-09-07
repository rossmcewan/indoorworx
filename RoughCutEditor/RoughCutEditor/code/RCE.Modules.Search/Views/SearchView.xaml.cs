// <copyright file="SearchView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SearchView.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Search
{
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class SearchView : UserControl, ISearchView
    {
        public SearchView()
        {
            InitializeComponent();
        }

        public ISearchViewPresentationModel Model
        {
            get { return this.DataContext as ISearchViewPresentationModel; }
            set { this.DataContext = value; }
        }

        /// <summary>
        /// Handles the KeyUp event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    this.Model.SearchCommand.Execute(textBox.Text);
                }
            }
        }
    }
}
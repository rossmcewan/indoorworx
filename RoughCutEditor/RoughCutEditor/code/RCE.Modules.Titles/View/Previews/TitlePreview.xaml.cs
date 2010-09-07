// <copyright file="TitlePreview.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TitlePreview.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Infrastructure.Models;

    /// <summary>
    /// The preview view for the title.
    /// </summary>
    public partial class TitlePreview : IAssetPreview
    {
        /// <summary>
        /// The <seealso cref="Canvas"/> to store the XAML of the title template of the title.
        /// </summary>
        private Canvas xamlResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitlePreview"/> class.
        /// </summary>
        public TitlePreview()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the title template.
        /// </summary>
        /// <value>The title template.</value>
        public TitleTemplate TitleTemplate
        {
            get
            {
                return this.DataContext as TitleTemplate;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Gets the XAML of a title template of the title.
        /// </summary>
        /// <returns>A <seealso cref="Canvas"/> that represents the title template.</returns>
        private Canvas GetPreview()
        {
            if (this.xamlResource == null && this.TitleTemplate != null)
            {
                this.xamlResource = XamlReader.Load(this.TitleTemplate.XamlResource) as Canvas;
            }

            return this.xamlResource;
        }

        /// <summary>
        /// Begins the in animation of the title template animation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="MouseEventArgs"/>.</param>
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas preview = this.GetPreview();
            if (preview != null)
            {
                if (!this.PreviewCanvas.Children.Contains(preview))
                {
                    preview.RenderTransform = new ScaleTransform
                                                  {
                                                      ScaleX = 0.75,
                                                      ScaleY = 0.75
                                                  };

                    // clip
                    this.PreviewCanvas.Clip = new RectangleGeometry
                                                  {
                                                      Rect = new Rect(0, 0, this.PreviewCanvas.ActualWidth, this.PreviewCanvas.ActualHeight)
                                                  };
                    this.PreviewCanvas.Children.Add(preview);
                }

                this.DescriptionCanvas.Visibility = Visibility.Collapsed;
                this.PreviewCanvas.Visibility = Visibility.Visible;
                Storyboard inStoryboard = (Storyboard) preview.Resources["InTransition"];
                inStoryboard.Begin();
            }
        }

        /// <summary>
        /// Begins the out animation of the title template animation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="MouseEventArgs"/>.</param>
        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas preview = this.GetPreview();

            if (preview != null)
            {
                Storyboard outStoryboard = (Storyboard) preview.Resources["OutTransition"];
                outStoryboard.Completed += delegate
                                               {
                                                   this.DescriptionCanvas.Visibility = Visibility.Visible;
                                                   this.PreviewCanvas.Visibility = Visibility.Collapsed;
                                               };

                outStoryboard.Begin();
            }
        }
    }
}
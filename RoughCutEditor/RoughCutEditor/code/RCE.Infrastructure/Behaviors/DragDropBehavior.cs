// <copyright file="DragDropBehavior.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: DragDropBehavior.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Infrastructure.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using DragDrop;

    /// <summary>
    /// Behavior that allows to manage the drag and drop of elements.
    /// </summary>
    public class DragDropBehavior : IDisposable
    {
        /// <summary>
        /// The element being dragged.
        /// </summary>
        private readonly UIElement dragSource;

        /// <summary>
        /// Indicates whether the collection was disposed or not.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropBehavior"/> class.
        /// </summary>
        /// <param name="dragSource">The element being dragged.</param>
        public DragDropBehavior(UIElement dragSource)
        {
            this.DragPopupChild = new Grid();

            this.DragPopup = new Popup
            {
                IsOpen = false,
                Child = this.DragPopupChild
            };

            this.dragSource = dragSource;
            this.dragSource.MouseLeftButtonDown += this.DragSource_MouseLeftButtonDown;
            this.dragSource.MouseLeftButtonUp += this.DragSource_MouseLeftButtonUp;
            this.dragSource.MouseMove += this.DragSource_MouseMove;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is being dragged or not.
        /// </summary>
        /// <value>A true if the element is being dragged;otherwise false.</value>
        private bool IsDragging { get; set; }

        /// <summary>
        /// Gets or sets the drag popup to show the drag notification.
        /// </summary>
        /// <value>A popup to show the drag notification.</value>
        private Popup DragPopup { get; set; }

        /// <summary>
        /// Gets or sets the child of the drag popup that hosts the current notification.
        /// </summary>
        /// <value>The child of the drag popup.</value>
        private Grid DragPopupChild { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates if Dispose is being called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.dragSource.MouseLeftButtonDown -= this.DragSource_MouseLeftButtonDown;
                this.dragSource.MouseLeftButtonUp -= this.DragSource_MouseLeftButtonUp;
                this.dragSource.MouseMove -= this.DragSource_MouseMove;
                this.disposed = true;
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown of the drag source. Starts the dragging operation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The envet args instance containing event data.</param>
        private void DragSource_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragSource.CaptureMouse();

            if (!this.IsDragging)
            {
                DataTemplate dragTemplate = DragDropManager.GetDragTemplate(this.dragSource);
                object dragData = DragDropManager.GetDragData(this.dragSource);

                this.DragPopupChild.Children.Clear();
                this.IsDragging = true;

                DragDropControl dragDropControl = new DragDropControl
                {
                    DragContent = dragData,
                    DragTemplate = dragTemplate
                };

                this.DragPopupChild.Children.Add(dragDropControl);

                if (!this.DragPopup.IsOpen)
                {
                    this.DragPopupChild.Opacity = 0;
                    this.DragPopup.IsOpen = true;
                }

                this.ShowDragDrop(false);
            }
        }

        /// <summary>
        /// Handles the DragSource_MouseLeftButtonUp of the drag source. Completes the dragging operation and perform the drop if applies.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The envet args instance containing event data.</param>
        private void DragSource_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.dragSource.ReleaseMouseCapture();

            if (this.IsDragging)
            {
                IList<FrameworkElement> dropZones = this.CheckDropZones(e);

                foreach (FrameworkElement dropZone in dropZones)
                {
                    ICommand command = DragDropManager.GetDropCommand(dropZone);

                    if (command != null)
                    {
                        DropPayload dropPayload = new DropPayload
                                                      {
                                                          DraggedItem = DragDropManager.GetDragData(this.dragSource),
                                                          RelativePosition = e.GetPosition(dropZone),
                                                          MouseEventArgs = e,
                                                          DropData = DragDropManager.GetDropData(dropZone),
                                                      };
                        command.Execute(dropPayload);
                    }
                }
            }

            this.DragPopupChild.Children.Clear();
            this.DragPopup.IsOpen = false;
            this.IsDragging = false;
        }

        /// <summary>
        /// Handles the MouseMove of the drag source. Shows a visual notification if the asset can be dropped or not..
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The envet args instance containing event data.</param>
        private void DragSource_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsDragging)
            {
                if (this.DragPopup != null && this.DragPopupChild != null && this.DragPopupChild.Children.Count > 0
                    && this.DragPopupChild.ActualHeight > 0 && this.DragPopupChild.ActualWidth > 0)
                {
                    this.DragPopupChild.Opacity = 1;
                    Point point = e.GetPosition(Application.Current.RootVisual);

                    const double HeightOffset = 15;
                    double currentVerticalPosition = point.Y - this.DragPopupChild.ActualHeight + HeightOffset;
                    double currentHorizontalPosition = point.X - (this.DragPopupChild.ActualWidth / 2);
                    this.DragPopup.VerticalOffset = currentVerticalPosition;
                    this.DragPopup.HorizontalOffset = currentHorizontalPosition;

                    IList<FrameworkElement> dropZonesAtCurrentCord = this.CheckDropZones(e);

                    if (dropZonesAtCurrentCord.Count > 0)
                    {
                        this.ShowDragDrop(true);
                    }
                    else
                    {
                        this.ShowDragDrop(false);
                    }
                }
            }
        }

        /// <summary>
        /// Check if there are drop zones an specific position.
        /// </summary>
        /// <param name="e">The mouse event args.</param>
        /// <returns>A list of possible drop zones.</returns>
        private IList<FrameworkElement> CheckDropZones(MouseEventArgs e)
        {
            IList<FrameworkElement> dropZonesAtCurrentCord = new List<FrameworkElement>();

            if (this.DragPopup != null && this.DragPopupChild != null && this.DragPopupChild.Children.Count > 0)
            {
                DragDropControl dragDropControl = this.DragPopupChild.Children[0] as DragDropControl;

                if (dragDropControl != null)
                {
                    IList<FrameworkElement> invalidDropZones = new List<FrameworkElement>();
                    foreach (FrameworkElement frameworkElement in DragDropManager.DropZones)
                    {
                        IDropInfo dropInfo = DragDropManager.GetDropInfo(frameworkElement);
                        Type dragSourceType = dragDropControl.DragContent.GetType();
                        try
                        {
                            Point dropPoint = e.GetPosition(frameworkElement);

                            if ((dropPoint.X >= 0 && dropPoint.X <= frameworkElement.ActualWidth) &&
                                (dropPoint.Y >= 0 && dropPoint.Y <= frameworkElement.ActualHeight) &&
                                dropInfo.AllowedDragTypes.Contains(dragSourceType))
                            {
                                dropZonesAtCurrentCord.Add(frameworkElement);
                            }
                        }
                        catch (Exception)
                        {
                            invalidDropZones.Add(frameworkElement);
                        }
                    }

                    foreach (FrameworkElement element in invalidDropZones)
                    {
                        DragDropManager.DropZones.Remove(element);
                    }

                    invalidDropZones.Clear();
                }
            }

            return dropZonesAtCurrentCord;
        }

        /// <summary>
        /// Toggles the drop allowed value of the current drag drop control.
        /// </summary>
        /// <param name="dropAllowed">If the drop is allowed or not.</param>
        private void ShowDragDrop(bool dropAllowed)
        {
            if (this.DragPopup != null && this.DragPopupChild != null && this.IsDragging)
            {
                if (this.DragPopupChild.Children.Count > 0)
                {
                    DragDropControl dragDropControl = this.DragPopupChild.Children[0] as DragDropControl;

                    if (dragDropControl != null)
                    {
                        dragDropControl.DropAllowed = dropAllowed;
                    }
                }
            }
        }
    }
}

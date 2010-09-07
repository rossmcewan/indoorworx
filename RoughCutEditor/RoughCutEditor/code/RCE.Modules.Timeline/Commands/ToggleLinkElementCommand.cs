// <copyright file="ToggleLinkElementCommand.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ToggleLinkElementCommand.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline.Commands
{
    using Infrastructure.Models;

    /// <summary>
    /// A class that provides an implementation of the <seealso cref="UndoableCommand"/> class to link and unlink elements.
    /// </summary>
    public class ToggleLinkElementCommand : UndoableCommand
    {
        /// <summary>
        /// The <seealso cref="ITimelineModel"/> instance where the element is being linked/unlinked.
        /// </summary>
        private readonly ITimelineModel timelineModel;
        
        /// <summary>
        /// The <seealso cref="Track"/> where the elements being linked/unliked belongs.
        /// </summary>
        private readonly Track layer;

        /// <summary>
        /// The <seealso cref="TimelineElement"/> being linked/unliked.
        /// </summary>
        private readonly TimelineElement element;

        /// <summary>
        /// The <seealso cref="LinkPosition"/> used to link/unlink the element.
        /// </summary>
        private readonly LinkPosition linkPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleLinkElementCommand"/> class.
        /// </summary>
        /// <param name="timelineModel">The timeline model.</param>
        /// <param name="layer">The layer of the element.</param>
        /// <param name="element">The element being linked/unliked.</param>
        /// <param name="linkPosition">The <see cref="LinkPosition"/> of the link.</param>
        public ToggleLinkElementCommand(ITimelineModel timelineModel, Track layer, TimelineElement element, LinkPosition linkPosition)
        {
            this.timelineModel = timelineModel;
            this.layer = layer;
            this.element = element;
            this.linkPosition = linkPosition;
        }

        /// <summary>
        /// Toggles the link state of an element.
        /// </summary>
        protected override void ExecuteCommand()
        {
            this.InnerExecute();
        }

        /// <summary>
        /// Toggles the link state of an element.
        /// </summary>
        protected override void UnExecuteCommand()
        {
            this.InnerExecute();
        }

        /// <summary>
        /// Toggles the link state of an element based on the <see cref="LinkPosition"/>.
        /// </summary>
        private void InnerExecute()
        {
            if (this.linkPosition == LinkPosition.In)
            {
                TimelineElement previousElement = this.timelineModel.GetElementAtPosition(this.element.Position, this.layer, this.element);

                if (previousElement != null)
                {
                    if (this.timelineModel.IsElementLinkedTo(this.element, previousElement))
                    {
                        this.timelineModel.UnlinkElements(this.element, previousElement);
                    }
                    else
                    {
                        this.timelineModel.LinkPreviousElement(this.element, previousElement);
                    }
                }
            }
            else
            {
                TimelineElement nextElement = this.timelineModel.GetElementAtPosition(this.element.Position + this.element.Duration, this.layer, this.element);

                if (nextElement != null)
                {
                    if (this.timelineModel.IsElementLinkedTo(this.element, nextElement))
                    {
                        this.timelineModel.UnlinkElements(this.element, nextElement);
                    }
                    else
                    {
                        this.timelineModel.LinkNextElement(this.element, nextElement);
                    }
                }
            }
        }
    }
}

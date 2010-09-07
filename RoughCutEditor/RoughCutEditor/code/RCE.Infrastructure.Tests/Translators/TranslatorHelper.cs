﻿// <copyright file="TranslatorHelper.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TranslatorHelper.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Infrastructure.Tests.Translators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Ink;
    using System.Windows.Input;
    using Infrastructure.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.Contracts;
    using SMPTETimecode;
    using Comment = RCE.Services.Contracts.Comment;
    using InkComment = RCE.Services.Contracts.InkComment;
    using MediaBin = RCE.Services.Contracts.MediaBin;
    using Project = RCE.Services.Contracts.Project;
    using TitleTemplate = RCE.Services.Contracts.TitleTemplate;
    using Track = RCE.Services.Contracts.Track;

    /// <summary>
    /// A class with helper methods used in different tests.
    /// </summary>
    public sealed class TranslatorHelper
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="TranslatorHelper"/> class from being created.
        /// </summary>
        private TranslatorHelper()
        {
        }

        /// <summary>
        /// Creates a service project for testing.
        /// </summary>
        /// <returns>A project with values.</returns>
        public static Project CreateServiceProject()
        {
            var project = new Project();
            project.Id = CreateUri();
            project.Title = "MyProject";
            project.Creator = "Creator";
            project.Created = new DateTime(2009, 1, 1);
            project.ModifiedBy = "ModifiedBy";
            project.Modified = new DateTime(2009, 1, 2);
            project.Description = "Description";
            project.AutoSaveInterval = 10;
            project.Duration = 60;
            project.RippleMode = true;
            project.SmpteFrameRate = SmpteFrameRate.Smpte2997NonDrop.ToString();
            project.StartTimeCode = 60;
            project.MediaBin = new MediaBin
                                   {
                                       Id = CreateUri(),
                                       Creator = "Creator",
                                       Created = new DateTime(2009, 1, 1),
                                       ModifiedBy = "ModifiedBy",
                                       Modified = new DateTime(2009, 1, 2)
                                   };
            project.Resolution = "1280x800";
            project.Title = "Title";
            project.Comments = new CommentCollection();
            project.Comments.Add(CreateServiceComment(CommentType.Timeline));
            project.Comments.Add(CreateServiceComment(CommentType.Shot));
            project.Comments.Add(CreateServiceComment(CommentType.Ink));
            project.Timeline = new TrackCollection();
            project.Timeline.Add(CreateServiceTrack(RCE.Services.Contracts.TrackType.Visual));
            project.Timeline[0].Shots.Add(CreateServiceShot(CreateServiceVideoItem()));
            project.Timeline[0].Shots.Add(CreateServiceShot(CreateServiceImageItem()));
            project.Timeline[0].Shots[0].Comments.Add(project.Comments[1]);
            project.Timeline[0].Shots[0].Comments.Add(project.Comments[2]);
            project.Timeline.Add(CreateServiceTrack(RCE.Services.Contracts.TrackType.Audio));
            project.Timeline[1].Shots.Add(CreateServiceShot(CreateServiceAudioItem()));
            project.Titles = new TitleCollection();
            project.Titles.Add(CreateServiceTitle());

            return project;
        }

        /// <summary>
        /// Creates a project for testing.
        /// </summary>
        /// <returns>A project with values.</returns>
        public static Infrastructure.Models.Project CreateProject()
        {
            var project = new RCE.Infrastructure.Models.Project();
            project.ProviderUri = CreateUri();
            project.Name = "MyProject";
            project.Creator = "Creator";
            project.Created = new DateTime(2009, 1, 1);
            project.ModifiedBy = "ModifiedBy";
            project.Modified = new DateTime(2009, 1, 2);
            project.AutoSaveInterval = 10;
            project.Duration = 60;
            project.RippleMode = true;
            project.SmpteFrameRate = SmpteFrameRate.Smpte2997NonDrop;
            project.StartTimeCode = TimeCode.FromSeconds(60d, project.SmpteFrameRate);
            project.MediaBin = new RCE.Infrastructure.Models.MediaBin
                                   {
                                       ProviderUri = CreateUri(),
                                       Creator = "Creator",
                                       Created = new DateTime(2009, 1, 1),
                                       ModifiedBy = "ModifiedBy",
                                       Modified = new DateTime(2009, 1, 2)
                                   };

            project.Resolution = "1280x800";
            project.Comments.Add(CreateComment(CommentType.Timeline));
            project.Comments.Add(CreateComment(CommentType.Shot));
            project.Comments.Add(CreateComment(CommentType.Ink));
            project.Timeline.Add(CreateTrack(RCE.Infrastructure.Models.TrackType.Visual));
            project.Timeline[0].Shots.Add(CreateShot(CreateVideoAsset(), project.Comments[1]));
            project.Timeline[0].Shots.Add(CreateShot(CreateVideoAsset(), project.Comments[2]));
            project.Timeline[0].Shots.Add(CreateShot(CreateImageAsset(), null));
            project.Timeline.Add(CreateTrack(RCE.Infrastructure.Models.TrackType.Audio));
            project.Timeline[1].Shots.Add(CreateShot(CreateAudioAsset(), null));
            project.Timeline.Add(CreateTrack(RCE.Infrastructure.Models.TrackType.Title));
            project.Timeline[2].Shots.Add(CreateShot(CreateTitleAsset(), null));

            return project;
        }

        /// <summary>
        /// Asserts that the <paramref name="titleAsset"/> contains equivalent values from the <paramref name="serviceTitle"/>.
        /// </summary>
        /// <param name="serviceTitle">The title with expected values.</param>
        /// <param name="titleAsset">The title with actual values.</param>
        public static void AssertTitle(Title serviceTitle, TitleAsset titleAsset)
        {
            Assert.AreEqual(serviceTitle.Id, titleAsset.ProviderUri);
            Assert.AreEqual(serviceTitle.TextBlockCollection[0].Text, titleAsset.MainText);
            Assert.AreEqual(serviceTitle.TextBlockCollection[0].Text, titleAsset.SubText);
            AssertTitleTemplate(serviceTitle.TitleTemplate, titleAsset.TitleTemplate);
        }

        /// <summary>
        /// Asserts that the <paramref name="titleTemplate"/> contains equivalent values from the <paramref name="serviceTitleTemplate"/>.
        /// </summary>
        /// <param name="serviceTitleTemplate">The title template with expected values.</param>
        /// <param name="titleTemplate">The title template with actual values.</param>
        public static void AssertTitleTemplate(TitleTemplate serviceTitleTemplate, RCE.Infrastructure.Models.TitleTemplate titleTemplate)
        {
            Assert.AreEqual(serviceTitleTemplate.Id, titleTemplate.ProviderUri);
            Assert.AreEqual(serviceTitleTemplate.TemplateName, titleTemplate.Title);
        }

        /// <summary>
        /// Asserts that the <paramref name="project"/> contains equivalent values from the <paramref name="serviceProject"/>.
        /// </summary>
        /// <param name="serviceProject">The project with expected values.</param>
        /// <param name="project">The project with actual values.</param>
        public static void AssertProject(Project serviceProject, RCE.Infrastructure.Models.Project project)
        {
            Assert.AreEqual(serviceProject.Id, project.ProviderUri);
            Assert.AreEqual(serviceProject.Created, project.Created);
            Assert.AreEqual(serviceProject.Title, project.Name);
            Assert.AreEqual(serviceProject.Creator, project.Creator);
            Assert.AreEqual(serviceProject.ModifiedBy, project.ModifiedBy);
            Assert.AreEqual(serviceProject.Modified, project.Modified);
            Assert.AreEqual(serviceProject.Duration, project.Duration);
            Assert.AreEqual(serviceProject.AutoSaveInterval, project.AutoSaveInterval);
            Assert.AreEqual(serviceProject.StartTimeCode, project.StartTimeCode.TotalSeconds);
            Assert.AreEqual(serviceProject.SmpteFrameRate, project.SmpteFrameRate.ToString());
            Assert.AreEqual(serviceProject.RippleMode, project.RippleMode);
            Assert.AreEqual(serviceProject.Resolution, project.Resolution);

            Assert.AreEqual(serviceProject.Timeline.Count + 1, project.Timeline.Count);

            AssertTrack(serviceProject.Timeline[0], project.Timeline[0]);
            AssertTrack(serviceProject.Timeline[1], project.Timeline[1]);

            Assert.AreEqual(serviceProject.Comments.Count, project.Comments.Count);
            AssertComment(serviceProject.Comments[0], project.Comments[0]);
            AssertComment(serviceProject.Comments[1], project.Comments[1]);
            AssertComment(serviceProject.Comments[2], project.Comments[2]);

            Assert.AreEqual(serviceProject.Titles.Count, project.Timeline[2].Shots.Count);
            Assert.AreEqual(serviceProject.Titles[0].Id, project.Timeline[2].Shots[0].Asset.ProviderUri);
            Assert.AreEqual(serviceProject.Titles[0].TrackAnchor.Id, project.Timeline[2].Shots[0].TrackAnchorUri);
            Assert.AreEqual(serviceProject.Titles[0].TrackAnchor.MarkIn, project.Timeline[2].Shots[0].Position.TotalSeconds);
            Assert.AreEqual(0, project.Timeline[2].Shots[0].InPosition.TotalSeconds);
            Assert.AreEqual(serviceProject.Titles[0].TrackAnchor.MarkOut, project.Timeline[2].Shots[0].OutPosition.TotalSeconds);
            Assert.AreEqual(serviceProject.Titles[0].TitleTemplate.Id, ((TitleAsset)project.Timeline[2].Shots[0].Asset).TitleTemplate.ProviderUri);
            Assert.AreEqual(serviceProject.Titles[0].TitleTemplate.TemplateName, ((TitleAsset)project.Timeline[2].Shots[0].Asset).TitleTemplate.Title);
            Assert.AreEqual(serviceProject.Titles[0].TextBlockCollection[0].Text, ((TitleAsset)project.Timeline[2].Shots[0].Asset).MainText);
            Assert.AreEqual(serviceProject.Titles[0].TextBlockCollection[1].Text, ((TitleAsset)project.Timeline[2].Shots[0].Asset).SubText);
        }

        /// <summary>
        /// Asserts that the <paramref name="serviceProject"/> contains equivalent values from the <paramref name="project"/>.
        /// </summary>
        /// <param name="project">The project with expected values.</param>
        /// <param name="serviceProject">The project with actual values.</param>
        public static void AssertProject(RCE.Infrastructure.Models.Project project, Project serviceProject)
        {
            Assert.AreEqual(project.ProviderUri, serviceProject.Id);
            Assert.AreEqual(project.Created, serviceProject.Created);
            Assert.AreEqual(project.Name, serviceProject.Title);
            Assert.AreEqual(project.Creator, serviceProject.Creator);
            Assert.AreEqual(project.ModifiedBy, serviceProject.ModifiedBy);
            Assert.AreNotEqual(DateTime.MinValue, serviceProject.Modified);
            Assert.AreEqual(project.Duration, serviceProject.Duration);
            Assert.AreEqual(project.AutoSaveInterval, serviceProject.AutoSaveInterval);
            Assert.AreEqual(project.StartTimeCode.TotalSeconds, serviceProject.StartTimeCode);
            Assert.AreEqual(project.SmpteFrameRate.ToString(), serviceProject.SmpteFrameRate);
            Assert.AreEqual(project.RippleMode, serviceProject.RippleMode);
            Assert.AreEqual(project.Resolution, serviceProject.Resolution);

            Assert.AreEqual(project.Timeline.Count - 1, serviceProject.Timeline.Count);

            AssertTrack(project.Timeline[0], serviceProject.Timeline[0]);
            AssertTrack(project.Timeline[1], serviceProject.Timeline[1]);

            Assert.AreEqual(project.Comments.Count, serviceProject.Comments.Count);
            AssertComment(project.Comments[0], serviceProject.Comments[0]);
            AssertComment(project.Comments[1], serviceProject.Comments[1]);
            AssertComment(project.Comments[2], serviceProject.Comments[2]);

            Assert.AreEqual(project.Timeline[2].Shots.Count, serviceProject.Titles.Count);
            Assert.AreEqual(project.Timeline[2].Shots[0].Asset.ProviderUri, serviceProject.Titles[0].Id);
            Assert.AreEqual(project.Timeline[2].Shots[0].TrackAnchorUri, serviceProject.Titles[0].TrackAnchor.Id);
            Assert.AreEqual(project.Timeline[2].Shots[0].Position.TotalSeconds, serviceProject.Titles[0].TrackAnchor.MarkIn);
            Assert.AreEqual(project.Timeline[2].Shots[0].OutPosition.TotalSeconds, serviceProject.Titles[0].TrackAnchor.MarkOut);
            Assert.AreEqual(((TitleAsset)project.Timeline[2].Shots[0].Asset).TitleTemplate.ProviderUri, serviceProject.Titles[0].TitleTemplate.Id);
            Assert.AreEqual(((TitleAsset)project.Timeline[2].Shots[0].Asset).TitleTemplate.Title, serviceProject.Titles[0].TitleTemplate.TemplateName);
            Assert.AreEqual(((TitleAsset)project.Timeline[2].Shots[0].Asset).MainText, serviceProject.Titles[0].TextBlockCollection[0].Text);
            Assert.AreEqual(((TitleAsset)project.Timeline[2].Shots[0].Asset).SubText, serviceProject.Titles[0].TextBlockCollection[1].Text);
        }

        /// <summary>
        /// Creates a service title for testing.
        /// </summary>
        /// <returns>A title with values.</returns>
        public static Title CreateServiceTitle()
        {
            var title = new Title();
            title.Id = CreateUri();
            title.Creator = "Creator";
            title.Created = new DateTime(2009, 1, 1);
            title.TrackAnchor = CreateServiceAnchor();
            title.TitleTemplate = CreateServiceTitleTemplate();
            title.TextBlockCollection = new TextBlockCollection();
            title.TextBlockCollection.Add(CreateTextBlock());
            title.TextBlockCollection.Add(CreateTextBlock());

            return title;
        }

        /// <summary>
        /// Asserts that the <paramref name="container"/> contains equivalent values from the <paramref name="mediaBin"/>.
        /// </summary>
        /// <param name="container">The container with expected values.</param>
        /// <param name="mediaBin">The container with actual values.</param>
        public static void AssertContainer(MediaBin container, RCE.Infrastructure.Models.MediaBin mediaBin)
        {
            Assert.AreEqual(container.Id, mediaBin.ProviderUri);
            Assert.AreEqual(container.Items.Count, mediaBin.Assets.Count);
            Assert.AreEqual(container.Created, mediaBin.Created);
            Assert.AreEqual(container.Creator, mediaBin.Creator);
            Assert.AreEqual(container.Modified, mediaBin.Modified);
            Assert.AreEqual(container.ModifiedBy, mediaBin.ModifiedBy);
        }

        /// <summary>
        /// Creates a title asset for testing.
        /// </summary>
        /// <returns>A title with values.</returns>
        public static TitleAsset CreateTitleAsset()
        {
            var title = new TitleAsset();
            title.ProviderUri = CreateUri();
            title.TitleTemplate = CreateTitleTemplate();
            title.MainText = "MainText";
            title.SubText = "SubText";

            return title;
        }

        /// <summary>
        /// Creates a service title template for testing.
        /// </summary>
        /// <returns>A title template with values.</returns>
        public static TitleTemplate CreateServiceTitleTemplate()
        {
            var titleTemplate = new TitleTemplate();
            titleTemplate.Id = CreateUri();
            titleTemplate.TemplateName = "Spinner";
            titleTemplate.Creator = "Creator";
            titleTemplate.Created = new DateTime(2009, 1, 1);

            return titleTemplate;
        }

        /// <summary>
        /// Creates a media bin for testing.
        /// </summary>
        /// <returns>A media bin with values.</returns>
        public static MediaBin CreateServiceMediaBin()
        {
            var mediaBin = new MediaBin();
            mediaBin.Id = CreateUri();
            mediaBin.Title = "Title";
            mediaBin.Creator = "Creator";
            mediaBin.ModifiedBy = "ModifiedBy";
            mediaBin.Modified = new DateTime(2009, 1, 2);
            mediaBin.Created = new DateTime(2009, 1, 1);
            mediaBin.Items = new ItemCollection();
            mediaBin.Items.Add(CreateServiceMediaItem());
            mediaBin.Containers = new ContainerCollection();
            mediaBin.Containers.Add(CreateServiceContainer());

            return mediaBin;
        }

        /// <summary>
        /// Creates a title template for testing.
        /// </summary>
        /// <returns>A title template with values.</returns>
        public static RCE.Infrastructure.Models.TitleTemplate CreateTitleTemplate()
        {
            var titleTemplate = new RCE.Infrastructure.Models.TitleTemplate();
            titleTemplate.ProviderUri = CreateUri();
            titleTemplate.Title = "Spinner";

            return titleTemplate;
        }

        /// <summary>
        /// Creates a service track for testing.
        /// </summary>
        /// <param name="trackType">The type of the track(Visual/Audio/Title).</param>
        /// <returns>A track with values.</returns>
        private static Track CreateServiceTrack(RCE.Services.Contracts.TrackType trackType)
        {
            var track = new Track();
            track.Id = CreateUri();
            track.Number = 0;
            track.TrackType = trackType.ToString();
            track.Volume = 100;
            track.Creator = "Creator";
            track.Created = new DateTime(2009, 1, 1);
            track.Shots = new ShotCollection();

            return track;
        }

        /// <summary>
        /// Creates a container for testing.
        /// </summary>
        /// <returns>A container with values.</returns>
        private static Container CreateServiceContainer()
        {
            var container = new Container();
            container.Id = CreateUri();
            container.Title = "Title";
            container.Creator = "Creator";
            container.Created = new DateTime(2009, 1, 1);
            container.Items = new ItemCollection();
            container.Items.Add(CreateServiceMediaItem());

            return container;
        }

        /// <summary>
        /// Creates a track for testing.
        /// </summary>
        /// <param name="trackType">The type of the track(Visual/Audio/Title).</param>
        /// <returns>A track with values.</returns>
        private static Infrastructure.Models.Track CreateTrack(RCE.Infrastructure.Models.TrackType trackType)
        {
            var track = new Infrastructure.Models.Track();
            track.ProviderUri = CreateUri();
            track.TrackType = trackType;
            
            return track;
        }

        /// <summary>
        /// Creates a service shot for testing.
        /// </summary>
        /// <param name="item">Item to be converted into <see cref="Shot"/>.</param>
        /// <returns>A shot with values.</returns>
        private static Shot CreateServiceShot(Item item)
        {
            var shot = new Shot();
            shot.Id = CreateUri();
            shot.Title = "Title";
            shot.Description = "Description";
            shot.Creator = "Creator";
            shot.Created = new DateTime(2009, 1, 1);
            shot.Source = item;
            shot.SourceAnchor = CreateServiceAnchor();
            shot.TrackAnchor = CreateServiceAnchor();
            shot.Volume = (decimal)0.7;
            shot.Comments = new CommentCollection();

            return shot;
        }

        /// <summary>
        /// Creates a shot for testing.
        /// </summary>
        /// <param name="asset">The <see cref="Asset"/>.</param>
        /// <param name="comment">The shot's comment.</param>
        /// <returns>A shot with values.</returns>
        private static RCE.Infrastructure.Models.TimelineElement CreateShot(Asset asset, Infrastructure.Models.Comment comment)
        {
            var shot = new TimelineElement();
            shot.ProviderUri = CreateUri();
            shot.Asset = asset;
            shot.InPosition = TimeCode.FromSeconds(60d, SmpteFrameRate.Smpte2997Drop);
            shot.OutPosition = TimeCode.FromSeconds(200d, SmpteFrameRate.Smpte2997Drop);
            shot.Position = TimeCode.FromSeconds(30d, SmpteFrameRate.Smpte2997Drop);
            shot.TrackAnchorUri = CreateUri();
            shot.SourceAnchorUri = CreateUri();
            shot.Volume = 0.7;

            if (comment != null)
            {
                shot.Comments.Add(comment);
            }

            return shot;
        }

        /// <summary>
        /// Creates a service comment for testing.
        /// </summary>
        /// <param name="commentType">The type of the comment.</param>
        /// <returns>A comment with values.</returns>
        private static Comment CreateServiceComment(CommentType commentType)
        {
            Comment comment;

            if (commentType == CommentType.Ink)
            {
                string strokes = @"<?xml version=""1.0"" encoding=""utf-16"" ?> <StrokeCollection xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> <Stroke> <Stroke.DrawingAttributes> <DrawingAttributes Width=""200"" Height=""300"" Color=""#01020304"" OutlineColor=""#02020304""/> </Stroke.DrawingAttributes> <Stroke.StylusPoints> <StylusPointCollection> <StylusPoint X=""100"" Y=""40""/> </StylusPointCollection> </Stroke.StylusPoints> </Stroke> </StrokeCollection>";
                comment = new InkComment { Strokes = strokes, Text = "InkComment" };
            }
            else
            {
                comment = new Comment { Text = "Text" };
            }

            comment.Id = CreateUri();
            comment.Type = commentType.ToString();
            comment.MarkIn = 5;
            comment.MarkOut = 7.6;

            comment.Creator = "Creator";
            comment.Created = new DateTime(2009, 1, 1);

            return comment;
        }

        /// <summary>
        /// Creates a comment for testing.
        /// </summary>
        /// <param name="commentType">The type of the comment.</param>
        /// <returns>A comment with values.</returns>
        private static Infrastructure.Models.Comment CreateComment(CommentType commentType)
        {
            var comment = commentType == CommentType.Ink ? new Infrastructure.Models.InkComment() : new Infrastructure.Models.Comment();
            comment.ProviderUri = CreateUri();
            comment.CommentType = commentType;
            comment.MarkIn = 5;
            comment.MarkOut = 7.6;

            if (commentType == CommentType.Ink)
            {
                comment.Text = "Ink Comment";
                Stroke stroke = new Stroke(new StylusPointCollection { new StylusPoint(10, 20) });
                stroke.DrawingAttributes = new DrawingAttributes
                                               {
                                                   Color = System.Windows.Media.Color.FromArgb(0, 0, 0, 0),
                                                   OutlineColor = System.Windows.Media.Color.FromArgb(1, 2, 3, 4)
                                               };
                ((Infrastructure.Models.InkComment)comment).InkCommentStrokes = new StrokeCollection { stroke };
            }
            else
            {
                comment.Text = "Text";
            }

            comment.Creator = "Creator";
            comment.Created = new DateTime(2009, 1, 1);

            return comment;
        }

        /// <summary>
        /// Creates an anchor for testing.
        /// </summary>
        /// <returns>An anchor with values.</returns>
        private static Anchor CreateServiceAnchor()
        {
            var anchor = new Anchor();
            anchor.Id = CreateUri();
            anchor.MarkIn = 5;
            anchor.MarkOut = 6.907;
            anchor.Creator = "Creator";
            anchor.Created = new DateTime(2009, 1, 1);

            return anchor;
        }

        /// <summary>
        /// Creates a text block for testing.
        /// </summary>
        /// <returns>A textblock with values.</returns>
        private static TextBlock CreateTextBlock()
        {
            var textBlock = new TextBlock();
            textBlock.Id = CreateUri();
            textBlock.Text = "text";
            textBlock.Creator = "Creator";
            textBlock.Created = new DateTime(2009, 1, 1);

            return textBlock;
        }

        /// <summary>
        /// Creates a service media item form testing.
        /// </summary>
        /// <returns>A service media item with values.</returns>
        private static MediaItem CreateServiceMediaItem()
        {
            var mediaItem = new MediaItem();
            mediaItem.Id = CreateUri();
            mediaItem.Title = "Title";
            mediaItem.Description = "Description";
            mediaItem.Resources = new ResourceCollection();
            mediaItem.Resources.Add(CreateServiceResource());
            mediaItem.Creator = "Creator";
            mediaItem.Created = new DateTime(2009, 1, 1);

            return mediaItem;
        }

        /// <summary>
        /// Creates a service video item for testing.
        /// </summary>
        /// <returns>A service video item with values.</returns>
        private static VideoItem CreateServiceVideoItem()
        {
            var item = new VideoItem();
            item.Id = CreateUri();
            item.Title = "Title";
            item.Height = 200;
            item.Height = 300;
            item.Duration = 500;
            item.FrameRate = SmpteFrameRate.Smpte2997Drop;
            item.Description = "Description";
            item.Resources = new ResourceCollection();
            item.Resources.Add(CreateServiceResource());
            item.Creator = "Creator";
            item.Created = new DateTime(2009, 1, 1);
            item.Metadata = new List<MetadataField> { new MetadataField("TestName", "TestValue") };
            item.ThumbnailSource = "http://test1/test.png";
           
            return item;
        }

        /// <summary>
        /// Creates a service audio item for testing.
        /// </summary>
        /// <returns>A service audio item with values.</returns>
        private static AudioItem CreateServiceAudioItem()
        {
            var item = new AudioItem();
            item.Id = CreateUri();
            item.Title = "Title";
            item.Duration = 10;
            item.Description = "Description";
            item.Resources = new ResourceCollection();
            item.Resources.Add(CreateServiceResource());
            item.Creator = "Creator";
            item.Created = new DateTime(2009, 1, 1);
            item.Metadata = new List<MetadataField> { new MetadataField("TestName", "TestValue") };

            return item;
        }

        /// <summary>
        /// Creates a service image item for testing.
        /// </summary>
        /// <returns>A service image item with values.</returns>
        private static ImageItem CreateServiceImageItem()
        {
            var item = new ImageItem();
            item.Id = CreateUri();
            item.Title = "Title";
            item.Height = 20;
            item.Width = 30;
            item.Description = "Description";
            item.Resources = new ResourceCollection();
            item.Resources.Add(CreateServiceResource());
            item.Creator = "Creator";
            item.Created = new DateTime(2009, 1, 1);
            item.Metadata = new List<MetadataField> { new MetadataField("TestName", "TestValue") };
            
            return item;
        }

        /// <summary>
        /// Creates a video asset for testing.
        /// </summary>
        /// <returns>A video asset with values.</returns>
        private static VideoAsset CreateVideoAsset()
        {
            var item = new VideoAsset();
            item.ProviderUri = CreateUri();
            item.Source = CreateUri();
            item.Title = "Title";
            item.ResourceType = ResourceType.Master;
            item.FrameRate = SmpteFrameRate.Smpte2997NonDrop;
            item.Height = 200;
            item.Width = 200;
            item.Duration = TimeCode.FromSeconds(60d, item.FrameRate);
            item.Metadata = new List<MetadataField> { new MetadataField("TestName", "TestValue") };
            item.ThumbnailSource = "http://test1/test.png";

            return item;
        }

        /// <summary>
        /// Creates a image asset for testing.
        /// </summary>
        /// <returns>A image asset with values.</returns>
        private static ImageAsset CreateImageAsset()
        {
            var item = new ImageAsset();
            item.Source = CreateUri();
            item.ProviderUri = CreateUri();
            item.Title = "Title";
            item.ResourceType = ResourceType.Master;
            item.Height = 200;
            item.Width = 200;
            item.Metadata = new List<MetadataField> { new MetadataField("TestName", "TestValue") };

            return item;
        }

        /// <summary>
        /// Creates a audio asset for testing.
        /// </summary>
        /// <returns>A audio asset with values.</returns>
        private static AudioAsset CreateAudioAsset()
        {
            var item = new AudioAsset();
            item.ProviderUri = CreateUri();
            item.Source = CreateUri();
            item.Title = "Title";
            item.ResourceType = ResourceType.Master;
            item.Duration = 200;
            item.Metadata = new List<MetadataField> { new MetadataField("TestName", "TestValue") };

            return item;
        }

        /// <summary>
        /// Creates a resource for testing.
        /// </summary>
        /// <returns>A resource with values.</returns>
        private static Resource CreateServiceResource()
        {
            var resource = new Resource();
            resource.Id = CreateUri();
            resource.Ref = "ref";
            resource.ResourceType = "Master";
            resource.MimeType = "mimeType";
            resource.Creator = "Creator";
            resource.Created = new DateTime(2009, 1, 1);

            return resource;
        }

        /// <summary>
        /// Asserts that the <paramref name="track"/> contains equivalent values from the <paramref name="serviceTrack"/>.
        /// </summary>
        /// <param name="serviceTrack">The track with expected values.</param>
        /// <param name="track">The track with actual values.</param>
        private static void AssertTrack(Track serviceTrack, RCE.Infrastructure.Models.Track track)
        {
            Assert.AreEqual(serviceTrack.Id, track.ProviderUri);
            Assert.AreEqual(serviceTrack.TrackType, track.TrackType.ToString());
            Assert.AreEqual(serviceTrack.Number, track.Number);

            AssertShot(serviceTrack.Shots[0], track.Shots[0]);
        }

        /// <summary>
        /// Asserts that the <paramref name="serviceTrack"/> contains equivalent values from the <paramref name="track"/>.
        /// </summary>
        /// <param name="track">The track with expected values.</param>
        /// <param name="serviceTrack">The track with actual values.</param>
        private static void AssertTrack(RCE.Infrastructure.Models.Track track, Track serviceTrack)
        {
            Assert.AreEqual(track.ProviderUri, serviceTrack.Id);
            Assert.AreEqual(track.TrackType.ToString(), serviceTrack.TrackType);
            Assert.AreEqual(track.Number, serviceTrack.Number);

            AssertShot(track.Shots[0], serviceTrack.Shots[0]);
        }

        /// <summary>
        /// Asserts that the <paramref name="shot"/> contains equivalent values from the <paramref name="serviceShot"/>. 
        /// </summary>
        /// <param name="serviceShot">The shot with expected values.</param>
        /// <param name="shot">The shot with actual values.</param>
        private static void AssertShot(Shot serviceShot, RCE.Infrastructure.Models.TimelineElement shot)
        {
            Assert.AreEqual(serviceShot.Id, shot.ProviderUri);
            Assert.AreEqual((double)serviceShot.Volume / 100, shot.Volume);
            Assert.AreEqual(serviceShot.TrackAnchor.Id, shot.TrackAnchorUri);
            Assert.AreEqual(serviceShot.TrackAnchor.MarkIn, shot.Position.TotalSeconds);

            // Assert.AreEqual(serviceShot.TrackAnchor.MarkOut, shot.OutPosition);
            Assert.AreEqual(serviceShot.SourceAnchor.Id, shot.SourceAnchorUri);
            Assert.AreEqual(serviceShot.SourceAnchor.MarkIn, shot.InPosition.TotalSeconds);
            Assert.AreEqual(serviceShot.SourceAnchor.MarkOut, shot.OutPosition.TotalSeconds);

            AssertItem(serviceShot.Source, shot.Asset);
        }

        /// <summary>
        /// Asserts that the <paramref name="serviceShot"/> contains equivalent values from the <paramref name="shot"/>. 
        /// </summary>
        /// <param name="shot">The shot with expected values.</param>
        /// <param name="serviceShot">The shot with actual values.</param>
        private static void AssertShot(TimelineElement shot, Shot serviceShot)
        {
            Assert.AreEqual(shot.ProviderUri, serviceShot.Id);
            Assert.AreEqual(shot.Volume * 100, (double)serviceShot.Volume);
            Assert.AreEqual(shot.TrackAnchorUri, serviceShot.TrackAnchor.Id);
            Assert.AreEqual(shot.Position.TotalSeconds, serviceShot.TrackAnchor.MarkIn);

            // Assert.AreEqual(serviceShot.TrackAnchor.MarkOut, shot.OutPosition);
            Assert.AreEqual(shot.SourceAnchorUri, serviceShot.SourceAnchor.Id);
            Assert.AreEqual(shot.InPosition.TotalSeconds, serviceShot.SourceAnchor.MarkIn);
            Assert.AreEqual(shot.OutPosition.TotalSeconds, serviceShot.SourceAnchor.MarkOut);
        }

        /// <summary>
        /// Asserts that the <paramref name="asset"/> contains equivalent values from the <paramref name="serviceItem"/>.
        /// </summary>
        /// <param name="serviceItem">The container with expected values.</param>
        /// <param name="asset">The container with actual values.</param>
        private static void AssertItem(Item serviceItem, Asset asset)
        {
            Assert.AreEqual(serviceItem.Id, asset.ProviderUri);
            Assert.AreEqual(serviceItem.Title, serviceItem.Title);
            Assert.AreEqual(serviceItem.Resources.First().ResourceType, asset.ResourceType.ToString());
            
            // Assert.AreEqual(serviceItem.Metadata, asset.Metadata);
        }

        /// <summary>
        /// Asserts that the <paramref name="comment"/> contains equivalent values from the <paramref name="serviceComment"/>.
        /// </summary>
        /// <param name="serviceComment">The comment with expected values.</param>
        /// <param name="comment">The comment with actual values.</param>
        private static void AssertComment(Comment serviceComment, RCE.Infrastructure.Models.Comment comment)
        {
            Assert.AreEqual(serviceComment.Id, comment.ProviderUri);
            Assert.AreEqual(serviceComment.Creator, comment.Creator);
            Assert.AreEqual(serviceComment.Created, comment.Created);
            Assert.AreEqual(serviceComment.MarkIn, comment.MarkIn);
            Assert.AreEqual(serviceComment.MarkOut, comment.MarkOut);
            Assert.AreEqual(serviceComment.Type, comment.CommentType.ToString());

            if (serviceComment.Type == CommentType.Ink.ToString())
            {
                Assert.AreEqual(serviceComment.Text, comment.Text);
                Assert.AreEqual(1, ((Infrastructure.Models.InkComment)comment).InkCommentStrokes.Count());
            }
            else
            {
                Assert.AreEqual(serviceComment.Text, comment.Text);
            }
        }

        /// <summary>
        /// Asserts that the <paramref name="serviceComment"/> contains equivalent values from the <paramref name="comment"/>.
        /// </summary>
        /// <param name="comment">The comment with expected values.</param>
        /// <param name="serviceComment">The comment with actual values.</param>
        private static void AssertComment(RCE.Infrastructure.Models.Comment comment, Comment serviceComment)
        {
            Assert.AreEqual(comment.ProviderUri, serviceComment.Id);
            Assert.AreEqual(comment.Creator, serviceComment.Creator);
            Assert.AreEqual(comment.Created, serviceComment.Created);
            Assert.AreEqual(comment.MarkIn, serviceComment.MarkIn);
            Assert.AreEqual(comment.MarkOut, serviceComment.MarkOut);
            Assert.AreEqual(comment.CommentType.ToString(), serviceComment.Type);
            Assert.AreEqual(comment.Text, serviceComment.Text);

            InkComment inkComment = serviceComment as InkComment;

            if (inkComment != null)
            {
                StringAssert.Contains(inkComment.Strokes, "#00000000");
                StringAssert.Contains(inkComment.Strokes, "#01020304");
            }
        }

        /// <summary>
        /// Creates a random uri for testing.
        /// </summary>
        /// <returns>The random uri.</returns>
        private static Uri CreateUri()
        {
            var id = Guid.NewGuid().ToString();

            var uriString = String.Concat("http://test/", id);

            return new Uri(uriString);
        }
    }
}
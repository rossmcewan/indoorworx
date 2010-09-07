// <copyright file="DataProvider.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: DataProvider.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Objects;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using LAgger;
    using RCE.Data.Sql.Translators;
    using RCE.Services.Contracts;

    /// <summary>
    /// Provides the implementation for <see cref="IDataProvider"/> and <see cref="IAssetsDataProvider"/> that will retrieve data from a SQL server.
    /// </summary>
    public class DataProvider : IDataProvider, IAssetsDataProvider
    {
        /// <summary>
        /// The <see cref="ILoggerService"/>.
        /// </summary>
        private readonly ILoggerService loggerService;

        /// <summary>
        /// The default time out value to get the response from the database.
        /// </summary>
        private const int DefaultCommandTimeout = 180;

        /// <summary>
        ///  The default library id used 
        /// </summary>
        private Uri defaultLibraryId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider"/> class.
        /// </summary>
        /// <param name="loggerService">The logger.</param>
        public DataProvider(ILoggerService loggerService, Uri defaultLibraryId)
        {
            this.loggerService = loggerService;
            this.defaultLibraryId = defaultLibraryId;
        }
        
        /// <summary>
        /// Returns back given no. of items from the library.
        /// </summary>
        /// <param name="maxNumberOfItems">Maximum no. of records in the result.</param>
        /// <returns>A <see cref="Container"/> with the items.</returns>
        public RCE.Services.Contracts.Container LoadLibrary(int maxNumberOfItems)
        {
            return this.LoadLibrary(null, maxNumberOfItems);
        }

        /// <summary>
        /// Returns back all of the items that are contained in the library filtering them using the filter provided.
        /// </summary>
        /// <param name="filter">The filter used to get the items.</param>
        /// <param name="maxNumberOfItems">Maximum no. of records in the result.</param>
        /// <returns>A <see cref="Container"/> with the items.</returns>
        public RCE.Services.Contracts.Container LoadLibrary(string filter, int maxNumberOfItems)
        {
            return LoadLibrary(this.defaultLibraryId, filter, maxNumberOfItems);
        }

        /// <summary>
        /// Returns back given no. of items from the library.
        /// </summary>
        /// <param name="libraryId">The <see cref="Uri"/> of the container to load from the library.</param>
        /// <param name="maxNumberOfItems">Maximum no. of records in the result.</param>
        /// <returns>A <see cref="Services.Contracts.Container"/> with the items.</returns>
        public Services.Contracts.Container LoadLibraryById(Uri libraryId, int maxNumberOfItems)
        {
            return LoadLibrary(libraryId, null, maxNumberOfItems);
        }

        /// <summary>
        /// Returns back all of the items that are contained in the library filtering them using the filter provided.
        /// </summary>
        /// <param name="libraryId">The <see cref="Uri"/> of the container to load from the library.</param>
        /// <param name="filter">The filter used to get the items.</param>
        /// <param name="maxNumberOfItems">Maximum no. of records in the result.</param>
        /// <returns>A <see cref="Container"/> with the items.</returns>
        private static RCE.Services.Contracts.Container LoadLibrary(Uri libraryId, string filter, int maxNumberOfItems)
        {
            RoughCutEditorEntities context = new RoughCutEditorEntities { CommandTimeout = DefaultCommandTimeout };

            string uriString = libraryId.ToString();

            ObjectQuery<Container> sqlContainerQuery = context.Container
                .Include("Containers");

            if (!string.IsNullOrEmpty(filter))
            {
                sqlContainerQuery = sqlContainerQuery.Include("Containers.Items");
            }

            Container sqlContainer = sqlContainerQuery.FirstOrDefault(x => x.Id == uriString);

            ObjectQuery<Item> query = context.Item;

            // .Include("Resources")
            // .Include("Resources.VideoFormat")
            // .Include("Resources.AudioFormat")
            // .Include("Resources.ImageFormat")
            if (maxNumberOfItems > 0)
            {
                query.Top(maxNumberOfItems.ToString(CultureInfo.InvariantCulture));
            }

            IQueryable<Item> items = query.Where(x => x.Container.FirstOrDefault().Id == sqlContainer.Id)
                .SelectMany(x => x.Resources.Where(r => r.ResourceType != "LiveSmoothStream"))
                .Select(x => x.Item);

            RCE.Services.Contracts.Container library = null;

            if (sqlContainer != null)
            {
                foreach (Item item in items)
                {
                    if (!item.Resources.IsLoaded)
                    {
                        item.Resources.Load();

                        foreach (Resource resource in item.Resources)
                        {
                            if (!resource.VideoFormatReference.IsLoaded)
                            {
                                resource.VideoFormatReference.Load();
                            }

                            if (!resource.AudioFormatReference.IsLoaded)
                            {
                                resource.AudioFormatReference.Load();
                            }

                            if (!resource.ImageFormatReference.IsLoaded)
                            {
                                resource.ImageFormatReference.Load();
                            }
                        }
                    }
                }

                sqlContainer.Items.Attach(items);
                library = SqlDataProviderTranslator.ConvertToContainer(sqlContainer, filter, maxNumberOfItems);
            }

            return library;
        }

        /// <summary>
        /// Loads the MediaBin <see cref="Container" /> with a <see cref="ItemCollection" /> that contains the items in the media bin.
        /// </summary>
        /// <param name="mediaBinUri">The <see cref="Uri"/> of the media bin to load.</param>
        /// <returns>A <see cref="Container"/> with the media elements for the project.</returns>
        public Services.Contracts.MediaBin LoadMediaBin(Uri mediaBinUri)
        {
            RoughCutEditorEntities context = new RoughCutEditorEntities { CommandTimeout = 180 };

            string uriString = mediaBinUri.ToString();

            Container mediaBin = context.Container
                .Include("Items")
                .Include("Items.Resources")
                .Include("Items.Resources.VideoFormat")
                .Include("Items.Resources.AudioFormat")
                .Include("Items.Resources.ImageFormat")
                .Include("Containers")
                
                // .Include("Containers.Items")
                // .Include("Containers.Items.Resources")
                // .Include("Containers.Items.Resources.VideoFormat")
                // .Include("Containers.Items.Resources.AudioFormat")
                // .Include("Containers.Items.Resources.ImageFormat")
                .Where(x => x.Id == uriString)
                .FirstOrDefault();

            return SqlDataProviderTranslator.ConvertToMediaBin(mediaBin);
        }

        /// <summary>
        /// Loads a <see cref="TitleTemplate"/> from the repository.
        /// </summary>
        /// <returns>The <see cref="TitleTemplateCollection"/> of the titles template that were loaded.</returns>
        public TitleTemplateCollection LoadTitleTemplates()
        {
            RoughCutEditorEntities context = new RoughCutEditorEntities { CommandTimeout = DefaultCommandTimeout };

            List<TitleTemplate> titleTemplates = context.TitleTemplate.ToList();

            return SqlDataProviderTranslator.ConvertToTitleTemplates(titleTemplates);
        }

        /// <summary>
        /// Loads a project from the repository returning back the details.
        /// </summary>
        /// <param name="site">The <see cref="Uri"/> of the project site.</param>
        /// <returns>The <see cref="Services.Contracts.Project"/> that exists at the specified <see cref="Uri"/>.</returns>
        public Services.Contracts.Project LoadProject(Uri site)
        {
            RoughCutEditorEntities context = new RoughCutEditorEntities { CommandTimeout = DefaultCommandTimeout };

            string projectUriString = site.ToString();
            Project project = LoadProject(projectUriString, context);

            if (project == null)
            {
                return new Services.Contracts.Project();
            }

            return SqlDataProviderTranslator.ConvertToProject(project);
        }

        /// <summary>
        /// Saves a project into the repository.
        /// </summary>
        /// <param name="project">The project to be saved.</param>
        /// <returns>true, indicates that the project was saved. false, that the save failed.</returns>
        public bool SaveProject(Services.Contracts.Project project)
        {
            RoughCutEditorEntities context = new RoughCutEditorEntities { CommandTimeout = 1000 };

            try
            {
                IList<Item> items = new List<Item>();

                RetrieveItems(items, project.MediaBin.Items, context);
                RetrieveItems(items, project.MediaBin.Containers, context);

                string uriString = project.Id.ToString();
                Project sqlProject = LoadFullProject(uriString, context);

                if (sqlProject == null)
                {
                    sqlProject = new Project { Id = uriString };
                    context.AddToProject(sqlProject);
                }

                SqlDataProviderTranslator.ConvertToSqlProject(project, sqlProject);

                uriString = project.MediaBin.Id.ToString();

                Container sqlMediaBin = context.Container.Where(m => m.Id == uriString).FirstOrDefault();

                if (sqlMediaBin == null)
                {
                    sqlMediaBin = new Container { Id = uriString, Title = project.MediaBin.Title };
                }

                sqlMediaBin.Items.Clear();

                sqlProject.MediaBin = SqlDataProviderTranslator.ConvertToSqlMediaBin(project.MediaBin, sqlMediaBin, items);

                foreach (RCE.Services.Contracts.Container container in project.MediaBin.Containers)
                {
                    uriString = container.Id.ToString();
                    Container sqlContainer = context.Container.Where(m => m.Id == uriString).FirstOrDefault();

                    if (sqlContainer == null)
                    {
                        sqlContainer = new Container { Id = uriString, Title = container.Title };
                        sqlMediaBin.Containers.Add(sqlContainer);
                    }

                    sqlContainer.Items.Clear();
                    SqlDataProviderTranslator.ConvertToSqlMediaBin(container, sqlContainer, items);
                }

                sqlMediaBin.Containers.Where(x => x.EntityState == EntityState.Unchanged)
                    .ToList()
                    .ForEach(x =>
                                 {
                                     x.Items.Where(i => i.EntityState == EntityState.Unchanged)
                                         .ToList()
                                         .ForEach(i =>
                                                      {
                                                          x.Items.Remove(i);
                                                      });

                                     sqlMediaBin.Containers.Remove(x);
                                     context.DeleteObject(x);
                                 });

                foreach (Services.Contracts.Comment comment in project.Comments)
                {
                    uriString = comment.Id.ToString();

                    Comment sqlComment = sqlProject.Comments.Where(m => m.Id == uriString).FirstOrDefault();

                    if (sqlComment == null)
                    {
                        sqlComment = new Comment { Id = uriString };
                        sqlProject.Comments.Add(sqlComment);
                    }

                    SqlDataProviderTranslator.ConvertToSqlComment(comment, sqlComment);
                }

                sqlProject.Comments.Where(x => x.EntityState == EntityState.Unchanged)
                    .ToList()
                    .ForEach(x =>
                                 {
                                     sqlProject.Comments.Remove(x);
                                     context.DeleteObject(x);
                                 });

                foreach (Services.Contracts.Track track in project.Timeline)
                {
                    uriString = track.Id.ToString();

                    Track sqlTrack = sqlProject.Tracks.Where(m => m.Id == uriString).FirstOrDefault();

                    if (sqlTrack == null)
                    {
                        sqlTrack = new Track { Id = uriString };
                        sqlProject.Tracks.Add(sqlTrack);
                    }

                    SqlDataProviderTranslator.ConvertToSqlTrack(track, sqlTrack);

                    foreach (Services.Contracts.Shot shot in track.Shots)
                    {
                        uriString = shot.Id.ToString();

                        Shot sqlShot = sqlTrack.Shots.Where(m => m.Id == uriString).FirstOrDefault();

                        if (sqlShot == null)
                        {
                            sqlShot = new Shot { Id = uriString };
                            sqlTrack.Shots.Add(sqlShot);
                        }

                        SqlDataProviderTranslator.ConvertToSqlShot(shot, sqlShot, items);

                        foreach (Services.Contracts.Comment comment in shot.Comments)
                        {
                            uriString = comment.Id.ToString();

                            Comment sqlComment = sqlProject.Comments.Where(m => m.Id == uriString).FirstOrDefault();

                            if (sqlComment == null)
                            {
                                sqlComment = new Comment { Id = uriString };
                                sqlShot.Comments.Add(sqlComment);
                            }
                            else if (sqlComment.EntityState == EntityState.Added)
                            {
                                sqlShot.Comments.Add(sqlComment);
                            }

                            SqlDataProviderTranslator.ConvertToSqlComment(comment, sqlComment);
                        }

                        sqlShot.Comments.Where(x => x.EntityState == EntityState.Unchanged)
                            .ToList()
                            .ForEach(x =>
                                         {
                                             sqlProject.Comments.Remove(x);
                                             context.DeleteObject(x);
                                         });
                    }

                    sqlTrack.Shots.Where(x => x.EntityState == EntityState.Unchanged)
                        .ToList()
                        .ForEach(x =>
                                     {
                                         sqlTrack.Shots.Remove(x);
                                         context.DeleteObject(x);
                                     });
                }

                sqlProject.Tracks.Where(x => x.EntityState == EntityState.Unchanged)
                    .ToList()
                    .ForEach(x =>
                                 {
                                     sqlProject.Tracks.Remove(x);
                                     context.DeleteObject(x);
                                 });

                foreach (Services.Contracts.Title title in project.Titles)
                {
                    uriString = title.Id.ToString();

                    Title sqlTitle = sqlProject.Titles.Where(m => m.Id == uriString).FirstOrDefault();

                    if (sqlTitle == null)
                    {
                        sqlTitle = new Title { Id = uriString };
                        string tempId = title.TitleTemplate.Id.ToString();
                        sqlTitle.TitleTemplate = context.TitleTemplate.Where(m => m.Id == tempId).FirstOrDefault();
                        sqlProject.Titles.Add(sqlTitle);
                    }

                    SqlDataProviderTranslator.ConvertToSqlTitle(title, sqlTitle);
                }

                sqlProject.Titles.Where(x => x.EntityState == EntityState.Unchanged)
                    .ToList()
                    .ForEach(x =>
                                 {
                                     sqlProject.Titles.Remove(x);
                                     context.DeleteObject(x);
                                 });

                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                this.Log(ex);

                return false;
            }
        }

        /// <summary>
        /// Get the projects collection of the given user.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>A <see cref="ProjectCollection"/> with all the projects of the user.</returns>
        public ProjectCollection GetProjectsByUser(string userName)
        {
            RoughCutEditorEntities context = new RoughCutEditorEntities();

            List<Project> projects = context.Project
                .Include("MediaBin")
                .Where(x => x.Creator == userName)
                .ToList();

            return SqlDataProviderTranslator.ConvertToProjects(projects);
        }

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="site">The <see cref="Uri"/> of the project site.</param>
        /// <returns>True if project have been deleted else false. </returns>
        public bool DeleteProject(Uri site)
        {
            try
            {
                using (RoughCutEditorEntities context = new RoughCutEditorEntities { CommandTimeout = DefaultCommandTimeout })
                {
                    string uriString = site.ToString();
                    Project sqlProject = LoadFullProject(uriString, context);

                    if (sqlProject != null)
                    {
                        sqlProject.Titles.ToList().ForEach(context.DeleteObject);
                        sqlProject.Comments.ToList().ForEach(context.DeleteObject);
                        sqlProject.MediaBin.Containers.ToList().ForEach(context.DeleteObject);
                        sqlProject.Comments.ToList().ForEach(context.DeleteObject);
                        sqlProject.Tracks.ToList().ForEach(x =>
                        {
                            x.Shots.ToList().ForEach(context.DeleteObject);
                            context.DeleteObject(x);
                        });
                        sqlProject.Titles.ToList().ForEach(context.DeleteObject);
                        context.DeleteObject(sqlProject);
                        context.SaveChanges(true);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                this.Log(ex);
                return false;
            }
        }

        /// <summary>
        /// Retrieves the items.
        /// </summary>
        /// <param name="retrievedItems">The list of retrieved items..</param>
        /// <param name="items">The items being retrieved.</param>
        /// <param name="context">The context used to retrieve the items.</param>
        private static void RetrieveItems(IList<Item> retrievedItems, IEnumerable<Services.Contracts.Item> items, RoughCutEditorEntities context)
        {
            foreach (MediaItem item in items)
            {
                string itemIdString = item.Id.ToString();
                Item existentItem = context.Item
                    .Include("Resources")
                    .Include("Resources.VideoFormat")
                    .Include("Resources.AudioFormat")
                    .Include("Resources.ImageFormat")
                    .Where(x => x.Id == itemIdString)
                    .FirstOrDefault();

                if (existentItem != null)
                {
                    retrievedItems.Add(existentItem);
                }
            }
        }

        /// <summary>
        /// Retrieves the items of the given <paramref name="containers"/>.
        /// </summary>
        /// <param name="items">The list of retrieved items.</param>
        /// <param name="containers">The containers with items.</param>
        /// <param name="context">The context used to retrieve the items.</param>
        private static void RetrieveItems(IList<Item> items, IEnumerable<Services.Contracts.Container> containers, RoughCutEditorEntities context)
        {
            foreach (RCE.Services.Contracts.Container container in containers)
            {
                RetrieveItems(items, container.Items, context);
            }
        }

        /// <summary>
        /// Loads a project without retrieving the media bin container items.
        /// </summary>
        /// <param name="projectUriString">The project uri of the project being loaded.</param>
        /// <param name="context">The context used to load the project.</param>
        /// <returns>A <see cref="Project"/> if exists or null.</returns>
        private static Project LoadProject(string projectUriString, RoughCutEditorEntities context)
        {
            return context.Project
                .Include("Tracks")
                .Include("Tracks.Shots")
                .Include("Tracks.Shots.Comments")
                .Include("Tracks.Shots.Item")
                .Include("Tracks.Shots.Item.Resources")
                .Include("Tracks.Shots.Item.Resources.VideoFormat")
                .Include("Tracks.Shots.Item.Resources.AudioFormat")
                .Include("Tracks.Shots.Item.Resources.ImageFormat")
                .Include("Comments")
                .Include("MediaBin")
                .Include("MediaBin.Items")
                .Include("MediaBin.Containers")
                .Include("Titles")
                .Include("Titles.TitleTemplate")
                .Where(p => p.Id == projectUriString)
                .FirstOrDefault();
        }

        /// <summary>
        /// Loads a project.
        /// </summary>
        /// <param name="projectUriString">The project uri of the project being loaded.</param>
        /// <param name="context">The context used to load the project.</param>
        /// <returns>A <see cref="Project"/> if exists or null.</returns>
        private static Project LoadFullProject(string projectUriString, RoughCutEditorEntities context)
        {
            return context.Project
                .Include("Tracks")
                .Include("Tracks.Shots")
                .Include("Tracks.Shots.Comments")
                .Include("Tracks.Shots.Item")
                .Include("Tracks.Shots.Item.Resources")
                .Include("Tracks.Shots.Item.Resources.VideoFormat")
                .Include("Tracks.Shots.Item.Resources.AudioFormat")
                .Include("Tracks.Shots.Item.Resources.ImageFormat")
                .Include("Comments")
                .Include("MediaBin")
                .Include("MediaBin.Items")
                .Include("MediaBin.Containers")
                .Include("MediaBin.Containers.Items")
                .Include("Titles")
                .Include("Titles.TitleTemplate")
                .Where(p => p.Id == projectUriString)
                .FirstOrDefault();
        }

        /// <summary>
        /// Logs the given the exception with the <see cref="ILoggerService"/>.
        /// </summary>
        /// <param name="ex">The exception beign logged.</param>
        private void Log(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Exception: {0}{1}", ex.Message, Environment.NewLine);
            builder.AppendFormat("Stack Trace: {0}", ex.StackTrace);

            if (ex.InnerException != null)
            {
                builder.AppendFormat("{0}Inner Exception: {1}{2}", Environment.NewLine, ex.InnerException.Message, Environment.NewLine);
                builder.AppendFormat("Inner Stack Trace: {0}", ex.InnerException.StackTrace);
            }

            LogEntry entry = new LogEntry(builder.ToString(), "Data Service", 1, 0, TraceEventType.Error, "Error in Data Service");

            this.loggerService.LogEntries(new[] { entry });
        }
    }
}

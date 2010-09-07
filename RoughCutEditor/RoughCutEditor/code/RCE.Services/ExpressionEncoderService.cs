// <copyright file="ExpressionEncoderService.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ExpressionEncoderService.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Services
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization;
    using System.ServiceModel.Activation;
    using System.Web;
    using Contracts;

    /// <summary>
    /// Service used to save the output project on the server.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ExpressionEncoderService : IExpressionEncoderService
    {
        /// <summary>
        /// Saves the project on the server.
        /// </summary>
        /// <param name="project">The project being saved.</param>
        public void EnqueueJob(Project project)
        {
            string queuePath = HttpContext.Current.Server.MapPath("encode/Queue");

            if (!Directory.Exists(queuePath))
            {
                Directory.CreateDirectory(queuePath);
            }

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            string tmpFilePath = Path.Combine(queuePath, string.Format(CultureInfo.InvariantCulture, "{0}-{1}.jobtmp", project.Title.ToString(), datetime));
            string finalFilePath = Path.Combine(queuePath, string.Format(CultureInfo.InvariantCulture, "{0}-{1}.jobreq", project.Title.ToString(), datetime));

            using (FileStream fs = new FileStream(tmpFilePath, FileMode.Create, FileAccess.Write))
            {
                DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(Project));
                dataContractSerializer.WriteObject(fs, project);
                fs.Flush();
            }

            if (File.Exists(tmpFilePath))
            {
                if (File.Exists(finalFilePath))
                {
                    File.Delete(finalFilePath);
                }

                File.Move(tmpFilePath, finalFilePath);
            }
        }
    }
}

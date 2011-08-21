using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace IndoorWorx.SmoothStreaming
{
    public class DownloaderManager
    {
        public virtual Stream DownloadManifest(Uri manifestUri, bool forceNewDownload, CookieContainer cookies)
        {
            if (forceNewDownload)
            {
                string newUriString = String.Concat(manifestUri.AbsoluteUri, "?ignore=", Guid.NewGuid());
                manifestUri = new Uri(newUriString);
            }

            HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(manifestUri);
            client.Timeout = (1000 * 60) * 5;

            if (cookies != null)
            {
                client.CookieContainer = cookies;
            }

            HttpWebResponse response = (HttpWebResponse)client.GetResponse();

            return response.GetResponseStream();
        }

        public virtual Stream DownloadManifest(Uri manifestUri, bool forceNewDownload)
        {
            return this.DownloadManifest(manifestUri, forceNewDownload, null);
        }
    }
}

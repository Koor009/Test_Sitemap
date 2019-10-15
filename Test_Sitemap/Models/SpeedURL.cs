using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Test_Sitemap.Models
{
    internal sealed class SpeedURL
    {
        private static WebClient _wc;
        private static Stopwatch _watch;

        static SpeedURL()
        {
            _wc = new WebClient();
            _watch = new Stopwatch();
        }

        /// <summary>
        /// Speed of each page in the site map.
        /// </summary>
        /// <param name="site_Maps">Sitemap list. </param>
        internal static void DetermineSpeed(ref List<Site_Map> site_Maps)
        {
            if (site_Maps != null)
            {
                foreach (var item in site_Maps)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        _watch = SiteTimeOut(item.UrlSite.ToString());

                        if (item.MaxSpeed < _watch.ElapsedMilliseconds)
                        {
                            item.MaxSpeed = _watch.ElapsedMilliseconds;

                            if (item.MinSpeed == 0) item.MinSpeed = item.MaxSpeed;
                        }
                        else if (item.MinSpeed > _watch.ElapsedMilliseconds) item.MinSpeed = _watch.ElapsedMilliseconds;
                    }
                }
            }
        }

        /// <summary>
        /// Сheck for page request.
        /// </summary>
        /// <param name="url">URL for request. </param>
        /// <returns></returns>
        internal static bool URLExists(string url)
        {
            try
            {
                HttpWebRequest webRequest = WebRequest.CreateHttp(url);
                webRequest.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse()) { return response.StatusCode == HttpStatusCode.OK; }
            }
            catch (Exception) { return false; }

        }

        /// <summary>
        /// Load a site and measure the loading time.
        /// </summary>
        /// <param name="url">URL page. </param>
        /// <returns>Site load time. </returns>
        internal static Stopwatch SiteTimeOut(string url)
        {
            _wc.Headers.Add("User-Agent: Other");

            _watch.Reset();
            _watch.Start();
            byte[] data = _wc.DownloadData(url);
            _watch.Stop();
            return _watch;
        }
    }
}
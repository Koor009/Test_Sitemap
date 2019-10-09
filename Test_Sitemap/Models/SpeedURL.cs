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
                        _watch.Reset();
                        _watch.Start();
                        byte[] data = _wc.DownloadData(item.UrlSite.ToString());
                        _watch.Stop();
                        
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
            bool result = false;

            var webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)webRequest.GetResponse();
                result = true;
            }
            catch (WebException){}
            finally { if (response != null) response.Close(); }

            return result;
        }
    }
}
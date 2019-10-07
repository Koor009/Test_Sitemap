using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Test_Sitemap.Models
{
    public sealed class SpeedURL
    {
        /// <summary>
        /// Speed of each page in the site map
        /// </summary>
        /// <param name="site_Maps">Sitemap list</param>
        public static void DetermineSpeed(ref List<Site_Map> site_Maps)
        {
            if (site_Maps != null)
            {
                WebClient _wc;
                Stopwatch _watch;

                foreach (var item in site_Maps)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        _wc = new WebClient();
                        _watch = new Stopwatch();
                        
                        _watch.Start();
                        byte[] data = _wc.DownloadData(item.UrlSite.ToString());
                        _watch.Stop();

                        if (item.MaxSpeed < _watch.ElapsedMilliseconds)
                        {
                            item.MaxSpeed = _watch.ElapsedMilliseconds;

                            if (item.MinSpeed == 0)
                            {
                                item.MinSpeed = item.MaxSpeed;
                            }
                        }
                        else if (item.MinSpeed > _watch.ElapsedMilliseconds)
                        {
                            item.MinSpeed = _watch.ElapsedMilliseconds;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Сheck for page request
        /// </summary>
        /// <param name="url">URL for request</param>
        /// <returns></returns>
        public static bool URLExists(string url)
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
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
    }
}
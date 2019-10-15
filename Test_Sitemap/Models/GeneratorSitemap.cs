using AngleSharp;
using System.Collections.Generic;
using AngleSharp.Dom;
using System.Net;
using System.Threading.Tasks;

namespace Test_Sitemap.Models
{
    internal sealed class GeneratorSitemap
    {
        /// <summary>
        /// Get the whole page.
        /// </summary>
        /// <param name="url">request URL for generation. </param>
        /// <returns>return document the whole page. </returns>
        internal static IDocument GetPage(Url url)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            var document = BrowsingContext.New(config).OpenAsync(url).Result;

            if (document == null) return null;

            if (document.StatusCode != HttpStatusCode.OK) return null;

            return document;
        }

        /// <summary>
        ///  Defines the sitemap of the requested URL.
        /// </summary>
        /// <param name="url">Request URL search at site. </param>
        /// <returns>Sitemap list</returns>
        internal async static Task<List<Site_Map>> CreateSiteMap(string url)
        {
            var siteMap = new List<Site_Map>();
            Url _url = null;

            _url = ResponseUriSite(url);

            IDocument document = GetPage(_url);

            if (document == null) return null;

            IHtmlCollection<IElement> links = document.QuerySelectorAll("a");

            foreach (var link in links)
            {
                var href = link.GetAttribute("href");

                if (href != null)
                {

                    if (href.StartsWith("http://") || href.StartsWith("https://") || href.StartsWith("www") || href.StartsWith("//"))
                    {
                        if (!href.StartsWith("//"))
                        {
                            if (SpeedURL.URLExists(href)) siteMap.Add(new Site_Map() { UrlSite = href });
                            else
                            {
                                if (!siteMap.Contains(new Site_Map() { UrlSite = ResponseUriSite(href).ToString() }))
                                {
                                    if (SpeedURL.URLExists(ResponseUriSite(href).ToString())) siteMap.Add(new Site_Map() { UrlSite = ResponseUriSite(href).ToString() });
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!href.StartsWith(url.Replace("https://", "")) && !href.StartsWith(url.Replace("http://", "")) && !href.StartsWith(url))
                            if (SpeedURL.URLExists(href)) siteMap.Add(new Site_Map() { UrlSite = ResponseUriSite(href).ToString() });
                            else
                            {
                                if (SpeedURL.URLExists(url + href)) siteMap.Add(new Site_Map() { UrlSite = ResponseUriSite(url + href).ToString() });
                            }
                        else if (href.StartsWith(url))
                        {
                            siteMap.Add(new Site_Map() { UrlSite = ResponseUriSite(href).ToString() });
                        }
                    }
                }
            }

            await Task.Run(() => SpeedURL.DetermineSpeed(ref siteMap)).ConfigureAwait(false);

            return siteMap;
        }


        /// <summary>
        /// Response to protocol extension by URL.
        /// </summary>
        /// <param name="url">Accepts the address. </param>
        /// <returns>Url extension</returns>
        internal static Url ResponseUriSite(string url)
        {
            if (!(url.StartsWith("http") || url.StartsWith("https")))
            {
                HttpWebRequest request = WebRequest.CreateHttp("http://" + url);
                request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { return Url.Convert(response.ResponseUri); }
            }
            else return new Url(url);
        }
    }
}
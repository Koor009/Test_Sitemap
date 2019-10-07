using System;

namespace Test_Sitemap.Models
{
    public class Site_Map
    {
        public Guid Id { get; set; }
        public string UrlSite { get; set; }
        public long MaxSpeed { get; set; }
        public long MinSpeed { get; set; }
        public int? WebSiteId { get; set; }
        public WebSite WebSite { get; set; }

        public Site_Map()
        {
            Id = Guid.NewGuid();
        }
    }
}
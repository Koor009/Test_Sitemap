using System;
using System.Collections.Generic;

namespace Test_Sitemap.Models
{
    /// <summary>
    /// Stores the site link and child elements.
    /// </summary>
    public sealed class WebSite
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Site_Map> Pages { get; set; }

        public WebSite()
        {
            Id = Guid.NewGuid();
            Pages = new List<Site_Map>();
        }
    }
}
using System.Data.Entity;

namespace Test_Sitemap.Models
{
    public class SiteContext : DbContext
    {
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<Site_Map> Site_Maps { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Sitemap.Models
{
    interface ISiteMapRepository
    {
        Task SaveSiteMapAsync(string name, List<Site_Map> siteMap);
        Task<IEnumerable<WebSite>> GetAllSitesMap();
        Task<WebSite> GetSitesMap(string name);
        Task UpdateSiteMap(string name, List<Site_Map> siteMap);
    }
}

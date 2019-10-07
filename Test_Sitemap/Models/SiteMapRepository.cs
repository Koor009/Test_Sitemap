using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Sitemap.Models
{
    public sealed class SiteMapRepository : ISiteMapRepository
    {

        /// <summary>
        /// Saves the result of generating a sitemap
        /// </summary>
        /// <param name="name">Name url request</param>
        /// <param name="siteMap">List sitemap</param>
        public async Task SaveSiteMapAsync(string name, List<Site_Map> siteMap)
        {
            try
            {
                using (SiteContext db = new SiteContext())
                {
                    if (db.WebSites.Include(s=>s.Pages).Where(n=>n.Name==name).Count()!=0 )
                    {
                        await UpdateSiteMap(name, siteMap).ConfigureAwait(false);
                    }
                    else
                    {
                        db.WebSites.Add(entity: new WebSite { Name = name, Pages = siteMap});
                        await db.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Requests all sites that were requested
        /// </summary>
        /// <returns>Return all sites </returns>
        public async Task<IEnumerable<WebSite>> GetAllSitesMap()
        {
            try
            {
                using (SiteContext db = new SiteContext())
                {
                    return await db.WebSites.Include(s=>s.Pages).ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Requests the site from the database with detailed information about the sitemap
        /// </summary>
        /// <param name="name">Looking for the name site in the database</param>
        /// <returns>Return all sitemap</returns>
        public async Task<WebSite> GetSitesMap(string name)
        {
            try
            {
                using (SiteContext db = new SiteContext())
                {
                    return await db.WebSites.Where(n=>n.Name==name).Include(s=>s.Pages).FirstOrDefaultAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update database with detailed information about the sitemap
        /// </summary>
        /// <param name="name">Name url request</param>
        /// <param name="siteMap">List sitemap</param>
        public async Task UpdateSiteMap(string name, List<Site_Map> siteMap)
        {
            try
            {
                using (SiteContext db = new SiteContext())
                {
                    WebSite webSite = await db.WebSites.Include(s => s.Pages).Where(n => n.Name == name).FirstAsync().ConfigureAwait(false);
                    webSite.Pages = siteMap;
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
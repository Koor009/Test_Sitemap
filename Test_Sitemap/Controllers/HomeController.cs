using System.Web.Mvc;
using Test_Sitemap.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using Ninject;
using Test_Sitemap.Filters;

namespace Test_Sitemap.Controllers
{   
    public sealed class HomeController : Controller
    {
        ISiteMapRepository siteMapRepository;
        private WebClient _wc;
        private Stopwatch _watch;
        
        public HomeController()
        {
            _wc = new WebClient();
            _watch = new Stopwatch();

            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ISiteMapRepository>().To<SiteMapRepository>();
            siteMapRepository = ninjectKernel.Get<ISiteMapRepository>();

        }
        /// <summary>
        /// Home page of the site.
        /// </summary>
        /// <returns>Home page. </returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Accepts post request Url addresses for further sitemap generation.
        /// </summary>
        /// <param name="url"> URL for generete site map. </param>
        /// <returns>Show site map details. </returns>
        [HttpPost]
        [FilesError]
        public async Task<ActionResult> Index(string url)
        {
            if (string.IsNullOrEmpty(url)) return View();


            var _url = GeneratorSitemap.ResponseUriSite(url).ToString();

            _watch.Start();
            byte[] data = _wc.DownloadData(_url);
            _watch.Stop();

            var res = await Task.Run(()=> GeneratorSitemap.CreateSiteMap(_url)).ConfigureAwait(false);
            
            await siteMapRepository.SaveSiteMapAsync(_url, res).ConfigureAwait(false);

            ViewBag.Watch = _watch.ElapsedMilliseconds;

            return View("WebsiteDetails",await siteMapRepository.GetSitesMap(_url).ConfigureAwait(false));
        }

        /// <summary>
        /// Opens the site with the details of the site map.
        /// </summary>
        /// <param name="url"> URL for search site map. </param>
        /// <returns>Returns  all sitemap. </returns>
        [HttpGet]
        [FilesError]
        public async Task<ActionResult> WebsiteDetails(string url)
        {
            var _url = GeneratorSitemap.ResponseUriSite(url).ToString();

            _watch.Start();
            byte[] data = _wc.DownloadData(_url);
            _watch.Stop();

            ViewBag.Watch = _watch.ElapsedMilliseconds;

            return View(await siteMapRepository.GetSitesMap(url).ConfigureAwait(false));
        }

        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// All sites after generation.
        /// </summary>
        /// <returns>Previously processed requests. </returns>
        [HttpGet]
        [FilesError]
        public async Task<ActionResult> AllSites()
        {
            return View(await siteMapRepository.GetAllSitesMap().ConfigureAwait(false));
        }
    }
}
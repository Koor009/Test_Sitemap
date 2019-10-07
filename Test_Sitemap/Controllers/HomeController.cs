﻿using System.Web.Mvc;
using Test_Sitemap.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using Ninject;

namespace Test_Sitemap.Controllers
{   
    public class HomeController : Controller
    {
        ISiteMapRepository siteMapRepository;
        private WebClient _wc;
        private Stopwatch _watch;

        
        public HomeController()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ISiteMapRepository>().To<SiteMapRepository>();
            siteMapRepository = ninjectKernel.Get<ISiteMapRepository>();

        }
        /// <summary>
        /// Home page of the site
        /// </summary>
        /// <returns>Home page</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Accepts post request Url addresses for further sitemap generation
        /// </summary>
        /// <param name="url"> URL for generete site map</param>
        /// <returns>Show site map details</returns>
        [HttpPost]
        public async Task<ActionResult> Index(string url)
        {
            if (string.IsNullOrEmpty(url)) return View();

            _wc = new WebClient();
            _watch = new Stopwatch();
            var _url = Generate.ResponseUriSite(url).ToString();
            _watch.Start();

            byte[] data = _wc.DownloadData(_url);

            _watch.Stop();

            var res = await Task.Run(()=>Generate.CreateSiteMap(_url)).ConfigureAwait(false);
            
            await siteMapRepository.SaveSiteMapAsync(_url, res).ConfigureAwait(false);

            ViewBag.Watch = _watch.ElapsedMilliseconds;
            return View("WebsiteDetails",await siteMapRepository.GetSitesMap(_url).ConfigureAwait(false));
        }

        /// <summary>
        /// Opens the site with the details of the site map
        /// </summary>
        /// <param name="url"> URL for search site map</param>
        /// <returns>Returns  all sitemap</returns>
        [HttpGet]
        public async Task<ActionResult> WebsiteDetails(string url)
        {

            _wc = new WebClient();
            _watch = new Stopwatch();
            var _url = Generate.ResponseUriSite(url).ToString();

            _watch.Start();
            byte[] data = _wc.DownloadData(_url);
            _watch.Stop();

            ViewBag.Watch = _watch.ElapsedMilliseconds;

            return View(await siteMapRepository.GetSitesMap(url).ConfigureAwait(false));
        }

        /// <summary>
        /// All sites after generation
        /// </summary>
        /// <returns>Previously processed requests</returns>
        [HttpGet]
        public async Task<ActionResult> AllSites()
        {
            return View(await siteMapRepository.GetAllSitesMap().ConfigureAwait(false));
        }


        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UkadTestTask.Base;
using UkadTestTask.Models;

namespace UkadTestTask.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new MainModel());
        }

        [HttpPost]
        public ActionResult HandleUrl(MainModel asd)
        {
           /* DateTime from = DateTime.Now;
            Sitemap sitemap = (new SitemapProvider()).GetSitemapFromUrl(asd.Url);            
            return PartialView("ResultDataPartialView", new ResultDataModel(sitemap, DateTime.Now.Subtract(from).Milliseconds));*/
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiteAnalyzer.Base;
using SiteAnalyzer.Models;
using SiteAnalyzer.Scanning;

namespace SiteAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        private MainModel _currentModel;

        public ActionResult Index()
        {
            if (_currentModel == null)
                _currentModel = new MainModel();
            return View(_currentModel);
        }

        [HttpPost]
        public ActionResult HandleUrl(MainModel model)
        {
            _currentModel = model;

            ScannerProvider.Scanner.ScanSite(model.Url, false);
            model.Completed = true;
            return PartialView("UrlInputBlock", _currentModel);
        }

        public ActionResult ViewResult(string url)
        {
            SiteScanTask task = ScannerProvider.Scanner.ScanTasks.FirstOrDefault(s => s.Site.Url == url);
            return View("ViewResult", new ResultDataModel(task.Site, task.ResultState));
        }
    }
}
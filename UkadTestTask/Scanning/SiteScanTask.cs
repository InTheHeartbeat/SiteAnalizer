using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using UkadTestTask.Base;

namespace UkadTestTask.Scanning
{
    [Serializable]
    public class SiteScanTask
    {
        [XmlIgnore]
        private BackgroundWorker _scanningWorker;

        public WebSite Site { get; private set; }

        [XmlIgnore]
        public bool SiteScanned => 
            Site != null 
            && Site.SitemapLoaded
            && Site.Sitemaps.All(t => t.Urls.All(u => u.Completed));

        [XmlIgnore]
        public bool Completed => SiteScanned;

        [XmlIgnore]
        public bool IsRunning => _scanningWorker.IsBusy;

        public SiteScanTask()
        { }
        public SiteScanTask(WebSite site)
        {
            Site = site;
        }        

        public void Run()
        {
            if(Site == null)
                throw new ArgumentNullException($"Web site is null");
            if(string.IsNullOrWhiteSpace(Site.Url))
                throw new ArgumentNullException($"Web site url is null or empty");

            _scanningWorker = new BackgroundWorker() {WorkerSupportsCancellation = true};
            _scanningWorker.DoWork += ScanWork;
            _scanningWorker.RunWorkerAsync();
        }

        private void ScanWork(object sender, DoWorkEventArgs e)
        {
            if (!Site.SitemapLoaded)
                Site.Sitemaps = (new SitemapProvider()).GetSitemapsFromUrl(Site.Url);
        }
    }
}
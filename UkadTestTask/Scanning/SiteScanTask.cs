using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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

            foreach (Sitemap sitemap in Site.Sitemaps)
            {
                foreach (ScannableUrl notScannedUrl in sitemap.Urls.Where(u=>!u.Completed))
                {
                    ScannableUrl scannedUrl = InterviewUrl(notScannedUrl);
                    notScannedUrl.LoadTimeMs = scannedUrl.LoadTimeMs;
                    notScannedUrl.Lenght = scannedUrl.Lenght;                    
                    notScannedUrl.Completed = true;
                    if (_scanningWorker.CancellationPending) return;
                }
            }                                
        }

        private ScannableUrl InterviewUrl(ScannableUrl url)
        {
            using (var client = new HttpClient {BaseAddress = new Uri(url.Url)})
            {
                Stopwatch sw = Stopwatch.StartNew();
                string result = client.GetStringAsync("").Result;
                sw.Stop();
                url.LoadTimeMs = sw.ElapsedMilliseconds;
                url.Lenght = result.Length;
                return url;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
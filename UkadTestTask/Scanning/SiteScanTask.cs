using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using SiteAnalyzer.Base;
using SiteAnalyzer.Scanning.Interfaces;
using WebGrease.Css.Extensions;

namespace SiteAnalyzer.Scanning
{    
    public class SiteScanTask : ISiteScanTask
    {
        /// <summary>
        /// Current scanning web site
        /// </summary>
        public WebSite Site { get; set; }        

        /// <summary>
        /// Result state after scanning
        /// </summary>
        public ScanResult ResultState { get; private set; }
        
        /// <summary>
        /// Current state
        /// </summary>
        public ScanState State { get; private set; }

        /// <summary>
        /// Occurs when call method Stop(), but not at the actual stop.
        /// </summary>
        public event Action Stopped;
        /// <summary>
        /// Occurs when scanning is begun
        /// </summary>
        public event Action Started;
        /// <summary>
        /// Return true if site successful scanned
        /// </summary>       
        public bool Completed => Site != null
                                 && Site.Sitemaps.All(t => t.Urls.All(u => u.Completed));

        private bool _cancelationPending;

        public SiteScanTask(WebSite site)
        {
            Site = site;
            State = new ScanState();            
            ResultState = new ScanResult();
            _cancelationPending = false;            
        }

        /// <summary>
        /// Start scanning
        /// </summary>
        public void Scan()
        {
            if (Site == null)
                throw new NullReferenceException($"Web site is null");
            if (string.IsNullOrWhiteSpace(Site.Url))
                throw new NullReferenceException($"Web site url is null or empty");

            ScanTask();
        }

        private async Task ScanTask()
        {                     
            State.ProccessState = ScanProccessState.Scanning;            
            OnStarted();

            if (!Site.SitemapLoaded)
                LoadSitemap();
                                  
            Stopwatch scanningTotalTimeSpent = Stopwatch.StartNew();
            foreach (Sitemap sitemap in Site.Sitemaps)
            {
                foreach (SitemapUrl notScannedUrl in sitemap.Urls.Where(u => !u.Completed))
                {
                    if (_cancelationPending)                                            
                        return;                    

                    State.TextState = "Scanning address " + notScannedUrl.Url + "(" + State.ScannedAddresses + 1 + "/" +
                                      State.TotalAddresses + ")";

                    SitemapUrl scannedUrl = InterviewUrl(notScannedUrl);
                    notScannedUrl.LoadTime = scannedUrl.LoadTime;
                    notScannedUrl.Lenght = scannedUrl.Lenght;
                    notScannedUrl.Completed = true;

                    ResultState.ScannedAddresses = ++State.ScannedAddresses;                                        
                }
                ResultState.ScannedSitemaps = ++State.ScannedSitemaps;
            }
            scanningTotalTimeSpent.Stop();
            ResultState.TotalTimeSpentScanning = scanningTotalTimeSpent.Elapsed;

            Site.Sitemaps.ForEach(sm => ResultState.PageLoadedMinTime = TimeSpan.FromMilliseconds(sm.Urls.Min(t => t.LoadTime.Milliseconds)));
            Site.Sitemaps.ForEach(sm => ResultState.PageLoadedMaxTime = TimeSpan.FromMilliseconds(sm.Urls.Max(t => t.LoadTime.Milliseconds)));
            Site.Sitemaps.ForEach(sm => ResultState.PageLoadedMiddleTime = TimeSpan.FromMilliseconds(sm.Urls.Average(t => t.LoadTime.Milliseconds)));            
        }

        private void LoadSitemap()
        {
            State.TextState = "Loading sitemap";

            Stopwatch siteMapLoadTime = Stopwatch.StartNew();
            Site.Sitemaps = (new SitemapProvider()).GetSitemapsFromUrl(Site.Url);
            siteMapLoadTime.Stop();

            ResultState.SitemapLoadedTime = siteMapLoadTime.Elapsed;
            ResultState.SitemapLoaded = State.SitemapLoaded = Site.SitemapLoaded;
            ResultState.TotalSitemaps = State.TotalSitemaps = Site.Sitemaps.Count;
            Site.Sitemaps.ForEach(t => ResultState.TotalAddresses = State.TotalAddresses += t.Urls.Count);
            // TODO try catch
        }

        private SitemapUrl InterviewUrl(SitemapUrl url)
        {
            using (HttpClient client = new HttpClient {BaseAddress = new Uri(url.Url)})
            {
                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    string result = client.GetStringAsync("").Result;
                    sw.Stop();
                    url.LoadTime = sw.Elapsed;
                    url.Lenght = result.Length;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    url.Lenght = 0;
                    url.LoadTime = TimeSpan.Zero;
                }
                return url;
            }
        }

        public void Stop()
        {
            _cancelationPending = true;
            OnStopped();
        }


        protected virtual void OnStopped()
        {
            Stopped?.Invoke();
        }

        protected virtual void OnStarted()
        {
            Started?.Invoke();
        }
    }
}